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
    public partial class TKHCI_ForgotPass_LC_REset_FourPage : Form
    {
        KeeperDBEntities DBS = new KeeperDBEntities();    
        public TKHCI_ForgotPass_LC_REset_FourPage()
        {
            InitializeComponent();
        }

        private void BU_MakeRequest_Click(object sender, EventArgs e)
        {
            var user1 = DBS.UserTBs.FirstOrDefault(a => a.Username.Equals(Tx_Username.Text));
            var cat = (from s in DBS.UserTBs where s.Username == Tx_Username.Text select s).FirstOrDefault();

            string name = cat.Fullname.ToString();
            string empl_ID = cat.TKHS_ID.ToString();
            PWDC_WCompleteTB acc = new PWDC_WCompleteTB();
            try
            {
                acc.ResetType = COM_ReqType.Text;
                acc.Username = Tx_Username.Text;
                acc.ReqDT = DateTime.Now;
                acc.Platform = COM_Module.Text;
                acc.TKHS_ID = empl_ID;
                acc.fullname = name;
                acc.complete = 0;
                string message = "Are you sure you want to request for Reset?";
                string title = "Requesting......";
                MessageBoxButtons but = MessageBoxButtons.YesNo;
                DialogResult result = MessageBox.Show(message, title, but);
                if (result == DialogResult.Yes)
                {
                    DBS.PWDC_WCompleteTB.Add(acc);
                    DBS.SaveChanges();
                    Info_RequestSent info_RequestSent = new Info_RequestSent();
                    this.Hide();
                    info_RequestSent.ShowDialog();
                    this.Close();
                }
                else return;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
