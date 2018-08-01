using Amazon.S3;
using Amazon.S3.Transfer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace WoodloversWebApp.Controllers
{
    [EnableCors(origins: "http://www.woodlovers.pt", headers: "*", methods: "*")]
    public class HalftonerController : ApiController
    {
        
        // GET: api/Halftoner
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Halftoner/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Halftoner
        [System.Web.Mvc.HttpPost]
        public async Task<HttpResponseMessage> Edit()
        {
            // CleanUpData();
            // Check if the request contains multipart/form-data.
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            string root = HttpContext.Current.Server.MapPath("~/App_Data");
            var provider = new MultipartFormDataStreamProvider(root);

            try
            {
                // Read the form data.
                await Request.Content.ReadAsMultipartAsync(provider);
                string system = Environment.GetEnvironmentVariable("DEV_SYSTEM");
                string cfgFile = provider.FormData["settings-file"];
                if (!String.IsNullOrEmpty(system))
                {
                    cfgFile = "dev_halftoner.cfg";
                }

                //// This illustrates how to get the file names.
                foreach (MultipartFileData file in provider.FileData)
                {
                    
                    var client = new AmazonS3Client(Amazon.RegionEndpoint.USEast1);
                    var transferUtility = new TransferUtility(new AmazonS3Client(Amazon.RegionEndpoint.USEast1));

                    var bytes = File.ReadAllBytes(file.LocalFileName);
                    using (var ms = new MemoryStream(bytes))
                    using (Bitmap bitmap = new Bitmap(ms))
                    using (var fileStream = transferUtility.OpenStream("woodlovers.orders", cfgFile) )
                    {
                        using (Image img = (new Halftoner.MainForm("Not used for now", fileStream)).Convert(bitmap))
                        using (MemoryStream outStream = new MemoryStream())
                        {
                            img.Save(outStream, System.Drawing.Imaging.ImageFormat.Png);

                            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                            string base64String = Convert.ToBase64String(outStream.ToArray());
                            result.Content = new StringContent(base64String);

                            return result;
                        }
                    }
                }
                
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }
        private void CleanUpData()
        {
            string root = HttpContext.Current.Server.MapPath("~/App_Data");
            CleanTempFiles(root, 3);
        }
        private void CleanTempFiles(string dir, int ageInMinutes)
        {
            string[] files = Directory.GetFiles(dir);

            foreach (string file in files)
            {
                var time = File.GetCreationTime(file);

                if (time.AddMinutes(ageInMinutes) < DateTime.Now)
                {
                    File.Delete(file);
                }
            }
        }

        [System.Web.Mvc.HttpPost]
        public async Task<HttpResponseMessage> PostToS3()
        {
            // CleanUpData();
            // Check if the request contains multipart/form-data.
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            string root = HttpContext.Current.Server.MapPath("~/App_Data");
            var provider = new MultipartFormDataStreamProvider(root);
            
            try
            {
                // Read the form data.
                await Request.Content.ReadAsMultipartAsync(provider);
                String name = provider.FormData["name"];
                if (String.IsNullOrEmpty(name))
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "File is empty or not exists");
                }
                //// This illustrates how to get the file names.
                foreach (MultipartFileData file in provider.FileData)
                {
                    Trace.WriteLine(file.Headers.ContentDisposition.FileName);
                    Trace.WriteLine("Server file path: " + file.LocalFileName);
                    var client = new AmazonS3Client(Amazon.RegionEndpoint.USEast1);
                    var transferUtility = new TransferUtility(client);
                    
                    var permission = S3CannedACL.AuthenticatedRead;
                    if (name.StartsWith("tmp/"))
                    {
                        permission = S3CannedACL.PublicRead;
                    }
                    var uploadRequest = new TransferUtilityUploadRequest();
                    uploadRequest.FilePath = file.LocalFileName;
                    uploadRequest.BucketName = "woodlovers.orders";
                    uploadRequest.Key = name;
                    uploadRequest.CannedACL = permission;
                        
                    await transferUtility.UploadAsync(uploadRequest);
                    
                    return Request.CreateResponse(HttpStatusCode.OK, "Good");
                    
                }


                return Request.CreateResponse(HttpStatusCode.BadRequest, "File could not be saved or no file at all");

            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message);
            }

            
        }
        [System.Web.Mvc.HttpPost]
        public async Task<HttpResponseMessage> PostOrder()
        {
            string root = HttpContext.Current.Server.MapPath("~/App_Data");
            var provider = new MultipartFormDataStreamProvider(root);
            try
            {
                // Read the form data.
                await Request.Content.ReadAsMultipartAsync(provider);
                String orderNumber = provider.FormData["order"];
                String files = provider.FormData["files"];
                String[] fileIds = files.Split('@');
                var client = new AmazonS3Client(Amazon.RegionEndpoint.USEast1);
                // var transferUtility = new TransferUtility(client);

                foreach (var id in fileIds)
                {
                    String tempName = id;
                    if (id.Contains("_"))
                    {
                        tempName = id.Split('_')[0];
                    }
                    await client.CopyObjectAsync("woodlovers.orders", "tmp/" + tempName, "woodlovers.orders", "/orders/" + orderNumber + "/" + id);
                }
                return Request.CreateResponse(HttpStatusCode.OK, "Posted Order");
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }
        // PUT: api/Halftoner/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Halftoner/5
        public void Delete(int id)
        {
        }
    }
}
