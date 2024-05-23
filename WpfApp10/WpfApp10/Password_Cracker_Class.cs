using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp10
{
    internal class Password_Cracker_Class
    {
        private byte[] Combination_To_Password(int number, int base_Value, byte[] ascii_Range)
        {
            int length = (int)Math.Ceiling(Math.Log(number + 1, base_Value));
            byte[] result = new byte[length];
            int index = length - 1;
            int char_Index;
            do
            {//it is converation to number with base of 74 and take respective character from ASCII range
                char_Index = number % base_Value;
                result[index] = ascii_Range[char_Index];
                number /= base_Value;
                index--;
            }
            while (number > 0);

            return result;
        }

        private bool Check_Password(byte[] test_Password_b, byte[] target_Hash)
        {
            string test_Passwor_s = Encoding.ASCII.GetString(test_Password_b);
            byte[] computed_Hash = Password_Creator_Class.Compute_Hash(test_Passwor_s);
            return computed_Hash.SequenceEqual(target_Hash);
        }

        public string Find_Password(byte[] ascii_Range, int Password_Length, string salt, byte[] target_Hash)
        {
            int max_Combinations = (int)Math.Pow(ascii_Range.Length, Password_Length);

            byte[] salt_Bytes = Encoding.UTF8.GetBytes(salt);
            for (int i = 1; i < max_Combinations; i++)
            {
                byte[] generated_Password = Combination_To_Password(i, ascii_Range.Length,ascii_Range);
                byte[] test_Password = new byte[generated_Password.Length + salt_Bytes.Length];

                for (int a = 0; a < generated_Password.Length; a++)
                {
                    test_Password[a] = generated_Password[a];
                }
                for (int b = 0; b < salt_Bytes.Length; b++)
                {
                    int c = b + generated_Password.Length;
                    test_Password[c] = salt_Bytes[b];
                }

                if (Check_Password(test_Password, target_Hash))
                {
                    return Encoding.UTF8.GetString(test_Password);
                }
            }
            return "Password not found";
        }
    }
}
