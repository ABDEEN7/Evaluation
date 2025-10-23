using Azure;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Exceptions;
using Evaluation.SharedHelper.Helper;
using Evaluation.SharedHelper.Models.Api.FormBuilderDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Azure.Storage.Sas;


namespace Evaluation.Services.Special
{
    public class AzureBlobStorageService
    {

        private readonly BlobServiceClient _blobServiceClient = new(ClsAppSetting.AzureBlobConnectionString);
        private readonly StorageTransferOptions uploadTrasnsferOption = default;
        private readonly FileExtensionContentTypeProvider _contentTypeProvider = new();

        public async Task StreamFileAsync(string blobName, Stream destinationStream, StorageContainerType? storageContainerType = StorageContainerType.evaluation)
        {
            var blobContainerClient = _blobServiceClient.GetBlobContainerClient(storageContainerType!.Value.ToString());
            string fileExtension = Path.GetExtension(blobName);
            string newFileName = $"{Guid.NewGuid()}{fileExtension}";

            var blobClient = blobContainerClient.GetBlobClient(newFileName);

            var response = await blobClient.DownloadAsync();
            await response.Value.Content.CopyToAsync(destinationStream);
        }
        public async Task<FileVm> UploadFileAsync(IFormFile file, StorageContainerType? storageContainerType = StorageContainerType.evaluation)
        {
            var blobContainerClient = _blobServiceClient.GetBlobContainerClient(storageContainerType!.Value.ToString());

            // Generate a new filename for the blob
            string fileExtension = Path.GetExtension(file.FileName);
            string newFileName = $"{Guid.NewGuid()}{fileExtension}";

            var blobClient = blobContainerClient.GetBlobClient(newFileName);

            await blobClient.UploadAsync(file.OpenReadStream(), true);
            FileVm response = new FileVm
            {
                CustomFileName = newFileName,
                FileName = file.FileName,
                FileType = fileExtension,
                FileLength = file.Length
            };
            return response;
        }

        public async Task<FileVm> UploadFileAsync(MemoryStream file, string fileName, StorageContainerType? storageContainerType = StorageContainerType.evaluation)
        {
            file.Seek(0, SeekOrigin.Begin);
            var blobContainerClient = _blobServiceClient.GetBlobContainerClient(storageContainerType!.Value.ToString());

            // Generate a new filename for the blob
            string fileExtension = Path.GetExtension(fileName);
            string newFileName = $"{Guid.NewGuid()}{fileExtension}";

            var blobClient = blobContainerClient.GetBlobClient(newFileName);

            await blobClient.UploadAsync(file, true);
            FileVm response = new FileVm
            {
                CustomFileName = newFileName,
                FileName = fileName,
                FileType = fileExtension,
                FileLength = file.Length
            };
            return response;
        }
        public BlobClient GetBlobClientWithSas(string name, StorageContainerType? storageContainerType = StorageContainerType.evaluation)
        {
            var blobUri = new Uri($@"{ClsAppSetting.BlobEndUrl}/{storageContainerType!.Value.ToString()}/{name}");

            var blobUriBuilder = new UriBuilder(blobUri)
            {
                Query = ClsAppSetting.BlobSasToken
            };

            var authorizedBlobUri = blobUriBuilder.Uri;
            return new BlobClient(authorizedBlobUri);
        }


        public async Task<Response<BlobContentInfo>> UploadStreamBlobAsyncWithSas(Stream stream, string fileName)
        {
            var blobClient = GetBlobClientWithSas(fileName);

            stream.Position = 0;
            var response = await blobClient.UploadAsync(content: stream,
                     httpHeaders: new BlobHttpHeaders { ContentType = GetContentType(fileName) },
                    transferOptions: this.uploadTrasnsferOption);
            return response;
        }

        public async Task<FileVm> UploadFormFileAsync(FileFieldDTO uploadFile, StorageContainerType? storageContainerType = StorageContainerType.evaluation)
        {
            if (null == uploadFile)
                throw new BusinessException("File not found");

            string fileName = uploadFile.File.FileName;
            string fileExtension = Path.GetExtension(fileName);
            string customFileName = Guid.NewGuid().ToString() + fileExtension;
            FileVm response = new FileVm
            {
                CustomFileName = customFileName,
                File = uploadFile.File,
                FileName = fileName,
                FileType = fileExtension,
                FileLength = uploadFile.File.Length,
                FieldId = uploadFile.FieldId,
            };

            using (var memoryStream = new MemoryStream())
            {
                await uploadFile.File.CopyToAsync(memoryStream);

                memoryStream.Seek(0, SeekOrigin.Begin);
                var blobContainerClient = _blobServiceClient.GetBlobContainerClient(storageContainerType!.Value.ToString());


                var blobClient = blobContainerClient.GetBlobClient(customFileName);

                await blobClient.UploadAsync(memoryStream, true);

                return response;
            }
        }

        public async Task<FileVm> UploadFormFileAsync(IFormFile uploadFile, StorageContainerType? storageContainerType = StorageContainerType.evaluation)
        {
            if (null == uploadFile)
                throw new BusinessException("File not found");

            string fileName = uploadFile.FileName;
            string fileExtension = Path.GetExtension(fileName);
            string customFileName = Guid.NewGuid().ToString() + fileExtension;
            FileVm response = new FileVm
            {
                CustomFileName = customFileName,
                File = uploadFile,
                FileName = fileName,
                FileType = fileExtension,
                FileLength = uploadFile.Length,
                //FieldId = uploadFile.FieldId,
            };

            using (var memoryStream = new MemoryStream())
            {
                await uploadFile.CopyToAsync(memoryStream);

                memoryStream.Seek(0, SeekOrigin.Begin);
                var blobContainerClient = _blobServiceClient.GetBlobContainerClient(storageContainerType!.Value.ToString());


                var blobClient = blobContainerClient.GetBlobClient(customFileName);

                await blobClient.UploadAsync(memoryStream, true);

                return response;
            }
        }


        public async Task<List<FileVm>> UploadFormFilesAsync(IEnumerable<FileFieldDTO> uploads, StorageContainerType? storageContainerType = StorageContainerType.evaluation)
        {
            var result = new List<FileVm>();

            foreach (var item in uploads)
            {
                try
                {
                    if (item.File != null && item.File.Length > 0)
                    {
                        result.Add(await UploadFormFileAsync(item));
                    }
                    else if (item.FileBytes != null && item.FileBytes.Length > 0)
                    {
                        // Handle base64-origin file (converted to byte[])
                        using var stream = new MemoryStream(item.FileBytes);
                        var blobContainerClient = _blobServiceClient.GetBlobContainerClient(storageContainerType!.Value.ToString());

                        string fileExtension = Path.GetExtension(item.FileName);
                        string newFileName = $"{Guid.NewGuid()}{fileExtension}";

                        var blobClient = blobContainerClient.GetBlobClient(newFileName);
                        await blobClient.UploadAsync(stream, true);

                        result.Add(new FileVm
                        {
                            CustomFileName = newFileName,
                            FileName = item.FileName,
                            FileType = fileExtension,
                            FileLength = item.FileBytes.Length,
                            FieldId = item.FieldId,
                        });
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"File upload error: {ex.Message}");
                }
            }

            return result;
        }

        public async Task<List<FileVm>> UploadFormFilesAsync(IEnumerable<IFormFile> uploads)
        {
            var uploadTasks = new Queue<Task<FileVm>>();
            foreach (var item in uploads)
            {
                try
                {
                    if (null != item && item.FileName.Length > 0)
                    {
                        uploadTasks.Enqueue(this.UploadFormFileAsync(item));
                    }
                }
                catch (Exception iex)
                {
                    Console.WriteLine(iex.Message);
                }
            }
            var response = await Task.WhenAll(uploadTasks);

            return response.ToList();
        }

        public string GenerateSasToken(string blobName, int mins = 2, string? uiFileName = null, bool isDownload = false, StorageContainerType? storageContainerType = StorageContainerType.evaluation)
        {
            var blobContainerClient = _blobServiceClient.GetBlobContainerClient(storageContainerType!.Value.ToString());
            BlobClient blobClient;


            blobClient = blobContainerClient.GetBlobClient(blobName);

            if (!isDownload)
            {
                var blobHttpHeaders = new BlobHttpHeaders();
                blobClient.SetHttpHeaders(blobHttpHeaders);
            }
            DateTime ExpiryDate = DateTime.UtcNow.AddMinutes(mins);
            if (mins == 0)
            {
                ExpiryDate = new DateTime(9999, 12, 31);
            }
            var sasBuilder = new BlobSasBuilder
            {
                BlobContainerName = storageContainerType.Value.ToString(),
                BlobName = blobName,
                Resource = "b", // b stands for blob
                StartsOn = DateTimeOffset.UtcNow.AddMinutes(-15),
                ExpiresOn = ExpiryDate,
                IPRange = new SasIPRange(IPAddress.None, IPAddress.None),
            };
            if (isDownload && uiFileName != null)
            {
                sasBuilder.ContentDisposition = $"attachment; filename=\"{uiFileName}\"";
            }
            sasBuilder.SetPermissions(BlobSasPermissions.Read);

            var sasToken = blobClient.GenerateSasUri(sasBuilder);

            return sasToken.ToString();
        }

        private string GetContentType(string fileName)
        {
            if (!_contentTypeProvider.TryGetContentType(fileName, out var contentType))
            {
                contentType = "application/octet-stream";
            }
            return contentType;

        }


        public async Task<List<string>> UploadBlobsAsyncFromBlobUris(bool generateFilesWithNewName, StorageContainerType? storageContainerType = StorageContainerType.evaluation, params string[] sourceBlobUris)
        {
            // ConcurrentQueue to hold tasks for parallel execution
            ConcurrentQueue<Task> uploadTasks = new ConcurrentQueue<Task>();
            List<string> tempFilePaths = new List<string>();
            List<string> processedBlobNames = new List<string>();

            try
            {
                // Process each sourceBlobUri asynchronously
                foreach (string sourceBlobUri in sourceBlobUris)
                {
                    string tempFilePath = Path.GetTempFileName();
                    tempFilePaths.Add(tempFilePath); // Add to list for cleanup later

                    try
                    {
                        // Download the blob
                        BlobClient sourceBlobClient = new BlobClient(new Uri(sourceBlobUri));
                        BlobDownloadInfo download = await sourceBlobClient.DownloadAsync();

                        // Write the downloaded blob content to the temporary file
                        using (FileStream fs = File.OpenWrite(tempFilePath))
                        {
                            await download.Content.CopyToAsync(fs);
                        }

                        // Upload the blob to the same container
                        BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(storageContainerType!.Value.ToString());

                        string destinationBlobName;
                        string fileName = Path.GetFileName(new Uri(sourceBlobUri).AbsolutePath);
                        if (generateFilesWithNewName)
                        {
                            string newFileName = $"{Guid.NewGuid()}{Path.GetExtension(fileName)}";
                            destinationBlobName = newFileName;
                        }
                        else
                        {
                            destinationBlobName = fileName; // Use the source blob name for destination
                        }

                        BlobClient destinationBlobClient = containerClient.GetBlobClient(destinationBlobName);

                        // Check if the blob already exists in the container
                        bool blobExists = await destinationBlobClient.ExistsAsync();

                        if (blobExists)
                        {
                            // Handle conflict: Generate a new unique name or log the conflict
                            Console.WriteLine($"Blob '{destinationBlobName}' already exists in the container.");
                            // For demonstration purpose, adding to processedBlobNames list
                            processedBlobNames.Add(destinationBlobName);
                        }
                        else
                        {
                            // Enqueue the upload task
                            uploadTasks.Enqueue(destinationBlobClient.UploadAsync(tempFilePath, true));

                            // Add to processedBlobNames list
                            processedBlobNames.Add(destinationBlobName);
                        }

                    }
                    catch (Exception ex)
                    {
                        // Handle exceptions as needed
                        Console.WriteLine($"Error occurred: {ex.Message}");
                        // Optionally, continue to the next iteration or handle the error scenario
                    }
                }

                // Wait for all upload tasks to complete
                await Task.WhenAll(uploadTasks);
            }
            finally
            {
                // Cleanup: Delete all temporary files
                foreach (string filePath in tempFilePaths)
                {
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                }
            }

            // Return the list of processed blob names (both uploaded and conflicted)
            return processedBlobNames;
        }
        public async Task<FileVm> UploadStreamWithPathAsync(Stream stream, string blobPath, string fileName, StorageContainerType? storageContainerType = StorageContainerType.evaluation)
        {
            if (stream == null || stream.Length == 0)
                throw new BusinessException("Stream is empty or null");

            // Normalize the path (remove leading/trailing slashes)
            blobPath = blobPath.Trim('/');

            var fileExtension = Path.GetExtension(fileName);
            var newFileName = $"{Guid.NewGuid()}{fileExtension}";

            // Combine path and filename for the final blob name
            var fullBlobName = string.IsNullOrEmpty(blobPath)
                ? newFileName
                : $"{blobPath}/{newFileName}";

            var blobContainerClient = _blobServiceClient.GetBlobContainerClient(storageContainerType!.Value.ToString());
            var blobClient = blobContainerClient.GetBlobClient(fullBlobName);

            // Ensure stream is at beginning
            if (stream.CanSeek)
            {
                stream.Seek(0, SeekOrigin.Begin);
            }

            // Upload the stream
            await blobClient.UploadAsync(stream, true);

            return new FileVm
            {
                CustomFileName = fullBlobName, // Now includes the full path
                FileName = fileName,
                FileType = fileExtension,
                FileLength = stream.Length
            };
        }

        public async Task<Stream> GetFileStreamAsync(string blobName, StorageContainerType? storageContainerType = StorageContainerType.evaluation)
        {
            var blobContainerClient = _blobServiceClient.GetBlobContainerClient(storageContainerType!.Value.ToString());
            var blobClient = blobContainerClient.GetBlobClient(blobName);

            var downloadInfo = await blobClient.DownloadAsync();
            return downloadInfo.Value.Content;
        }

        public async Task<(Stream Stream, string ContentType)> GetFileWithContentTypeAsync(string blobName, StorageContainerType? storageContainerType = StorageContainerType.evaluation)
        {
            var blobContainerClient = _blobServiceClient.GetBlobContainerClient(storageContainerType!.Value.ToString());
            var blobClient = blobContainerClient.GetBlobClient(blobName);

            var downloadInfo = await blobClient.DownloadAsync();
            var properties = await blobClient.GetPropertiesAsync();
            return (downloadInfo.Value.Content, properties.Value.ContentType);
        }

        public async Task<byte[]> DownloadFileBytesAsync(string blobName, StorageContainerType? storageContainerType = StorageContainerType.evaluation)
        {
            if (string.IsNullOrWhiteSpace(blobName))
                throw new BusinessException("Blob name is empty.");

            var blobContainerClient = _blobServiceClient.GetBlobContainerClient(storageContainerType!.Value.ToString());
            var blobClient = blobContainerClient.GetBlobClient(blobName);

            if (!await blobClient.ExistsAsync())
                throw new BusinessException($"Blob '{blobName}' not found in container '{storageContainerType!.Value.ToString()}'.");

            using var ms = new MemoryStream();
            await blobClient.DownloadToAsync(ms);
            return ms.ToArray();
        }

        public async Task<FileVm> UploadBytesAsync(byte[] data, string fileName, string? blobPath = null, StorageContainerType? storageContainerType = StorageContainerType.evaluation)
        {
            if (data == null || data.Length == 0)
                throw new BusinessException("File data is empty.");
            if (string.IsNullOrWhiteSpace(fileName))
                throw new BusinessException("File name is empty.");

            var fileExtension = Path.GetExtension(fileName);
            var newFileName = $"{Guid.NewGuid()}{fileExtension}";

            // If you want a subfolder in blob storage
            var fullBlobName = string.IsNullOrEmpty(blobPath)
                ? newFileName
                : $"{blobPath.TrimEnd('/')}/{newFileName}";

            var blobContainerClient = _blobServiceClient.GetBlobContainerClient(storageContainerType!.Value.ToString());
            var blobClient = blobContainerClient.GetBlobClient(fullBlobName);

            using var ms = new MemoryStream(data);
            await blobClient.UploadAsync(ms, overwrite: true);

            return new FileVm
            {
                CustomFileName = fullBlobName, // blob key
                FileName = fileName,
                FileType = fileExtension,
                FileLength = data.Length
            };
        }

    }
}
