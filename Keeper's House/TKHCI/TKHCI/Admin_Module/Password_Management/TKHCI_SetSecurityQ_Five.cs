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
    public partial class TKHCI_SetSecurityQ_Five : Form
    {
        KeeperDBEntities DBS = new KeeperDBEntities();
        public TKHCI_SetSecurityQ_Five()
        {
            InitializeComponent();
        }

        private void BT_Save_Click(object sender, EventArgs e)
        {
            if (conboQuestion1.SelectedIndex == 0 && conboQuestion2.SelectedIndex == 0 && conboQuestion3.SelectedIndex == 0
                && txtAnswerOne.Text == "" && txtAnswerTwo.Text == "" && txtAnswerThree.Text == "")
            {
                Error_AnswerAllSQ errorOne = new Error_AnswerAllSQ();
                errorOne.ShowDialog();
            }
            else
            {
                var user1 = DBS.UserTBs.FirstOrDefault(a => a.Username.Equals(TKHCI_Main_Login.MainLogin.Username));
                user1.Security_question1 = conboQuestion1.Text;
                user1.Security_question2 = conboQuestion2.Text;
                user1.Security_question3 = conboQuestion3.Text;
                user1.Secu_quest_answer1 = txtAnswerOne.Text;
                user1.Secu_quest_answer2 = txtAnswerTwo.Text;
                user1.Secu_quest_answer3 = txtAnswerThree.Text;
                user1.Admin_loc = "Unlocked";
                user1.securitySet = 0;
                DBS.SaveChanges();
                Info_Successful suc = new Info_Successful();
                suc.ShowDialog();
                this.Close();

            }
        }
    }
}
