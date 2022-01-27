using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace IOTEndToEnd
{
    public class HomeController : Controller
    {
        // GET: HomeController
        public ActionResult Index()
        {
            return View();
        }
        [Route("home/rimoFunc")]
        public string rimoFunc(string value)
        {
            TimeZoneInfo zone = TimeZoneInfo.FindSystemTimeZoneById("Pakistan Standard Time");
            var time = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, zone);
            return "Value: " + value + " :Size: " + value.Length + " :Time: " + time.TimeOfDay + " :Date: " + DateTime.Now.ToLongDateString() + ":Bytes Length:" + Encoding.ASCII.GetBytes(value).Length;
        }
        [Route("Home/RimoEncrypt")]
        public async Task<string> RimoEncrypt(string clearText)
        {
            string copylocal = clearText;
            string EncryptionKey = "Xder$3dvBngFd#45TgfdCvf5$dfgdDSDFG435#rdtfdSCf.";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x22, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                // encryptor.KeySize = 256;
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        await cs.WriteAsync(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            TimeZoneInfo zone = TimeZoneInfo.FindSystemTimeZoneById("Pakistan Standard Time");
            var time = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, zone);
            return clearText + ":Size After:" + clearText.Length + " :Time: " + time.TimeOfDay + " :Date: " + DateTime.Now.ToLongDateString() + ":Bytes Length:" + Encoding.ASCII.GetBytes(clearText).Length;
        }
        [Route("Home/RimoDecrypt")]
        public async Task<string> RimoDecrypt(string cipherText)
        {
            string copyLocal = cipherText;
            string EncryptionKey = "Xder$3dvBngFd#45TgfdCvf5$dfgdDSDFG435#rdtfdSCf.";
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x22, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                // encryptor.KeySize = 256;

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        await cs.WriteAsync(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            TimeZoneInfo zone = TimeZoneInfo.FindSystemTimeZoneById("Pakistan Standard Time");
            var time = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, zone);
            return cipherText + ":Size After:" + cipherText.Length + " :Time: " + time.TimeOfDay + " :Date: " + DateTime.Now.ToLongDateString() + ":Bytes Length:" + Encoding.ASCII.GetBytes(cipherText).Length;
        }

    }
}
