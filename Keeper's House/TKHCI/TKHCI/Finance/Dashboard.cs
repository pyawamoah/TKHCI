using Guna.UI2.WinForms;
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

namespace TKHCI.Finance
{
    public partial class Dashboard : Form
    {
        KeeperDBEntities DBS = new KeeperDBEntities();
        private Form currentChildForm;
        int startpoint = 0;
        public Dashboard()
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
                if (form.GetType().Name == form.Name)
                    return true;
            return false;
        }
        private void Dashboard_Load(object sender, EventArgs e)
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
        private void Bu_ShutDown_Click(object sender, EventArgs e)
        {
            this.Close();
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
        private void But_Offering_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<Finance.OfferingsPay>().Count() == 1)
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
                OpenChildForm(new Finance.OfferingsPay());
                AppStart_Menu.Visible = false;
            }
        }
        private void BN_GL_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<Finance.GL_s>().Count() == 1)
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
                OpenChildForm(new Finance.GL_s());
                AppStart_Menu.Visible = false;
            }
        }
        private void BN_Payroll_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<Finance.PayrollCRM_ORigin>().Count() == 1)
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
                OpenChildForm(new Finance.PayrollCRM_ORigin());
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
        private void BN_ChangePass_Click(object sender, EventArgs e)
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
        private void BN_FinanceV_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<VisualFinance.Finance_Graph>().Count() == 1)
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
                OpenChildForm(new VisualFinance.Finance_Graph());
                AppStart_Menu.Visible = false;


            }
        }
        private void BT_Asserts_Click(object sender, EventArgs e)
        {

            if (Application.OpenForms.OfType<Finance.AssertModule>().Count() == 1)
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
                OpenChildForm(new Finance.AssertModule());
                AppStart_Menu.Visible = false;


            }
            
        }
        private void BU_GeneralExp_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<Finance.GeneralExpense>().Count() == 1)
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
                OpenChildForm(new Finance.GeneralExpense());
                AppStart_Menu.Visible = false;
            }
        }
        private void BN_Reports_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<Finance.Wallet_Info>().Count() == 1)
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
                OpenChildForm(new Finance.Wallet_Info());
                AppStart_Menu.Visible = false;
            }
        }
        private void Tmr_Tick(object sender, EventArgs e)
        {
            //myLoading.Visible = true;
            //loadingText.Visible = true;
            loadingText.Text = myLoading.ProgressPercentText;
            startpoint += 1;
            myLoading.Value = startpoint;
            if (myLoading.Value == 100)
            {
                Tmr.Stop();
                DialogResult Result = MessageBox.Show("Do you want to set your security password now?.", "Set Security Question", MessageBoxButtons.OKCancel);
                myLoading.Value = 0;
               

                if (Result == DialogResult.Cancel)
                {
                    startpoint = 0;
                    Tmr.Interval += 200;
                    Tmr.Start();
                }
                else
                {

                    if (Application.OpenForms.OfType<TKHCI_SetSecurityQ_Five>().Count() == 1)
                    {
                        return;
                    }
                    else
                    {
                        TKHCI_SetSecurityQ_Five secirutyQ = new TKHCI_SetSecurityQ_Five();
                        secirutyQ.Show();
                }
            }

        }
        }
        private void BU_Gen_Reports_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<Reports.Reports>().Count() == 1)
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
                OpenChildForm(new Reports.Reports());
                AppStart_Menu.Visible = false;
            }
        }
    }
}

