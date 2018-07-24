using Amazon.S3;
using Amazon.S3.Transfer;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.ModelBinding;
using System.Web;
using Amazon.S3.Model;

namespace WoodloversWebApp.Controllers
{
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
                string cfgFile = "Halftoner.cfg";
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
        [System.Web.Mvc.HttpPost]
        public async Task<HttpResponseMessage> PostToS3()
        {
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

                    var transferUtility = new TransferUtility(new AmazonS3Client(Amazon.RegionEndpoint.USEast1));
                    
                    using (FileStream fs = new FileStream(file.LocalFileName, FileMode.Open, FileAccess.Read))
                    {
                        //var name = Request.GetQueryNameValuePairs().
                        await transferUtility.UploadAsync(fs, "woodlovers.orders", name);
                        return Request.CreateResponse(HttpStatusCode.OK, "Good");
                    }
                }


                return Request.CreateResponse(HttpStatusCode.BadRequest, "File could not be saved or no file at all");

            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
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
