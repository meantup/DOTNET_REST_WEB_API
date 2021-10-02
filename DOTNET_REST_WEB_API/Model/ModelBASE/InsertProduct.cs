using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DOTNET_REST_WEB_API.Model
{
    public class InsertProduct
    {
        public string iname { get; set; }
        public string idesc { get; set; }
        public string icode { get; set; }
        public double amount { get; set; }
        public int quantity { get; set; }
        public string filepath { get; set; }
    }
    public class OrderList
    {
        public int id { get; set; }
        public string iname { get; set; }
        public string idesc { get; set; }
        public string icode { get; set; }
        public decimal amount { get; set; }
        public int quantity { get; set; }
        public DateTime tdt { get; set; }
        public string filepath { get; set; }
    }
}
