using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
namespace Server
{
    public partial class Sever : Form
    {
        public Sever()
        {
            InitializeComponent();


        }
        public void serverstart()
        {
            SeverSocket ss = new SeverSocket();
            ss.start();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            tbMSG.Text += "server start;";
            serverstart();
        }
    }
}
