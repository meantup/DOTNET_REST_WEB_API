using System.Web;
using System.Web.Mvc;

namespace RD_MUSIC_PLAYER
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
