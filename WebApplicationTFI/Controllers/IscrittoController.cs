using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplicationTFI.Controllers
{
    public class IscrittoController : Controller
    {
        // GET: Iscritto
        public ActionResult Index()
        {
            if (Session["NomeUtente"] != null) ViewBag.NomeUtente = Session["NomeUtente"].ToString();
            return View("~/Views/Home/Index.cshtml");
        }
        public ActionResult Anagrafica()
        {
            if (Session["NomeUtente"] != null) ViewBag.NomeUtente = Session["NomeUtente"].ToString();
            if (Session["layout"].ToString() == "~/Views/Shared/_AmministrativoLayout.cshtml")
            {
                return View("~/Views/Iscritto/Anagrafica.cshtml", "~/Views/Shared/_AmministrativoLayout.cshtml");
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

        public ActionResult Contact()
        {
            if (Session["NomeUtente"] != null) ViewBag.NomeUtente = Session["NomeUtente"].ToString();
            return View();
        }
        public ActionResult Convenzioni()
        {
            if (Session["NomeUtente"] != null) ViewBag.NomeUtente = Session["NomeUtente"].ToString();
            return View();
        }
        public ActionResult CertificazioniUniche()
        {
            if (Session["NomeUtente"] != null) ViewBag.NomeUtente = Session["NomeUtente"].ToString();
            return View();
        }
        public ActionResult ProspettiPagamento()
        {
            if (Session["NomeUtente"] != null) ViewBag.NomeUtente = Session["NomeUtente"].ToString();
            return View();
        }

        public ActionResult Privacy()
        {
            if (Session["NomeUtente"] != null) ViewBag.NomeUtente = Session["NomeUtente"].ToString();
            return View();
        }
        public ActionResult RichiestaContoIndividuale()
        {
            if (Session["NomeUtente"] != null) ViewBag.NomeUtente = Session["NomeUtente"].ToString();
            return View();
        }
        public ActionResult RichiestaTFR()
        {
            if (Session["NomeUtente"] != null) ViewBag.NomeUtente = Session["NomeUtente"].ToString();
            return View();
        }
        public ActionResult AnticipazioneTFR()
        {
            if (Session["NomeUtente"] != null) ViewBag.NomeUtente = Session["NomeUtente"].ToString();
            return View();
        }
    }
}