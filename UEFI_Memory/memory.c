//Author George Koshelev : george.koshelev@hackerdom.ru

#include <efi.h>
#include <efilib.h>

typedef CHAR16 * string;

const string GetMemoryTypeDisplayValue(UINT32 MemoryType);
EFI_STATUS PrintMemoryMap(void);
EFI_STATUS GetMemoryMap(EFI_MEMORY_DESCRIPTOR ** map , UINTN * size , UINTN * key , UINTN * dsize , UINT32 * dversion );
void HandleAllocatePoolError(EFI_STATUS err);
void HandleGetMemoryMapError(EFI_STATUS err);

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

EFI_STATUS PrintMemoryMap(void){
	EFI_STATUS err;
	EFI_MEMORY_DESCRIPTOR * map;
	UINTN size;
	UINTN key;
	UINTN dsize;
	UINT32 dversion;
	UINT64 MemoryArray[14] = {0};
	UINT64 TotalMemory = 0;
	err = GetMemoryMap (&map , &size , &key , &dsize , &dversion);
	if (err != EFI_SUCCESS)
		return err;
	EFI_MEMORY_DESCRIPTOR * pointer = map;
	int i = 0;
	UINT64 temp;
	while((void *) pointer < (void *) map + size){
		temp = pointer->NumberOfPages*4096;
		switch(pointer->Type){
			case EfiReservedMemoryType:
				MemoryArray[0] += temp;
				break;
			case EfiLoaderCode:
				MemoryArray[1] +=temp;
				break;
			case EfiLoaderData:
				MemoryArray[2] += temp;
				break;
			case EfiBootServicesCode:
				MemoryArray[3] += temp;
				break;
			case EfiBootServicesData:
				MemoryArray[4] += temp;
				break;
			case EfiRuntimeServicesCode:
				MemoryArray[5] += temp;
				break;
			case EfiRuntimeServicesData:
				MemoryArray[6] += temp;
				break;
			case EfiConventionalMemory:
				MemoryArray[7] += temp;
				break;
			case EfiUnusableMemory:
				MemoryArray[8] += temp;
				break;
			case EfiACPIReclaimMemory:
				MemoryArray[9] += temp;
				break;
			case EfiACPIMemoryNVS:
				MemoryArray[10] += temp;
				break;
			case EfiMemoryMappedIO:
				MemoryArray[11] += temp;
				break;
			case EfiMemoryMappedIOPortSpace:
				MemoryArray[12] += temp;
				break;
			case EfiPalCode:
				MemoryArray[13] += temp;
				break;
		}
		TotalMemory += temp;	
		pointer = (void *) pointer + dsize;
		++i;
	}
	
	for(i = 0 ; i < 14 ; i++){
		Print(L"%s : %d\n" , DisplayMemoryTypes[i] , MemoryArray[i] );
	}
	Print(L"Total Memory : %d\n" , TotalMemory );

	uefi_call_wrapper(BS->FreePool , 1 , map);

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
		Print(L"The pool requested could not be allocated.\n");
	}
	else if (err == EFI_INVALID_PARAMETER){
		Print(L"PoolType was invalid.\n");
	}
	else{
		Print(L"Buffer was NULL.\n");
	}
}
void HandleGetMemoryMapError(EFI_STATUS err){
	Print(L"Get Memory Map error: ");
	if (err == EFI_BUFFER_TOO_SMALL){
		Print(L"The MemoryMap buffer was too small.\n");
	}
	else{
	Print(L"MemoryMapSize is NULL or The MemoryMap buffer is not too small and MemoryMap is NULL.");
	}
}
