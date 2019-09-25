

namespace Stopify.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;
    using CloudinaryDotNet;
    using CloudinaryDotNet.Actions;
    using Microsoft.AspNetCore.Http;

    public class CloudinaryService : ICloudinaryService
    {

        private readonly Cloudinary cloudinaryUtility;

        public CloudinaryService(Cloudinary cloudinary)
        {
            this.cloudinaryUtility = cloudinary;
        }
        //with memory stream 
        public async Task<string> UploadPictureAsync(IFormFile pictureFile, string fileName)
        {
            byte[] destinationData;

            using (var ms = new MemoryStream())
            {
                await pictureFile.CopyToAsync(ms);
                destinationData = ms.ToArray();
            }

            UploadResult uploadResult = null;

            using (var ms = new MemoryStream(destinationData))
            {
                ImageUploadParams uploadParams = new ImageUploadParams
                {
                    Folder = "test_folder",
                    File = new FileDescription(fileName, ms)
                };

                uploadResult = this.cloudinaryUtility.Upload(uploadParams);
            }

            return uploadResult?.SecureUri.AbsoluteUri;
        }



        //without stream , only with pathFile 
        public async Task<string> UploadPictureAsync(IFormFile file)
        {

            //can work with link directly (if you just put in a link!) 
            //var tempPath = @"https://cdn.shopify.com/s/files/1/2660/5202/products/j0hzrhyh0ccntu3xrwtq_76a6ba3e-c96f-4ef0-aae8-241ca466bae1_1400x.jpg?v=1537306439";

            //DRAWBACK: file needs to be in BIN. folder + always copied!
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription("cool", file.FileName),
                Folder = "test_folder"
            };

            var uploadResult = this.cloudinaryUtility.Upload(uploadParams);

            return uploadResult?.Uri.AbsoluteUri;

        }
    }
}
