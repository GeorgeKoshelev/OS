This is a demonstration of working with mmap and sharing memory between processes.
The code of game was taken from http://rosettacode.org/wiki/Conway%27s_Game_of_Life#C. I suspect that the game
algorithm has some bugs, but in reality it doesn't matter, because I only need to demonstrate that client gets correct data
from server.

The memory is devided into 3 parts:
1) one byte, it says which block to use
2) 1-st block
3) 2-nd block

After the server evaluates next generation it does next steps:
1) It determines the block, it will be working with.
2) It writes the data in block 
3) It changes block flag.

After client accept next request it does next steps:
1) It creates map of file
2) It takes a look at flag and determines which block to use
3) It reads necessary block.

As you see, client has own unchangable copy files' map in memory and it means that client will get correct data. 