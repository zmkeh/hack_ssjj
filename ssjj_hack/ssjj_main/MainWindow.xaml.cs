using ssjj_hack.Module;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace ssjj_main
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        Lanzou lan = new Lanzou();
        DispatcherTimer timer = new DispatcherTimer();
        private void Grid_Initialized(object sender, System.EventArgs e)
        {
            InitIni();
            Task.Run(lan.UpdateDLL);
            label.Content = "准备中...";
            progressBar.Value = 0;
            button.IsEnabled = false;
            timer.Interval = new TimeSpan(100000);
            timer.Tick += OnTimer;
            timer.Start();
        }

        private void InitIni()
        {
            if (!File.Exists(Settings.iniPath))
            {
                File.Create(Settings.iniPath).Close();
                Settings.Save();
            }
            else
            {
                Settings.Read();
            }

            ViewUpdate();
        }

        private void OnTimer(object sender, EventArgs e)
        {
            if (lan.isDownloading)
            {
                label.Content = $"准备中({(int)(progressBar.Value * 100)}%)...";
                progressBar.Value += 0.0015f;
            }
            else
            {
                label.Content = "准备完成";
                timer.Stop();
                button.IsEnabled = true;
                progressBar.Value = 1f;
            }
        }

        private string FindRoot()
        {
            var p = Process.GetProcessesByName("AirLobbyPreloader");
            if (p == null || p.Length == 0)
                return null;
            return Path.GetDirectoryName(Path.GetDirectoryName(Helper.GetProcessFilename(p[0])));
        }

        private string FixBattles(string root)
        {
            var battle = root.Combine("battle");
            if (!Directory.Exists(battle))
                return "文件不存在：" + battle;
            foreach (var d in new DirectoryInfo(battle).GetDirectories())
            {
                var err = FixBattle(d.FullName);
                if (!string.IsNullOrEmpty(err))
                {
                    return err;
                }
            }
            return null;
        }

        private string FixBattle(string path)
        {
            var dll = path.Combine("SSJJ_BattleClient_Unity_Data/Managed/Assembly-CSharp.dll");
            if (!File.Exists(dll))
            {
                return "文件不存在：" + dll;
            }
            var err = SetDll(dll);
            if (!string.IsNullOrEmpty(err))
            {
                return err;
            }


            var md5 = path.Combine("md5cache");
            if (!Directory.Exists(md5))
            {
                return "文件不存在：" + md5;
            }
            err = SetMd5(md5, dll);
            if (!string.IsNullOrEmpty(err))
            {
                return err;
            }

            return null;
        }

        private string GetDll()
        {
            return new FileInfo("Assembly-CSharp.dll").FullName;
        }

        private string SetDll(string dll)
        {
            var newDll = GetDll();
            if (!File.Exists(newDll))
                return "文件不存在：" + newDll;
            try
            {
                File.Copy(newDll, dll, true);
            }
            catch (Exception e)
            {
                return e.Message;
            }
            return null;
        }

        private string SetMd5(string path, string dllPath)
        {
            try
            {
                var dllmd5 = Helper.GetFileMD5(dllPath);
                foreach (var f in new DirectoryInfo(path).GetFiles("*.list"))
                {
                    var text = "";
                    foreach (var l in File.ReadAllLines(f.FullName))
                    {
                        if (string.IsNullOrEmpty(l))
                            continue;
                        string str = l;
                        if (str.StartsWith(@"SSJJ_BattleClient_Unity_Data\Managed\Assembly-CSharp.dll"))
                            continue;
                        text += str + "\r\n";
                    }
                    File.WriteAllText(f.FullName, text);
                    var md5 = Helper.GetFileMD5(f.FullName);
                    var _md5path = f.FullName + ".md5";
                    File.WriteAllText(_md5path, md5);
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }
            return null;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var root = FindRoot();
            if (root == null)
            {
                //启动失败
                //label.Content = "启动失败：没有找到生死狙击程序";
                MessageBox.Show("请先启动生死狙击游戏。", "启动失败", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var error = FixBattles(root);
            if (!string.IsNullOrEmpty(error))
            {
                //启动失败
                //label.Content = "启动失败：" + error;
                MessageBox.Show(error, "启动失败", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            button.Content = "启动成功";
            button.IsEnabled = false;
        }

        private void settings_changed(object sender, RoutedEventArgs e)
        {
            ViewToSettings();
            ViewUpdate();
            Settings.Save();
        }

        private void ViewUpdate()
        {
            esp_friendly.IsEnabled = Settings.isEsp;
        }

        private void ViewToSettings()
        {
            Settings.isAim = (bool)aim.IsChecked;
            Settings.isEsp = (bool)esp.IsChecked;
            Settings.isEspFriendly = (bool)esp_friendly.IsChecked;
        }
    }


    internal static class Helper
    {
        [Flags]
        private enum ProcessAccessFlags : uint
        {
            QueryLimitedInformation = 0x00001000
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool QueryFullProcessImageName(
              [In] IntPtr hProcess,
              [In] int dwFlags,
              [Out] StringBuilder lpExeName,
              ref int lpdwSize);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr OpenProcess(
         ProcessAccessFlags processAccess,
         bool bInheritHandle,
         int processId);

        public static String GetProcessFilename(Process p)
        {
            int capacity = 2000;
            StringBuilder builder = new StringBuilder(capacity);
            IntPtr ptr = OpenProcess(ProcessAccessFlags.QueryLimitedInformation, false, p.Id);
            if (!QueryFullProcessImageName(ptr, 0, builder, ref capacity))
            {
                return String.Empty;
            }

            return builder.ToString();
        }

        public static string Combine(this string str, string des)
        {
            return Path.Combine(str, des);
        }

        /// <summary>
        /// 获取文件MD5值
        /// </summary>
        /// <param name="fileName">文件绝对路径</param>
        /// <returns>MD5值</returns>
        public static string GetFileMD5(string fileName)
        {
            try
            {
                FileStream file = new FileStream(fileName, FileMode.Open);
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(file);
                file.Close();

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("GetMD5HashFromFile() fail,error:" + ex.Message);
            }
        }
    }
}
