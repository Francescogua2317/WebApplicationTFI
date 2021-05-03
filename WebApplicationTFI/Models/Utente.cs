using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using IBM.Data.DB2.iSeries;


namespace WebApplicationTFI.Models
{
    public class Utente
    {
        private int _id;
        private string _userName;
        private string _pwd;
        private string _tipoPin;
        private string _stato;
        private string _posizione;
        private bool _loggato;
        private bool _errConn;
        private bool _errQuery;
        private int _IdPos;
        private string _consorzioOfConsulente;

        public int ID { get {return _id; } }
        public string Login { get { return _userName; } }
        public string Tipopin { get { return _tipoPin; } }
        public string Stato { get { return _stato; } }
        public bool Loggato { get { return _loggato; } }
        public bool ErroreConnessione { get { return _errConn; } }
        public bool ErroreQuery{ get { return _errQuery; } }
        public int IdPosizione { get { return _IdPos; } }
        public string Posizione { get { return _posizione; } }
        public string ConsorzioOfConsulente { get { return _consorzioOfConsulente; } }
        public bool LoginUtente(string login, string pwd, string tipo)
        {
            try
            {
                string strSQL = "";
                DataLayer dl = new DataLayer();
                DataSet Ds = new DataSet();
                string err = "";
                switch (tipo)
                {
                    case "A":
                    case "C":
                        strSQL = " SELECT id, login, codpin, tipopin, Stato FROM tbint001 WHERE login ='" + login + "' AND codpin='" + pwd + "'";
                        break;
                    case "I":
                    case "E":
                        strSQL = "  SELECT CODUTE, DENUTE, CODTIPUTE, ULTAGG, UTEAGG, OLDCODUTE, IDLOGIN, EMAIL, CODFIS, UTEWINDOWS, USER_FAX, NUMFAXOUT, FLGINAZ, PININAZ, CODTIPUTE2 ";
                        strSQL += " FROM UTENTI ";
                        strSQL += " WHERE (UTENTI.CODTIPUTE = '" + tipo + "') AND CODUTE ='" + login + "' AND codpin='" + pwd + "'";
                        break;
                    default:
                        break;
                }
                Ds = dl.GetDataSet(strSQL, ref err);
                if (err != "")
                {
                    this._errConn = true;
                    this._loggato = false;
                    this._id = 0;
                    this._userName = "";
                    this._pwd = "";
                    this._tipoPin = "";
                    this._stato = "";
                }
                else
                {
                    if (queryOk(Ds))
                    {
                        //Utente di tipo Azienda e Consulente
                        if (tipo == "A" || tipo == "C")
                        {
                            if (Ds.Tables[0].Rows[0]["stato"].ToString().ToUpper() == "V")
                            {
                                this._loggato = true;
                                this._id = int.Parse(Ds.Tables[0].Rows[0]["ID"].ToString());
                                this._userName = Ds.Tables[0].Rows[0]["login"].ToString();
                                this._pwd = Ds.Tables[0].Rows[0]["codpin"].ToString();
                                this._tipoPin = Ds.Tables[0].Rows[0]["tipopin"].ToString();
                                this._stato = Ds.Tables[0].Rows[0]["stato"].ToString();

                                if (_tipoPin == "A")
                                {
                                    strSQL = "SELECT rtrim(ltrim(bon_rag_soc)) || ' ' || rtrim(ltrim(bon_rag_soc_sin)) AS RagSociale ";
                                    strSQL += "FROM tbCon WHERE bon_pos=" + this._userName;
                                    DataSet DS2 = new DataSet();
                                    DS2 = dl.GetDataSet(strSQL, ref err);

                                    this._loggato = true;
                                    this._IdPos = int.Parse(Ds.Tables[0].Rows[0]["login"].ToString());
                                    this._posizione = DS2.Tables[0].Rows[0]["RagSociale"].ToString();

                                    strSQL = "SELECT id_Tbint001 FROM tbInt001a WHERE codposiz=" + _IdPos + " AND Stato='V'";
                                    DS2 = dl.GetDataSet(strSQL, ref err);
                                    if (queryOk(DS2))
                                    {
                                        this._consorzioOfConsulente = DS2.Tables[0].Rows[0]["id_Tbint001"].ToString();
                                    }
                                }
                            }
                            else
                            {
                                _loggato = false;
                                return false;
                            }
                        }
                        else if(tipo == "I" || tipo == "E")
                        {

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this._loggato = false; 
                this._id = 0;
                this._userName = "";
                this._pwd = "";
                this._tipoPin = "";
                this._stato = "";
            }
            finally
            {
            }
            return _loggato;
        }

        public static bool queryOk(DataSet dS2)
        {
            if (dS2 != null)
            {
                if (dS2.Tables.Count > 0)
                {
                    if (dS2.Tables[0].Rows.Count > 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}