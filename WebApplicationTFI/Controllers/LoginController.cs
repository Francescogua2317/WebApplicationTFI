using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IBM.Data.DB2.iSeries;
using WebApplicationTFI.Models;
using WebApplicationTFI.Utilities;

namespace WebApplicationTFI.Controllers
{
    public class LoginController : Controller
    {
        DataLayer objDataAccess;
        private string strAzienda;

        // GET: Login
        public ActionResult Index()
        {
     
            return View();

        }
        [HttpPost]
        public ActionResult Index(Utente u)
        {
            string selected = Request.Form["idLogin"];
            string login = Request.Form["Login"];
            string pwd = Request.Form["Password"];

            if (u.LoginUtente(login, pwd))
            {
                if(selected.EndsWith("I"))
                { 
                Session["utente"] = u;

               return View("~/Views/Home/Index.cshtml", "~/Views/Shared/_IscrittoLayout.cshtml");
                }
                if (selected.EndsWith("E"))
                {
                    Session["utente"] = u;

                return View("~/Views/Home/Index.cshtml", "~/Views/Shared/_AmministrativoLayout.cshtml");
                }
                if (selected.EndsWith("A") || selected.EndsWith("C"))
                {
                    Session["utente"] = u;

                    return View("~/Views/Home/Index.cshtml", "~/Views/Shared/_AziendaConsulenteLayout.cshtml");
                }
                if (selected.EndsWith("AD"))
                {
                    Session["utente"] = u;

                    return View("~/Views/Home/Index.cshtml", "~/Views/Shared/_AdminLayout.cshtml");
                }
                return View();
            }
            else
            {
               if (u.ErroreConnessione)
                {
                   ViewBag.Visibility = true;

                    ViewBag.ErrorMessage = "Attenzione! Si è verificato un problema durante la connessione.";
               }
               else
                {
                   ViewBag.Visibility = true;
                    ViewBag.ErrorMessage = "Utente o password errati.";
                }
                return View();

            }
        }
        public void ShowAlert(System.Web.UI.Page senderPage, string strMsg, bool blnShowAtStartup, bool blnRedirect = false, string strUrlRedirect = "")
        {
            // Crea dinamicamente un alert javascript visualizzato al caricamento
            // ------------------------------------------------------------------
            string strScript;

            strScript = "<script language=JavaScript>alert('" + strMsg + "');";
            if (blnRedirect == true & strUrlRedirect.Trim() != "")
                strScript += "document.location.href = '" + strUrlRedirect + "';";
            strScript += "</script>";
            //if (blnShowAtStartup == true)
            //{
            //    if (!ClientScript.IsStartupScriptRegistered("showmsg"))
            //        ClientScript.RegisterStartupScript(this.GetType(), "showmsg", strScript, false);
            //}
            //else if (!ClientScript.IsClientScriptBlockRegistered("showmsg"))
            //    ClientScript.RegisterClientScriptBlock(this.GetType(), "showmsg", strScript, false);
        }


        public string DoublePeakForSql(string strValue)
        {
            // Prepara i valori di tipo stringa per le istruzioni SQL
            // ------------------------------------------------------
            strValue = strValue.Replace("'", "''");
            strValue = "'" + strValue.Trim() + "'";
            return strValue;
        }

        private bool VerifyLogin(string user, string pwd, string tipoUte)
        {
            // Esegue l'autenticazione dell'utente
            // -----------------------------------
            string strUser = "";
            string strPassword;
            string strSQL;
            DataTable objDt = new DataTable();
            bool blnEnd;
            bool blnResult = false;
            DateTime datDataIni;
            DateTime datDataFin;
            bool blnTran = false;
            bool blnCommit = false;

            string strIva = "";
            DataTable objDtUte = new DataTable();
            Int32 KK = 0;
            bool blnEnp = false;

            try
            {
                // Controllo se i parametri per l'autenticazione provengono
                // direttamente dalla procedura interna o sono stati digitati
                // ----------------------------------------------------------     

                objDataAccess = new DataLayer();

                if (false == false)
                {
                    // strUser = DoublePeakForSql(txtUser.Text)
                    // strPassword = txtPassword.Text

                    strIva = DoublePeakForSql(user.ToUpper());
                    strPassword = pwd;

                    strSQL = "SELECT COUNT(*) FROM UTENTI WHERE CODUTE = " + strIva + " AND CODTIPUTE = 'E'";

                    if (Convert.ToInt16("0" + objDataAccess.Get1ValueFromSQL(strSQL, CommandType.Text)) == 0)
                    {
                        strSQL = "SELECT CODUTE, CODTIPUTE FROM UTENTI WHERE CODFIS = " + strIva;
                        objDtUte = objDataAccess.GetDataTable(strSQL);

                        for (KK = 0; KK <= objDtUte.Rows.Count - 1; KK++)
                        {
                            if (objDtUte.Rows[KK]["CODTIPUTE"].ToString().Trim() == "C")
                            {
                                strSQL = "SELECT COUNT(*) FROM CONSUL WHERE CURRENT_DATE BETWEEN DATINI AND VALUE(DATFIN, '9999-12-31')";
                                strSQL += " AND CODUTE = " + DoublePeakForSql(Convert.ToString("" + objDtUte.Rows[KK]["CODUTE"]).Trim().ToUpper());

                                if (Convert.ToInt16("0" + objDataAccess.Get1ValueFromSQL(strSQL, CommandType.Text)) > 0)
                                {
                                    strSQL = "SELECT COUNT(*) FROM UTEPIN WHERE STAPIN = 'A' AND DATINI = (SELECT MAX(DATINI) FROM UTEPIN WHERE";
                                    strSQL += " CODUTE = " + DoublePeakForSql(Convert.ToString("" + objDtUte.Rows[KK]["CODUTE"]).Trim());
                                   strSQL += " AND STAPIN = 'A') AND PIN = " + DoublePeakForSql(Cypher.CryptPassword(strPassword).Trim());
                                    strSQL += " AND CODUTE = " + DoublePeakForSql(Convert.ToString("" + objDtUte.Rows[KK]["CODUTE"]).Trim());

                                    if (Convert.ToInt16("0" + objDataAccess.Get1ValueFromSQL(strSQL, CommandType.Text)) > 0)
                                    {
                                        strUser = DoublePeakForSql(Convert.ToString("" + objDtUte.Rows[KK]["CODUTE"]).Trim().ToUpper());
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                strSQL = "SELECT COUNT(*) FROM UTEPIN WHERE STAPIN = 'A' AND DATINI = (SELECT MAX(DATINI) FROM UTEPIN WHERE";
                                strSQL += " CODUTE = " + DoublePeakForSql(Convert.ToString("" + objDtUte.Rows[KK]["CODTIPUTE"]).Trim());
                                strSQL += " AND STAPIN = 'A') AND PIN = " + DoublePeakForSql(Cypher.CryptPassword(strPassword).Trim());
                                strSQL += " AND CODUTE = " + DoublePeakForSql(Convert.ToString("" + objDtUte.Rows[KK]["CODUTE"]).Trim().ToUpper());

                                if (Convert.ToInt16("0" + objDataAccess.Get1ValueFromSQL(strSQL, CommandType.Text)) > 0)
                                {
                                    strUser = DoublePeakForSql(Convert.ToString("" + objDtUte.Rows[KK]["CODUTE"]).Trim().ToUpper());
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        strUser = DoublePeakForSql(user.ToUpper());
                        blnEnp = true;
                    }
                }
                else
                {
                    strUser = DoublePeakForSql(user);
                    strPassword = pwd;
                }
                // --------------------------
                // Verifica userid e password
                // --------------------------

                if (blnEnp == true)
                {
                    strSQL = "SELECT DISTINCT A.PIN, A.DATINI, A.DATFIN, B.CODFIS, B.CODUTE, B.DENUTE, B.CODTIPUTE, CURRENT_DATE AS ";
                    strSQL += "TODAY, CURRENT_TIME AS NOW, (SELECT COUNT(CODPOS) FROM AZIUTE WHERE CODUTE = " + strUser + ") ";
                    strSQL += "AS NUM_AZIENDE, E.CODPOS, E.RAGSOC FROM UTEPIN A LEFT JOIN UTENTI B ON A.CODUTE = B.CODUTE LEFT ";
                    strSQL += "JOIN AZIUTE D ON B.CODUTE = D.CODUTE LEFT JOIN AZI E ON D.CODPOS = E.CODPOS WHERE B.CODUTE = ";
                    strSQL += strUser + " AND A.STAPIN <> 'D' AND A.DATINI = (SELECT MAX(DATINI) FROM UTEPIN WHERE CODUTE = ";
                    strSQL += strUser + ")";
                    strSQL += " AND (E.DATCHI IS NULL OR VALUE(E.DATCHI, '9999-12-31') = '9999-12-31')";
                    objDt = objDataAccess.GetDataTable(strSQL);

                    if (objDt.Rows.Count > 0)
                    {
                    //    if (strPassword.ToUpper() == Cypher.DeCryptPassword(Convert.ToString("" + objDt.Rows[KK]["PIN"]).Trim()).ToUpper())
                            blnResult = true;
                    }
                }
                else if (strUser == null == false)
                {
                    strSQL = "SELECT DISTINCT A.PIN, A.DATINI, A.DATFIN, B.CODFIS, B.CODUTE, B.DENUTE, B.CODTIPUTE, B.CODTIPUTE2, CURRENT_DATE AS ";
                    strSQL += "TODAY, CURRENT_TIME AS NOW, (SELECT COUNT(CODPOS) FROM AZIUTE WHERE CODUTE = " + strUser + ") ";
                    strSQL += "AS NUM_AZIENDE, E.CODPOS, E.RAGSOC FROM UTEPIN A LEFT JOIN UTENTI B ON A.CODUTE = B.CODUTE LEFT ";
                    strSQL += "JOIN AZIUTE D ON B.CODUTE = D.CODUTE LEFT JOIN AZI E ON D.CODPOS = E.CODPOS WHERE B.CODFIS = ";
                    strSQL += strIva + " AND B.CODUTE = " + strUser + " AND A.STAPIN <> 'D' ";
                    if (tipoUte == "I")
                        strSQL += " and (CODTIPUTE = 'I' OR CODTIPUTE2 = 'I')";
                    else
                        strSQL += " and (CODTIPUTE = 'C' OR CODTIPUTE2 = 'C' OR CODTIPUTE = 'A')";
                    strSQL += " AND A.DATINI = (SELECT MAX(DATINI) FROM UTEPIN WHERE CODUTE = ";
                    strSQL += strUser + ")";

                    // --- 10-12-2010 SOLO PER AZIENDE APERTE
                    strSQL += " AND (E.DATCHI IS NULL OR VALUE(E.DATCHI, '9999-12-31') = '9999-12-31')";
                    objDt = objDataAccess.GetDataTable(strSQL);

                    if (objDt.Rows.Count > 0)
                    {
                        if (strPassword.ToUpper() == Cypher.DeCryptPassword(Convert.ToString("" + objDt.Rows[0]["PIN"]).Trim()).ToUpper())
                            blnResult = true;
                    }
                }
                else
                    blnResult = false;

                // Controlliamo la password
                // ------------------------

                if (blnResult == true)
                {
                    // Controlliamo nel caso di consulenti che dispongano di deleghe attive
                    // --------------------------------------------------------------------
                    if (Convert.ToString(objDt.Rows[0]["CODTIPUTE"]).Trim() == "C")
                    {
                        strSQL = "SELECT VALUE(COUNT(*), 0) AS AZIENDE FROM AZIUTE A INNER JOIN AZI B ON A.CODPOS = ";
                        strSQL += "B.CODPOS WHERE A.CODUTE = '" + Convert.ToString(objDt.Rows[0]["CODUTE"]).Trim();
                        strSQL += "' AND VALUE(B.DATCHI, '9999-12-31') = '9999-12-31'";
                        blnResult = Convert.ToInt16("0" + objDataAccess.Get1ValueFromSQL(strSQL, CommandType.Text)) > 0;
                    }
                    if (blnResult == true)
                    {
                        blnResult = false;

                        if (tipoUte == "I")
                        {
                            if (Convert.ToString(objDt.Rows[0]["CODTIPUTE"]).Trim() == "E")
                            {
                                if (blnEnp == true)
                                    strSQL = "SELECT DISTINCT MAT FROM RAPLAV WHERE VALUE(CODCAUCES, 0) <> 50 AND MAT IN (SELECT MAT FROM ISCT WHERE CODFIS = '" + Convert.ToString(objDt.Rows[0]["CODUTE"]).Trim() + "' AND CURRENT_DATE BETWEEN DATISC AND VALUE (DATCHIISC, '9999-12-31'))";
                                else
                                    strSQL = "SELECT DISTINCT MAT FROM RAPLAV WHERE VALUE(CODCAUCES, 0) <> 50 AND MAT IN (SELECT MAT FROM ISCT WHERE CODFIS = '" + Convert.ToString(objDt.Rows[0]["CODFIS"]).Trim() + "' AND CURRENT_DATE BETWEEN DATISC AND VALUE (DATCHIISC, '9999-12-31'))";

                                Session["strMat"] = Convert.ToInt32("0" + objDataAccess.Get1ValueFromSQL(strSQL, CommandType.Text));

                                strAzienda = Session["strMat"].ToString() + " - " + Convert.ToString(objDt.Rows[0]["DENUTE"]).Trim();
                            }
                            else
                            {
                                strSQL = " SELECT VALUE(COUNT(*), 0) FROM GRUISCT_P WHERE CODFIS = '" + Convert.ToString(objDt.Rows[0]["CODUTE"]).Trim() + "'";

                                if (Convert.ToInt32("0" + objDataAccess.Get1ValueFromSQL(strSQL, CommandType.Text)) > 0)
                                {
                                    Session["strPens"] = "S";
                                    strAzienda = Convert.ToString(objDt.Rows[0]["CODUTE"]).Trim() + " - " + Convert.ToString(objDt.Rows[0]["DENUTE"]).Trim();
                                }
                                else
                                {
                                    if (blnEnp == true)
                                        strSQL = "SELECT DISTINCT MAT FROM RAPLAV WHERE VALUE(CODCAUCES, 0) <> 50 AND MAT IN (SELECT MAT FROM ISCT WHERE CODFIS = '" + Convert.ToString(objDt.Rows[0]["CODUTE"]).Trim() + "' AND CURRENT_DATE BETWEEN DATISC AND VALUE (DATCHIISC, '9999-12-31'))";
                                    else
                                        strSQL = "SELECT DISTINCT MAT FROM RAPLAV WHERE VALUE(CODCAUCES, 0) <> 50 AND MAT IN (SELECT MAT FROM ISCT WHERE CODFIS = '" + Convert.ToString(objDt.Rows[0]["CODFIS"]).Trim() + "' AND CURRENT_DATE BETWEEN DATISC AND VALUE (DATCHIISC, '9999-12-31'))";

                                    Session["strMat"] = Convert.ToInt32("0" + objDataAccess.Get1ValueFromSQL(strSQL, CommandType.Text));

                                    strAzienda = Convert.ToString(Session["strMat"]) + " - " + Convert.ToString(objDt.Rows[0]["DENUTE"]).Trim();
                                }
                            }
                        }
                        else
                            strAzienda = Convert.ToString(objDt.Rows[0]["CODPOS"]).Trim() + " - " + Convert.ToString(objDt.Rows[0]["RAGSOC"]).Trim();

                        blnEnd = false;
                        while (!blnEnd == true)
                        {
                            if (strAzienda.Substring(0, 1) == "0")
                                strAzienda = strAzienda.Substring(1);
                            else
                                blnEnd = true;
                        }
                        // Verifica validità password
                        // --------------------------
                        if (objDt.Rows[0]["DATINI"] == null)
                        {
                            datDataIni = Convert.ToDateTime(objDt.Rows[0]["DATINI"]);
                            if (DateTime.Compare(datDataIni, DateTime.Today) > 0)
                            {
                                // base.ShowAlert(this, this.MyResourceManager.GetString("utentenoninattesadiattivazione"), true);
                            }
                            else
                            {
                                // -------------------------------------
                                // Valorizziamo le variabili di sessione
                                // -------------------------------------
                                if (blnEnp == true)
                                    Session["strParIva"] = objDt.Rows[0]["CODUTE"].ToString().Trim();
                                else
                                    Session["strParIva"] = objDt.Rows[0]["CODFIS"].ToString().Trim();

                                Session["strCodUte"] = objDt.Rows[0]["CODUTE"].ToString().Trim();
                                Session["DateToday"] = objDt.Rows[0]["TODAY"].ToString();
                                Session["strNumeroAziende"] = objDt.Rows[0]["NUM_AZIENDE"].ToString();

                                //TIPO UTENTE DA DDLIST
                                if (tipoUte == "I")
                                {
                                    if (Session["strTipoUtente"].ToString() == "E")
                                    {
                                        Session["strTipoUtente"] = "I";
                                        blnEnp = false;
                                    }
                                    else
                                        Session["strTipoUtente"] = "I";
                                }
                                else
                                {
                                    switch (objDt.Rows[0]["CODTIPUTE"].ToString().Trim())
                                    {
                                        case "C":
                                            {
                                                Session["strTipoUtente"] = "C";
                                                break;
                                            }

                                        case "A":
                                            {
                                                Session["strTipoUtente"] = "A";
                                                break;
                                            }

                                        case "E":
                                            {
                                                Session["strTipoUtente"] = "E";
                                                break;
                                            }
                                    }

                                    if (Session["strTipoUtente"] == null == true)
                                    {
                                        switch (Convert.ToString(objDt.Rows[0]["CODTIPUTE2"]).Trim())
                                        {
                                            case "C":
                                                {
                                                    Session["strTipoUtente"] = "C";
                                                    break;
                                                }

                                            case "A":
                                                {
                                                    Session["strTipoUtente"] = "A";
                                                    break;
                                                }

                                            case "E":
                                                {
                                                    Session["strTipoUtente"] = "E";
                                                    break;
                                                }
                                        }
                                    }
                                }

                                Session["strCodPos"] = Convert.ToString(objDt.Rows[0]["CODPOS"]);
                                if (Session["strCodPos"].ToString() == "")
                                    Session["strCodPos"] = "0";

                                if (Session["strMat"] == null)
                                    Session["strMat"] = "0";
                                // ---------------------------
                                // Controllo scadenza password
                                // ---------------------------
                                if (objDt.Rows[0]["DATFIN"] == null)
                                {
                                    datDataFin = Convert.ToDateTime(objDt.Rows[0]["DATFIN"]);
                                    if (DateTime.Compare(datDataFin, DateTime.Today) < 0)
                                    {
                                        if (blnEnp == false)
                                        {
                                            // Controlliamo se l'utente è al primo accesso alla procedura
                                            // ----------------------------------------------------------
                                            //if (DateTime.Compare(datDataFin, DateTime.Parse("31/12/1899")) == 0)
                                            //    base.ShowAlert(this, this.MyResourceManager.GetString("utenteprimoaccesso"), true);
                                            //else
                                            //    base.ShowAlert(this, this.MyResourceManager.GetString("utentepasswordscaduta"), true);
                                            //ShowConfermaPassword(true);

                                            switch (Session["strTipoUtente"].ToString().Trim())
                                            {
                                                case "E":
                                                    {
                                                        //this.lblMsgReq.Visible = true;
                                                        break;
                                                    }

                                                default:
                                                    {
                                                        //this.lblMsg.Visible = true;
                                                        break;
                                                    }
                                            }
                                        }
                                        else
                                        {
                                            // PASSWORD VALIDA UTENTE ABILITATO
                                            // --------------------------------
                                            blnResult = true;
                                            // -----------------------------                                        
                                            objDataAccess.StartTransaction();
                                            blnTran = true;
                                            // Chiudiamo i record rimasti aperti per l'utente web
                                            // ---------------------------------------------------
                                            strSQL = "UPDATE UTEACC SET USCITA = ENTRATA + " + Session.Timeout.ToString() + " minutes WHERE CODUTE = '";
                                            strSQL += Session["strCodUte"].ToString() + "' AND ENTRATA <= (current_timestamp - " + Session.Timeout.ToString();
                                            strSQL += " minutes) AND USCITA IS NULL AND date(ENTRATA) = current_date AND UTEWEB = 'S'";
                                        //    blnCommit = objDataAccess.WriteTransactionData(strSQL, CommandType.Text);
                                            // Inseriamo un record in UTEACC
                                            // -----------------------------
                                            if (blnCommit == true)
                                            {
                                                strSQL = "INSERT INTO UTEACC (CODUTE, ENTRATA, USCITA, PCNAME, UTEWEB) VALUES ('";
                                            //    strSQL += Session["strCodUte"] + "', '" + objDataAccess.strTimeStamp + "', Null, '";
                                                    strSQL += Session["strCodUte"] + "', '" + DateTime.Now.ToString() + "', Null, '";
                                                    strSQL += Request.ServerVariables["REMOTE_ADDR"] + "', 'S')";
                                          //      blnCommit = objDataAccess.WriteTransactionData(strSQL, CommandType.Text);
                                            }
                                            objDataAccess.EndTransaction(blnCommit);
                                            blnTran = false;
                                        //    Session["LoginTime"] = objDataAccess.strTimeStamp;
                                                Session["LoginTime"] = DateTime.Now;

                                            }
                                        }
                                    else
                                        // If Date.Compare(DateAdd(DateInterval.Day, 90, datDataIni), Today) >= 0 Then
                                        if (DateTime.Compare(datDataFin, DateTime.Today) >= 0)
                                    {
                                        // PASSWORD VALIDA UTENTE ABILITATO
                                        // --------------------------------
                                        blnResult = true;
                                        // -----------------------------                                        
                                        objDataAccess.StartTransaction();
                                        blnTran = true;
                                        // Chiudiamo i record rimasti aperti per l'utente web
                                        // ---------------------------------------------------
                                        strSQL = "UPDATE UTEACC SET USCITA = ENTRATA + " + Session.Timeout.ToString() + " minutes WHERE CODUTE = '";
                                        strSQL += Session["strCodUte"].ToString() + "' AND ENTRATA <= (current_timestamp - " + Session.Timeout.ToString();
                                        strSQL += " minutes) AND USCITA IS NULL AND date(ENTRATA) = current_date AND UTEWEB = 'S'";
                                     //   blnCommit = objDataAccess.WriteTransactionData(strSQL, CommandType.Text);
                                        // Inseriamo un record in UTEACC
                                        // -----------------------------
                                        if (blnCommit == true)
                                        {
                                            strSQL = "INSERT INTO UTEACC (CODUTE, ENTRATA, USCITA, PCNAME, UTEWEB) VALUES ('";
                                            strSQL += Session["strCodUte"] + "', '" + DateTime.Now.ToString() + "', Null, '";
                                            strSQL += Request.ServerVariables["REMOTE_ADDR"].ToString() + "', 'S')";
                                       //     blnCommit = objDataAccess.WriteTransactionData(strSQL, CommandType.Text);
                                        }
                                        objDataAccess.EndTransaction(blnCommit);
                                        blnTran = false;
                                        Session["LoginTime"] = DateTime.Now;
                                    }
                                    else
                                    {
                                        // Cambio password
                                        // ---------------
                                     //   base.ShowAlert(this, this.MyResourceManager.GetString("utentepasswordprivacyscaduta"), true);
                                        //ShowConfermaPassword(true);
                                    }
                                }
                                else
                                {
                                    // Cambio password
                                    // ---------------
                                 //   base.ShowAlert(this, this.MyResourceManager.GetString("utentepasswordprivacyscaduta"), true);
                                    //ShowConfermaPassword(true);
                                }
                            }
                        }
                        else
                        {
                            //   base.ShowAlert(this, this.MyResourceManager.GetString("utentenonattivato"), true);

                        }
                    }
                    else
                    {               
                        //    base.ShowAlert(this, this.MyResourceManager.GetString("aziendechiuse"), true);
                    }
                }
                else
                {
                    // Controlliamo se il PIN è disabilitato
                    // -------------------------------------
                    strSQL = "SELECT COUNT(*) FROM UTEPIN WHERE CODUTE = " + strUser + " AND PIN = '" + Cypher.CryptPassword(strPassword);
                    strSQL += "' AND current_date BETWEEN DATINI AND DATFIN AND STAPIN = 'D'";
                    if ((int.Parse("0" + objDataAccess.Get1ValueFromSQL(strSQL, CommandType.Text))) > 0)
                    {
                        //     base.ShowAlert(this, this.MyResourceManager.GetString("utentedisabilitato"), true);

                    }
                    else
                        // Nome utente o password non validi  
                        // ---------------------------------
                    //    base.ShowAlert(this, this.MyResourceManager.GetString("utentenontrovato"), true);
                    blnResult = false;
                }
                //if (blnResult == true) Reset_Form(true);
            }
            catch (Exception ex)
            {
                Session["LastException"] = ex;
            }
            finally
            {
                if (objDt != null)
                    objDt.Dispose();
                if (objDataAccess != null)
                {
                    if (blnTran == true)
                        objDataAccess.EndTransaction(false);
                //    objDataAccess.Dispose();
                }
                //base.ErrorHandler();
            }
            return blnResult;
        }
    }

}

