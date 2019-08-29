using System.Collections.Generic;
using System.Linq;
using IRunes.App.Extensions;
using IRunes.Data;
using IRunes.Models;
using IRunes.Services;
using SIS.HTTP.Requests;
using SIS.HTTP.Responses;
using SIS.MvcFramework;
using SIS.MvcFramework.Attributes;
using SIS.MvcFramework.Attributes.Http;
using SIS.MvcFramework.Attributes.Security;
using SIS.MvcFramework.Result;

namespace IRunes.App.Controllers
{
    public class TracksController : Controller
    {

        private readonly TrackService trackService;
        private readonly AlbumService albumService;

        public TracksController()
        {
            this.trackService = new TrackService();
            this.albumService = new AlbumService();
        }

        [Authorize]
        public ActionResult Create()
        {
            string albumId = this.Request.QueryData["albumId"].ToString();

            this.ViewData["AlbumId"] = albumId;
            return this.View();
        }


        [HttpPost(ActionName = "Create")]
        [Authorize]
        public ActionResult CreateConfirm()
        {
            string albumId = this.Request.QueryData["albumId"].ToString();
            Album albumFromDb = albumService.GetAlbumById(albumId);

            if (albumFromDb == null)
            {
                return this.Redirect("/Albums/All");
            }

            string name = ((ISet<string>)this.Request.FormData["name"]).FirstOrDefault();
            string link = ((ISet<string>)this.Request.FormData["link"]).FirstOrDefault();
            string price = ((ISet<string>)this.Request.FormData["price"]).FirstOrDefault();

            Track trackForDb = new Track
            {
                Name = name,
                Link = link,
                Price = decimal.Parse(price)
            };

            //the bool is redundant , because we `ve already checked above if the album is there or not.
            //but.. we`ll have unit tests so... i guess
            if (albumService.AddTrackToAlbum(albumId, trackForDb))
            {
                return this.Redirect("/Albums/All");
            }

            return this.Redirect($"/Albums/Details?id={albumId}");
        }


        [Authorize]
        public ActionResult Details()
        {

            string albumId = this.Request.QueryData["albumId"].ToString();
            string trackId = this.Request.QueryData["trackId"].ToString();


            Track trackFromDb = trackService.GetTrackById(trackId);

            if (trackFromDb == null)
            {
                return this.Redirect($"/Albums/Details?id={albumId}");
            }

            this.ViewData["AlbumId"] = albumId;
            this.ViewData["Track"] = trackFromDb.ToHtmlDetails(albumId);

            return this.View();

        }
    }
}
