using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static TKHCI.TKHCI_Main_Login;

namespace TKHCI
{
    public partial class Admin_Password_ResetCount : Form
    {
        KeeperDBEntities DBS = new KeeperDBEntities();
        public static class UserSession
        {
            public static string Username;
        }
        public Admin_Password_ResetCount()
        {
            InitializeComponent();
            populateRequest();
            populateWorkedResetTB();
        }
        SqlConnection Con = new SqlConnection(@"Data Source = PY\PY;Initial Catalog = KeeperDB; Integrated Security = True");

        private void populateWorkedResetTB()
        {
            Con.Open();
            string query = "select TKHS_ID, username, name, platformTB, requestDT, PWD_ResetDT from [PWDR_ManageTB]";
            SqlDataAdapter sda = new SqlDataAdapter(query, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            DataGV_WorkedRq.DataSource = ds.Tables[0];
            Con.Close();
        }

        private void populateRequest()
        {
            Con.Open();
            string query = "select PWDC_ID, TKHS_ID, fullname, ResetType, [Platform], ReqDT from [PWDC_WCompleteTB] ";
            SqlDataAdapter sda = new SqlDataAdapter(query, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            UserDGV_LocReq.DataSource = ds.Tables[0];
            Con.Close();
        }

        private void UserDGV_LocReq_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            txtUerID_Hide.Text = UserDGV_LocReq.SelectedRows[0].Cells[0].Value.ToString();
            UserSession.Username = txtUerID_Hide.Text;
            BOM_Spool_Click(null,null);


            if (e.RowIndex == -1) return;
            decimal t_ID = Convert.ToDecimal(UserDGV_LocReq.Rows[e.RowIndex].Cells[0].Value);
            var item = DBS.PWDC_WCompleteTB.FirstOrDefault(a => a.PWDC_ID == t_ID);
            if (item == null) { return; }
            txtUerID_Hide.Text = Convert.ToString(item.PWDC_ID);
            Tx_EmplID.Text = item.TKHS_ID;
            Tx_Name.Text = item.fullname;
            Tx_ReqType.Text = item.ResetType;
            Tx_Platform.Text = item.Platform;
            Request_DT.Value = (DateTime)(item.ReqDT);
        }

        private void BOM_Spool_Click(object sender, EventArgs e)
        {
            var user1 = DBS.PWDC_WCompleteTB.FirstOrDefault(a => a.Username.Equals(UserSession.Username));
            if (!String.IsNullOrEmpty(UserSession.Username))
            {
                if (user1 != null)
                {
                    txtUerID_Hide.Text = Convert.ToString(user1.PWDC_ID);
                    Tx_EmplID.Text = user1.TKHS_ID;
                    Tx_Name.Text = user1.fullname;
                    Tx_ReqType.Text = user1.ResetType;
                    Tx_Platform.Text = user1.Platform;
                    Request_DT.Value = (DateTime)(user1.ReqDT);
                }
            }
        }

        private void BT_ResetMLC_Click(object sender, EventArgs e)
        {
            var user1 = DBS.UserTBs.FirstOrDefault(a => a.TKHS_ID.Equals(Tx_EmplID.Text));
            decimal id = Convert.ToDecimal(txtUerID_Hide.Text);
            var cat = (from s in DBS.PWDC_WCompleteTB where s.PWDC_ID == id select s).FirstOrDefault();
            
            PWDR_ManageTB acc = new PWDR_ManageTB();



            try
            {
                if(cat.complete == 1)
                {
                    Error_Request_Treated rqTreat = new Error_Request_Treated();
                    rqTreat.Show();
                }
                else
                {
                    acc.TKHS_ID = user1.TKHS_ID;
                    acc.name = user1.Fullname;
                    acc.platformTB = Tx_Platform.Text;
                    acc.Request_Type = Tx_ReqType.Text;
                    acc.username = user1.Username;
                    acc.requestDT = Request_DT.Value.Date;
                    acc.creatBY = MainLogin.Username;
                    acc.PWD_ResetDT = DateTime.Now;
                    string message = "Are you sure you want to reset " + user1 + "password count?";
                    string title = "Reseting count...";
                    MessageBoxButtons but = MessageBoxButtons.YesNo;
                    DialogResult result = MessageBox.Show(message, title, but);
                    if (result == DialogResult.Yes)
                    {

                        DBS.PWDR_ManageTB.Add(acc);
                        DBS.SaveChanges();

                        populateWorkedResetTB();
                    }
                    else return;


                    user1.Pwd_lock = 0;
                    DBS.SaveChanges();
                    MessageBox.Show("Count Reset Done!");

                    cat.complete = 1;
                    DBS.SaveChanges();
                }

                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ResetMLP_Click(object sender, EventArgs e)
        {
            var user1 = DBS.UserTBs.FirstOrDefault(a => a.TKHS_ID.Equals(Tx_EmplID.Text));
            PWDR_ManageTB acc = new PWDR_ManageTB();


            decimal ID = Convert.ToDecimal(txtUerID_Hide.Text);
            var pwdReset = (from s in DBS.PWDC_WCompleteTB where s.PWDC_ID == ID select s).FirstOrDefault();


            try
            {
                if (pwdReset.complete == 1)
                {
                    Error_Request_Treated rqTreat = new Error_Request_Treated();
                    rqTreat.Show();
                }
                else
                {
                    acc.TKHS_ID = user1.TKHS_ID;
                    acc.name = user1.Fullname;
                    acc.platformTB = Tx_Platform.Text;
                    acc.Request_Type = Tx_ReqType.Text;
                    acc.username = user1.Username;
                    acc.requestDT = Request_DT.Value.Date;
                    acc.creatBY = MainLogin.Username;
                    acc.PWD_ResetDT = DateTime.Now;
                    string message = "Are you sure you want to reset " + user1 + "password count?";
                    string title = "Reseting count...";
                    MessageBoxButtons but = MessageBoxButtons.YesNo;
                    DialogResult result = MessageBox.Show(message, title, but);
                    if (result == DialogResult.Yes)
                    {

                        DBS.PWDR_ManageTB.Add(acc);
                        DBS.SaveChanges();
                        populateWorkedResetTB();
                    }
                    else return;

                    string Password = Cryptography.Encrypt(Tx_Password.Text);
                    user1.Password = Password;
                    user1.Pwd_lock = 0;
                    user1.Admin_loc = "Locked";
                    DBS.SaveChanges();
                    MessageBox.Show("Password reset Successful!");

                    pwdReset.complete = 1;
                    DBS.SaveChanges();
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
