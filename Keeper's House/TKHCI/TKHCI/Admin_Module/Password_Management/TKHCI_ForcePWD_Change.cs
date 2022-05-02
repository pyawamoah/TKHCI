using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static TKHCI.TKHCI_Main_Login;

namespace TKHCI
{
    public partial class TKHCI_ForcePWD_Change : Form
    {
        KeeperDBEntities DBS = new KeeperDBEntities();
        public TKHCI_ForcePWD_Change()
        {
            InitializeComponent();
        }
        private void TKHCI_ForcePWD_Change_Load(object sender, EventArgs e)
        {
            var user1 = DBS.UserTBs.FirstOrDefault(a => a.Username.Equals(MainLogin.Username));
            if (!String.IsNullOrEmpty(MainLogin.Username))
            {

                if (user1 != null)
                {
                    txtUsername.Text = user1.Username;
                }
                else { return; }
            }
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
                            if (cat.securitySet == 1 || cat.securitySet ==null)
                            {
                                TKHCI_SetSecurityQ_Five secirutyQ = new TKHCI_SetSecurityQ_Five();
                                secirutyQ.ShowDialog();
                                this.Close();
                            }
                            else
                            {
                                return;
                            }
                          
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


    }
    
}
