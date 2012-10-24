Emulator
========

Computer Emulator

In app.config are some keys for signals. 0 means that signal is corrupted , 1 is ok.
Also there are some start value for registers.
Try different combinations if you want.

Use WinHex or any hex-editor to change start memory.

Some tests:

Memory Shapshot for write:
01 04 00 00 FF 00

Memory Snapshot for reading memory:
11 00 FF 00

Memory Snapshot for reading IA:
15 01 FF 00

Memory Snapshot for write to IR:
11 00 02 00 FF 00

Memory Snapshot for sum:
11 00 21 00 FF 00

Memory Snapshot for sum with IA:
11 00 25 01 FF 00

Memory Snapshot for min :
11 00 31 00 FF 00

Memory Snapshot for always jump:
11 00 FE 00

Memory snapshot for == 0:
true: 11 03 F0 00 FF 00
false: 11 01 F0 00 FF 00

Memory snapshot for >0:
true: 11 01 F1 00 FF 00
false: 11 03 F1 00 FF 00

Koshelev George.