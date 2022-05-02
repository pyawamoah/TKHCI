using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TKHCI
{
    public partial class TKHCI_Main_Login : Form
    {
        KeeperDBEntities DBS = new KeeperDBEntities();
        int startpoint = 0;

        public static class MainLogin
        {
            public static string Username;
        }
        public static class SysUserLogin
        {
            public static string Username;
        }
        public TKHCI_Main_Login()
        {
            InitializeComponent();
            new Guna2ShadowForm(this);
        }

        private void BT_Login_MouseHover(object sender, EventArgs e)
        {
            BT_Login.Width = 182;
            BT_Login.Height = 55;
        }

        private void BT_Login_MouseLeave(object sender, EventArgs e)
        {
            BT_Login.Width = 180;
            BT_Login.Height = 45;
        }

        private void TimerOne_Tick(object sender, EventArgs e)
        {
            myLoading.Visible = true;
            loadingText.Visible = true;
            loadingText.Text = myLoading.ProgressPercentText;
            startpoint += 1;
            myLoading.Value = startpoint;
            if (myLoading.Value == 100)
            {
                myLoading.Value = 0;
                TimerOne.Stop();
                Admin_Dashbaord Dash = new Admin_Dashbaord();
                this.Hide();
                Dash.ShowDialog();
                this.Close();

            }
        }
        private void TimerTwo_Tick(object sender, EventArgs e)
        {
            myLoading.Visible = true;
            loadingText.Visible = true;
            loadingText.Text = myLoading.ProgressPercentText;
            startpoint += 1;
            myLoading.Value = startpoint;
            if (myLoading.Value == 100)
            {
                myLoading.Value = 0;
                TimerTwo.Stop();
                TKHCI_ForcePWD_Change chgPass = new TKHCI_ForcePWD_Change();
                this.Hide();
                chgPass.ShowDialog();
                this.Close();

            }
        }
        private void TimerThree_Tick(object sender, EventArgs e)
        {
            myLoading.Visible = true;
            loadingText.Visible = true;
            loadingText.Text = myLoading.ProgressPercentText;
            startpoint += 1;
            myLoading.Value = startpoint;
            if (myLoading.Value == 100)
            {
                myLoading.Value = 0;
                TimerThree.Stop();
                Admin_Dashbaord Dash = new Admin_Dashbaord();
                this.Hide();
                Dash.ShowDialog();
                this.Close();

            }
        }

        private void BT_Login_Click(object sender, EventArgs e)
        {
           

            try
            {
                var user1 = DBS.UserTBs.FirstOrDefault(a => a.Username.Equals(Tx_Username.Text));
                var cat = (from s in DBS.UserTBs where s.Username == Tx_Username.Text select s).FirstOrDefault();
                decimal pwLockNo = 0;

                if (Tx_Username.Text == "" || Tx_Password.Text == "" || Cm_SysRole.Text == "SELECT SYSTEM ROLE")
                {
                    MessageBox.Show("User");
                }else if(user1 == null)
                {
                    return;
                }else if(user1.Pwd_lock == 3)
                {
                    Error_PwdLockCount pwdLockCount = new Error_PwdLockCount();
                    this.Hide();
                    pwdLockCount.ShowDialog();
                    this.Close();
                }
                else if (Cryptography.Decrypt(user1.Password).Equals("password") || Cryptography.Decrypt(user1.Password).Equals("Password") || user1.Admin_loc.Equals("Locked"))
                {
                    MainLogin.Username = user1.Username;
                    TimerTwo.Start();
                }
                else if (Cryptography.Decrypt(user1.Password).Equals(Tx_Password.Text) && user1.SysRoll.Equals("Administrator") && Cm_SysRole.SelectedItem.ToString() == "Administrator" && user1.Member_Status.Equals("Active") && user1.Admin_loc.Equals("Unlocked"))
                {
                    MainLogin.Username = user1.Username;
                    cat.Pwd_lock = 0;
                    DBS.SaveChanges();
                    TimerOne.Start();
                }
                else
                {
                    pwLockNo++;
                    cat.Pwd_lock += pwLockNo;
                    DBS.SaveChanges();
                    MainLogin.Username = user1.Username;
                    Error_failedLock_Count pwdCount = new Error_failedLock_Count();
                    pwdCount.ShowDialog();

                }





            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }

        private void BT_ForgotPass_Click(object sender, EventArgs e)
        {
            TKHCI_ForgotPass_FirstPage isu = new TKHCI_ForgotPass_FirstPage();
            this.Hide();
            isu.ShowDialog();
            this.Close();
        }

        private void TKHCI_Main_Login_Load(object sender, EventArgs e)
        {
            this.AcceptButton = BT_Login;
            myLoading.Visible = false;
            loadingText.Visible = false;
        }
    }
}
