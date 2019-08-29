

using System.Linq;
using IRunes.Data;
using IRunes.Models;
using Microsoft.EntityFrameworkCore;

namespace IRunes.Services
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    public class AlbumService : IAlbumService
    {
        private readonly RunesDbContext context;

        public AlbumService()
        {
            this.context = new RunesDbContext();
        }

        public Album CreateAlbum(Album album)
        {

            //note: interesting! It returns the object itself.
            album = this.context.Add(album).Entity;
            this.context.SaveChanges();
            return album;
        }

        public bool AddTrackToAlbum(string albumId, Track trackFromDb)
        {
            Album albumFromDb = this.GetAlbumById(albumId);

            if (albumFromDb is null)
            {
                return false;
            }

            //we add the track
            albumFromDb.Tracks.Add(trackFromDb);
            //afterwards we correct the price of the album
            albumFromDb.Price = (albumFromDb.Tracks
                                     .Select(track => track.Price)
                                     .Sum() * 87) / 100;

            context.Update(albumFromDb);
            context.SaveChanges();

            return true;
        }

        public ICollection<Album> GetAllAlbums()
        {
            return this.
                context
                .Albums
                .ToList();
        }

        public Album GetAlbumById(string albumId)
        {
            return context.Albums
                .Include(album => album.Tracks)
                .SingleOrDefault(album => album.Id == albumId);
        }
    }
}
