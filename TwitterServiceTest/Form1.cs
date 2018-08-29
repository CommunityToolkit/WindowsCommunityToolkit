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

        private async void sendTweet_Click(object sender, EventArgs e)
        {
            if (tweetText.Text != "" && tweetText.Text != null)
            {
                await TwitterService.Instance.TweetStatusAsync(tweetText.Text);
                label1.Text = "Tweet Sent!";
            }
            else
            {
                label1.Text = "Please enter a text for your tweet.";
            }
        }
    }
}
