using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace RD_MUSIC_PLAYER.ClassCustom
{
	public class ValidateForgeryToken : FilterAttribute, IAuthorizationFilter
	{
		public void OnAuthorization(AuthorizationContext filterContext)
		{
			filterContext.Controller.TempData.Remove("ErrorText");

			var request = filterContext.HttpContext.Request;
			try
			{
				if (request.HttpMethod.ToLower() == "post")
				{
					var antiForgeryCookie = request.Cookies[AntiForgeryConfig.CookieName];
					var cookieValue = antiForgeryCookie != null
						? antiForgeryCookie.Value
						: null;

					var ss1 = request.Cookies["Sec-Fetch-Site"];
					var ss2 = request.Cookies["Sec-Fetch-Mode"];
					var ss3 = request.Cookies["Sec-Fetch-Des"];
					var ss4 = request.Cookies["Origin"];
					var ss5 = request.Cookies["__RequestVerificationToken"];
					var ss6 = request.Cookies["Sec-Fetch-Site"];
					var ss7 = request.Cookies["Sec-Fetch-Site"];
					var ss8 = request.Cookies["Sec-Fetch-Site"];
					var ss9 = request.Cookies["User-Agent"];
					var ss10 = request.Cookies["Cookie"];
					var ss11 = request.Cookies["X-Requested-With"];
					var requestForm = request.Form["__RequestVerificationToken"]?.ToString() != null ? request.Form["__RequestVerificationToken"].ToString()
									: request.Headers["__RequestVerificationToken"]?.ToString() != null ? request.Headers["__RequestVerificationToken"].ToString() : null;
					//AntiForgery.Validate();
					AntiForgery.Validate(cookieValue, requestForm);
				}
				else
				{
					//filterContext.Result = new JsonResult
					//{
					//	Data = new { Message = "Your are not allowed to do that", Status = "002", Datetime = DateTime.Now.ToString("MMM dd, yyyy hh:mm:ss tt") },
					//	ContentEncoding = System.Text.Encoding.UTF8,
					//	ContentType = "application/json",
					//	JsonRequestBehavior = JsonRequestBehavior.AllowGet
					//};
					filterContext.Controller.TempData.Add("ErrorText", "Not Allowed, try refresh the page");

				}
			}
			catch (Exception ee)
			{
				//filterContext.Result = new JsonResult
				//{
				//	Data = new { Message = "Request Token is invalid", Status = "003", Datetime = DateTime.Now.ToString("MMM dd, yyyy hh:mm:ss tt") },
				//	ContentEncoding = System.Text.Encoding.UTF8,
				//	ContentType = "application/json",
				//	JsonRequestBehavior = JsonRequestBehavior.AllowGet
				//};
				var ss = request.IsAjaxRequest();
				if (request.IsAjaxRequest())
				{
					var requestContext = HttpContext.Current.Request.RequestContext;
					var reqUrl = new UrlHelper(requestContext).Action("Index", "Home");
					var response = filterContext.HttpContext.Response;
					response.StatusCode = 201;
					filterContext.Result = new JsonResult
					{
						Data = new { Message = "Request Token is invalid", Status = "40", Datetime = DateTime.Now.ToString("MMM dd, yyyy hh:mm:ss tt"), Type = "RequestToken", url = "" },
						ContentEncoding = System.Text.Encoding.UTF8,
						ContentType = "application/json",
						JsonRequestBehavior = JsonRequestBehavior.AllowGet
					};
				}
				else
				{
					filterContext.Controller.TempData.Add("ErrorText", "Not Allowed, try refresh the page");

				}

			}
		}
	}
}