using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;


//16进制变为2进制，前面的0不能去
//检错

namespace CRC
{
    public partial class CRC : Form
    {
        public CRC()
        {
            InitializeComponent();
        }
        int CRC_F;
        private String u;
        private void button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                string t;
                int q;
               
                string s = textBox1.Text;  //输入框的信息
                int f = textBox1.Text.Length;  //输入框信息的长度
                s = s.Replace(" ", "");  //将空格删去

                string[] c = new string[s.Length];
                if (s.Length / 2 != 0)
                    q = s.Length - 1;
                else
                    q = s.Length;
                
                for (int i = 2, j = 0, k = 0; j < q; j += 2, k++)
                {
                    c[k] = s.Substring(j, i);//将s的字符串每两个字节赋值给string数组的一个内存空间
                }
                for (int i = 0; i < c.Length / 2; i++)
                {
                    c[i] = Convert.ToString(Convert.ToInt32(c[i], 16));//把c中的每个元素，转化为16进制
                }

                byte[] by = new byte[c.Length / 2];
                for (int i = 0; i < c.Length / 2; i++)
                {
                    by[i] = Convert.ToByte(c[i]);//将c转化为字符数组by               
                }


                int CRC = 0xFFFF;  //初始为0xFFFF
                //int temp = 0xA001;
                for (int k = 0; k < by.Length; k++)
                {
                    
                    CRC ^= by[k];
                    for (int i = 0; i < 8; i++)
                    {
                        int j = Convert.ToInt32(CRC & 1);  //判断首位为1还是0
                        if (j == 1)
                            CRC = (CRC >> 1) ^ 0xA001;// A001是8005反过来，8005是CRC-16的多项式对应的二进制数  
                        else
                            CRC = (CRC >> 1);
                    }
                    /*
                    CRC ^= by[k];
                    for (int i = 0; i < 8; i++)
                    {
                        int j = CRC & 1;
                        CRC >>= 1;
                        CRC &= 0x7FFF;
                        if (j == 1)
                            CRC ^= temp;
                    }*/

                    byte hi = (byte)((CRC & 0xFF00) >> 8);  //高位
                    byte lo = (byte)(CRC & 0x00FF);         //低位
                    CRC_F = hi << 8 | lo;  //最终的CRC-16编码码字
                    
                }

                t = Convert.ToString(CRC_F, 16);  //格式转化



                string[] n = new string[2];
                for (int i = 2, j = 0, k = 0; j <= n.Length; j += 2, k++)
                {
                    n[k] = t.Substring(j, i);
                }
                Array.Reverse(n);

                for (int i = 0; i < n.Length; i++)
                {
                    u += n[i];
                }

                int CRC_b = Convert.ToInt32(u, 16);
                string CRC_bs = Convert.ToString(CRC_b, 2);
                CRC_bs = CRC_bs.PadLeft(16, '0');
                textBox3.Text = s + CRC_bs;

                int int_len = s.Length;
                byte[] bytes = new byte[int_len];
                for (int i = 0; i < int_len; i++)
                {
                    bytes[i] = (byte)(s[i]);
                }

                u = u.Insert(2, " ");  //一个字节中间用一个空格隔开
                textBox2.Text = u.ToUpper();  //字母变大写

            }
            catch (Exception)
            {
                MessageBox.Show("请重新发送数据！！！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            u = "";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox4.Text.Trim() == String.Empty)
            {
                MessageBox.Show("请输入接收到的信息！！！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                textBox5.Text = "";
            }
            textBox5.Text = "";
            try
            {
                string s = textBox1.Text;  //输入框的信息
                int f = textBox1.Text.Length;  //输入框信息的长度
                s = s.Replace(" ", "");  //将空格删去



                string s1 = textBox4.Text;
                int f1 = textBox4.Text.Length;  //输入框信息的长度
                s1 = s1.Replace(" ", "");  //将空格删去


                int[] result = new int[s1.Length];
                int i1 = 0;
                int flag = 0;
                // for (int i = 0; i < by1.Length; i++)
                while (i1 < s1.Length)
                {
                    if (s1[i1] == s[i1])
                    {
                        result[i1] = 1;
                    }
                    else
                    {
                        result[i1] = 0;
                        flag = 1;
                    }
                    i1 += 1;
                }
                int count = 0;
                if (flag == 0)
                {
                    if (textBox4.Text.Trim() != String.Empty)
                    {
                        textBox5.Text = "数据传输全部正确！！";
                        flag = 0;
                    }
                    
                }
                else
                {
                    textBox5.Text = "数据的以下位出现错误:\r\n";
                    for (int i = 0; i < result.Length; i++)
                    {
                        if (result[i] == 0)
                        {
                            textBox5.Text += "第" + Convert.ToString(result.Length-1-i) + "位"+"\r\n";
                            textBox5.Text += "请重新发送数据!!" ;
                            count++;
                            if (count > 1)
                            {
                                textBox5.Text = "";
                                textBox5.Text = "检查出错！应该是不止一位出现了问题！！"+"\r\n";
                                MessageBox.Show("请重新发送数据！！！", "信息提示",MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }                      
                    }
                    //textBox5.Text += "共有" + Convert.ToString(count) + "个错误位，请改正!";
                }
                
                
               // textBox5.Text = Convert.ToString(c1.Length);
            }
            catch (Exception)
            {
                MessageBox.Show("请输入正确的数值");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            CRC_F = 0;
            u = "";
    }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();  //退出系统
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            StreamReader sr = File.OpenText("hex.txt");
            string text = sr.ReadToEnd();//这个是把文件从头读到结尾
            sr.Close();
            //txtSend.Text += "\r\n";
            textBox1.Text = "";
            textBox1.Text += text;
        }
    }
}
