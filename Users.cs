﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital
{
    class Users:MakeConnection
    {
        public int Login(string uname, string pass)
        {
            cmd.Connection = con;
            cmd.CommandText = "Select * from Users where Name=@uname";
                SqlParameter p = new SqlParameter();
                p.ParameterName = "@uname";
                p.Value = uname;
                cmd.Parameters.Add(p);
                con.Open();
                SqlDataReader r = cmd.ExecuteReader();
                while (r.Read())
                {
                    string x = r["Name"].ToString();
                    string y = r["Password"].ToString();
                    int Uid = Convert.ToInt32(r["Id"]);
                    Console.WriteLine(x, y);
                    if (y != pass || x == null)
                        return -99;
                    else
                        return Uid;
                }
                return -99;
        }
    }
}