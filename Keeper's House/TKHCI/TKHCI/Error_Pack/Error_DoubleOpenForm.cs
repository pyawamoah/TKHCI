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
    public partial class Error_DoubleOpenForm : Form
    {
        public Error_DoubleOpenForm()
        {
            InitializeComponent();
            new Guna2ShadowForm(this);
        }
    }
}
