#include "cli_mode.h"

bool cliModeInit(void)
{
  cliOpen(HW_LOG_CH, 115200);
  uartOpen(HW_UART_CH_DEBUG, 115200);
  uartOpen(HW_UART_CH_EXT, 115200);
  uartOpen(HW_UART_CH_USB, 115200);
  return true;
}

void cliModeMain(mode_args_t *args)
{
  logPrintf("cliMode in\r\n");
#ifndef _USE_HW_RTOS
  uint32_t cli_pre_time = millis();
#endif
  while(args->keepLoop())
  {

#ifdef _USE_HW_RTOS
    cliMain();
    delay(2);
#else
    if(millis() - cli_pre_time >= 2)
    {
      cli_pre_time = millis();
      cliUpdate();
    }
    else
    {

    }
#endif    
    
  }

  logPrintf("cliMode out\r\n");

}

void cliUpdate(void)
{
  uint8_t cli_ch;

  if(usbIsOpen() && usbGetType() == USB_CON_CLI)
  {
    cli_ch = HW_UART_CH_USB;    // Channel 4
  }
  else
  {
    cli_ch = HW_UART_CH_DEBUG;  // Channel 1
  }
  if(cli_ch != cliGetPort())
  {
    cliOpen(cli_ch, 115200);
  }
  cliMain();
}