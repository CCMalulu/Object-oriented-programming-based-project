﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Data;
namespace Hospital
{
    class DoctorFunctions:MakeConnection
    {
        DataTable dt;
        public DataTable GetAllAppointments(string ser)
        {
            try
            {
                dt = new DataTable();
                cmd.Connection = con;
                //if 0 then show all appoints
                if(ser.Length.Equals(0))
                    cmd.CommandText = "select * from Appointsments where Approved_or_not='false'";
                else
                    cmd.CommandText = "select * from Appointsments where PatientId like @ser+'%' or PName like @ser+'%'";
                con.Open();
                cmd.Parameters.AddWithValue("@Approved_or_not", false);
                cmd.Parameters.AddWithValue("@ser",ser);
                SqlDataReader r = cmd.ExecuteReader();
                dt.Load(r);
                dt.Columns.Remove("Id");
                dt.Columns.Remove("DoctorId");
                dt.Columns.Remove("Approved_or_not");
                dt.Columns[1].ColumnName = "Patient Name";
                dt.Columns[2].ColumnName = "Consultant";
                dt.Columns[3].ColumnName = "Date of Appointment";
                return dt;
            }
            catch (Exception)
            {
                MessageBox.Show("Error in getting details", "Error");
                return null;
            }
            finally
            {
                cmd.Parameters.RemoveAt("@Approved_or_not");
                cmd.Parameters.RemoveAt("@ser");
                con.Close();
            }
        }
        //Search Algorithm
        public DataTable GetPatient(string searchString,bool PData)
        {
            //if PData=true=>select from Patient_record,else select from Patient_Presc
           try
            {
                dt = new DataTable();
                cmd.Connection = con;
                if (PData)
                    cmd.CommandText = "select * from Patient_Record WHERE Id like @searchString+'%' or PName like @searchString+'%' or PAddress like @searchString+'%' or PContact like @searchString+'%'";
                else
                    cmd.CommandText = "select * from Patient_Presc where PId=@searchString";
                con.Open();
                cmd.Parameters.AddWithValue("@searchString", searchString);
                SqlDataReader r = cmd.ExecuteReader();
                dt.Load(r);
                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("No Data Found", "Info");
                    return null;
                }
                dt.Columns[0].ColumnName = "PatientId";
                dt.Columns[1].ColumnName = "Patient Name";
                dt.Columns[3].ColumnName = "Residential Address";
                dt.Columns[4].ColumnName = "Age";
                dt.Columns[8].ColumnName = "Details";
                dt.Columns[9].ColumnName = "Addmission Date";
                dt.Columns[10].ColumnName = "Discharge Date";
                return dt;
            }
            catch (Exception)
            {
                MessageBox.Show("Something went wrong,Please try again","Error");
                return null;
            }
            finally
            {
                con.Close();
                cmd.Parameters.RemoveAt("@searchString");
            }
        }
        public void MakePrescription(int p_id,string medicine)
        {
            cmd.Connection = con;
            try
            {
                DataTable x = GetPatient(p_id.ToString(),true);
                string pa_name = (from DataRow dr in x.Rows
                                  where (int)dr["Id"] == p_id
                                  select (string)dr["PName"]).FirstOrDefault();
                Console.WriteLine(pa_name);
                Console.WriteLine();
                int Uid = SessionClass.SessionId;
                cmd.CommandText = "select * from Users where Id=@Uid";
                con.Open();
                SqlDataReader r = cmd.ExecuteReader();
                string uname = "";
                while (r.Read())
                     uname = r["Name"].ToString();
                con.Close();
                cmd.CommandText = "insert into Patient_Presc(PId,PName,Prescprition,Did,Dname) Values(@p_id,@pa_name,@medicine,@Uid,@uname)";
                con.Open();
                cmd.Parameters.AddWithValue("@p_id", p_id);
                cmd.Parameters.AddWithValue("@pa_name", pa_name);
                cmd.Parameters.AddWithValue("@medicine", medicine);
                cmd.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("Success", "Done");
            }
            catch (Exception e)
            {
                MessageBox.Show("Patient data is not available", "Info");
            }
        }
    }
}