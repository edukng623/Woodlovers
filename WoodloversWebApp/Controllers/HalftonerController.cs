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
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

using System.Web.Mvc;
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

                //// This illustrates how to get the file names.
                foreach (MultipartFileData file in provider.FileData)
                {
                    Trace.WriteLine(file.Headers.ContentDisposition.FileName);
                    Trace.WriteLine("Server file path: " + file.LocalFileName);
                    var bytes = File.ReadAllBytes(file.LocalFileName);
                    using (var ms = new MemoryStream(bytes))
                    {
                        Bitmap bitmap = new Bitmap(ms);
                        string appdatafolder = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, "App_Data");
                        string path = Path.Combine(appdatafolder, "Halftoner.cfg");
                        Bitmap half = new Halftoner.MainForm(path).Convert(bitmap);
                        half.Save(root + @"\test.jpeg");

                        //return Request.CreateResponse(HttpStatusCode.OK, half, "image/png");
                        Image img = half;
                        using (MemoryStream outStream = new MemoryStream())
                        {
                            img.Save(outStream, System.Drawing.Imaging.ImageFormat.Png);

                            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                            string base64String = Convert.ToBase64String(outStream.ToArray());
                            result.Content = new StringContent(base64String);
                            //result.Content.Headers.ContentType = new MediaTypeHeaderValue("image/png");

                            return result;
                        }
                    }
                }
                
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
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

                //// This illustrates how to get the file names.
                foreach (MultipartFileData file in provider.FileData)
                {
                    Trace.WriteLine(file.Headers.ContentDisposition.FileName);
                    Trace.WriteLine("Server file path: " + file.LocalFileName);
                    
                }



                return Request.CreateResponse(HttpStatusCode.OK, "Good");
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
