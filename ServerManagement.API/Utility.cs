using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RSACryptoPad;

namespace ServerManagement.API
{
    public class Utility
    {
        private const int KeySize = 1024;

        public static string Decrypt(string text, string keyPairPath)
        {
            var encryptionThread = new EncryptionThread();
            return encryptionThread.DecryptString(text, KeySize, keyPairPath);
        }
    }
}