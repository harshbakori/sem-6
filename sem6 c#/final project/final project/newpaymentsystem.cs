﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace final_project
{
    class evol
    {
        public const int minimumamount = 5000;
        public static string password, number;
        public static bool adminlogin = false;
        public static bool loginflag = false;
        public static void logout() { loginflag = false; adminlogin = false; }
        public void main()
        {
            
            Console.WriteLine("1)diposit");
            Console.WriteLine("2)check balance");
            Console.WriteLine("3)pay");
            Console.WriteLine("4)logout");
            Console.WriteLine("enter your choice");
            int ch = Convert.ToInt32(Console.ReadLine());
            switch(ch)
            {
                case 1:
                    deposit();
                    break;
                case 2:
                    check_ballance();
                    break;
                case 3:
                    pay();
                    break;
                case 4:
                    logout();
                    break;
            }
        }
        public void pay()
        {
            Console.WriteLine("pay to");
            string usr = Console.ReadLine();
            Console.WriteLine("enter amount to pay");
            int ball = Convert.ToInt32(Console.ReadLine());

            SqlConnection con = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = 'C:\Users\admin\source\repos\final project\final project\Database1.mdf'; Integrated Security = True");
            //////////////////////////////////////////////////////////////pay to client//////////////////////////////////////////////////////////////
            SqlDataAdapter adp1 = new SqlDataAdapter("select * from Login where number ='" + usr + "'", con); /////select query for database
            DataSet ds1 = new DataSet();
            adp1.Fill(ds1, "Login");
            int sba2 = Convert.ToInt32(ds1.Tables["Login"].Rows[0]["ballance"]); //////////////////fech old diposit and enter into sball
            sba2 = sba2 + ball; /////////////////////////increas ballance with sball
            ds1.Tables["Login"].Rows[0]["ballance"] = sba2; ////////////////enter ballance into database
            SqlCommandBuilder scmd1 = new SqlCommandBuilder(adp1);
            adp1 = scmd1.DataAdapter;
            adp1.Update(ds1, "Login"); /////////////update the ds
            Console.WriteLine("Record updated");
            ///////////////////////////////////////////////////////////diduct the amount////////////////////////////////////////////////////////////////////////

            SqlDataAdapter adp = new SqlDataAdapter("select * from Login where number ='" + number + "'", con); /////select query for database
            DataSet ds = new DataSet();
            adp.Fill(ds, "Login");
            int sbal = Convert.ToInt32(ds.Tables["Login"].Rows[0]["ballance"]); //////////////////fech old diposit and enter into sball
            ball = sbal - ball; /////////////////////////dicreas ballance with sball
                if(ball<30)
            {
                Console.WriteLine("insaficiant funds please try again");
                goto inf;
            }
            ds.Tables["Login"].Rows[0]["ballance"] = ball; ////////////////enter ballance into database
            SqlCommandBuilder scmd = new SqlCommandBuilder(adp);
            adp = scmd.DataAdapter;
            adp.Update(ds, "Login"); /////////////update the ds
            Console.WriteLine("Record updated"); ////////////update into database
            Console.WriteLine("new ballance is" + ds.Tables["Login"].Rows[0]["ballance"]);///////////////print current ballance
         inf:
            Console.ReadKey();
        }
        public void check_ballance()
        {
           
            SqlConnection con = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = 'C:\Users\admin\source\repos\final project\final project\Database1.mdf'; Integrated Security = True");
            SqlDataAdapter adp = new SqlDataAdapter("select * from Login where number ='" + number + "'", con);
            DataSet ds = new DataSet();
            adp.Fill(ds, "Login");
            Console.WriteLine(" ballance is" + ds.Tables["Login"].Rows[0]["ballance"]);///////////////print current ballance
            Console.ReadKey();
        }
        public void deposit()
        {
            Console.WriteLine("enter amount to diposit");
            int ball = Convert.ToInt32(Console.ReadLine());
            SqlConnection con = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = 'C:\Users\admin\source\repos\final project\final project\Database1.mdf'; Integrated Security = True");
            SqlDataAdapter adp = new SqlDataAdapter("select * from Login where number ='"+number+"'", con);/////select query for database
            DataSet ds = new DataSet();
            adp.Fill(ds, "Login");
            int sbal = Convert.ToInt32(ds.Tables["Login"].Rows[0]["ballance"]); //////////////////fech old diposit and enter into sball
            ball = sbal + ball; /////////////////////////increas ballance with sball
            ds.Tables["Login"].Rows[0]["ballance"] = ball; ////////////////enter ballance into database
            SqlCommandBuilder scmd = new SqlCommandBuilder(adp);
            adp = scmd.DataAdapter;
            adp.Update(ds, "Login"); /////////////update the ds
            Console.WriteLine("Record updated"); ////////////update into database
            Console.WriteLine("new ballance is"+ds.Tables["Login"].Rows[0]["ballance"]);///////////////print current ballance
            Console.ReadKey();
        }
    }
    class login1 : evol
    {
        public static void ragister()
        {
            /////////////////////////////////////////user data entry ////////////////////////////////////////
            relogin:
            Console.WriteLine("enter new user number");
            number = Console.ReadLine();
            Console.WriteLine("enter password");
            password = Console.ReadLine();
            Console.WriteLine("re enter password");
            String re_password = Console.ReadLine();
            if (password.Equals(re_password))
            {
            rebalance:
            Console.WriteLine("balance pleas");
            int balance = Convert.ToInt32(Console.ReadLine());
            if (balance < minimumamount)
            {
                Console.WriteLine("pleas re-enter balance above 5000");
                goto rebalance;
            }
            ////////////////////////////user data enterd by admin to ragister////////////////////////////////
            SqlConnection con = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = 'C:\Users\admin\source\repos\final project\final project\Database1.mdf'; Integrated Security = True");
            SqlDataAdapter adp = new SqlDataAdapter("select * from login", con);  ///fech all the entryes in the table login
            DataSet ds = new DataSet();
            adp.FillSchema(ds, SchemaType.Source, "login"); //////initializing schema of the ds
            adp.Fill(ds); ///set entryes in data adapter
            DataRow dr = ds.Tables["login"].NewRow(); //entering new row to ds//////
            dr["number"] = number;
            dr["password"] = password;
            dr["ballance"] = balance;
            ds.Tables["login"].Rows.Add(dr);
            SqlCommandBuilder scmb = new SqlCommandBuilder(adp); //////built the update query
            adp = scmb.DataAdapter; /////set the data adapter for updation
            adp.Update(ds, "login"); ////  update adp use data in login table from dataset
            Console.Clear();
            Console.WriteLine("you are ragistered");
            }
            else
            {
                Console.WriteLine("please enter correct password and ragister again");
                goto relogin;
            }
        }
        public static void login()
        {
        retry:

            SqlConnection con = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = 'C:\Users\admin\source\repos\final project\final project\Database1.mdf'; Integrated Security = True");
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            ///////////////////////////////////////////////////connected to database/////////////////////////////////////////////////////////////////
            Console.WriteLine("connected");
            Console.WriteLine("Enter user number");
            number = Console.ReadLine();
            Console.WriteLine("enter the password");
            password = Console.ReadLine();
            if (number.Equals("Admin") && password.Equals("admin"))
            {
                adminlogin = true;
                goto end;                         //goto end of login function
            }
            /////////////user input complite////////////
            cmd.CommandText = "select * from login where number =" + number + "";   /// find number in database and fatch data 
            SqlDataReader reader = cmd.ExecuteReader(); //execute the query to read
            if (reader.HasRows)
            {
                reader.Read(); //read data from sql command 
                if (reader.GetString(1).Equals(password))  //check for password correction
                {
                    Console.WriteLine("login");
                    loginflag = true;
                }
                else { Console.Clear(); Console.WriteLine("incorrect password"); }
            }
            else { Console.Clear(); Console.WriteLine("enter correct usernumber"); }
        end:
            // jump for admin login
            con.Close();
            if(adminlogin==true || loginflag==true)
            {
                Console.Clear();
            Console.WriteLine("you are logedin");
            evol e = new evol();
            e.main();
            }
            else { goto retry; }
            //////////////////////////////////////////////////////////////////////////////////////////////////
            Console.ReadKey();
        }
    }
    class newpaymentsystem : login1
    {
        static void Main(string[] args)
        {
        // showdata();
            one:
                Console.WriteLine("enter your choice \n1)login \n2)ragister");
                int ch = Convert.ToInt32(Console.ReadLine());
                switch (ch)
                {
                    case 1:
                        login();
                        break;
                    case 2:
                        ragister(); 
                        break;
                }
            goto one;
        }
    }
}
