using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    public class ControllerCommon : ControllerBase
    {
        public string CurrentUserName { get; set; }

        public ControllerCommon() 
        {
            //string userName = User.Claims.First(x => x.Type == "UserName").Value;
            //var header = Request.Headers.FirstOrDefault(h => h.Key.Equals("Authorization"));

            //CurrentUserName = userName;
        }

        //if(HttpContext.Current.Session["User"] != null)
        //{
        //    return (User)HttpContext.Current.Session["User"];
        //}
        //else
        //{
        //    HttpContext.Current.Response.Redirect("Login.aspx", true);
        //    return null;
        //}
}
}
