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
    public partial class TKHCI_ForgotPass_FirstPage : Form
    {
        public TKHCI_ForgotPass_FirstPage()
        {
            InitializeComponent();
        }

        private void bt_PasswordReset_Click(object sender, EventArgs e)
        {
            TKHCI_ForgotPass_Username_SecondPage pass = new TKHCI_ForgotPass_Username_SecondPage();
            this.Hide();
            pass.ShowDialog();
            this.Close();
        }

        private void bt_ResetFailedLogin_Click(object sender, EventArgs e)
        {
            TKHCI_ForgotPass_LC_REset_FourPage pass = new TKHCI_ForgotPass_LC_REset_FourPage();
            this.Hide();
            pass.ShowDialog();
            this.Close();
        }
    }
}
