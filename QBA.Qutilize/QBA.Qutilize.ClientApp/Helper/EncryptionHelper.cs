using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBA.Qutilize.ClientApp.Helper
{
    public class EncryptionHelper
    {
        public string Encryptdata(string password)
        {
            if (password != null && Convert.ToString(password) != string.Empty)
            {
                string strmsg = string.Empty;
                byte[] encode = new byte[password.Length];
                encode = Encoding.UTF8.GetBytes(password);
                strmsg = Convert.ToBase64String(encode);
                return strmsg;
            }
            else return "";
        }
        public string Decryptdata(string encryptpwd)
        {
            if (encryptpwd != null && Convert.ToString(encryptpwd) != string.Empty)
            {
                string decryptpwd = string.Empty;
                UTF8Encoding encodepwd = new UTF8Encoding();
                Decoder Decode = encodepwd.GetDecoder();
                byte[] todecode_byte = Convert.FromBase64String(encryptpwd);
                int charCount = Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
                char[] decoded_char = new char[charCount];
                Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
                decryptpwd = new String(decoded_char);
                return decryptpwd;
            }
            else return "";
        }


        #region One way Encryption using MD5
        //Password encryption alogorthm for one way encryption (cannot decrypted) using MD5
        public static string ConvertStringToMD5(string sPassword)
        {
            if (Convert.ToString(sPassword) != string.Empty && sPassword != null)
            {
                System.Security.Cryptography.MD5CryptoServiceProvider x = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] bs = System.Text.Encoding.UTF8.GetBytes(sPassword);
                bs = x.ComputeHash(bs);
                System.Text.StringBuilder s = new System.Text.StringBuilder();
                foreach (byte b in bs)
                {
                    s.Append(b.ToString("x2").ToLower());
                }
                return s.ToString();
            }
            else
            {
                return "";
            }
        }
        #endregion
    }
}
