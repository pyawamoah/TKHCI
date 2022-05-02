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

namespace TKHCI
{
    public partial class TKHCI_ForgotPass_Username_SecondPage : Form
    {
        KeeperDBEntities DBS = new KeeperDBEntities();
        public static class CheckUsername
        {
            public static string Mainusername;
        }
        public TKHCI_ForgotPass_Username_SecondPage()
        {
            InitializeComponent();
        }
        SqlConnection Con = new SqlConnection(@"Data Source = PY\PY;Initial Catalog = KeeperDB; Integrated Security = True");
        private void BT_NextPage_Click(object sender, EventArgs e)
        {
            bool exits = false;
            SqlCommand cmd = new SqlCommand("Select count(*) from  UserTB where Username=@Username", Con);
            cmd.CommandType = CommandType.Text;
            Con.Open();

            cmd.Parameters.AddWithValue("@Username", this.txtUsername.Text);
            exits = (int)cmd.ExecuteScalar() > 0;
            if (exits)
            {
                CheckUsername.Mainusername = txtUsername.Text;
                TKHCI_ForgotPass_SQ_Answers_ThirdPage mainSQ = new TKHCI_ForgotPass_SQ_Answers_ThirdPage();
                this.Hide();
                mainSQ.ShowDialog();
                this.Close();                
            }
            else
            {
                MessageBox.Show("Username does not exist...");
            }
            Con.Close();
        }
    }
}
