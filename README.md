OS
==

Задания по курсу операционных систем

3-е задание: Память EFI.

Сама виртуальная машина запускалась под windows , необходимы: Qemu-windows-1.2.0 , OVMF_IA32
Пример .bat файла для запуска VM:
	quemu qemu-system-i386w.exe -L Bios -hda fat:hda-contents
Efi файл компилился под Unutu 10.04. makefile в комплекте.

Результат выполнения программы - список Efi memory type-ов и соответствующие им значения памяти.

4-е задание: Compare And Swap

