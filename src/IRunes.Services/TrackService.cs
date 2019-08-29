

using IRunes.Data;
using IRunes.Models;

namespace IRunes.Services
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Linq;

    public class TrackService : ITrackService
    {
        private RunesDbContext context;

        public TrackService()
        {
            this.context=new RunesDbContext();
            ;
        }

        public Track GetTrackById(string trackId)
        {
            return context.Tracks.SingleOrDefault(track => track.Id == trackId);
        }
    }
}
