using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Firefox;
using System.Security.Cryptography;
using System.Threading;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            //IWebDriver driver = new InternetExplorerDriver();
            string login = "trofimov-ak";
            string get_pass_bd = bd_work.GetFromDB(login);
            var password_decrypt = CryptoClass.Decrypt(bd_work.GetFromDB(login), "super_key");
            string ip = "10.112.240.7";
            string url = "https://erpm/Login.asp";
            ERPM_login page = new ERPM_login();
            var value = page.Login(login, password_decrypt, url, ip);
            Console.Write("ERPM_login end");


        }
    }
}
