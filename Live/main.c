#include <stdio.h>
#include <stdlib.h>
#include <unistd.h>
#include <fcntl.h> 
#include <sys/mman.h> 
#include <sys/stat.h> 
#include <time.h> 
#include <string.h>
#include <poll.h>
 
#define for_x for (int x = 0; x < w; x++)
#define for_y for (int y = 0; y < h; y++)
#define for_xy for_x for_y
#define handle_error(msg) do {perror(msg);exit(EXIT_FAILURE);} while(0)

const int default_x = 30;
const int default_y = 30;
int block_offset = 0;
void show(int w, int h)
{
	int fd;
	int length = w*h;
	char * file;
	while(1){
		if ((fd = open("shelf" , O_RDONLY)) < 0)
			handle_error("open");
		if ((file = mmap(0,length*2+1,PROT_READ,MAP_PRIVATE,fd,0)) == MAP_FAILED)
			handle_error("mmap");
		close (fd); 
		int offset = (file[0]=='0' ? 1 : 1+ length);
		int counter = 0;
		printf("\033[H");
		for_xy {
			printf(file[offset++] == '1' ? "\033[07m  \033[m" : "  ");
			if (++counter%w == 0)
				printf("\033[E");
		}
		fflush(stdout);
		poll(NULL,0,1000);
	}
}
 
void evolve(void *u, int w, int h)
{
	char (*univ)[w] = u;
	char new[h][w];
	for_y for_x {
		int n = 0;
		for (int y1 = y - 1; y1 <= y + 1; y1++)
			for (int x1 = x - 1; x1 <= x + 1; x1++)
				if (univ[(y1 + h) % h][(x1 + w) % w] == '1')
					n++;
		if (univ[y][x] == '1') n--;
		if (n == 3 || (n ==2 && univ[y][x] == '1')){
			new[y][x] = '1';
		}
		else{
			new[y][x] = '0';
		}
	}
	for_y for_x univ[y][x] = new[y][x];
}
 
void save(void *u , int w , int h){
	int fd , length;
	void * file_memory;
	off_t offset;
	if ((fd = open ("shelf", O_RDWR | O_CREAT, S_IRUSR | S_IWUSR)) < 0)
		handle_error("open");
	length = w * h;
	offset = 1 + length * block_offset; // 1-st byte is a flag for client , it tells from which block to read.
	if ((file_memory = mmap (0, 2*length+1, PROT_WRITE, MAP_SHARED, fd,0)) == MAP_FAILED)
		handle_error("mmap");
	memcpy((char *)file_memory + offset , u , length);
	block_offset = (block_offset+1)%2;
	lseek(fd , 0 , SEEK_SET);
	write(fd , &block_offset , sizeof(int) );
	close(fd);
	munmap (file_memory, length); 
} 
 
void game(int w, int h)
{
	char univ[h][w];
	for_xy univ[y][x] = rand() < RAND_MAX / 5  ? '1' : '0';
	while (1) {
		evolve(univ, w, h);
		save (univ , w, h);
		poll(NULL,0,1000);
	}
}

void init(int w , int h){
	char * empty_string;
	int fd;
	int length = w*h*2+1;
	if ((empty_string = (char *)malloc(length))==NULL)
		handle_error("can't allocate memory");
	else if ((fd = open("shelf",O_RDWR | O_CREAT ,S_IRUSR | S_IWUSR)) < 0)
		handle_error("can't open or create shelf");
	else{
		memset(empty_string , '0' , length);
		if (write(fd , empty_string , length) == -1)
			handle_error("can't write empty string to file");
	}		
	close(fd);
	free (empty_string);
}

int main(int c, char **v)
{
	int x,y;
	x = default_x;
	y = default_y;
	if (c > 1) x = atoi(v[1]);
	if (c > 2) y = atoi(v[2]);
	init(x,y);
	pid_t pid;
	pid = fork();
	if (pid >=0){
		if (pid != 0)
			game(x,y);
		else{
			show(x,y);
		}
	}
	else
		handle_error("can't fork()");
}
