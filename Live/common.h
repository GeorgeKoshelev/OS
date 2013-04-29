#define X 30
#define Y 30
#define NUMBER_OF_CLIENTS 2

#define for_x for (int x = 0; x < X; x++)
#define for_y for (int y = 0; y < Y; y++)
#define for_xy for_x for_y
#define handle_error(msg) do {perror(msg);exit(EXIT_FAILURE);} while(0)

typedef struct configuration{
	char field[X][Y];
} configuration;

typedef struct game {
	int currentBlock;
	int blockBusyness [NUMBER_OF_CLIENTS + 2];
	configuration configurations[NUMBER_OF_CLIENTS + 2]; 
} game;