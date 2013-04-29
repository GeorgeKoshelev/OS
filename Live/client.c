#include <stdio.h>
#include <stdlib.h>
#include <unistd.h>
#include <fcntl.h> 
#include <sys/mman.h> 
#include <sys/stat.h> 
#include <poll.h>
#include "common.h"

void show(void * u);

void start_client(){
	int fd = open ("shelf" , O_RDWR);
	if (fd < 0)
		handle_error("open");
	game * game = mmap(NULL, sizeof(game), PROT_READ|PROT_WRITE, MAP_SHARED, fd, 0);
	if (game == MAP_FAILED)
		handle_error("mmap");
	while(1){
		show(game);
		poll(NULL , 0 , 1000);
	}
}

void show(void *u)
{
	game * game = u;
	int currentBlock = game->currentBlock;
	game->blockBusyness[currentBlock]++;
	printf("\033[H");
	for_y {
		for_x printf(game->configurations[currentBlock].field[y][x] == '1' ? "\033[07m  \033[m" : "  ");
		printf("\033[E");
	}
	fflush(stdout);
	game->blockBusyness[currentBlock]--;
}
 
int main(){
	start_client();
}