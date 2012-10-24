//Author George Koshelev : george.koshelev@hackerdom.ru

#include <efi.h>
#include <efilib.h>

typedef CHAR16 * string;

const string GetMemoryTypeDisplayValue(UINT32 MemoryType);
EFI_STATUS PrintMemoryMap(void);
EFI_STATUS GetMemoryMap(EFI_MEMORY_DESCRIPTOR ** map , UINTN * size , UINTN * key , UINTN * dsize , UINT32 * dversion );
void HandleAllocatePoolError(EFI_STATUS err);
void HandleGetMemoryMapError(EFI_STATUS err);
void PrintMemoryStatus(UINTN size , UINTN key , UINTN dsize , UINT32 dversion);
void PrintMappingStatus(int index , EFI_MEMORY_DESCRIPTOR *pointer );

const string DisplayMemoryTypes[] = {
    L"EfiReservedMemoryType",
    L"EfiLoaderCode",
    L"EfiLoaderData",
    L"EfiBootServicesCode",
    L"EfiBootServicesData",
    L"EfiRuntimeServicesCode",
    L"EfiRuntimeServicesData",
    L"EfiConventionalMemory",
    L"EfiUnusableMemory",
    L"EfiACPIReclaimMemory",
    L"EfiACPIMemoryNVS",
    L"EfiMemoryMappedIO",
    L"EfiMemoryMappedIOPortSpace",
    L"EfiPalCode",
};

EFI_STATUS efi_main(EFI_HANDLE ImageHandle , EFI_SYSTEM_TABLE * SystemTable){
	InitializeLib(ImageHandle , SystemTable);
	return PrintMemoryMap();
}

const string GetMemoryTypeDisplayValue(UINT32 MemoryType){
	return (MemoryType > sizeof(DisplayMemoryTypes)) ?
		L"Error: Unknown Memory Type" : DisplayMemoryTypes[MemoryType]; 
		
}

void PrintMappingStatus(int index , EFI_MEMORY_DESCRIPTOR *pointer){
	UINTN size = pointer->NumberOfPages * 4096; 
	EFI_PHYSICAL_ADDRESS PhysicalEnd = pointer->PhysicalStart + size;
	EFI_VIRTUAL_ADDRESS VirtualEnd = pointer->VirtualStart + size;
	Print(L"%d Type: %s\n", index , GetMemoryTypeDisplayValue(pointer->Type));
	Print(L"PhysicalMemory : %016llx-%016llx\n" , pointer->PhysicalStart , PhysicalEnd);
	Print(L"VirtualMemory : %016llx-%016llx\n\n", pointer->VirtualStart , VirtualEnd);
}

EFI_STATUS PrintMemoryMap(void){
	EFI_STATUS err;
	EFI_MEMORY_DESCRIPTOR * map;
	UINTN size;
	UINTN key;
	UINTN dsize;
	UINT32 dversion;
	err = GetMemoryMap (&map , &size , &key , &dsize , &dversion);
	if (err != EFI_SUCCESS)
		return err;
	PrintMemoryStatus(size,key,dsize,dversion);
	EFI_MEMORY_DESCRIPTOR * pointer = map;
	int i = 0;

	while((void *) pointer < (void *) map + size){
		PrintMappingStatus(i , pointer);
		pointer = (void *) pointer + dsize;
		++i;
	}
	return EFI_SUCCESS;
}

EFI_STATUS GetMemoryMap(EFI_MEMORY_DESCRIPTOR ** map , UINTN * size , UINTN * key , UINTN * dsize , UINT32 * dversion ){
	*size = sizeof(**map) * 10;
	EFI_STATUS err;
	for(;;){
		*size += sizeof(**map);
		err = uefi_call_wrapper(BS->AllocatePool , 3 , EfiLoaderData ,* size , (void **) map );
		if (err != EFI_SUCCESS){
			HandleAllocatePoolError(err);
			return err;
		}
		err = uefi_call_wrapper(BS->GetMemoryMap , 5 , size , *map , key,dsize,dversion);
		if (err != EFI_SUCCESS){
			HandleGetMemoryMapError(err);
			if (err != EFI_BUFFER_TOO_SMALL ) return err;
			uefi_call_wrapper(BS->FreePool, 1, (void *)*map);	
		}
		else{
			return err;
		}
	}
}

void HandleAllocatePoolError(EFI_STATUS err){
	Print(L"Allocation error: ");
	if (err == EFI_OUT_OF_RESOURCES){
		Print(L"The pool requested could not be allocated.");
	}
	else if (err == EFI_INVALID_PARAMETER){
		Print(L"PoolType was invalid.");
	}
	else{
		Print(L"Buffer was NULL.");
	}
}
void HandleGetMemoryMapError(EFI_STATUS err){
	Print(L"Get Memory Map error: ");
	if (err == EFI_BUFFER_TOO_SMALL){
		Print(L"The MemoryMap buffer was too small.");
	}
	Print(L"MemoryMapSize is NULL or The MemoryMap buffer is not too small and MemoryMap is NULL."
	);
}
void PrintMemoryStatus(UINTN size , UINTN key , UINTN dsize , UINT32 dversion){
	Print(L"MemoryMapSize: %d\n",size);
	Print(L"MapKey: %d\n",key);
	Print(L"DescriptorSize: %d\n",dsize);
	Print(L"DescriptorVersion : %d\n",dversion);
	Print(L"-------------------------------------------------------------\n");
}
