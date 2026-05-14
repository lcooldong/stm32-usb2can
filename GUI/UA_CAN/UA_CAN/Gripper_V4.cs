using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace UA_CAN
{
    internal class Gripper_V4
    {
        // 1. 아두이노와 100% 일치하는 8바이트 구조체 설계 (Union 구조 재현)
        [StructLayout(LayoutKind.Explicit, Pack = 1)]
        public struct CanPacket
        {
            [FieldOffset(0)] public byte cmd;
            [FieldOffset(1)] public byte id;

            // [송신용] CMD_MOVE 시 사용 (Offset 2부터 시작)
            [FieldOffset(2)] public ushort target_position;
            [FieldOffset(4)] public ushort speed;

            // [수신용] CMD_HEARTBEAT / ARRIVED 수신 시 사용
            [FieldOffset(2)] public ushort gripper_pos;
            [FieldOffset(4)] public ushort pusher_pos;
            [FieldOffset(6)] public byte isConnect;
            [FieldOffset(7)] public byte error_code;
        }

        // 2. 통신 명령어 상수 (아두이노 Enum과 일치)
        public const byte CMD_STOP = 0x01;
        public const byte CMD_MOVE = 0x02;
        public const byte CMD_ARRIVED = 0x03;
        public const byte CMD_GET_STATE = 0x10;
        public const byte CMD_TORQUE_ON = 0x11;
        public const byte CMD_TORQUE_OFF = 0x12;
        public const byte CMD_HEARTBEAT = 0xFF;

        public const byte ID_GRIPPER = 1;
        public const byte ID_PUSHER = 2;

        // 3. 내부 통신 객체
        private USB2CAN _serial;
        private CANablePro _can;

        // 4. 외부에서 참조할 실시간 상태 데이터
        public ushort CurrentGripperPos { get; private set; }
        public ushort CurrentPusherPos { get; private set; }
        public byte ConnectionStatus { get; private set; } // 0x03이면 정상
        public byte LastErrorCode { get; private set; }

        public Gripper_V4(USB2CAN serial)
        {
            _serial = serial;
            _can = new CANablePro(_serial);
        }

        #region 장치 연결 제어
        public bool Connect(string port, int baudrate = 115200)
        {
            // USB2CAN의 begin 호출
            if (_serial.begin(port, baudrate))
            {
                _can.openChannel(); // CAN 채널 오픈 (O\r)
                _can.read();        // 수신 스레드 시작
                return true;
            }
            return false;
        }

        public void Disconnect()
        {
            _can.stopRead();
            _can.closeChannel();
            _serial.close();
        }

        // USB2CAN의 자동 연결 기능을 활성화할 때 사용
        public void EnableAutoReconnect()
        {
            _serial.autoConnect();
        }
        #endregion

        #region 명령 송신 (PC -> Arduino)

        private void SendPacket(CanPacket pkt)
        {
            byte[] data = StructToBytes(pkt);
            // 아두이노가 0x123 필터링 중이므로 ID 0x123으로 송신
            // 8바이트 표준 CAN 프레임 사용
            _can.write(CAN_TYPE.CAN_CLASSIC, 0x123, CAN_DLC.FDCAN_DLC_BYTE_8, data);
        }

        public void MoveGripper(ushort position, ushort speed = 100)
        {
            CanPacket pkt = new CanPacket
            {
                cmd = CMD_MOVE,
                id = ID_GRIPPER,
                target_position = position,
                speed = speed
            };
            SendPacket(pkt);
        }

        public void MovePusher(ushort position, ushort speed = 100)
        {
            CanPacket pkt = new CanPacket
            {
                cmd = CMD_MOVE,
                id = ID_PUSHER,
                target_position = position,
                speed = speed
            };
            SendPacket(pkt);
        }

        public void EmergencyStop()
        {
            CanPacket pkt = new CanPacket { cmd = CMD_STOP };
            SendPacket(pkt);
        }

        public void SetTorque(byte motorId, bool isOn)
        {
            CanPacket pkt = new CanPacket
            {
                cmd = isOn ? CMD_TORQUE_ON : CMD_TORQUE_OFF,
                id = motorId
            };
            SendPacket(pkt);
        }

        public void RequestState()
        {
            CanPacket pkt = new CanPacket { cmd = CMD_GET_STATE };
            SendPacket(pkt);
        }
        #endregion

        #region 데이터 수신 및 파싱 (Arduino -> PC)

        /// <summary>
        /// 이 메서드를 타이머(예: 20ms)에서 주기적으로 호출하여 상태를 갱신하십시오.
        /// </summary>
        public void ParseIncomingData()
        {
            var lastPkt = _can.GetLastPacket();
            if (lastPkt == null || lastPkt.data.Count < 8) return;

            // 수신된 byte 리스트를 CanPacket 구조체로 변환
            CanPacket received = BytesToStruct(lastPkt.data.ToArray());

            switch (received.cmd)
            {
                case CMD_HEARTBEAT: // 0xFF
                    CurrentGripperPos = received.gripper_pos;
                    CurrentPusherPos = received.pusher_pos;
                    ConnectionStatus = received.isConnect;
                    LastErrorCode = received.error_code;
                    break;

                case CMD_ARRIVED: // 0x03
                    string motor = (received.id == ID_GRIPPER) ? "GRIPPER" : "PUSHER";
                    Console.WriteLine($"[ACK] {motor} Arrived at {received.gripper_pos}");
                    break;

                case CMD_STOP:
                    Console.WriteLine("[ACK] Emergency Stop Active");
                    break;
            }
        }
        #endregion

        #region Marshal Helper (구조체 <-> 바이트배열 변환)
        private byte[] StructToBytes(CanPacket pkt)
        {
            int size = Marshal.SizeOf(pkt);
            byte[] arr = new byte[size];
            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(pkt, ptr, false);
            Marshal.Copy(ptr, arr, 0, size);
            Marshal.FreeHGlobal(ptr);
            return arr;
        }

        private CanPacket BytesToStruct(byte[] data)
        {
            GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);
            try
            {
                return (CanPacket)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(CanPacket));
            }
            finally
            {
                handle.Free();
            }
        }
        #endregion
    }
}