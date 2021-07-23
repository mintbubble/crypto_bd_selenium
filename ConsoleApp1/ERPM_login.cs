using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using System.Data.SQLite;
using System.Threading;


namespace ConsoleApp1
{
    class ERPM_login
    {
        //FirefoxDriver driver;
        IWebDriver driver;

        public ERPM_login()
        {
            this.driver = new InternetExplorerDriver();
            this.driver.Manage().Window.Maximize();
        }
        public bool Login(string login, string password, string url, string ip)
        {
            //driver = new InternetExplorerDriver();
            Thread tread = new Thread(() =>
            {
                IntPtr hwnd_login;
                IntPtr hwnd_password;
                IntPtr hwnd_btn_ok;
                IntPtr htmp;
                IntPtr[] child = new IntPtr[15];
                DateTime start = DateTime.Now;
                IntPtr ptr;
                do
                {
                    if ((WinAPI.FindWindow(null, "Windows Security")).ToInt32() != 0)
                    {
                        ptr = WinAPI.FindWindow(null, "Windows Security");
                    }
                    else { ptr = WinAPI.FindWindow(null, "Безопасность Windows"); }
                    Thread.Sleep(new TimeSpan(0, 0, 10));
                }
                while (ptr.ToInt32() == 0 && ((DateTime.Now - start) < new TimeSpan(0, 5, 0)));
                //Если окно найдено, то обращаемся к его дочерним объектам
                if (ptr.ToInt32() != 0)
                {
                    child[0] = WinAPI.GetWindow(ptr, WinAPI.GetWindow_Cmd.GW_CHILD);
                    child[1] = WinAPI.GetWindow(child[0], WinAPI.GetWindow_Cmd.GW_CHILD);
                    for (int i = 2; i < 8; i++) child[i] = WinAPI.GetWindow(child[i - 1], WinAPI.GetWindow_Cmd.GW_HWNDNEXT);
                    //получение первого эдита
                    htmp = WinAPI.GetWindow(child[7], WinAPI.GetWindow_Cmd.GW_CHILD);
                    //формирование параметров
                    uint LParam = WinAPI.MapVirtualKey((uint)WinAPI.VK_Codes.VK_DOWN, 0) << 16 | 1;
                    uint LParam2 = (uint)(1 << 31 | 1 << 30 | WinAPI.MapVirtualKey((uint)WinAPI.VK_Codes.VK_DOWN, 0) << 16 | 1);
                    //переключаемся на нижний блок
                    WinAPI.PostMessage(htmp, Convert.ToInt32(WinAPI.GetWindow_Cmd.WM_SETFOCUS), (IntPtr)0, 0);
                    WinAPI.PostMessage(htmp, Convert.ToInt32(WinAPI.GetWindow_Cmd.WM_KEYDOWN), (IntPtr)WinAPI.VK_Codes.VK_DOWN, LParam);
                    WinAPI.PostMessage(htmp, Convert.ToInt32(WinAPI.GetWindow_Cmd.WM_KEYUP), (IntPtr)WinAPI.VK_Codes.VK_DOWN, LParam2);
                    WinAPI.SendMessage(child[0], Convert.ToInt32(WinAPI.GetWindow_Cmd.WM_LBUTTONDOWN), IntPtr.Zero, 0x00D60043);
                    WinAPI.SendMessage(child[0], Convert.ToInt32(WinAPI.GetWindow_Cmd.WM_LBUTTONUP), IntPtr.Zero, 0x00D60043);
                    //допулучаем другие контролы
                    for (int i = 8; i < 10; i++)
                    {
                        child[i] = WinAPI.GetWindow(child[i - 1], WinAPI.GetWindow_Cmd.GW_HWNDNEXT);
                        if (child[i].ToInt32() == 0) i -= 1;
                    }
                    hwnd_btn_ok = WinAPI.GetWindow(child[3], WinAPI.GetWindow_Cmd.GW_CHILD);
                    hwnd_login = WinAPI.GetWindow(child[8], WinAPI.GetWindow_Cmd.GW_CHILD);
                    hwnd_password = WinAPI.GetWindow(child[9], WinAPI.GetWindow_Cmd.GW_CHILD);
                    WinAPI.PostMessage(hwnd_login, Convert.ToInt32(WinAPI.GetWindow_Cmd.WM_SETFOCUS), (IntPtr)0, 0);

                    WinAPI.SendMessage(hwnd_login, Convert.ToInt32(WinAPI.GetWindow_Cmd.WM_SETTEXT), IntPtr.Zero, string.Format("Alpha\\{0}", login));
                    WinAPI.PostMessage(hwnd_password, Convert.ToInt32(WinAPI.GetWindow_Cmd.WM_SETFOCUS), (IntPtr)0, 0);
                    //password = "11111";
                    WinAPI.SendMessage(hwnd_password, Convert.ToInt32(WinAPI.GetWindow_Cmd.WM_SETTEXT), IntPtr.Zero, password);
                    WinAPI.PostMessage(hwnd_btn_ok, Convert.ToInt32(WinAPI.GetWindow_Cmd.WM_LBUTTONDOWN), IntPtr.Zero, 0x10001);
                    WinAPI.PostMessage(hwnd_btn_ok, Convert.ToInt32(WinAPI.GetWindow_Cmd.WM_LBUTTONUP), IntPtr.Zero, 0x10001);
                    Console.WriteLine("winAPI part - done");
                }
            });

            tread.Start();
            try
            {
                //string ip = "10.112.240.7";
                //string cmd = string.Format(@"cmdkey /generic:TERMSRV/{0} /user:alpha\{1} /pass:{2}", ip, login,password);
                //Process.Start("cmd.exe",cmd);
                //ProcessStartInfo psi = new ProcessStartInfo();
                //psi.FileName = "cmd";
                //psi.Arguments = cmd;
                //Process.Start(psi);
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
                driver.Navigate().GoToUrl(url);//"https://erpm/Login.asp"
                driver.FindElement(By.Id("Username")).SendKeys(login);
                driver.FindElement(By.Id("Password")).SendKeys(password);
                var domain = driver.FindElement(By.Id("DomainSelect"));
                var select_domain = new SelectElement(domain);
                select_domain.SelectByValue("ALPHA");
                driver.FindElement(By.XPath("//*[@language='LOCALIZED_STRING' and text()='Вход в систему']")).Click();
                //Запустить приложение по ссылке
                Thread.Sleep(new TimeSpan(0, 0, 1));
                driver.Navigate().GoToUrl("https://erpm/LaunchApp.asp?AccountType=Windows&HideShadowAccountApps=&Namespace=V%2DUVHD%2D12R2%2D004&SystemName=V%2DUVHD%2D12R2%2D004");
                //жмем система
                //driver.FindElement(By.Id("ThemeClassic/Computer.jpg")).Click();
                //жмем Запустить приложение
                //Thread.Sleep(new TimeSpan(0, 0, 3));
                //driver.FindElement(By.LinkText("Запустить приложение")).Click();
                Thread.Sleep(new TimeSpan(0, 0, 3));
                driver.FindElement(By.XPath("//*[@language='LOCALIZED_STRING' and text()='Запуск']")).Click();
                Console.WriteLine("selenium part - done");
               // driver.Close();
                return true;
            }
            catch(Exception TypeError)
            {
                Console.WriteLine(TypeError.Message);
                return false;
            }
        }
        
    }
}
