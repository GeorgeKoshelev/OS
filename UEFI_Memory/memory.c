//Author George Koshelev : george.koshelev@hackerdom.ru

#include <efi.h>
#include <efilib.h>
#include <stdio.h>

typedef char * string;

const string GetMemoryTypeDisplayValue(UINT32 MemoryType);
EFI_STATUS PrintMemoryMap(void);
EFI_STATUS GetMemoryMap(EFI_MEMORY_DESCRIPTOR ** map , UINTN * size , UINTN * key , UINTN * dsize , UINT32 * dversion );
void HandleAllocatePoolError(EFI_STATUS err);
void HandleGetMemoryMapError(EFI_STATUS err);

const string DisplayMemoryTypes[] = {
    "EfiReservedMemoryType",
    "EfiLoaderCode",
    "EfiLoaderData",
    "EfiBootServicesCode",
    "EfiBootServicesData",
    "EfiRuntimeServicesCode",
    "EfiRuntimeServicesData",
    "EfiConventionalMemory",
    "EfiUnusableMemory",
    "EfiACPIReclaimMemory",
    "EfiACPIMemoryNVS",
    "EfiMemoryMappedIO",
    "EfiMemoryMappedIOPortSpace",
    "EfiPalCode",
};

EFI_STATUS efi_main(EFI_HANDLE ImageHandle , EFI_SYSTEM_TABLE * SystemTable){
	InitializeLib(ImageHandle , SystemTable);
	return PrintMemoryMap();
}

const string GetMemoryTypeDisplayValue(UINT32 MemoryType){
	return (MemoryType > sizeof(DisplayMemoryTypes)) ?
		 "Error: Unknown Memory Type" : DisplayMemoryTypes[MemoryType]; 
		
}

EFI_STATUS PrintMemoryMap(void){
	
	return EFI_SUCCESS;
}

EFI_STATUS GetMemoryMap(EFI_MEMORY_DESCRIPTOR ** map , UINTN * size , UINTN * key , UINTN * dsize , UINT32 * dversion ){
	*size = 0;
	EFI_STATUS err;
	for(;;){
		*size += sizeof(**map) * 10;
		err = uefi_call_wrapper(BS->AllocatePool , 3 , EfiLoaderData ,* size , (void **) map );
		if (err != EFI_SUCCESS){
			HandleAllocatePoolError(err);
			return err;
		}
		err = uefi_call_wrapper(BS->GetMemoryMap , 5 , size , *map , key,dsize,dversion);
		if (err!= EFI_SUCCESS){
			HandleGetMemoryMapError(err);
			if (err != EFI_BUFFER_TOO_SMALL ) return err;
			uefi_call_wrapper(BS->FreePool, 1, (void *)*map);	
		}
		else{
			return err;
		}
	}
	return EFI_SUCCESS;
}

void HandleAllocatePoolError(EFI_STATUS err){
	printf("Allocation error: ");
	if (err == EFI_OUT_OF_RESOURCES){
		printf ("The pool requested could not be allocated.");
	}
	else if (err == EFI_INVALID_PARAMETER){
		printf ("PoolType was invalid.");
	}
	else{
		printf("Buffer was NULL.");
	}
}
void HandleGetMemoryMapError(EFI_STATUS err){
	printf("Get Memory Map error: ");
	if (err == EFI_BUFFER_TOO_SMALL){
		printf("The MemoryMap buffer was too small.");
	}
	printf("MemoryMapSize is NULL or The MemoryMap buffer is not too small and MemoryMap is NULL."
	);
}
