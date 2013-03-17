You should be root to launch "downloader.sh" script

Usage:
	$ export DISPLAY=:32.0
	$ bash downloader.sh login pass

You have to export DISPLAY before launching script, otherwise script wouldn't work well or even may fall.
You may have some luakit exceptions, I didn't move error output to null because lua sometimes got strange problems.

Ubuntu 12.10 was used for writing this script.
Before launching you should install:
Xvfb 
xdotool
luakit
and some of xfonts that xvfb will ask