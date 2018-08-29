using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Toolkit.Services.Twitter;

namespace TwitterServiceTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            TwitterService.Instance.Initialize("SPcuLi753IsRYsXfiTrA4kHNh", "ycZybJ7WxIwzVTdNjGtUO8RNNiWdFuy9026EvClUA3UXvzNiVc", "http://localhost:54501/");

        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if(await TwitterService.Instance.LoginAsync())
            {
                label1.Text = "Logged in!";
                await TwitterService.Instance.TweetStatusAsync("It works! Ice cream for the team on me! :D");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

            if (!TwitterService.Instance.Logout()) {
                label1.Text = "Logged off";
            }

            
        }

        private void label1_Click(object sender, EventArgs e)
        {
            
        }
    }
}
