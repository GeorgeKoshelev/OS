You should be root to launch "downloader.sh" script

Usage:
	$ bash downloader.sh login pass

You may have some luakit exceptions, I didn't move error output to null because lua sometimes got strange problems that may be enteresting

Ubuntu 12.10 was used for writing this script.
Before launching you should install:
Xvfb 
xdotool
luakit
and some of xfonts that xvfb will ask