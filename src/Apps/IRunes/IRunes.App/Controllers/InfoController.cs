
namespace IRunes.App.Controllers
{
    using SIS.HTTP.Requests;
    using SIS.HTTP.Responses;
    using SIS.MvcFramework;
    using SIS.MvcFramework.Attributes.Action;
    using SIS.MvcFramework.Result;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class InfoController :Controller
    {

        public ActionResult Json()
        {
            return Json(new { });
        }

        public ActionResult File()
        {
            //here we read the location
            string folderPrefix = "/../";
            string assemblyLocation = this.GetType().Assembly.Location;
            string resourceFolderPath = "Resources/";
            string requestedResource = this.Request.QueryData["file"].ToString();

            string fullPathToResource = assemblyLocation + folderPrefix + resourceFolderPath + requestedResource;

            if (System.IO.File.Exists(fullPathToResource))
            {
                //if it exists, then it is taken as bytes and given to the Result (which gives proper headers, so the 
                //server knows what it is and etc...
                // TODO: Students, Do this!!!
                string mimeType = null;
                string fileName = null;

                byte[] content = System.IO.File.ReadAllBytes(fullPathToResource);
                return File(content);
            }

            return this.NotFound();
        }

        public ActionResult About()
        {
            return this.View();
        }
    }
}
