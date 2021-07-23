using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace ConsoleApp1
{
    class bd_work
    {
        public static string SendUserDB(string login, string password)
        {

            try
            {
                //string dbpath = @"D:\distr\SQLiteStudio\users_login_data";
                string dbpath = @"D:\distr\SQLiteStudio\users_login_data";
                SQLiteConnection sqlconnect = new SQLiteConnection(string.Format("Data Source={0};", dbpath));
                sqlconnect.Open();
                try
                {
                    SQLiteCommand check_command = new SQLiteCommand(String.Format("SELECT LOGIN FROM 'LOGIN_CHIPHER' WHERE LOGIN = '{0}';", login), sqlconnect);
                    var rez = check_command.ExecuteScalar();
                    //Console.WriteLine(rez);
                    //SQLiteCommand add_command = new SQLiteCommand(String.Format("INSERT INTO 'LOGIN_CHIPHER' ('LOGIN', 'CHIPHER') VALUES ('{0}', '{1}');", login, password), sqlconnect);
                    //SQLiteCommand delete_command = new SQLiteCommand(String.Format("DELETE FROM LOGIN_CHIPHER WHERE LOGIN = '{0}';", login), sqlconnect);
                    if (rez == null)
                    {
                        SQLiteCommand add_command = new SQLiteCommand(String.Format("INSERT INTO 'LOGIN_CHIPHER' ('LOGIN', 'CHIPHER') VALUES ('{0}', '{1}');", login, password), sqlconnect);
                        //SQLiteCommand addcommand = new SQLiteCommand("INSERT INTO 'LOGIN_CHIPHER' ('LOGIN', 'CHIPHER') VALUES ('lodld11', 'hjsjs11');", sqlconnect);
                        add_command.ExecuteNonQuery();
                        sqlconnect.Close();
                        Console.WriteLine("login was add");
                        return "login was add";
                    }
                    else
                    {
                        SQLiteCommand update_command = new SQLiteCommand(String.Format("UPDATE LOGIN_CHIPHER SET CHIPHER = '{0}' WHERE LOGIN = '{1}';", password, login), sqlconnect);
                        //SQLiteCommand delete_command = new SQLiteCommand(String.Format("DELETE FROM 'LOGIN_CHIPHER' WHERE 'LOGIN' = '{0}';", login), sqlconnect);
                        update_command.ExecuteNonQuery();
                        //delete_command.ExecuteNonQuery();
                        //add_command.ExecuteNonQuery();
                        sqlconnect.Close();
                        Console.WriteLine("Password update");
                        return "password was update";
                    }

                }
                catch ( Exception e) { return e.Message; }

                //return "OK";
            }
            catch (Exception e) { return e.Message; }

        }
        public static string GetFromDB(string login)
        {
            try
            {
                //string dbpath = @"D:\distr\SQLiteStudio\users_login_data";
                string dbpath = @"D:\distr\SQLiteStudio\users_login_data";
                SQLiteConnection sqlconnect = new SQLiteConnection(string.Format("Data Source={0};", dbpath));
                sqlconnect.Open();
                SQLiteCommand get_pass = new SQLiteCommand(String.Format("SELECT CHIPHER FROM 'LOGIN_CHIPHER' WHERE LOGIN = '{0}';", login), sqlconnect);
                var rez = get_pass.ExecuteScalar();
                //return Convert.ToBase64String(Convert.FromBase64String(rez.ToString()));
                return rez.ToString();
                
            }
            catch (Exception e) { return e.Message; }
        }
    }
}
