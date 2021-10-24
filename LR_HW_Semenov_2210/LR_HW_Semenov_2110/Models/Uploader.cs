using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Azure;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace LR_HW_Semenov_2110.Models
{
    public class Uploader
    {
        private string containerName;
        private readonly BlobServiceClient blobService;
        public Uploader(BlobServiceClient  client)
        {
            this.containerName = "home";
            blobService = client;
        }

        public async Task<bool> AddImage(string filename, byte[] data)
        {
            BlobContainerClient container =blobService.GetBlobContainerClient(containerName);
            await container.CreateIfNotExistsAsync(PublicAccessType.Blob);
            BlobClient blob = container.GetBlobClient(filename);
            var res = await blob.UploadAsync(BinaryData.FromBytes(data));
            if(res.GetRawResponse().Status == 201)
            {
                return true;
            }
            return false;
        }

        public async Task<List<Tuple<string, string>>> GetImages()
        {
            List<Tuple<string, string>> list = new List<Tuple<string, string>>();
            BlobContainerClient container = blobService.GetBlobContainerClient(containerName);
            await foreach (var item in container.GetBlobsAsync())
            {
                BlobClient blob = container.GetBlobClient(item.Name);
                list.Add(new Tuple<string, string>(blob.Name, blob.Uri.AbsoluteUri));
            }
            return list;
        }
    }
}
