#!/bin/sh

if [ "$(whoami)" != "root" ]; then
	echo "Only root can launch script."
	exit 1
fi

USAGE="Usage: downloader.sh login pass"

if [ $# -eq 1 ] && [ "$1" == "-h" ]; then
	echo "$USAGE"
	exit 0
elif [ $# -eq 2 ]; then
	LOGIN=$1
	PASS=$2
else
	echo "$USAGE"
	exit 1
fi

test -d "temp" || mkdir -p temp

xdpyinfo -display :32.0 >/dev/null 2>&1 && echo "32.0 already in use, please kill Xvfb before relaunching script" && exit 1

Xvfb :32 -screen 0 1024x768x24 -fbdir temp &
XVFB=$!

sleep 2

export DISPLAY=:32.0 &
sleep 1
DISPLAY=:32 luakit &
LUA=$!
sleep 2

xdotool selectwindow &
sleep 2
xdotool type ":open http://lostfilm.tv"
xdotool key KP_Enter
sleep 20
xdotool type "gi"
xdotool key Tab
xdotool type $LOGIN
xdotool key Tab
xdotool type $PASS
xdotool key Tab
xdotool key KP_Enter
sleep 20
for i in {1..10}
	do
		 xdotool key Right 
	done
coordinates=$(convert temp/Xvfb_screen0 -alpha off -fill white +opaque 'rgb(213,35,39)' -alpha on txt: | grep 213,35,39,1 | head -n 1 | sed -n 's/^\([0-9]*\),\([0-9]*\).*$/\1 \2/p')
echo "coordinates: $coordinates"
xdotool mousemove $coordinates click 1
xdotool click 1
sleep 5
convert temp/Xvfb_screen0 screenshot.png

kill $LUA
kill $XVFB
rm -rf temp
