#include <ucontext.h>
#include <sys/types.h>
#include <sys/time.h>
#include <signal.h>
#include <stdio.h>
#include <unistd.h>
#include <stdlib.h>
#include <poll.h>

#define NUMCONTEXTS 10              
#define STACKSIZE 4096              
#define INTERVAL 100

int isDebug = 0;
int context_index = -1;

sigset_t set;                       
ucontext_t scheduler_context;
void *signal_stack;

ucontext_t contexts[NUMCONTEXTS];


ucontext_t * current_context;
struct itimerval it;

void sheduler_sleep(int);

void scheduler()
{
	context_index = ++context_index % NUMCONTEXTS;
	current_context = &contexts[context_index];
	printf("run thread %d\n", context_index);
	setcontext(current_context);
}


void timer_interrupt(int j, siginfo_t *si, void *old_context)
{
	printf("\npause thread %d\n",context_index);
	contexts[context_index % NUMCONTEXTS] = *( ucontext_t *) old_context;
	swapcontext(current_context , &scheduler_context);
}

void init_scheduler_context(ucontext_t * signal_cont){
	getcontext(signal_cont);
	signal_cont->uc_stack.ss_sp = signal_stack;
	signal_cont->uc_stack.ss_size = STACKSIZE;
	signal_cont->uc_stack.ss_flags = 0;
	sigemptyset(&signal_cont->uc_sigmask);
	makecontext(signal_cont , scheduler , 0);
}

void setup_signals(void)
{
    	struct sigaction action;
    	action.sa_sigaction = timer_interrupt;
    	int res = sigemptyset(&action.sa_mask);
    	if (res < 0){
		printf("Can't do sigemptyset\n");
		exit(1);
	}
	action.sa_flags = SA_RESTART | SA_SIGINFO;
	sigemptyset(&set);
	sigaddset(&set, SIGALRM);
	if(sigaction(SIGALRM, &action, NULL) != 0) {
        	printf("Can't bind action to signal\n");
		exit(1);
    	}
}

void low_priority_thread(int i)
{
    while(1) {
	int j = 0;
	for(j = 0; j < 20 ; j++){
		printf(" %d ",j);
		poll(NULL,0,100);
	}
    };
}

void create_thread(ucontext_t *uc,  void *function)
{
    void * stack;
    getcontext(uc);
    stack = malloc(STACKSIZE);
    if (stack == NULL) {
        printf("Can't allocate memory for stack\n");
        exit(1);
    }
    uc->uc_stack.ss_sp = stack;
    uc->uc_stack.ss_size = STACKSIZE;
    uc->uc_stack.ss_flags = 0;
    if (sigemptyset(&uc->uc_sigmask) < 0){
      printf("Can't initialize the signal set\n");
      exit(1);
    }
    makecontext(uc, function, 1 , 9);
}

int main(int argc , char* argv[])
{
	int c;
    	signal_stack = malloc(STACKSIZE);
	if (signal_stack == NULL) {
        	printf("Can't allocate signal stack\n");
        	exit(1);
    	}
	int i;
    	for( i =  0; i < NUMCONTEXTS; i++){
        	create_thread(&contexts[i], low_priority_thread);
	}
	setup_signals();
	init_scheduler_context(&scheduler_context);
	current_context = &scheduler_context;
    	sheduler_sleep(1);
	setcontext(current_context);
	return 0;
}

void sheduler_sleep(int sec){
	it.it_interval.tv_sec = sec;
	it.it_value.tv_sec = sec;
	if (setitimer(ITIMER_REAL , &it , NULL) < 0){
		printf("Can't set timer");
		exit(1);
	}
}
