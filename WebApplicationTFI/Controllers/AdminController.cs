using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplicationTFI.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            if (Session["NomeUtente"] != null) ViewBag.NomeUtente = Session["NomeUtente"].ToString();
            return View("~/Views/Home/Index.cshtml");
        }
    }
}