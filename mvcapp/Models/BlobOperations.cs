using System;
using System.Web;

using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Configuration;
using System.Threading.Tasks;
using System.IO;


namespace mvcapp.Models
{

    /// <summary>
    /// Class to Store BLOB Info
    /// </summary>
    
    
    
    /// <summary>
    /// Class to Work with Blob
    /// </summary>
    public class BlobOperations
    {
        private static CloudBlobContainer profileBlobContainer;
        
        /// <summary>
        /// Initialize BLOB and Queue Here
        /// </summary>
        public BlobOperations()
        {
            var storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["webjobstorage"].ToString());

            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Get the blob container reference.
            profileBlobContainer = blobClient.GetContainerReference("profiles");
            //Create Blob Container if not exist
            profileBlobContainer.CreateIfNotExists();
         }

    
        /// <summary>
        /// Method to Upload the BLOB
        /// </summary>
        /// <param name="profileFile"></param>
        /// <returns></returns>
        public async Task<CloudBlockBlob> UploadBlob(HttpPostedFileBase profileFile)
        {
            string blobName = Guid.NewGuid().ToString() + Path.GetExtension(profileFile.FileName);
            // GET a blob reference. 
            CloudBlockBlob profileBlob = profileBlobContainer.GetBlockBlobReference(blobName);
            // Uploading a local file and Create the blob.
            using (var fs = profileFile.InputStream)
            {
                await profileBlob.UploadFromStreamAsync(fs);
            }
            return profileBlob;
        }

    }
}