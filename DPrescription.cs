﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace Hospital
{
    public partial class DPrescription : UserControl
    {
        DoctorFunctions df = new DoctorFunctions();
        public DPrescription()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            //ADD Prescription By Doctor
            int patient_id = Convert.ToInt32(PID.Text);
            string med = medicine.Text;
            df.MakePrescription(patient_id, med);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            //See Presc
            DataTable dt= df.GetPatient(PID.Text, false);
            if (dt != null)
            {
                dt.Columns.Remove("Id");
                dt.Columns.Remove("DId");
                dt.Columns.Remove("Dname");
            }
            dataGridView1.DataSource = dt;
            for (int i = 0; i < dataGridView1.Columns.Count; i++)
                dataGridView1.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }
    }
}