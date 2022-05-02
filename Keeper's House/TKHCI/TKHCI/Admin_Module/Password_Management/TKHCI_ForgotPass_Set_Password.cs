using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static TKHCI.TKHCI_ForgotPass_Username_SecondPage;
using static TKHCI.TKHCI_Main_Login;

namespace TKHCI
{
    public partial class TKHCI_ForgotPass_Set_Password : Form
    {
        KeeperDBEntities DBS = new KeeperDBEntities();
        public TKHCI_ForgotPass_Set_Password()
        {
            InitializeComponent();
        }

        private void BT_SetPass_Click(object sender, EventArgs e)
        {
            var user1 = DBS.UserTBs.FirstOrDefault(a => a.Username.Equals(txtUsername.Text));
            var cat = (from s in DBS.UserTBs where s.Username == txtUsername.Text select s).FirstOrDefault();
            try
            {
                if (txtPassword1.Text != "" && txtPassword2.Text != "")
                {
                    if (txtPassword1.Text.ToString().Trim().ToLower() == txtPassword2.Text.ToString().Trim().ToLower())
                    {
                        string message = cat.FName + ", " + "are you sure yoou want to change your password?";
                        string title = "Changing Password...";
                        MessageBoxButtons but = MessageBoxButtons.YesNo;
                        DialogResult result = MessageBox.Show(message, title, but);
                        if (result == DialogResult.Yes)
                        {
                            String password = Cryptography.Encrypt(txtPassword1.Text.ToString());
                            cat.Password = password;
                            cat.Admin_loc = "Unlocked";
                            cat.Pwd_lock = 0;
                            DBS.SaveChanges();
                            Info_Successful suc = new Info_Successful();
                            suc.Show();
                            this.Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Passwords do not match");
                    }
                }
                else
                {
                    MessageBox.Show("Passwords cannot be empty");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void TKHCI_ForgotPass_Set_Password_Load(object sender, EventArgs e)
        {
            var user1 = DBS.UserTBs.FirstOrDefault(a => a.Username.Equals(CheckUsername.Mainusername));
            if (!String.IsNullOrEmpty(TKHCI_ForgotPass_Username_SecondPage.CheckUsername.Mainusername))
            {

                if (user1 != null)
                {
                    txtUsername.Text = user1.Username;
                }
            }
        }
    }
}
