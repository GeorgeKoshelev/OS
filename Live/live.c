#include "common.h"
#include <stdio.h>

void evolve(void * u)
{
	game *game = u;
	int current_block = game->currentBlock;
	game->blockBusyness[current_block]++;
	int new_block = -1;
	for (int i = 0 ; i < NUMBER_OF_CLIENTS + 2 ; i++)
		if (game->blockBusyness[i] == 0)
			new_block = i;		
	for_y for_x {
		int n = 0;
		for (int y1 = y - 1; y1 <= y + 1; y1++)
			for (int x1 = x - 1; x1 <= x + 1; x1++)
				if (game->configurations[current_block].field[(y1 + Y) % Y][(x1 + X) % X] == '1')
					n++;
		if (game->configurations[current_block].field[y][x] == '1') n--;
		if (n == 3 || (n ==2 && game->configurations[current_block].field[y][x] == '1')){
			game->configurations[new_block].field[y][x] = '1';
		}
		else{
			game->configurations[new_block].field[y][x] = '0';
		}
	}
	game->blockBusyness[current_block]--;
	game->currentBlock = new_block;
}
