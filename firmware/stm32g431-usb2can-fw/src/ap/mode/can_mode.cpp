#include "can_mode.h"

// #define UART2_DEBUG

uint32_t can_index = 0;
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
  // canOpen(_DEF_CAN1, CAN_NORMAL, CAN_FD_NO_BRS, CAN_500K, CAN_2M);
  
  
  canOpen(_DEF_CAN1, CAN_NORMAL, CAN_CLASSIC, CAN_500K, CAN_2M);  // Sync to uart 115200
  

  canConfigFilter(_DEF_CAN1, 0, CAN_STD, 0x0123, 0x0000); // TODO
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
      canHeartBeat();
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
    

    if(canMsgAvailable(_DEF_CAN1))
    {
      canMsgRead(_DEF_CAN1, &msg);

      can_index %= 10000;
      uartPrintf(HW_UART_CH_RS485, "%03d(R) <- id ", can_index++);
      if (msg.id_type == CAN_STD)
      {
        uartPrintf(HW_UART_CH_RS485, "std ");
      }
      else
      {
        uartPrintf(HW_UART_CH_RS485, "ext ");
      }
      uartPrintf(HW_UART_CH_RS485, ": 0x%08X, L:%02d, ", msg.id, msg.length);

      for (int i = 0; i < msg.length; i++)
      {
        uartPrintf(HW_UART_CH_RS485, "0x%02X ", msg.data[i]);
      }
      uartPrintf(HW_UART_CH_RS485, "\r\n");
    }

    if(can_cur_time - can_pre_time[1] >= 1000)
    {
      can_pre_time[1] = can_cur_time;
      ledToggle(_DEF_LED2);
    }

#ifdef UART2_DEBUG
    if(uartAvailable(_DEF_UART2) > 0)
    {
      uartPrintf(_DEF_UART2, "RX : 0x%X\r\n", uartRead(_DEF_UART2));
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

