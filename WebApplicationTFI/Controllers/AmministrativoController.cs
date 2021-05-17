using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplicationTFI.Controllers
{
    public class AmministrativoController : BaseController
    {
        // GET: Amministrativo
        public ActionResult Index()
        {
            return View("~/Views/Home/Index.cshtml");
        }
        public ActionResult Scadenzario()
        {
            return View();
        }
        public ActionResult Scadenzario_Infortuni()
        {
            return View();
        }
        public ActionResult ScadenzarioFondo()
        {
            return View();
        }


        public ActionResult Liquidazione()
        {            
            return View();
        }
        public ActionResult ListaUtenti()
        {            
            return View();
        }
        public ActionResult ProspettoCartaEnpaia()
        {            
            return View();
        }
        public ActionResult ProspettoPREV()
        {            
            return View();
        }
        public ActionResult ProspettoFondo()
        {            
            return View();
        }
        public ActionResult Anagrafica() 
        {            
            return View();
        }
        public ActionResult EstrattoContoAzienda()
        {            
            return View();
        }
        public ActionResult ListaIscritti()
        {
            ViewBag.src = @Request.QueryString["src"];            
            return View();
        }
        public ActionResult ListaAziende()
        {
            ViewBag.src = @Request.QueryString["src"];
            return View();
        }
        public ActionResult AnagraficaAzienda()
        {
            ViewBag.src = @Request.QueryString["src"];
            return View();
        }
    }
}