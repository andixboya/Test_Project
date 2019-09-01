using Sis.MvcFramework.Validation;

namespace IRunes.App.ViewModels.Tracks
{
    public class CreateInputModel
    {

        private const string defaultNameErrorMessage = "Album name must be between 3 and 30 symbols.";
        private  const string defaultLinkErrorMessage = "Album name must be between 3 and 30 symbols.";

        
        public string AlbumId { get; set; }

        [StringLengthSis(3, 30,defaultNameErrorMessage)]
        public string Name { get; set; }

        [StringLengthSis(3, 30, defaultLinkErrorMessage)]
        public string Link { get; set; }

        public decimal Price { get; set; }
    }
}
