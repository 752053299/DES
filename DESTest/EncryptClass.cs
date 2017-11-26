using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace DESTest
{
    class EncryptClass
    {
        private static byte[] Keys = { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x1D, 0xFF };//自定义密匙
     

        /// <summary>
        /// 文件加密
        /// </summary>
        /// <param name="inFile">文件储存路径</param>
        /// <param name="outFile">储存文件复制的路径</param>
        /// <param name="encryptKey"></param>
        /// <returns></returns>
        public static bool EncryptDES(string inFile, string outFile, string encryptKey)
        {
            byte[] rgb = Keys;
            try
            {
                byte[] rgbKeys = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 8));
                FileStream inFs = new FileStream(inFile, FileMode.Open, FileAccess.Read);//读入流
                FileStream outFs = new FileStream(outFile, FileMode.OpenOrCreate, FileAccess.Write);// 等待写入流
                outFs.SetLength(0);//帮助读写的变量
                byte[] byteIn = new byte[1024*1024*5];//放临时读入的流
                long readLen = 0;//读入流的长度
                long totalLen = inFs.Length;//读入流的总长度
                int everylen = 0;//每次读入流的长度
                DES des = new DESCryptoServiceProvider();//将inFile加密后放到outFile
                CryptoStream encStream = new CryptoStream(outFs, des.CreateEncryptor(rgb, rgbKeys), CryptoStreamMode.Write);
                while (readLen < totalLen)
                {
                    everylen = inFs.Read(byteIn, 0, byteIn.Length);
                    encStream.Write(byteIn, 0, everylen);
                    readLen = readLen + everylen;
                }
                encStream.Close();
                inFs.Close();
                outFs.Close();
                return true;//加密成功
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;//加密失败
            }
        }

        public static bool DecryptDES(string inFile, string outFile, string encryptKey)
        {
            byte[] rgb = Keys;
            try
            {
                byte[] rgbKeys = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 8));
                FileStream inFs = new FileStream(inFile, FileMode.Open, FileAccess.Read);//读入流
                FileStream outFs = new FileStream(outFile, FileMode.OpenOrCreate, FileAccess.Write);// 等待写入流
                outFs.SetLength(0);//帮助读写的变量
                byte[] byteIn = new byte[1024 * 1024 * 5];//放临时读入的流
                long readLen = 0;//读入流的长度
                long totalLen = inFs.Length;//读入流的总长度
                int everylen = 0;//每次读入流的长度
                DES des = new DESCryptoServiceProvider();//将inFile加密后放到outFile
                CryptoStream encStream = new CryptoStream(outFs, des.CreateDecryptor(rgb, rgbKeys), CryptoStreamMode.Write);
                while (readLen < totalLen)
                {
                    everylen = inFs.Read(byteIn, 0, byteIn.Length);
                    encStream.Write(byteIn, 0, everylen);
                    readLen = readLen + everylen;
                }
                encStream.Close();
                inFs.Close();
                outFs.Close();
                return true;//解密成功
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;//解密失败
            }
        }

        ///// <summary>
        ///// 拷贝文件
        ///// </summary>
        //public void copyFile()
        //{
        //    filePathA = this.fei.PostedFile.FileName;//获取文件全部路径
        //    string fileName = this.fei.FileName;
        //    string path = System.IO.Path.GetDirectoryName(filePathA);
        //    filePathB = path + "\\1" + fileName;//重新设置文件名
        //    File.Copy(filePathA, filePathB);
        //}
    }
}
