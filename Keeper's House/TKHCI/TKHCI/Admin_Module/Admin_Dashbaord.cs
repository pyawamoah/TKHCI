﻿using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static TKHCI.TKHCI_Main_Login;

namespace TKHCI
{
    public partial class Admin_Dashbaord : Form
    {
        private Form currentChildForm;
        KeeperDBEntities DBS = new KeeperDBEntities();
        public Admin_Dashbaord()
        {
            InitializeComponent();
            new Guna2ShadowForm(this);
        }
        private void OpenChildForm(Form childForm)
        {
            currentChildForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            AppContainer.Controls.Add(childForm);
            AppContainer.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }
        public bool IsFormOpen(Type formType)
        {
            foreach (Form form in Application.OpenForms)
                if(form.GetType().Name == form.Name)
                    return true;
            return false;
        }

        private void Admin_Dashbaord_Load(object sender, EventArgs e)
        {
            AppStart_Menu.Visible = false;

            var user = DBS.UserTBs.FirstOrDefault(a => a.Username.Equals(MainLogin.Username));
            if (user != null)
            {
                userName_Login.Text = user.FName;
                byte[] img = user.Picture;
                MemoryStream ms = new MemoryStream(img);
                LogUserImage.Image = Image.FromStream(ms);

                StartMen_UserImg.Image = Image.FromStream(ms);
                userName_Login.Text = user.FName;
                StartMen_UserName.Text = user.Fullname;

            }

        }
        private void Bn_Start_Click(object sender, EventArgs e)
        {
            if (AppStart_Menu.Visible == false)
            {
                AppStart_Menu.Visible = true;

            }
            else if (AppStart_Menu.Visible == true)
            {
                AppStart_Menu.Visible = false;

            }
            else
            {
                AppStart_Menu.Visible = false;
            }
        }

        private void Bu_ShutDown_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Bn_SystemUser_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<Admin_Module.SysUser_CRM.Admin_sysUser_CRM_MAIN>().Count() == 1)
            {
                if (Application.OpenForms.OfType<Error_DoubleOpenForm>().Count() == 1)
                {
                    return;
                }
                else
                {
                    Error_DoubleOpenForm eropen = new Error_DoubleOpenForm();
                    eropen.Show();
                }

            }
            else
            {
                OpenChildForm(new Admin_Module.SysUser_CRM.Admin_sysUser_CRM_MAIN());
                AppStart_Menu.Visible = false;

            }
        }
        private void BT_Dept_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<Admin_Module.Department.DeptModule>().Count() == 1)
            {
                if (Application.OpenForms.OfType<Error_DoubleOpenForm>().Count() == 1)
                {
                    return;
                }
                else
                {
                    Error_DoubleOpenForm eropen = new Error_DoubleOpenForm();
                    eropen.Show();
                }

            }
            else
            {
                OpenChildForm(new Admin_Module.Department.DeptModule());
                AppStart_Menu.Visible = false;

            }
        }

        private void But_Reset_PWD_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<Admin_Password_ResetCount>().Count() == 1)
            {
                if (Application.OpenForms.OfType<Error_DoubleOpenForm>().Count() == 1)
                {
                    return;
                }
                else
                {
                    Error_DoubleOpenForm eropen = new Error_DoubleOpenForm();
                    eropen.Show();
                }

            }
            else
            {
                OpenChildForm(new Admin_Password_ResetCount());
                AppStart_Menu.Visible = false;

            }

        }

        private void Tx_SearchBar_MouseLeave(object sender, EventArgs e)
        {
            //Tx_SearchBar.Height = 25;
            //Tx_SearchBar.Width = 322; 
        }

        private void Tx_SearchBar_Click(object sender, EventArgs e)
        {
            //Tx_SearchBar.Height = 45;
            //Tx_SearchBar.Width = 510;
        }

        private void BU_Service_Click(object sender, EventArgs e)
        {

            if (Application.OpenForms.OfType<Admin_Module.Service_CRM.Service>().Count() == 1)
            {
                if (Application.OpenForms.OfType<Error_DoubleOpenForm>().Count() == 1)
                {
                    return;
                }
                else
                {
                    Error_DoubleOpenForm eropen = new Error_DoubleOpenForm();
                    eropen.Show();
                }

            }
            else
            {
                OpenChildForm(new Admin_Module.Service_CRM.Service());
                AppStart_Menu.Visible = false;

            }
        }

        private void BU_Memeber_CRM_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<Admin_Module.Member_CRM.Member_CRM_MAIN>().Count() == 1)
            {
                if (Application.OpenForms.OfType<Error_DoubleOpenForm>().Count() == 1)
                {
                    return;
                }
                else
                {
                    Error_DoubleOpenForm eropen = new Error_DoubleOpenForm();
                    eropen.Show();
                }

            }
            else
            {
                OpenChildForm(new Admin_Module.Member_CRM.Member_CRM_MAIN());
                AppStart_Menu.Visible = false;

            }
        }

        private void BN_UserInfoEdit_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<Public_Domain.userSettings>().Count() == 1)
            {
                if (Application.OpenForms.OfType<Error_DoubleOpenForm>().Count() == 1)
                {
                    return;
                }
                else
                {
                    Error_DoubleOpenForm eropen = new Error_DoubleOpenForm();
                    eropen.Show();
                }

            }
            else
            {
                OpenChildForm(new Public_Domain.userSettings());
                AppStart_Menu.Visible = false;
            }
        }

        private void BN_Calculator_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<Public_Domain.Calculator>().Count() == 1)
            {
                if (Application.OpenForms.OfType<Error_DoubleOpenForm>().Count() == 1)
                {
                    return;
                }
                else
                {
                    Error_DoubleOpenForm eropen = new Error_DoubleOpenForm();
                    eropen.Show();
                }

            }
            else
            {
                Public_Domain.Calculator calc = new Public_Domain.Calculator();

                calc.Show();
                this.WindowState = FormWindowState.Minimized;
            }
        }

        private void BT_Change_PWD_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<Admin_Module.Password_Management.Password_Update>().Count() == 1)
            {
                if (Application.OpenForms.OfType<Error_DoubleOpenForm>().Count() == 1)
                {
                    return;
                }
                else
                {
                    Error_DoubleOpenForm eropen = new Error_DoubleOpenForm();
                    eropen.Show();
                }

            }
            else
            {
                OpenChildForm(new Admin_Module.Password_Management.Password_Update());
                AppStart_Menu.Visible = false;


            }
        }
        private void BN_G_Reporting_Click(object sender, EventArgs e)
        {
            OpenChildForm(new Reports.Reports());
            AppContainer.Visible = false;
        }
    }
}


