using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplicationTFI.Controllers
{
    public class AziendaConsulenteController : BaseController
    {
        // GET: Azienda
        public ActionResult Index()
        {
            return View("~/Views/Home/Index.cshtml");
        }
        public ActionResult EstrattoContoAzienda()
        {
            return View();
        }
        public ActionResult Infortuni()
        {            
            return View();
        }
        public ActionResult DettaglioInfortuni()
        {
            return View();
        }
    }
}