using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace HashDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            //var md5 = MD5.Create();

            //string pass = "PASSWORD";
            //string salt = "ABCD";

            //var h = md5.ComputeHash(Encoding.ASCII.GetBytes(pass + salt));

            //string computedHash = BitConverter.ToString(h);

            //Console.WriteLine(computedHash.Replace("-", ""));

            byte[] salt;

            //Generate a SALT
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);

            var pbkdf2 = new Rfc2898DeriveBytes("PASSWORD", salt, 10000);

            var hash = pbkdf2.GetBytes(20);

            byte[] hashbyte = new byte[36];

            Array.Copy(salt, 0, hashbyte, 0, 16);
            Array.Copy(hash, 0, hashbyte, 16, 20);

            string resulthash = Convert.ToBase64String(hashbyte);

            Console.WriteLine(resulthash);

            //from database
            byte[] verifybytes = Convert.FromBase64String(resulthash);

            byte[] verifysalt = new byte[16];

            Array.Copy(verifybytes, 0, verifysalt, 0, 16);

            var pbkdf2_B = new Rfc2898DeriveBytes("PASSWORD", verifysalt, 10000);

            byte[] hash2 = pbkdf2_B.GetBytes(20);

            bool ok = true;
            for (int i = 0; i < 20; i++)
            {
                if (verifybytes[i + 16] != hash2[i])
                {
                    ok = false;
                    break;
                }
            }

            Console.Write("RESULT IS ");
            Console.WriteLine(ok ? "PASS" : "FAIL");

            Console.ReadKey();

        }
    }
}
