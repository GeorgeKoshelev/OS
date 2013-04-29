CC = gcc
RM = rm -f

all : main

main:
	$(CC) -std=gnu99 -Wall main.c live.c -DA -o server
	$(CC) -std=gnu99 -Wall client.c -o client
clean:
	$(RM) client
	$(RM) server

