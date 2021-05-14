using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplicationTFI.Controllers
{
    public class AmministrativoController : Controller
    {
        // GET: Amministrativo
        public ActionResult Index()
        {
            if (Session["NomeUtente"] != null) ViewBag.NomeUtente = Session["NomeUtente"].ToString();
            return View("~/Views/Home/Index.cshtml");
        }
        public ActionResult Scadenzario()
        {
            if (Session["NomeUtente"] != null) ViewBag.NomeUtente = Session["NomeUtente"].ToString();

            return View();
        }
        public ActionResult Scadenzario_Infortuni()
        {
            if (Session["NomeUtente"] != null) ViewBag.NomeUtente = Session["NomeUtente"].ToString();

            return View();
        }
        public ActionResult ScadenzarioFondo()
        {
            if (Session["NomeUtente"] != null) ViewBag.NomeUtente = Session["NomeUtente"].ToString();

            return View();
        }


        public ActionResult Liquidazione()
        {
            if (Session["NomeUtente"] != null) ViewBag.NomeUtente = Session["NomeUtente"].ToString();
            return View();
        }
        public ActionResult ListaUtenti()
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
        public ActionResult Anagrafica() 
        {
            if (Session["NomeUtente"] != null) ViewBag.NomeUtente = Session["NomeUtente"].ToString();
            return View();
        }
        public ActionResult EstrattoContoAzienda()
        {
            if (Session["NomeUtente"] != null) ViewBag.NomeUtente = Session["NomeUtente"].ToString();
            return View();
        }
        public ActionResult ListaIscritti()
        {
            if (Session["NomeUtente"] != null) ViewBag.NomeUtente = Session["NomeUtente"].ToString();
            return View();
        }
        public ActionResult ListaAziende()
        {
            if (Session["NomeUtente"] != null) ViewBag.NomeUtente = Session["NomeUtente"].ToString();
            return View();
        }
    }
}