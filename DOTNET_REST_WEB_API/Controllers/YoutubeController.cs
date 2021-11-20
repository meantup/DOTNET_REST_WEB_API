using DOTNET_REST_WEB_API.Repository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static DOTNET_REST_WEB_API.Model.YoutubeModel;

namespace DOTNET_REST_WEB_API.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class YoutubeController : BaseController
    {
        private readonly IWebHostEnvironment _hosting;
        private readonly IAdapterRepository _repost;
        public YoutubeController(IAdapterRepository repost, IWebHostEnvironment hosting)
        {
            _repost = repost;
            _hosting = hosting;
        }
        //Get List of Youtube Videos
        [SwaggerOperation(Summary = "Search Song in a search box.", Description = "Search your selected song. Return you a list of songs.")]
        [HttpGet]
        [Route("GetListVideos/{query}")]
        public async Task<IActionResult> GetListVideos(string query)
        {
            var val = await _repost.yout.getData(query);
            return Ok(val);
        }
        [SwaggerOperation(Summary = "Adding a Song to the Playlist", Description = "Adding a Song to the Playlist")]
        [HttpPost]
        [Route("AddPlaylist")]
        public async Task<IActionResult> AddPlaylist(AddPlaylist add)
        {
            var val = await _repost.yout.addPlaylist(add);
            return Ok(val);
        }
        [SwaggerOperation(Summary = "Get all Playlist", Description = "Get the whole Playlist that is in queue.")]
        [HttpGet]
        [Route("GetPlaylist")]
        public async Task<IActionResult> GetPlaylist()
        {
            var val = await _repost.yout.getPlaylist();
            return Ok(val);
        }
        [SwaggerOperation(Summary = "Play Next Song", Description = "This will play the next song by getting the link of the video.")]
        [HttpGet]
        [Route("PlaySong")]
        public async Task<IActionResult> PlaySong()
        {
            var val = await _repost.yout.playSong();
            return Ok(val);
        }
        [SwaggerOperation(Summary = "Update Song", Description = "This will update the song to finished then move to next song.")]
        [HttpGet]
        [Route("UpdateSong")]
        public async Task<IActionResult> UpdateSong()
        {
            var val = await _repost.yout.updateSong();
            return Ok(val);
        }
    }
}
