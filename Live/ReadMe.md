This is a demonstration of working with mmap and sharing memory between processes.
The code of game was taken from http://rosettacode.org/wiki/Conway%27s_Game_of_Life#C.
You are able to run server for any number of client and with any size of desk.
If you want change any of theese values you should edit common.h file.

If you launch more clients than you specified in common.h, code may run unstable.

The memory of shelf is divided into two principal different parts. First part is binary data - structure, which allows to see business of
blocks and specify actual block id. Second one is n+2 blocks, each block - configuration of game (where n - amount of clients).

Algorithm of server:
Each second server produce evaluation.
1) It chooses free block of memory (condition n+2 blocks garants that if each client assigned different block of memory ,
 there will be a free block to evaluate)

example for 3 clients:
C - current block , 1-3 id of clients

[C,1] [2] [3] [] []
server will evaluate to 4-th block , if clients are very very very slow , then next step will look like this
[1] [2] [3] [C] []
then condition n+2 guarantees that process won't stop.

2) evaluate to free block
3) set this block as current

Before reading, client blocks the block using common structure.

Using this strategy client will never get corrupted data.