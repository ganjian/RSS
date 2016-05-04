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


        SeverSocket ss = new SeverSocket();
        public Sever()
        {
            InitializeComponent();


        }
        public void serverstart()
        {
            ss.start();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            tbInfo.Text += "server start;" + "\r\n";
            serverstart();
        }

        private void BtnSendMsg_Click(object sender, EventArgs e)
        {
            tbInfo.Text += "Send Msg:"+ tbSendMsg.Text + "\r\n";
            ss.sendMessage(tbSendMsg.Text);
        }
    }
}
