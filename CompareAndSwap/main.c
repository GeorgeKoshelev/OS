#include <stdio.h>
#include <stdlib.h>
#include <pthread.h>

int lock = 0;
const int write_it_count = 1500;
const int buffer_size = 3000;

char *buffer;
char *pointer;

void *threadFunc(void *arg){
	int process_id = (int) arg;
	while(c_cmpxchg(&lock , process_id , 0)!=1){
		usleep(1);
	}
	printf("Process %c locked buffer\n" , process_id);
	int i;
	for (i = 0 ; i < write_it_count ; i++)
	{
		*(pointer) = process_id;
		pointer++;
	}
	if (c_cmpxchg(&lock , 0 , process_id) == 0){
		printf("Error: Process %c , can't unlock buffer\n" , process_id);
	}
	else{
		printf("Process %c unlocked buffer\n" , process_id);
	}
	return NULL;
}

int createPthread(pthread_t * thread , int id){
	int res = pthread_create(thread , NULL , threadFunc , (void *) id);
	if (res == 0){
		printf("Process %c was sucessfully created\n" , id);
	}
	else{
		printf("Process %c wasn't created , error code : %d\n" , id , res);
	}
	return res;
}
int joinPthread(pthread_t *thread ,int id){
	int res = pthread_join(*thread , NULL);
	if (res == 0){
		printf("Process %c was joined\n" , id);
	}
	else{
		printf("Process %c wasn't joined , error code : %d\n" , id , res);
	}
	return res;
}


int main(void){
	buffer = (char * )malloc(sizeof(char) * buffer_size);
	pointer = buffer;
	if (buffer == NULL){
		printf("Allocation failed , now exit\n");
		return 0;
	}
	pthread_t thread;
	pthread_t thread1;
	if (createPthread(&thread , 'a') != 0){
		return 0;
	}
	if (createPthread(&thread1 , 'b') != 0){
		return 0;
	}
	if (joinPthread(&thread , 'a') !=0){
		return 0;
	}
	if (joinPthread(&thread1, 'b') != 0){
		return 0;
	}
	int err = 0;
	int i;
	for (i = 0 ; i < buffer_size-1 ; i++){
		if (buffer[i] != buffer[i+1] && i!=write_it_count-1){
			err = 1;
		}
	}
	if (err == 1){
		printf("Test Failed\n");
	}
	else{
		printf("Test Passed\n");
	}
	printf("Free allocated memory\n");
	free(buffer);
	return 0;
}

int c_cmpxchg(int *dest,int new_val , int cmp_val ){
	int tmp;
	__asm__ __volatile__(
	"lock cmpxchg %1,%3\n\t"
	: "=a" (tmp)
	: "r" (new_val) , "0" (cmp_val), "m" (*dest)
	: "memory" , "cc"
	);
	return tmp == cmp_val;
}
