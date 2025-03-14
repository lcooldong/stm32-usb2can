
 #ifndef SRC_AP_MODE_CLI_MODE_H_
 #define SRC_AP_MODE_CLI_MODE_H_
 
 
 #include "ap_def.h"
 
 bool cliModeInit(void);
 void cliModeMain(mode_args_t *args);
 void cliUpdate(void);
 
 #endif /* SRC_AP_MODE_CLI_MODE_H_ */