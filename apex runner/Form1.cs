using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static WindowsKey;
using static InputMethod;
using static AltShift;
using System.Diagnostics;
using System.Threading;
using System.IO;
using apex_runner.Properties;
using System.Security.Policy;


namespace apex_runner
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //获取当前输入法状态并且标识
            if(InputMethod.CurrentMethod() == InputMethodType.Chinese)
            {
                radioButton_chinese.Checked = true;
            }
            else
            {
                radioButton_eng.Checked = true;
            }
            //获取设置中的 uu 加速器路径
            textBox1.Text = Settings.Default.uupath;
            //获取语音软件路径
            textBox2.Text = Settings.Default.oopzpath;
            LoadExeIcon(pictureBox2, textBox1.Text);
            LoadExeIcon(pictureBox3, textBox2.Text);

        }


        //禁用 Windows 键功能
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked == true)
            {
                WindowsKey.Disable();
            }
            else
            {
                WindowsKey.Enable();
            }
        }

        //切换英文输入法功能
        private void radioButton_eng_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_eng.Checked == true)
            {
                InputMethod.ChangeToEnglish();
            }
            else
            {
                InputMethod.ChangeToChinese();
            }
        }

        //切换 alt shift 开关功能
        private void radioButtonaltshiftOff_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonaltshiftOff.Checked == true)
            {
                AltShift.Disable();
            }
            else
            {
                AltShift.Enable();
            }
        }

        //手抖多点出来的,懒得删了
        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }



        //一键优化 开启游戏模式
        private void button1_Click(object sender, EventArgs e)
        {
            radioButton2.Checked = true;
            radioButton_eng.Checked = true;
            radioButtonaltshiftOff.Checked = true;
        }

        //一键恢复 开启正常电脑模式
        private void button2_Click(object sender, EventArgs e)
        {
            radioButton1.Checked = true;
            radioButton_chinese.Checked = true;
            radioButtonaltshiftOn.Checked = true;
        }

        //textbox1 在修改时自动保存加速器路径
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Settings.Default.uupath = textBox1.Text;
            Settings.Default.Save();
        }
        //textbox2 在修改时自动保存路径
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            Settings.Default.oopzpath = textBox2.Text;
            Settings.Default.Save();
        }

        //右下关于按钮
        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show($"版本号: v1.5" +
                $"这个程序是免费的\n如果你感觉这个程序有帮助的话\n" +
                $"我希望你们可以去 github 帮我点星星\n---\n" +
                $"这个程序其实主要都是Chat-GPT 写的\n" +
                $"有时候我得承认\n" +
                $"他写的代码确实比我写的质量好多了\n" +
                $":P\n" +
                $"二次开发：小诗人\n增加功能：\n1.支持拖放桌面图标到输入框快速获取路径\n" +
                $"2.图标自动同步成加速器和语音软件的图标~\n3.修复了加速器需要管理员权限的情况下无法启动的问题\n" +
                $"4.修复了一些oopz无法正常启动的bug\n最后：感谢作者大大~");
        }
        
        //右下角链接 
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenUrl("https://github.com/cornradio");
        }

        //用于启动加速器的启动函数
        static void StartProgram(string path)
        {
            if (!File.Exists(path))
            {
                MessageBox.Show("程序路径不存在或无法启动，请检查路径是否正确。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                // 创建一个新的进程启动信息
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = path;
                startInfo.WorkingDirectory = Path.GetDirectoryName(path);
                // 启动程序
                Process.Start(startInfo);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("请求的操作需要提升。") == true)
                {
                    StartProgramAsAdmin(path);
                }
                else
                {
                    Console.WriteLine(ex.Message);
                    MessageBox.Show("无法启动程序：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }
        static void StartProgramAsAdmin(string path)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = path;
            startInfo.Verb = "runas";
            startInfo.WorkingDirectory = Path.GetDirectoryName(path);
            try
            {
                Process.Start(startInfo);
            }
            catch (Win32Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }


        //用于开启网页链接的函数
        static void OpenUrl(string url)
        {
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                };
                Process.Start(startInfo);
            }
            catch (Exception ex)
            {
                Console.WriteLine("无法打开URL：" + ex.Message);
            }
        }

        //开启加速器 图片按钮
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            string path = textBox1.Text;
            StartProgram(path);
        }
        //开启语音软件
        private void pictureBox3_Click(object sender, EventArgs e)
        {
            string path = textBox2.Text;
            StartProgram(path);
        }


        //打开显示设置

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            try
            {
                // 打开Windows的显示设置
                Process.Start("ms-settings:display");
                Console.WriteLine("显示设置已打开。");
            }
            catch (Exception ex)
            {
                Console.WriteLine("无法打开显示设置: " + ex.Message);
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void textBox1_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files.Length > 0)
            {
                string filePath = files[0];
                if (Path.GetExtension(filePath).Equals(".lnk", StringComparison.OrdinalIgnoreCase))
                {
                    string targetPath = GetShortcutTargetFile(filePath);
                    if (Path.GetExtension(targetPath).Equals(".exe", StringComparison.OrdinalIgnoreCase))
                    {
                        textBox1.Text = targetPath;
                        LoadExeIcon(pictureBox2, targetPath);
                    }
                    else
                    {
                        textBox1.Text = "快捷方式指向的不是.exe文件";
                    }
                }
                else
                {
                    textBox1.Text = filePath;
                }
            }
        }
        private void textBox2_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }
        private void textBox2_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files.Length > 0)
            {
                string filePath = files[0];
                if (Path.GetExtension(filePath).Equals(".lnk", StringComparison.OrdinalIgnoreCase))
                {
                    string targetPath = GetShortcutTargetFile(filePath);
                    if (Path.GetExtension(targetPath).Equals(".exe", StringComparison.OrdinalIgnoreCase))
                    {
                        textBox2.Text = targetPath;
                        LoadExeIcon(pictureBox3, targetPath);
                    }
                    else
                    {
                        textBox2.Text = "快捷方式指向的不是.exe文件";
                    }
                }
                else
                {
                    textBox2.Text = filePath;
                }
            }
        }
        private string GetShortcutTargetFile(string shortcutFilename)
        {
            IWshRuntimeLibrary.WshShell shell = new IWshRuntimeLibrary.WshShell();
            IWshRuntimeLibrary.IWshShortcut link = (IWshRuntimeLibrary.IWshShortcut)shell.CreateShortcut(shortcutFilename);
            return link.TargetPath;
        }
        private void LoadExeIcon(PictureBox pictureBox, string exePath)
        {
            try
            {
                Icon icon = Icon.ExtractAssociatedIcon(exePath);

                if (icon != null)
                {
                    Bitmap bitmap = icon.ToBitmap();

                    pictureBox.Image = bitmap;
                }
                else
                {
                    Console.WriteLine("没有找到该文件的图标！");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("加载图标的时候出现错误：" + ex.Message);
            }
        }
    }
}
