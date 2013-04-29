#include <stdio.h>
#include <stdlib.h>
#include <unistd.h>
#include <fcntl.h> 
#include <sys/mman.h> 
#include <sys/stat.h> 
#include <sys/types.h>
#include <time.h> 
#include <string.h>
#include <poll.h>
#include "live.h"
#include "common.h"


void * init(int fd){	
	if (fd < 0){
		handle_error("can't open or crate shelf");
	}
	if (ftruncate(fd , sizeof(game)) != 0){
		handle_error("can't truncate shelf");
	}
	game * game;
	if ((game = mmap (NULL, sizeof(game), PROT_READ | PROT_WRITE, MAP_SHARED, fd,0)) == MAP_FAILED){
		handle_error("mmap game info failed");
	}
	for_xy game->configurations[0].field[y][x] = rand() < RAND_MAX / 10  ? '1' : '0';
	return game;
}

int main()
{
	int fd = open ("shelf" , O_RDWR | O_CREAT );
	void * game = init(fd);
	while(1){
		evolve(game);
		poll(NULL , 0 , 1000);
	}
	munmap (game, sizeof(game));
	close(fd);
}
