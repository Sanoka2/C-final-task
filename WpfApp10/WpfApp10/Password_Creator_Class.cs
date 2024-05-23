using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp10
{
    internal class Password_Creator_Class
    {
        private string salt;
        private int salt_Length;
        private byte[] ascii_Range;
        private int Password_Length;

        public Password_Creator_Class(string salt, byte[] ascii_Range, int Password_Length)
        {
            this.salt = salt;
            this.salt_Length = salt.Length;
            this.ascii_Range = ascii_Range;
            this.Password_Length = Password_Length;
        }

        public static byte[] Compute_Hash(string password_s)
        {
            byte[] password = Encoding.ASCII.GetBytes(password_s);
            return new System.Security.Cryptography.SHA256Managed().ComputeHash(password);
        }

        public string Generate_Password()
        {
            Random rand = new Random();
            int length = Password_Length;

            byte[] password = new byte[length];
            for (int i = 0; i < length; i++)
            {
                password[i] = ascii_Range[rand.Next(ascii_Range.Length)];
            }
            string res0 = Encoding.UTF8.GetString(password);
            string res = res0 + salt;
            return res;
        }
    }
}
