using System;
using System.IO;
using System.Security.Cryptography;

namespace WebApplicationTFI.Utilities
{
    public class Cypher
    {

        public static string CryptPassword(string strPassword, string strKey = "", string strIV = "")
        {
            SymmetricAlgorithm mCryptProv;
            MemoryStream mMemStr;
            try
            {
                mCryptProv = SymmetricAlgorithm.Create("Rijndael");
                mCryptProv.BlockSize = 256;
                mCryptProv.KeySize = 256;
                if (string.IsNullOrEmpty(strKey.Trim()))
                {
                    strKey = "akjf78ytHr3z5RoEpm0pw7";
                }

                if (string.IsNullOrEmpty(strIV.Trim()))
                {
                    strIV = "BjdCH23973MbnjkcAsy82v";
                }
                // Creazione del Memory Stream
                // ---------------------------
                mMemStr = new MemoryStream();
                // Creazione dell'Encryptor, passando Key e IV
                // -------------------------------------------
                var mEncryptor = mCryptProv.CreateEncryptor(GetKey(strKey), GetIV(strIV));
                // Creiamo il Crypto Stream passandogli il Memory Stream e l'Encryptor
                // -------------------------------------------------------------------
                var mCryptStr = new CryptoStream(mMemStr, mEncryptor, CryptoStreamMode.Write);

                // Creazione di uno Stream Writer per scrivere i dati criptati
                // -----------------------------------------------------------
                var mStrWri = new StreamWriter(mCryptStr);
                // Cripto i dati
                // -------------
                mStrWri.Write(strPassword);
                mStrWri.Flush();
                mCryptStr.FlushFinalBlock();

                // Creazione array dal MemoryStream
                // --------------------------------
                var mBytes = new byte[(int)(mMemStr.Length - 1L + 1)];
                // Posizioniamo il cursore all'inizio
                // ----------------------------------
                mMemStr.Position = 0L;
                mMemStr.Read(mBytes, 0, (int)mMemStr.Length);
                // --------------------------------------
                mCryptStr.Close();
                mMemStr.Close();
                mStrWri.Close();
                // Codifica l'array di byte in UTF8
                // --------------------------------
                // Dim mEnc As New Text.UTF8Encoding()
                // Return mEnc.GetString(mBytes)
                return Convert.ToBase64String(mBytes);
            }
            catch
            {
                return "";
            }
        }

        public static string DeCryptPassword(string strPassword, string strKey = "", string strIV = "")
        {
            SymmetricAlgorithm mCryptProv;
            MemoryStream mMemStr;
            string strDecryptedPwd = "";
            try
            {
                mCryptProv = SymmetricAlgorithm.Create("Rijndael");
                mCryptProv.BlockSize = 256;
                mCryptProv.KeySize = 256;
                if (string.IsNullOrEmpty(strKey.Trim()))
                {
                    strKey = "akjf78ytHr3z5RoEpm0pw7";
                }

                if (string.IsNullOrEmpty(strIV.Trim()))
                {
                    strIV = "BjdCH23973MbnjkcAsy82v";
                }
                // Creazione del Memory Stream
                // ---------------------------
                var mBytes = Convert.FromBase64String(strPassword);
                mMemStr = new MemoryStream(mBytes);
                // Posizioniamo il cursore all'inizio
                // ----------------------------------
                mMemStr.Position = 0L;
                // Creazione del Decryptor cui passiamo Key e IV
                // ---------------------------------------------
                var mDecrypt = mCryptProv.CreateDecryptor(GetKey(strKey), GetIV(strIV));
                // Creiamo il Crypto Stream
                // ------------------------
                var mCSReader = new CryptoStream(mMemStr, mDecrypt, CryptoStreamMode.Read);
                // Creiamo lo Stream Reader
                // ------------------------
                var mStrRead = new StreamReader(mCSReader);
                strDecryptedPwd = mStrRead.ReadToEnd();
                mCSReader.Close();
                mStrRead.Close();
                mMemStr.Close();
                // --------------------------
                return strDecryptedPwd;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        private static byte[] GetIV(string strIV)
        {
            // Crea la IV
            // ----------
            int arrSize = (int)Math.Round(256d / 8d - 1d);
            var arrIV = new byte[arrSize + 1];
            int temp;
            if (strIV.Length < 1)
            {
                return arrIV;
            }

            int lastBound = strIV.Length;
            if (lastBound > arrSize)
                lastBound = arrSize;
            var loopTo = lastBound - 1;
            for (temp = 0; temp <= loopTo; temp++)
                arrIV[temp] = Convert.ToByte(strIV[temp]);
            return arrIV;
        }

        private static byte[] GetKey(string strKey)
        {
            // Crea la chiave
            // --------------
            int arrSize = (int)Math.Round(256d / 8d - 1d);
            int temp;
            var arrKey = new byte[arrSize + 1];
            if (strKey.Length < 1)
            {
                return arrKey;
            }

            int lastBound = strKey.Length;
            if (lastBound > arrSize)
                lastBound = arrSize;
            var loopTo = lastBound - 1;
            for (temp = 0; temp <= loopTo; temp++)
                arrKey[temp] = Convert.ToByte(strKey[temp]);
            return arrKey;
        }
    }
}