#ifndef AP_H_
#define AP_H_


#include "ap_def.h"

typedef enum
{
  MODE_IDLE,
  MODE_CLI,
  MODE_CAN,
} ap_mode_t;



void apInit(void);
void apMain(void);


#endif