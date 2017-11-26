using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace DESTest
{
    public partial class Form1 : Form
    {
        string OutFile;

        public Form1()
        {
            InitializeComponent();
        }




        private void button1_Click(object sender, EventArgs e)
        {

            string SourceFilePath = @textBox1.Text;
            string name = Path.GetFileName(SourceFilePath);           

            OutFile = @"./加密后的文件/encrypt-" + name; //组装新名字

            Encrypt encrypt = new Encrypt(EncryptClass.EncryptDES);

            encrypt.BeginInvoke(SourceFilePath, OutFile, "12345678", new AsyncCallback(encryptCompleted),null);
            labelState.Text = "开始加密,请稍等";
        }


        //异步加密需要的委托
        delegate bool Encrypt(string SourceFilePath, string OutFile, string key);

        /// <summary>
        /// 异步线程的回调
        /// </summary>
        /// <param name="result"></param>
        private void encryptCompleted(IAsyncResult result)
        {
            
            AsyncResult _result = (AsyncResult)result;
            Encrypt insertDelegate = (Encrypt)_result.AsyncDelegate;
            bool resultState = insertDelegate.EndInvoke(_result);
            string addMac = (string)result.AsyncState;
            if (resultState)
            {
                labelState.Text = "加密成功";
                textBox1.Clear();
                textBox1.Text = Path.GetFullPath(OutFile);
            }
            else
            {
                MessageBox.Show("加密失败");
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            textBox1.Text = openFileDialog1.FileName;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //取消线程间检查
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string SourceFilePath = @textBox1.Text;
            string name = Path.GetFileName(SourceFilePath);

            string OutFile = @"./解密后的文件/decrypt-" + name; //组装新名字

            Encrypt decrypt = new Encrypt(EncryptClass.DecryptDES);

            decrypt.BeginInvoke(SourceFilePath, OutFile, "12345678", new AsyncCallback(decryptCompleted), null);
            labelState.Text = "开始解密，请稍等";
        }

        /// <summary>
        /// 异步线程的回调
        /// </summary>
        /// <param name="result"></param>
        private void decryptCompleted(IAsyncResult result)
        {

            AsyncResult _result = (AsyncResult)result;
            Encrypt insertDelegate = (Encrypt)_result.AsyncDelegate;
            bool resultState = insertDelegate.EndInvoke(_result);
            string addMac = (string)result.AsyncState;
            if (resultState)
            {
                labelState.Text = "解密成功";
            }
            else
            {
                MessageBox.Show("解密失败，请选择正确的加密文件");
            }
        }
    }
}
