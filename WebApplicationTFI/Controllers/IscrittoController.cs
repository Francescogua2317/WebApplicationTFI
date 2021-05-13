using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using IBM.Data.DB2.iSeries;
using WebApplicationTFI.Models;
using WebApplicationTFI.Utilities;

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
            //string codfis = Session["CodiceFiscale"].ToString();
            //DataLayer objDataAccess = new DataLayer();
            //string strSQL = "SELECT C.SIGPRO, C.DENCOM, C.CODCOM FROM CODLOC_APP C, ISCTWEB I WHERE I.CODCOMNAS=C.CODCOM AND I.CODFIS='"+codfis+"'";
            //string strSQL2 = "SELECT MAT, COGNOME, NOME, CODFIS, DATNAS, SES, STAESTNAS, IND, NUMCIV, CAP, EMAIL, EMAILCERT, CELL, DENLOC, SIGPRO FROM ISCTWEB WHERE CODFIS='"+codfis+"'";
            //string strSQL3 = "SELECT T.DENTITSTU FROM TITSTU T, ISCTWEB I WHERE I.CODTITSTU=T.CODTITSTU";


            //DataSet objAnagrafica = new DataSet();
            //DataSet objAnagrafica2 = new DataSet();
            //DataSet objAnagrafica3 = new DataSet();


            //string errore = "";
            //objAnagrafica = objDataAccess.GetDataSet(strSQL, ref errore);
            //objAnagrafica2 = objDataAccess.GetDataSet(strSQL2, ref errore);
            //objAnagrafica3 = objDataAccess.GetDataSet(strSQL3, ref errore);


            //string mat = objAnagrafica2.Tables[0].Rows[0]["MAT"].ToString();
            //string nome = objAnagrafica2.Tables[0].Rows[0]["NOME"].ToString();
            //string cognome = objAnagrafica2.Tables[0].Rows[0]["COGNOME"].ToString();
            //string codiceFiscale = objAnagrafica2.Tables[0].Rows[0]["CODFIS"].ToString();
            //string sesso = objAnagrafica2.Tables[0].Rows[0]["SES"].ToString();
            //string dataNascita= objAnagrafica2.Tables[0].Rows[0]["DATNAS"].ToString().Substring(0, 10);
            //string statoEsteroNascita = objAnagrafica2.Tables[0].Rows[0]["STAESTNAS"].ToString();
            //string titoloStudio = objAnagrafica3.Tables[0].Rows[0]["DENTITSTU"].ToString();
            //string indirizzo = objAnagrafica2.Tables[0].Rows[0]["IND"].ToString();
            //string numeroCivico = objAnagrafica2.Tables[0].Rows[0]["NUMCIV"].ToString();
            //string cap = objAnagrafica2.Tables[0].Rows[0]["CAP"].ToString();
            //string comune = objAnagrafica2.Tables[0].Rows[0]["DENLOC"].ToString();
            //string sigpro2 = objAnagrafica2.Tables[0].Rows[0]["SIGPRO"].ToString();
            //string email = objAnagrafica2.Tables[0].Rows[0]["EMAIL"].ToString();
            //string emailCert = objAnagrafica2.Tables[0].Rows[0]["EMAILCERT"].ToString();
            //string cellulare = objAnagrafica2.Tables[0].Rows[0]["CELL"].ToString();
           
           



            //if (Utente.queryOk(objAnagrafica2))
            //{
            //    if (statoEsteroNascita == "")
            //    {
            //        string dencom = objAnagrafica.Tables[0].Rows[0]["DENCOM"].ToString();
            //        string sigpro = objAnagrafica.Tables[0].Rows[0]["SIGPRO"].ToString();
            //        ViewData["Comune"] = dencom;
            //        ViewData["Provincia"] = sigpro;
            //    }
            //    else
            //    {
            //        string strSQL4 = "SELECT C.SIGPRO, C.DENCOM, C.CODCOM FROM COM_ESTERO C, ISCTWEB I WHERE I.CODCOMNAS=C.CODCOM AND I.CODFIS='" + codfis + "'";
            //        DataSet objAnagrafica4 = new DataSet();
            //        objAnagrafica4 = objDataAccess.GetDataSet(strSQL4, ref errore);
            //        string comuneEstero = objAnagrafica4.Tables[0].Rows[0]["DENCOM"].ToString();
            //        string provinciaEstero = objAnagrafica4.Tables[0].Rows[0]["SIGPRO"].ToString();
            //        ViewData["Comune"] = comuneEstero;
            //        ViewData["Provincia"] = provinciaEstero;
            //    }
            //    ViewData["Matricola"] = mat;
            //    ViewData["Cognome"] = cognome;
            //    ViewData["Nome"] = nome;
            //    ViewData["CodiceFis"] = codiceFiscale;
            //    ViewData["DataNascita"] = dataNascita;
            //    ViewData["Sesso"] = sesso;
            //    ViewData["StatoEstero"] = statoEsteroNascita;
            //    ViewData["TitoloStudio"] = titoloStudio;
            //    ViewData["Indirizzo"] = indirizzo;
            //    ViewData["NumeroCivico"] = numeroCivico;
            //    ViewData["Cap"] = cap;
            //    ViewData["Comune2"] = comune;
            //    ViewData["Provincia2"] = sigpro2;
            //    ViewData["Email"] = email;
            //    ViewData["EmailCert"] = emailCert;
            //    ViewData["Cellulare"] = cellulare;
                
            //}
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

            //string codfis = Session["CodiceFiscale"].ToString();
            //DataLayer objDataAccess = new DataLayer();
            //string strSQL = "SELECT MAT, COGNOME, NOME, CODFIS, IND, NUMCIV, CAP, DENLOC, SIGPRO FROM ISCTWEB WHERE CODFIS='" + codfis + "'";


            //DataSet objAnagrafica = new DataSet();


            //string errore = "";
            //objAnagrafica = objDataAccess.GetDataSet(strSQL, ref errore);


            //string mat = objAnagrafica.Tables[0].Rows[0]["MAT"].ToString();
            //string nome = objAnagrafica.Tables[0].Rows[0]["NOME"].ToString();
            //string cognome = objAnagrafica.Tables[0].Rows[0]["COGNOME"].ToString();
            //string codiceFiscale = objAnagrafica.Tables[0].Rows[0]["CODFIS"].ToString();
           
            //string indirizzo = objAnagrafica.Tables[0].Rows[0]["IND"].ToString();
            //string numeroCivico = objAnagrafica.Tables[0].Rows[0]["NUMCIV"].ToString();
            //string cap = objAnagrafica.Tables[0].Rows[0]["CAP"].ToString();
            //string comune = objAnagrafica.Tables[0].Rows[0]["DENLOC"].ToString();
            //string sigpro2 = objAnagrafica.Tables[0].Rows[0]["SIGPRO"].ToString();
          





            //if (Utente.queryOk(objAnagrafica))
            //{
            //    ViewData["Matricola"] = mat;
            //    ViewData["NomeCognome"] = nome+" "+cognome;
            //    ViewData["CodiceFis"] = codiceFiscale;
            //    ViewData["Indirizzo"] = "Via "+indirizzo+" "+numeroCivico;
            //    ViewData["Cap"] = cap;
            //    ViewData["Comune2"] = comune;
            //    ViewData["Provincia2"] = sigpro2;
             
            //}


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

        public ActionResult Infortuni()
        {
            if (Session["NomeUtente"] != null) ViewBag.NomeUtente = Session["NomeUtente"].ToString();
            return View();
        }
    }
}