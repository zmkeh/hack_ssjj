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
        DispatcherTimer timer2 = new DispatcherTimer();
        private bool isReady = false;
        private bool isStartup = false;
        private void Grid_Initialized(object sender, System.EventArgs e)
        {
            InitIni();
            Task.Run(lan.UpdateDLL);
            button.Content = "准备中...";
            progressBar.Value = 0;
            button.IsEnabled = false;
            timer.Interval = new TimeSpan(100000);
            timer.Tick += OnTimer;
            timer.Start();

            timer2.Interval = new TimeSpan(1000000);
            timer2.Tick += OnTimer2;
            timer2.Start();
        }

        private void InitIni()
        {
            try
            {
                var _rootdir = "ssjj_libs";
                if (!Directory.Exists(_rootdir))
                    Directory.CreateDirectory(_rootdir);
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
            catch (Exception e)
            {
                MessageBoxShowException(e);
            }
        }

        private void OnTimer(object sender, EventArgs e)
        {
            try
            {
                if (lan.isDownloading)
                {
                    button.Content = $"更新中({(int)(progressBar.Value * 100)}%)...";
                    progressBar.Value += 0.0015f;
                }
                else
                {
                    button.Content = "启动";
                    timer.Stop();
                    button.IsEnabled = true;
                    progressBar.Value = 1f;
                    isReady = true;
                    ViewUpdate();
                }
            }
            catch (Exception ex)
            {
                MessageBoxShowException(ex);
            }
        }

        private void OnTimer2(object sender, EventArgs e)
        {
            try
            {
                if (!isReady)
                    return;

                // check settings
                if (Settings.isEsp != (bool)esp.IsChecked
                    || Settings.isEspFriendly != (bool)esp_friendly.IsChecked
                    || Settings.isEspHp != (bool)esp_hp.IsChecked
                    || Settings.isEspBox != (bool)esp_box.IsChecked
                    || Settings.isEspBoneLine != (bool)esp_boneline.IsChecked
                    || Settings.isEspAirLine != (bool)esp_airline.IsChecked
                    || Settings.isAim != (bool)aim.IsChecked
                    || Settings.isAimCircle != (bool)aim_circle.IsChecked
                    || Settings.isAimLine != (bool)aim_line.IsChecked
                    || Settings.aimRange != (int)(aim_range.SelectedIndex)
                    || Settings.aimPos != (AimPos)aim_pos.SelectedIndex
                    || Settings.isNoRecoil != (bool)no_recoil.IsChecked
                    || Settings.isNoSpread != (bool)no_spread.IsChecked
                    || Settings.isWindowed != (bool)is_windowed.IsChecked)
                {
                    ViewToSettings();
                    ViewUpdate();
                    Settings.Save();
                    if (isStartup)
                    {
                        FixInis(FindRoot());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBoxShowException(ex);
            }
        }

        private void MessageBoxShowException(Exception ex)
        {
            MessageBox.Show(ex.Message + "\n" + ex.StackTrace, "启动失败", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private string rootPath = "";
        private string FindRoot()
        {
            if (string.IsNullOrEmpty(rootPath))
            {
                var p = Process.GetProcessesByName("AirLobbyPreloader");
                if (p == null || p.Length == 0)
                    return null;
                rootPath = Path.GetDirectoryName(Path.GetDirectoryName(Helper.GetProcessFilename(p[0])));
            }
            return rootPath;
        }

        private string FixInis(string root)
        {
            var battle = root.Combine("battle");
            if (!Directory.Exists(battle))
                return "文件不存在：" + battle;
            foreach (var d in new DirectoryInfo(battle).GetDirectories())
            {
                var ini = d.FullName.Combine("SSJJ_BattleClient_Unity_Data/StreamingAssets/settings.ini");
                if (!File.Exists(ini))
                {
                    File.Create(ini).Close();
                }
                var err = SetIni(ini);
                if (!string.IsNullOrEmpty(err))
                {
                    return err;
                }
            }
            return null;
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
            var sdir = path.Combine("SSJJ_BattleClient_Unity_Data/StreamingAssets");
            if (!Directory.Exists(sdir))
            {
                Directory.CreateDirectory(sdir);
            }

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

            var ini = path.Combine("SSJJ_BattleClient_Unity_Data/StreamingAssets/settings.ini");
            if (!File.Exists(ini))
            {
                File.Create(ini).Close();
            }
            err = SetIni(ini);
            if (!string.IsNullOrEmpty(err))
            {
                return err;
            }

            var hack = path.Combine("SSJJ_BattleClient_Unity_Data/StreamingAssets/hack.dll");
            if (!File.Exists(ini))
            {
                File.Create(ini).Close();
            }
            err = SetHackDll(hack);
            if (!string.IsNullOrEmpty(err))
            {
                return err;
            }
            return null;
        }

        private string GetVerDir()
        {
            var _dmax = "";
            foreach (var d in Directory.GetDirectories(Settings.root))
            {
                if (d.CompareTo(_dmax) > 0)
                    _dmax = d;
            }
            return _dmax;
        }

        private string SetDll(string dll)
        {
            var newDll = GetVerDir().Combine("Assembly-CSharp.dll");
            if (!File.Exists(newDll))
                return null;
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

        private string SetHackDll(string dll)
        {
            var newDll = GetVerDir().Combine("hack.dll");
            if (!File.Exists(newDll))
                return null;
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

        private string SetIni(string ini)
        {
            var rawIni = Settings.iniPath;
            if (!File.Exists(ini))
                return "文件不存在：" + ini;
            try
            {
                File.Copy(rawIni, ini, true);
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
            try
            {
                var root = FindRoot();
                if (root == null)
                {
                    //启动失败
                    MessageBox.Show("请先启动生死狙击游戏。", "启动失败", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var error = FixBattles(root);
                if (!string.IsNullOrEmpty(error))
                {
                    //启动失败
                    MessageBox.Show(error, "启动失败", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                button.Content = "启动成功";
                button.IsEnabled = false;
                isStartup = true;
            }
            catch (Exception ex)
            {
                MessageBoxShowException(ex);
            }
        }

        private void ViewUpdate()
        {
            esp.IsChecked = Settings.isEsp;
            esp_friendly.IsChecked = Settings.isEspFriendly;
            esp_hp.IsChecked = Settings.isEspHp;
            esp_box.IsChecked = Settings.isEspBox;
            esp_boneline.IsChecked = Settings.isEspBoneLine;
            esp_airline.IsChecked = Settings.isEspAirLine;

            aim.IsChecked = Settings.isAim;
            aim_circle.IsChecked = Settings.isAimCircle;
            aim_line.IsChecked = Settings.isAimLine;
            aim_range.SelectedIndex = Settings.aimRange;
            aim_pos.SelectedIndex = (int)Settings.aimPos;

            no_recoil.IsChecked = Settings.isNoRecoil;
            no_spread.IsChecked = Settings.isNoSpread;
            is_windowed.IsChecked = Settings.isWindowed;

            // enable
            bool all_enable = isReady;
            esp.IsEnabled = all_enable;
            esp_friendly.IsEnabled = Settings.isEsp && all_enable;
            esp_hp.IsEnabled = Settings.isEsp && all_enable;
            esp_box.IsEnabled = Settings.isEsp && all_enable;
            esp_boneline.IsEnabled = Settings.isEsp && all_enable;
            esp_airline.IsEnabled = Settings.isEsp && all_enable;

            aim.IsEnabled = all_enable;
            aim_circle.IsEnabled = Settings.isAim && all_enable;
            aim_line.IsEnabled = Settings.isAim && all_enable;
            aim_range.IsEnabled = Settings.isAim && all_enable;
            aim_pos.IsEnabled = Settings.isAim && all_enable;

            no_recoil.IsEnabled = all_enable;
            no_spread.IsEnabled = all_enable;
            is_windowed.IsEnabled = all_enable;
        }

        private void ViewToSettings()
        {
            Settings.isEsp = (bool)esp.IsChecked;
            Settings.isEspFriendly = (bool)esp_friendly.IsChecked;
            Settings.isEspHp = (bool)esp_hp.IsChecked;
            Settings.isEspBox = (bool)esp_box.IsChecked;
            Settings.isEspBoneLine = (bool)esp_boneline.IsChecked;
            Settings.isEspAirLine = (bool)esp_airline.IsChecked;

            Settings.isAim = (bool)aim.IsChecked;
            Settings.isAimCircle = (bool)aim_circle.IsChecked;
            Settings.isAimLine = (bool)aim_line.IsChecked;
            Settings.aimRange = aim_range.SelectedIndex;
            Settings.aimPos = (AimPos)aim_pos.SelectedIndex;

            Settings.isNoRecoil = (bool)no_recoil.IsChecked;
            Settings.isNoSpread = (bool)no_spread.IsChecked;
            Settings.isWindowed = (bool)is_windowed.IsChecked;
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
