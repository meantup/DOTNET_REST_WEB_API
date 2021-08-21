using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DOTNET_REST_WEB_API.Model
{
    public class InsertProduct
    {
        public string iname { get; set; }
        public string status { get; set; }
        public string idesc { get; set; }
        public string icode { get; set; }
        public string amount { get; set; }
        public string quantity { get; set; }
        public IFormFile image { get; set; }

    }
    public class Image
    {
        public IFormFile image { get; set; }
    }
}
