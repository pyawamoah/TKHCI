using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static TKHCI.Admin_SystemUser_CRM;

namespace TKHCI
{
    public partial class Admin_SystemUser_SysInfo : Form
    {
        KeeperDBEntities DBS = new KeeperDBEntities();
        decimal id;
        public Admin_SystemUser_SysInfo()
        {
            InitializeComponent();
        }

        private void Admin_SystemUser_SysInfo_Load(object sender, EventArgs e)
        {

            var user1 = DBS.UserTBs.FirstOrDefault(a => a.Username.Equals(UserSession.Username));
            if (!String.IsNullOrEmpty(UserSession.Username))
            {

                if (user1 != null)
                {
                    Tx_Empl_ID.Text = user1.TKHS_ID.ToString();
                    Combo_Dept.Text = user1.Dep;
                    Combo_SysRole.Text = user1.SysRoll;
                    ComBo_OffiCapacity.Text = user1.OfficialCapacity;
                    Combo_Status.Text = user1.Member_Status;
                    Tx_Username.Text = user1.Username;
                    Tx_NOK_Name.Text = user1.NOK_Name;
                    Tx_NOK_Phone.Text = user1.NOK_Phone;
                    Tx_NOK_Address.Text = user1.NOK_Address; ;
                    Combo_NOK_Relation.Text = user1.NOKRela;
                    Tx_HideID.Text = user1.UT_ID.ToString();

                }
            }
        }

        private void Bt_Exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Bt_Update_Click(object sender, EventArgs e)
        {
            if (Tx_Username.Text != null)
            {
                id = Convert.ToDecimal(Tx_HideID.Text);
                var cat = (from s in DBS.UserTBs where s.UT_ID == id select s).FirstOrDefault();
                string UserName = Tx_Username.Text;
                cat.Username = UserName;              
                cat.NOK_Name = Tx_NOK_Name.Text;
                cat.NOK_Address = Tx_NOK_Address.Text;
                cat.NOK_Phone = Tx_NOK_Phone.Text;
                cat.NOKRela = Combo_NOK_Relation.Text;
                cat.TKHS_ID = Tx_Empl_ID.Text;
                cat.Dep = Combo_Dept.Text;
                cat.SysRoll = Combo_SysRole.Text;
                cat.OfficialCapacity = ComBo_OffiCapacity.Text;
                cat.Member_Status = Combo_Status.Text;
                cat.Username = Tx_Username.Text;

                string message = "Are you sure you want to update this record?";
                string title = "updating Record...";
                MessageBoxButtons but = MessageBoxButtons.YesNo;
                DialogResult result = MessageBox.Show(message, title, but);
                if (result == DialogResult.Yes)
                {

                    DBS.SaveChanges();
                    MessageBox.Show("User Record Updated Successfully");
                }
                else return;
            }
        }
    }
}
