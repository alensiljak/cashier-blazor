/*
 * OPFS Helper
 */

using KristofferStrube.Blazor.FileSystem;

namespace Cashier.Lib
{
    public class OpfsService
    {
        private IStorageManagerService _storageManagerService;

        public OpfsService(IStorageManagerService storageManagerService) {
            _storageManagerService = storageManagerService;
        }

        public async Task DeleteFile(string fileName)
        {
            var dir = await _storageManagerService.GetOriginPrivateDirectoryAsync();
            // var options = new FileSystemRemoveOptions
            await dir.RemoveEntryAsync(fileName);
        }

        public async Task<KristofferStrube.Blazor.FileAPI.File?> OpenFile(string fileName, bool create = true)
        {
            FileSystemDirectoryHandle directoryHandle = await _storageManagerService.GetOriginPrivateDirectoryAsync();
            FileSystemFileHandle fileHandle = await directoryHandle.GetFileHandleAsync(fileName, new() { Create = create });

            if (fileHandle == null)
            {
                return null;
            }

            var file = await fileHandle.GetFileAsync();

            return file;
        }

        public async Task<FileSystemWritableFileStream> OpenWritable(string fileName, bool create = true)
        {
            FileSystemDirectoryHandle directoryHandle = await _storageManagerService.GetOriginPrivateDirectoryAsync();
            FileSystemFileHandle fileHandle = await directoryHandle.GetFileHandleAsync(fileName, new() { Create = create });

            var stream = await fileHandle.CreateWritableAsync();

            return stream;
        }

    }
}
