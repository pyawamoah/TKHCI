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
    public partial class Error_failedLock_Count : Form
    {
        KeeperDBEntities DBS = new KeeperDBEntities();
        public Error_failedLock_Count()
        {
            InitializeComponent();
        }

        private void Error_failedLock_Count_Load(object sender, EventArgs e)
        {

            var user1 = DBS.UserTBs.FirstOrDefault(a => a.Username.Equals(MainLogin.Username));
            lbl_Name.Text = user1.FName + ",";
            lbl_Count.Text = Convert.ToString(user1.Pwd_lock);
        }
    }
}
