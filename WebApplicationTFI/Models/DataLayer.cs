using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using IBM.Data.DB2.iSeries;

namespace WebApplicationTFI.Models
{
    public class DataLayer
    {
        private string strConn;
        public int intNumRecords;

        public iDB2ProviderSettings objClear;
        public iDB2Connection objConnection = new iDB2Connection();
        public iDB2Transaction objTransaction;
        public iDB2Command objCommand;
        private bool blnTrans = false;
        private bool Disposed = false;
        private string strTimeStamp;
        private string strLastSQLExecuted;
        private Exception objException;

        public DataLayer()
        {
            this.strConn = (string)ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        }
        public void StartTransaction()
        {
            // Apre una transazione su cui eseguire comandi
            // --------------------------------------------

            try
            {
                objConnection = new iDB2Connection(strConn);
                objConnection.Open();
                // Creiamo un timestamp da usare negli aggiornamenti
                // -------------------------------------------------
                CreateTimestamp();
                objTransaction = objConnection.BeginTransaction();
                blnTrans = true;
                objCommand = new iDB2Command();
                objCommand.CommandTimeout = 180;
                objCommand.Connection = objConnection;
                objCommand.Transaction = objTransaction;
            }
            catch (Exception ex)
            {
                objException = ex;
            }
        }

        public DataSet GetDataSet(string strSQL, ref string Err)
        {
            var objDs = default(DataSet);
            var objDataAd = default(iDB2DataAdapter);
            var dsLocal = new DataSet();
            var objConn = default(iDB2Connection);
            try
            {
                objConn = new iDB2Connection(strConn);
                objConn.Open();
                objDs = new DataSet();
                objDataAd = new iDB2DataAdapter(strSQL, objConn);
                objDataAd.Fill(objDs);
            }
            catch (Exception ex)
            {
                Err = ex.Message;
                if (dsLocal is object)
                {
                    dsLocal.Dispose();
                }

                dsLocal = default;
                objException = ex;
            }
            finally
            {
                if (objConn is object)
                {
                    objConn.Close();
                    objConn.Dispose();
                }

                if (objDataAd is object)
                {
                    objDataAd.Dispose();
                }
            }

            return objDs;
        }
        private void CreateTimestamp()
        {
            // Crea un timestamp per l'inserimento in transazione
            // --------------------------------------------------
            string strTimeStamp;
            strTimeStamp = DateTime.Today.Year + "-" + DateTime.Today.Month.ToString().PadLeft(2, '0');
            strTimeStamp += "-" + DateTime.Today.Day.ToString().PadLeft(2, '0') + "-";
            strTimeStamp += DateTime.Now.Hour.ToString().PadLeft(2, '0') + ".";
            strTimeStamp += DateTime.Now.Minute.ToString().PadLeft(2, '0');
            strTimeStamp += "." + DateTime.Now.Second.ToString().PadLeft(2, '0') + ".";
            strTimeStamp += DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 6, 6);
            this.strTimeStamp = strTimeStamp;
        }

        public void EndTransaction(bool blnCommit)
        {
            try
            {
                if (blnCommit == true)
                {
                    objTransaction.Commit();
                }
                else
                {
                    objTransaction.Rollback();
                }
            }
            catch (Exception ex)
            {
                objException = ex;
            }
            finally
            {
                //objClear.CleanupPooledConnections();
                objConnection.Close();
                objTransaction.Dispose();
                objTransaction = null;
                blnTrans = false;
                if (objCommand is object)
                {
                    objCommand.Dispose();
                    objCommand = null;
                }
            }
        }

        public iDB2DataReader GetDataReaderFromProcedure(string strSQLWithoutCALLString, iDB2Parameter[] sqlParameters)
        {
            var objCmd = default(iDB2Command);
            iDB2DataReader drProcedure;
            var objConn = default(iDB2Connection);
            try
            {
                objConn = new iDB2Connection(strConn);
                objCmd = new iDB2Command("{CALL " + strSQLWithoutCALLString + "}", objConn);
                objCmd.CommandType = CommandType.StoredProcedure;
                foreach (var objPar in sqlParameters)
                    objCmd.Parameters.Add(objPar);
                objConn.Open();
                drProcedure = objCmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                drProcedure = default;
                if (objCmd is object)
                {
                    objCmd.Dispose();
                }

                if (objConn is object)
                {
                    objConn.Close();
                    objConn.Dispose();
                }

                objException = ex;
            }

            return drProcedure;
        }
        public DataSet GetDataSetFromProcedure(string strSQLWithoutCALLString, iDB2Parameter[] sqlParameters)
        {
            var objDataAd = default(iDB2DataAdapter);
            var objDs = default(DataSet);
            var objCmd = default(iDB2Command);
            var objConn = default(iDB2Connection);
            try
            {
                objConn = new iDB2Connection(strConn);
                objDataAd = new iDB2DataAdapter();
                objCmd = new iDB2Command("{CALL " + strSQLWithoutCALLString + "}", objConn);
                objDataAd.SelectCommand = objCmd;
                foreach (var objPar in sqlParameters)
                    objCmd.Parameters.Add(objPar);
                objDs = new DataSet();
                objDataAd.Fill(objDs);
            }
            catch (Exception ex)
            {
                if (objCmd is object)
                {
                    objCmd.Dispose();
                }

                if (objDs is object)
                {
                    objDs.Dispose();
                }

                objDs = default;
                objException = ex;
            }
            finally
            {
                if (objDataAd is object)
                {
                    objDataAd.Dispose();
                }

                if (objConn is object)
                {
                    objConn.Close();
                    objConn.Dispose();
                }
            }
            return objDs;
        }

        public iDB2DataReader GetDataReaderFromProcedureOnTrans(string strSQLWithoutCALLString, iDB2Parameter[] sqlParameters)
        {
            var objCmd = default(iDB2Command);
            iDB2DataReader drProcedure;
            try
            {
                objCmd = new iDB2Command("{CALL " + strSQLWithoutCALLString + "}", objConnection);
                objCmd.Transaction = objTransaction;
                objCmd.CommandType = CommandType.StoredProcedure;
                foreach (var objPar in sqlParameters)
                    objCmd.Parameters.Add(objPar);
                drProcedure = objCmd.ExecuteReader();
            }
            catch (Exception ex)
            {
                drProcedure = default;
                if (objCmd is object)
                {
                    objCmd.Dispose();
                }

                objException = ex;
            }

            return drProcedure;
        }
        public iDB2DataReader GetDataReaderFromQuery(string strQuery, CommandType intCommandType)
        {
            var objCmd = default(iDB2Command);
            iDB2DataReader objDr;
            var objConn = default(iDB2Connection);
            try
            {
                objConn = new iDB2Connection(strConn);
                objCmd = new iDB2Command(strQuery, objConn);
                objCmd.CommandType = intCommandType;
                objConn.Open();
                objDr = objCmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                objDr = default;
                if (objCmd is object)
                {
                    objCmd.Dispose();
                }

                if (objConn is object)
                {
                    objConn.Close();
                    objConn.Dispose();
                }

                objException = ex;
            }

            return objDr;
        }
        public iDB2DataReader GetDataReaderFromQueryOnTrans(string strQuery, CommandType intCommandType)
        {
            var objCmd = default(iDB2Command);
            iDB2DataReader objDr;
            try
            {
                objCmd = new iDB2Command(strQuery, objConnection);
                objCmd.CommandType = intCommandType;
                objCmd.Transaction = objTransaction;
                objDr = objCmd.ExecuteReader();
            }
            catch (Exception ex)
            {
                objDr = default;
                if (objCmd is object)
                {
                    objCmd.Dispose();
                }
            }

            return objDr;
        }
        public iDB2Parameter CreateParameter(string sName, iDB2DbType parameterType, int parameterSize, ParameterDirection paramDirection, string parameterValue)
        {
            var myParam = new iDB2Parameter(sName, parameterType, parameterSize);
            myParam.Value = parameterValue;
            myParam.Direction = paramDirection;
            return myParam;
        }

        public string Get1ValueFromSQL(string strSQL, CommandType tipoComando)
        {
            string strFieldValue = "";
            iDB2DataReader objDr;
            var objConn = default(iDB2Connection);
            var objCmd = default(iDB2Command);
            try
            {
                if (blnTrans == false)
                {
                    objConn = new iDB2Connection(strConn);
                    objCmd = new iDB2Command(strSQL, objConn);
                    objConn.Open();
                }
                else
                {
                    objCmd = new iDB2Command(strSQL, objConnection, objTransaction);
                }

                objCmd.CommandType = tipoComando;
                strFieldValue = Convert.ToString(objCmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                strFieldValue = "";
                objException = ex;
            }
            finally
            {
                if (objCmd is object)
                {
                    objCmd.Dispose();
                }

                if (objConn is object)
                {
                    objConn.Close();
                    objConn.Dispose();
                }
            }

            return strFieldValue;
        }

        public DataTable GetDataTable(string strSQL)
        {
            var objConn = default(iDB2Connection);
            var objDataAd = default(iDB2DataAdapter);
            DataTable objDt;
            try
            {
                if (blnTrans == false)
                {
                    objConn = new iDB2Connection(strConn);
                    objDataAd = new iDB2DataAdapter(strSQL, objConn);
                }
                else
                {
                    objDataAd = new iDB2DataAdapter();
                    objDataAd.SelectCommand = new iDB2Command(strSQL, objConnection, objTransaction);
                }
                strLastSQLExecuted = strSQL;
                objDt = new DataTable();
                objDataAd.Fill(objDt);
            }
            catch (Exception ex)
            {
                throw new Exception(strSQL);
            }
            finally
            {
                if (objDataAd is object)
                {
                    objDataAd.SelectCommand.Dispose();
                    objDataAd.Dispose();
                }

                if (objConn is object)
                {
                    objConn.Close();
                    objConn.Dispose();
                }
            }

            return objDt;
        }




        public bool WriteData(string strSQL, CommandType intCmdType)
        {
            iDB2Connection objCn;
            try
            {
                objCn = new iDB2Connection(strConn);
                objCn.Open();
                objCommand = new iDB2Command(strSQL, objCn);
                objCommand.CommandType = intCmdType;
                objCommand.CommandText = strSQL;
                intNumRecords = objCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                objException = ex;
                return false;
            }
            finally
            {
            }

            return true;
        }


    }
}