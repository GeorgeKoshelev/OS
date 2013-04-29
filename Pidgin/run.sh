#!/bin/bash

if [ $# -ne 3 ]
then
	echo "Usage : fromJabber toJabber fileToWatch"
	exit
fi
while inotifywait -e modify "$3"; do
	python main.py $1 $2 "File $3 was changed"
done
