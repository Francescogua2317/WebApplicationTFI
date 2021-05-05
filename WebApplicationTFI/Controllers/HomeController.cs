using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplicationTFI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (Session["NomeUtente"] != null) ViewBag.NomeUtente = Session["NomeUtente"].ToString();
            return View();
        }

        public ActionResult Contact()
        {
            if (Session["NomeUtente"] != null) ViewBag.NomeUtente = Session["NomeUtente"].ToString();
            return View();
        }
        public ActionResult Anagrafica()
        {
            if (Session["NomeUtente"] != null) ViewBag.NomeUtente = Session["NomeUtente"].ToString();
           if(Session["layout"].ToString()== "~/Views/Shared/_AmministrativoLayout.cshtml")
            {
                return View("~/Views/Home/Anagrafica.cshtml", "~/Views/Shared/_AmministrativoLayout.cshtml");
            }
            else
            {
                return View();        
            }
       
        }
        public ActionResult EstrattoConto()
        {
            if (Session["NomeUtente"] != null) ViewBag.NomeUtente = Session["NomeUtente"].ToString();
            return View();
        }

        public ActionResult Scadenzario()
        {
            if (Session["NomeUtente"] != null) ViewBag.NomeUtente = Session["NomeUtente"].ToString();
            
            return View();
        }

       
        public ActionResult Liquidazione()
        {
            if (Session["NomeUtente"] != null) ViewBag.NomeUtente = Session["NomeUtente"].ToString();
            return View();
        }
        public ActionResult ListaUtenti ()
        {
            if (Session["NomeUtente"] != null) ViewBag.NomeUtente = Session["NomeUtente"].ToString();
            return View();
        }
        public ActionResult ProspettoCartaEnpaia()
        {
            if (Session["NomeUtente"] != null) ViewBag.NomeUtente = Session["NomeUtente"].ToString();
            return View();
        }
        public ActionResult ProspettoPREV()
        {
            if (Session["NomeUtente"] != null) ViewBag.NomeUtente = Session["NomeUtente"].ToString();
            return View();
        }
        public ActionResult ProspettoFondo()
        {
            if (Session["NomeUtente"] != null) ViewBag.NomeUtente = Session["NomeUtente"].ToString();
            return View();
        }
    }
}