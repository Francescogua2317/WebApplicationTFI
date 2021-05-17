using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplicationTFI.Controllers
{
    public abstract class BaseController : Controller
    {        
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Session["NomeUtente"] != null)
            {
                ViewBag.NomeUtente = Session["NomeUtente"].ToString();
            }
            else
            {
                Response.Redirect(@Url.Action("Index", "Login"));
            }
            base.OnActionExecuting(filterContext);
        }
    }
}