using System.Collections.Generic;
using System.Linq;
using IRunes.App.Extensions;
using IRunes.Data;
using IRunes.Models;
using Microsoft.EntityFrameworkCore;
using SIS.HTTP.Requests;
using SIS.HTTP.Responses;
using SIS.MvcFramework;
using SIS.MvcFramework.Attributes;
using SIS.MvcFramework.Attributes.Action;
using SIS.MvcFramework.Attributes.Http;
using SIS.MvcFramework.Attributes.Security;
using SIS.MvcFramework.Result;

namespace IRunes.App.Controllers
{
    public class AlbumsController : Controller
    {
        
        [Authorize]
        public ActionResult All()
        {
            //this was replaced by authorized!
            //if (!this.IsLoggedIn())
            //{
            //    return this.Redirect("/Users/Login");
            //}

            using (var context = new RunesDbContext())
            {
                ICollection<Album> allAlbums = context.Albums.ToList();

                if (allAlbums.Count == 0)
                {
                    this.ViewData["Albums"] = "There are currently no albums.";
                }
                else
                {
                    this.ViewData["Albums"] =
                        string.Join(string.Empty, 
                        allAlbums.Select(album => album.ToHtmlAll()).ToList());
                }

                return this.View();
            }
        }


        [Authorize]
        public ActionResult Create()
        {
            //this was replaced by authorized!
            //if (!this.IsLoggedIn())
            //{
            //    return this.Redirect("/Users/Login");
            //}

            return this.View();
        }


        
        [HttpPost(ActionName ="Create")]
        [Authorize]
        public ActionResult CreateConfirm()
        {
            //this was replaced by authorized!
            //if (!this.IsLoggedIn())
            //{
            //    return this.Redirect("/Users/Login");
            //}

            using (var context = new RunesDbContext())
            {
                string name = ((ISet<string>)this.Request.FormData["name"]).FirstOrDefault();
                string cover = ((ISet<string>)this.Request.FormData["cover"]).FirstOrDefault();

                Album album = new Album
                {
                    Name = name,
                    Cover = cover,
                    Price = 0M
                };

                context.Albums.Add(album);
                context.SaveChanges();
            }

            return this.Redirect("/Albums/All");
        }


        [Authorize]
        public ActionResult Details()
        {

            //if (!this.IsLoggedIn())
            //{
            //    return this.Redirect("/Users/Login");
            //}

            string albumId = this.Request.QueryData["id"].ToString();

            using (var context = new RunesDbContext())
            {
                Album albumFromDb = context.Albums
                    .Include(album => album.Tracks)
                    .SingleOrDefault(album => album.Id == albumId);

                if (albumFromDb == null)
                {
                    return this.Redirect("/Albums/All");
                }

                this.ViewData["Album"] = albumFromDb.ToHtmlDetails();
                return this.View();
            }
        }

        [NonAction]
        public override string ToString()
        {
            return base.ToString();
        }
    }
}
