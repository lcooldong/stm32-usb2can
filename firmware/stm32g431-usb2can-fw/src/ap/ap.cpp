#include "ap.h"
#include "mode/cli_mode.h"
#include "mode/can_mode.h"


static ap_mode_t mode = MODE_IDLE;
static ap_mode_t mode_next = MODE_IDLE;
static mode_args_t mode_args;

static bool startFlag = true;
static uint32_t led_pre_time[2] = {0,};

static bool apLoopIdle(void);
static void apLedUpdate(void);
static void apGetModeNext(ap_mode_t *p_mode_next);


void apInit(void)
{
  
  i2cOpen(_DEF_I2C2, I2C_FREQ_400KHz);
  cliModeInit();
  canModeInit();

  mode_args.keepLoop = apLoopIdle;

  logBoot(false);
}

void apMain(void)
{
  while(1)
  {
    switch (mode)
    {
      case MODE_CLI:
        cliModeMain(&mode_args);
        break;

      case MODE_CAN:
        canModeMain(&mode_args);
        break;

      default:
        apLoopIdle();
        break;
    }
  }
}


// KeepLoop
bool apLoopIdle(void)
{
  bool ret = true;
  apLedUpdate();  // LED 제어
  apGetModeNext(&mode_next);
  
  if(mode != mode_next)
  {
    mode = mode_next;
    ret = false;
  }

  else if(startFlag)
  {
    startFlag = false;
  }

  return ret;
}

// 현재 모드 확인
void apGetModeNext(ap_mode_t *p_mode_next)
{
  if(usbIsOpen() == true)
  {
    *p_mode_next = MODE_CLI;
  }
  else
  {
    *p_mode_next = MODE_CAN;
  }
}

// LED 확인
void apLedUpdate(void)
{

  uint32_t led_interval;

  switch (mode)
  {
  case MODE_CLI:
    led_interval = 100;
    break;
  case MODE_CAN:
    led_interval = 1000;
    break;
  default:
    led_interval = 3000;
    ledOff(_DEF_LED2);
    break;
  }

  uint32_t cur_time = millis();
  if(cur_time - led_pre_time[0] >= led_interval)
  {
    led_pre_time[0] = cur_time;
    ledToggle(_DEF_LED1);
  }
}