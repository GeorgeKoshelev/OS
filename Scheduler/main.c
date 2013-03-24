#include <ucontext.h>
#include <sys/types.h>
#include <sys/time.h>
#include <signal.h>
#include <stdio.h>
#include <stdlib.h>
#include <poll.h>

#define NUMCONTEXTS 4
              
#define STACKSIZE 4096              
#define INTERVAL 100

typedef struct{
	ucontext_t context;
	int isTerminated;
	int priority;
	time_t wakeUpTime;
	int time_to_sleep;
} myContext;

volatile int context_index = -1;
volatile int threads_alive;                    
volatile ucontext_t scheduler_context;
volatile myContext contexts[NUMCONTEXTS];
volatile ucontext_t * current_context;
volatile int mutex = 0;

struct itimerval it;
sigset_t set;
void *signal_stack;
void sheduler_sleep(int);

void scheduler()
{
	sheduler_sleep(0);
	while(threads_alive != 0){
		int k;
		time_t now = time(NULL);
		for (k = NUMCONTEXTS/2;k<NUMCONTEXTS;k++){
			if (contexts[k].isTerminated == 1)
				continue;
			if (now > contexts[k].wakeUpTime){
				context_index = k;
				current_context = (ucontext_t *)&contexts[k].context;	
				printf("run thread %d,priority %d\n",k,contexts[k].priority);
				sheduler_sleep(1);
				setcontext((ucontext_t *)current_context);
			}
		}
		for (k = 0 ; k < NUMCONTEXTS/2;k++){
			if (contexts[k].isTerminated == 1)
				continue;
			if (now > contexts[k].wakeUpTime){
				context_index = k;
				current_context = (ucontext_t *)&contexts[k].context;
				printf("run thread %d , priority %d\n",k,contexts[k].priority);
				sheduler_sleep(1);
				setcontext((ucontext_t *)current_context);
			}
		}
		poll(NULL,0,50);
	}
}

void sleep_thread(int ms, int index){
	time_t wut = time(NULL);
	contexts[index].wakeUpTime = wut + ms;
}

void timer_interrupt(int j, siginfo_t *si, void *old_context)
{
	if (mutex == 1){
		printf("thread %d in crytical zone , get control back" , context_index);
		swapcontext((ucontext_t *)current_context,(ucontext_t *)old_context);
	}
	printf("\npause thread %d for %d seconds \n",context_index,contexts[context_index].time_to_sleep);
	contexts[context_index].context = *( ucontext_t *) old_context;
	sleep_thread(contexts[context_index].time_to_sleep,context_index); 
	swapcontext((ucontext_t *)current_context , (ucontext_t *)&scheduler_context);
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

void low_priority_thread()
{
	int j , z;
	for(j = 0; j < 15 ; j++){
		mutex = 1;
		//crytical zone
		for (z = 0 ; z < 2 ; z ++ ){
			printf(" %d ",j);
		}
		mutex = 0;
		poll (NULL,0,100);
    	}
    	contexts[context_index].isTerminated = 1;
	threads_alive--;
	printf("\nThread %d is terminated\n",context_index);
}

void high_priority_thread(){
	int j , z;
	for(j = 97 ; j < 122 ; j++ ){
		mutex = 1;
		//crytical zone
		for (z = 0 ; z < 2 ; z++){
			printf(" %c ",j);
		}
		mutex = 0;
		poll (NULL,0,100);
	}
	contexts[context_index].isTerminated = 1;
	threads_alive--;
	printf("\nThread %d is terminated\n",context_index);
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
    uc->uc_link = (ucontext_t *)&scheduler_context;
    if (sigemptyset(&uc->uc_sigmask) < 0){
      printf("Can't initialize the signal set\n");
      exit(1);
    }
    makecontext(uc, function, 0 );
}

int main(int argc , char* argv[])
{
	threads_alive = NUMCONTEXTS;
	int c;
    	signal_stack = malloc(STACKSIZE);
	if (signal_stack == NULL) {
        	printf("Can't allocate signal stack\n");
        	exit(1);
    	}
	int i;
    	for( i =  0; i < NUMCONTEXTS/2; i++){
        	create_thread((ucontext_t *)&contexts[i].context, low_priority_thread);
		contexts[i].isTerminated = 0;
		contexts[i].priority = 1;
		contexts[i].wakeUpTime = time(NULL);
		contexts[i].time_to_sleep = 6; 
	}
	for (i = NUMCONTEXTS/2 ; i< NUMCONTEXTS ; i++){
		create_thread((ucontext_t *)&contexts[i].context,high_priority_thread);
		contexts[i].isTerminated = 0;
		contexts[i].priority = 2;
		contexts[i].wakeUpTime = time(NULL);
		contexts[i].time_to_sleep = 4;
	}
	setup_signals();
	init_scheduler_context((ucontext_t *)&scheduler_context);
	current_context = &scheduler_context;
	setcontext((ucontext_t *)current_context);
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
