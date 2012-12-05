#include <stdio.h>
#include <pthread.h>

int lock = 0;

void *threadFunc(void *arg){
	while(c_cmpxchg(&lock , (int) arg , 0)!=1){
		usleep(1);
	}
	printf("Process %c lock output\n" , (int) arg);
	int i = 0;
	while(i++ < 100){
		usleep(1);
		printf("%c" , (int)arg);
	}
	if (c_cmpxchg(&lock , 0 , (int) arg) == 0){
		printf("Process %c , can't unlock output\n" , (int) arg);
	}
	else{
		printf("Process %c unlocked output\n" , (int) arg);
	}
	return NULL;
}

int main(void){
	pthread_t thread;
	pthread_t thread1;
	pthread_create(&thread , NULL,threadFunc,(void *)'a');
	pthread_create(&thread1 , NULL , threadFunc , (void *)'b');
	pthread_join(thread , NULL);
	pthread_join(thread1 , NULL);
	return 0;
}

int c_cmpxchg(int *dest,
	int new_val , int cmp_val ){
	int tmp;
	__asm__ __volatile__(
	"lock cmpxchg %1,%3\n\t"
	: "=a" (tmp)
	: "r" (new_val) , "0" (cmp_val), "m" (*dest)
	: "memory" , "cc"
	);
	return tmp == cmp_val;
}
