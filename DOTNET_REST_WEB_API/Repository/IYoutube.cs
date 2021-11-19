using DOTNET_REST_WEB_API.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static DOTNET_REST_WEB_API.Model.YoutubeModel;

namespace DOTNET_REST_WEB_API.Repository
{
    public interface IYoutube
    {
        Task<ServiceResponseT<object>> getData(string searchQuery);
        Task<ServiceResponseT<object>> addPlaylist(AddPlaylist add);
        Task<ServiceResponseT<object>> playSong();
        Task<ServiceResponseT<object>> updateSong();
    }
}
