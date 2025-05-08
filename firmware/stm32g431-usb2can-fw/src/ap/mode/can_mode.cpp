#include "can_mode.h"

// #define UART2_DEBUG

uint32_t can_index = 0;
uint8_t uart_rx_index = 0;
uint32_t count = 0;


uint8_t         can_ch = _DEF_CAN1;
CanFilterType_t can_mode_filter_type = CAN_ID_MASK;
CanIdType_t     can_mode_filter_id_type = CAN_STD;
uint32_t        can_mode_filter_id1 = 0x0000'0000;
uint32_t        can_mode_filter_id2 = 0x0000'0000;

extern can_tbl_t can_tbl[CAN_MAX_CH];
// extern volatile uint32_t err_int_cnt;

bool canModeInit(void)
{
  uartOpen(HW_UART_CH_RS485, 115200);
  canOpen(_DEF_CAN1, CAN_NORMAL, CAN_FD_NO_BRS, CAN_500K, CAN_2M);
  
  
  // canOpen(_DEF_CAN1, CAN_NORMAL, CAN_CLASSIC, CAN_500K, CAN_2M);  // Sync to uart 115200
  

  // canConfigFilter(_DEF_CAN1, 0, CAN_STD, 0x0123, 0x0000); // TODO
  return true;
}

void canModeMain(mode_args_t *args)
{
  logPrintf("canMode in\r\n");
  uint32_t can_cur_time = 0;
  uint32_t can_pre_time[2] = {0,};
  can_msg_t msg;
  uint32_t err_code;
  // uint8_t ch;
  // CanFrame_t frame;
  
  

  err_code = can_tbl[_DEF_CAN1].err_code;

  while (args->keepLoop())
  {
    can_cur_time = millis();
    if(can_cur_time - can_pre_time[0] >= 100)
    {
      can_pre_time[0] = can_cur_time;
      // canHeartBeat();
    }

    
    if (can_tbl[_DEF_CAN1].err_code != err_code)
    {
      cliPrintf("ErrCode : 0x%X\n", can_tbl[_DEF_CAN1].err_code);
      canErrPrint(_DEF_CAN1);
      err_code = can_tbl[_DEF_CAN1].err_code;
    }


    canErrUpdate(_DEF_CAN1);
    if(canUpdate())
    {
      uartPrintf(HW_UART_CH_RS485, "BusOff Recovery\r\n");
      // logPrintf("BusOff Recovery\r\n");
    }
    else
    {
      
    }
    
    // CAN -> RS485
    if(canMsgAvailable(_DEF_CAN1))
    {
      canMsgRead(_DEF_CAN1, &msg);
      // uint8_t byteData[1] = {0x50}; // 1byte
      // uartWrite(HW_UART_CH_RS485, byteData, 1); // Send Test
      
      uartWrite(HW_UART_CH_RS485, msg.data, msg.length);
      
      can_index %= 10000;
      uartPrintf(HW_UART_CH_USB, "%03d(R) <- id ", can_index++);
      if (msg.id_type == CAN_STD)
      {
        uartPrintf(HW_UART_CH_USB, "std ");
      }
      else
      {
        uartPrintf(HW_UART_CH_USB, "ext ");
      }
      uartPrintf(HW_UART_CH_USB, ": 0x%08X, L:%02d, ", msg.id, msg.length);
      
      // Receive Can Message
      for (int i = 0; i < msg.length; i++)
      {
        uartPrintf(HW_UART_CH_USB, "0x%02X ", msg.data[i]);
      }
      uartPrintf(HW_UART_CH_USB, "\r\n");
      

      // can_index %= 10000;
      // uartPrintf(HW_UART_CH_RS485, "%03d(R) <- id ", can_index++);
      // if (msg.id_type == CAN_STD)
      // {
      //   uartPrintf(HW_UART_CH_RS485, "std ");
      // }
      // else
      // {
      //   uartPrintf(HW_UART_CH_RS485, "ext ");
      // }
      // uartPrintf(HW_UART_CH_RS485, ": 0x%08X, L:%02d, ", msg.id, msg.length);

      // for (int i = 0; i < msg.length; i++)
      // {
      //   uartPrintf(HW_UART_CH_RS485, "0x%02X ", msg.data[i]);
      // }
      // uartPrintf(HW_UART_CH_RS485, "\r\n");
    }

    if(can_cur_time - can_pre_time[1] >= 500)
    {
      can_pre_time[1] = can_cur_time;
      count++;

      // Terminal 에 작성 해야만 uartAvailable(HW_UART_CH_USB) 가 +가 된다.
      uartPrintf(HW_UART_CH_USB, "[%d]:%d %d\r\n", count, uartAvailable(HW_UART_CH_USB), uartAvailable(HW_UART_CH_RS485));
      uartFlush(HW_UART_CH_USB);  // Queue Clear = uartAvailable(HW_UART_CH_USB) -> 0
      // uartPrintf(HW_UART_CH_RS485, "LED2 : %d\r\n", count);  // Transmit OK
      ledToggle(_DEF_LED2);
    }
    
    // uartAvailable USB Not Work
    if(uartAvailable(HW_UART_CH_RS485) > 0)
    {
      uartPrintf(HW_UART_CH_RS485, "LED2 : %d\r\n", count);
    }
    // RS485 -> USB
    uint8_t byteData1 = uartRead(HW_UART_CH_RS485); // 1byte
    if(byteData1 != 0x00)
    {
      // uartPrintf(HW_UART_CH_RS485, "RETURN : 0x%02X\r\n", byteData1);
      uartPrintf(HW_UART_CH_USB, "RECEIVED : 0x%02X\r\n", byteData1);
      uartWrite(HW_UART_CH_RS485, &byteData1, 1); // Send Test
    }


    // RS485 -> CAN
    if (uartAvailable(HW_UART_CH_RS485) > 0)
    {
      
      // uint8_t byteData = uartRead(HW_UART_CH_RS485); // 1byte
      // uartPrintf(HW_UART_CH_USB, "0x%02X\r\n", byteData);
      
      if(uart_rx_index >= sizeof(msg.data))
      {
        // canMsgWrite(_DEF_CAN1, &msg, 10); //
        uart_rx_index = 0;
      }
      

      for (int i = 0; i < msg.length; i++)
      {
        
      }
      // uartPrintf(HW_UART_CH_DEBUG, "D Hello\r\n");
      // uartPrintf(HW_UART_CH_EXT, "E Hello\r\n");
      // uartPrintf(HW_UART_CH_USB, "U Hello\r\n");
      
      

      // delay(1000);
      
      
    }

    
#ifdef UART2_DEBUG
    if(uartAvailable(HW_UART_CH_RS485) > 0)
    {
      // uartPrintf(HW_UART_CH_RS485, "RX : 0x%X\r\n", uartRead(HW_UART_CH_RS485));
      uint8_t byteData = uartRead(HW_UART_CH_RS485); // 1byte
      uartPrintf(HW_UART_CH_USB, "RX : 0x%02X\r\n", byteData);  // RS485 -> USB
    }
    else
    {
      delay(1);
    }
#endif
  }
  logPrintf("canMode out\r\n");
}

bool canHeartBeat(void)
{ 
  can_msg_t state_msg;

  state_msg.frame   = CAN_FD_NO_BRS;
  state_msg.id_type = CAN_STD;
  // state_msg.id_type = CAN_EXT;
  state_msg.dlc     = CAN_DLC_8;
  state_msg.id      = 0x123;
  state_msg.length  = 8;

  for (int i = 0; i < state_msg.length; i++)
  {
    state_msg.data[i] = i + can_index;
  }

  if(canMsgWrite(_DEF_CAN1, &state_msg, 10) > 0)
  {
    can_index %= 10000;
    uartPrintf(HW_UART_CH_RS485, "%03d(T) -> id ", can_index++);
    if (state_msg.id_type == CAN_STD)
    {
      uartPrintf(HW_UART_CH_RS485, "std ");
    }
    else
    {
      uartPrintf(HW_UART_CH_RS485, "ext ");
    }
    uartPrintf(HW_UART_CH_RS485, ": 0x%08X, L:%02d, ", state_msg.id, state_msg.length);
    for (int i=0; i<state_msg.length; i++)
    {
      uartPrintf(HW_UART_CH_RS485, "0x%02X ", state_msg.data[i]);
    }
    uartPrintf(HW_UART_CH_RS485, " | %d %d\r\n", can_tbl[_DEF_CAN1].err_code, can_tbl[_DEF_CAN1].state);
  }

  if (canGetRxErrCount(_DEF_CAN1) > 0 || canGetTxErrCount(_DEF_CAN1) > 0)
  {
    uartPrintf(HW_UART_CH_RS485, "ErrCnt : %d, %d\r\n", canGetRxErrCount(_DEF_CAN1), canGetTxErrCount(_DEF_CAN1));
  }


  // if (err_int_cnt > 0)
  // {
  //   uartPrintf(HW_UART_CH_RS485, "Cnt : %d\n",err_int_cnt);
  //   err_int_cnt = 0;
  // }


  return true;
}

