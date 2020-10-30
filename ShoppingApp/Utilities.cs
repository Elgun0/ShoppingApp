using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingApp
{
    public static class Utilities
    {
        public static bool IsEmpty(string[] myarr,string text)
        {
            foreach (var arr in myarr)
            {
                if (arr == text)
                {
                    return false;
                }
            }
            return true;
        }

        public static string HashMe(this string pas)
        {
            byte[] mypasByte = new ASCIIEncoding().GetBytes(pas);
            byte[] HassedPas = new SHA256Managed().ComputeHash(mypasByte);
            string hashedString = new ASCIIEncoding().GetString(HassedPas);
            return hashedString;
        }
    }
}
