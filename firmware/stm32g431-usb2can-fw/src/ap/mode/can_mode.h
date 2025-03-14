#ifndef SRC_AP_MODE_CAN_MODE_H_
#define SRC_AP_MODE_CAN_MODE_H_

#include "ap_def.h"

extern volatile uint32_t err_int_cnt;
extern can_tbl_t can_tbl[CAN_MAX_CH];

bool canModeInit(void);
void canModeMain(mode_args_t *args);
bool canHeartBeat(void);

#endif