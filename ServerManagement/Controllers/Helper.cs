using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web.Hosting;
using Newtonsoft.Json;
using RSACryptoPad;
using ServerManagement.Models;

namespace ServerManagement.Controllers
{
    public class Helper
    {
        private static byte[] _salt = Encoding.ASCII.GetBytes("o6806642kbM7c5");
        private string _sharedSecret = "nexcel@2015";
        private const int KeySize = 1024;

        /// <summary>
        /// Encrypt the given string using AES.  The string can be decrypted using
        /// DecryptStringAES().  The sharedSecret parameters must match.
        /// </summary>
        /// <param name="plainText">The text to encrypt.</param>
        /// <param name="sharedSecret">A password used to generate a key for encryption.</param>
        public string EncryptStringAES(string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
                throw new ArgumentNullException("plainText");
            if (string.IsNullOrEmpty(_sharedSecret))
                throw new ArgumentNullException("sharedSecret");

            string outStr = null;                       // Encrypted string to return
            RijndaelManaged aesAlg = null;              // RijndaelManaged object used to encrypt the data.

            try
            {
                // generate the key from the shared secret and the salt
                Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(_sharedSecret, _salt);

                // Create a RijndaelManaged object
                aesAlg = new RijndaelManaged();
                aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);

                // Create a decryptor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    // prepend the IV
                    msEncrypt.Write(BitConverter.GetBytes(aesAlg.IV.Length), 0, sizeof(int));
                    msEncrypt.Write(aesAlg.IV, 0, aesAlg.IV.Length);
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                    }
                    outStr = Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
            finally
            {
                // Clear the RijndaelManaged object.
                if (aesAlg != null)
                    aesAlg.Clear();
            }

            // Return the encrypted bytes from the memory stream.
            return outStr;
        }

        /// <summary>
        /// Decrypt the given string.  Assumes the string was encrypted using
        /// EncryptStringAES(), using an identical sharedSecret.
        /// </summary>
        /// <param name="cipherText">The text to decrypt.</param>
        /// <param name="sharedSecret">A password used to generate a key for decryption.</param>
        public string DecryptStringAES(string cipherText)
        {
            if (string.IsNullOrEmpty(cipherText))
                throw new ArgumentNullException("cipherText");
            if (string.IsNullOrEmpty(_sharedSecret))
                throw new ArgumentNullException("sharedSecret");

            // Declare the RijndaelManaged object
            // used to decrypt the data.
            RijndaelManaged aesAlg = null;

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            try
            {
                // generate the key from the shared secret and the salt
                Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(_sharedSecret, _salt);

                // Create the streams used for decryption.
                byte[] bytes = Convert.FromBase64String(cipherText);
                using (MemoryStream msDecrypt = new MemoryStream(bytes))
                {
                    // Create a RijndaelManaged object
                    // with the specified key and IV.
                    aesAlg = new RijndaelManaged();
                    aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);
                    // Get the initialization vector from the encrypted stream
                    aesAlg.IV = ReadByteArray(msDecrypt);
                    // Create a decrytor to perform the stream transform.
                    ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                    }
                }
            }
            finally
            {
                // Clear the RijndaelManaged object.
                if (aesAlg != null)
                    aesAlg.Clear();
            }

            return plaintext;
        }

        public DateTime ConvertDateTimeToGMT7(string localDate)
        {
            if (!string.IsNullOrWhiteSpace(localDate))
            {
                var dateTime = Convert.ToDateTime(localDate);
                var hoursOffset = TimeZone.CurrentTimeZone.GetUtcOffset(dateTime).Hours;
                var gmtDateTime = dateTime.AddHours((double)(0 - hoursOffset));
                var userDateTime = gmtDateTime.AddHours(7);

                return userDateTime;
            }

            return DateTime.Now;
        }

        public Result RebootServer(string serverIp)
        {
            string stderrx = string.Empty;

            try
            {
                var process = new Process();

                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.RedirectStandardOutput = true;

                var startInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = "/c\"" + string.Format("shutdown /m \\\\{0} /f /t 00 /r", serverIp) + "\""
                };

                process.StartInfo = startInfo;
                process.Start();
                process.BeginOutputReadLine();
                StreamReader streamReader = process.StandardOutput;
                process.WaitForExit();
                process.Close();
                process.Dispose();

                return new Result { ErrorCode = process.ExitCode, ErrorMessage = streamReader.ReadToEnd() };
            }
            catch (Exception ex)
            {
                return new Result { ErrorCode = -1, ErrorMessage = ex.Message };
            }
        }

        public T QueryProxyData<T>(Uri uri, object data)
        {
            var encryptedData = EncryptParameters(data);

            using (var client = new HttpClient())
            {
                var parameter = "parameter=" + Uri.EscapeDataString(encryptedData);
                var content = new StringContent(parameter, Encoding.UTF8, "application/x-www-form-urlencoded");
                var result = client.PostAsync(uri, content)
                             .Result
                             .Content
                             .ReadAsStringAsync()
                             .Result;

                try
                {
                    return JsonConvert.DeserializeObject<T>(result);
                }
                catch (JsonReaderException exception)
                {
                    throw new ArgumentException(result, exception);
                }
            }
        }

        private static byte[] ReadByteArray(Stream s)
        {
            byte[] rawLength = new byte[sizeof(int)];
            if (s.Read(rawLength, 0, rawLength.Length) != rawLength.Length)
            {
                throw new SystemException("Stream did not contain properly formatted byte array");
            }

            byte[] buffer = new byte[BitConverter.ToInt32(rawLength, 0)];
            if (s.Read(buffer, 0, buffer.Length) != buffer.Length)
            {
                throw new SystemException("Did not read byte array properly");
            }

            return buffer;
        }

        private static string EncryptParameters(object data)
        {
            var jsonData = JsonConvert.SerializeObject(data);
            var keyPath = HostingEnvironment.ApplicationPhysicalPath + @"App_Data\public.snk";
            var encryptionThread = new EncryptionThread();
            return encryptionThread.EncryptString(jsonData, KeySize, keyPath);
        }
    }
}