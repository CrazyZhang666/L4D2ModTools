using L4D2ModTools.Core;
using L4D2ModTools.Helper;
using L4D2ModTools.Utils;

namespace L4D2ModTools.Views;

/// <summary>
/// ConfigView.xaml 的交互逻辑
/// </summary>
public partial class ConfigView : UserControl
{
    public ConfigView()
    {
        InitializeComponent();
        this.DataContext = this;
        MainWindow.WindowClosingEvent += MainWindow_WindowClosingEvent;

        // 读取对应配置文件
        Globals.L4D2MainDir = IniHelper.ReadValue("Config", "L4D2MainDir");

        TextBox_L4D2MainDir.Text = Globals.L4D2MainDir;

        if (!string.IsNullOrWhiteSpace(Globals.L4D2MainDir))
        {
            CheckEnv();
        }
    }

    /// <summary>
    /// 主窗口关闭事件
    /// </summary>
    private void MainWindow_WindowClosingEvent()
    {
        SaveConfig();
    }

    /// <summary>
    /// 保存配置文件
    /// </summary>
    private void SaveConfig()
    {
        IniHelper.WriteValue("Config", "L4D2MainDir", Globals.L4D2MainDir);
    }

    /// <summary>
    /// 清空日志
    /// </summary>
    private void ClearLogger()
    {
        TextBox_Logger.Clear();
    }

    /// <summary>
    /// 增加日志信息
    /// </summary>
    /// <param name="log"></param>
    private void AddLogger(string log)
    {
        TextBox_Logger.AppendText($"[{DateTime.Now:T}]  {log}\n");
        TextBox_Logger.ScrollToEnd();
    }

    private void Button_L4D2MainDir_Click(object sender, RoutedEventArgs e)
    {
        SelectL4D2MainDir();
    }

    private void TextBox_L4D2MainDir_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        SelectL4D2MainDir();
    }

    private void Button_SteamAutoFind_Click(object sender, RoutedEventArgs e)
    {
        var dir = Workshop.GetL4D2InstallDir();

        if (!string.IsNullOrWhiteSpace(dir))
        {
            Globals.L4D2MainDir = dir;
            TextBox_L4D2MainDir.Text = Globals.L4D2MainDir;

            CheckEnv();
        }
        else
        {
            Globals.L4D2MainDir = string.Empty;
            TextBox_L4D2MainDir.Text = "Steam自动识别识别";
        }
    }

    private void SelectL4D2MainDir()
    {
        var folder = new OpenFileDialog
        {
            Title = "选择求生之路2主程序 left4dead2.exe",
            RestoreDirectory = true,
            Multiselect = false,
            DefaultExt = ".exe",
            Filter = "可执行文件 (*.exe)|*.exe",
            FileName = "left4dead2.exe"
        };

        if (!string.IsNullOrEmpty(Globals.L4D2MainDir))
            folder.InitialDirectory = Globals.L4D2MainDir;

        if (folder.ShowDialog() == true)
        {
            Globals.L4D2MainDir = Path.GetDirectoryName(folder.FileName);

            CheckEnv();
        }
    }

    /// <summary>
    /// 检测环境
    /// </summary>
    private void CheckEnv()
    {
        ClearLogger();
        Globals.IsConfigOk = false;

        TextBox_L4D2MainDir.Text = Globals.L4D2MainDir;

        if (CheckEnvDirPath())
        {
            AddLogger("✔ 求生之路2工具环境检查通过");

            AddLogger($"求生之路2 根目录路径: {Globals.L4D2MainDir}");

            AddLogger($"求生之路2 studiomdl.exe路径: {Globals.StudiomdlExec}");
            AddLogger($"求生之路2 vpk.exe路径: {Globals.VPKExec}");

            AddLogger($"求生之路2 Survivors路径: {Globals.L4D2SurvivorsDir}");
            AddLogger($"求生之路2 Weapons路径: {Globals.L4D2WeaponsDir}");
            AddLogger($"求生之路2 Addons路径: {Globals.L4D2AddonsDir}");

            Globals.IsConfigOk = true;
        }
    }

    /// <summary>
    /// 检测环境文件/文件夹
    /// </summary>
    /// <returns></returns>
    private bool CheckEnvDirPath()
    {
        if (!CheckEnvDirPathExists($"{Globals.L4D2MainDir}"))
            return false;
        if (!CheckEnvDirPathExists($"{Globals.StudiomdlExec}"))
            return false;
        if (!CheckEnvDirPathExists($"{Globals.VPKExec}"))
            return false;
        if (!CheckEnvDirPathExists($"{Globals.L4D2SurvivorsDir}"))
            return false;
        if (!CheckEnvDirPathExists($"{Globals.L4D2WeaponsDir}"))
            return false;
        if (!CheckEnvDirPathExists($"{Globals.L4D2AddonsDir}"))
            return false;

        if (!CheckEnvDirPathExists($"{Globals.L4D2MainDir}\\bin\\studiomdl.exe"))
            return false;
        if (!CheckEnvDirPathExists($"{Globals.L4D2MainDir}\\bin\\stdshader_dbg.dll"))
            return false;
        if (!CheckEnvDirPathExists($"{Globals.L4D2MainDir}\\bin\\stdshader_dx9.dll"))
            return false;
        if (!CheckEnvDirPathExists($"{Globals.L4D2MainDir}\\bin\\shaderapiempty.dll"))
            return false;
        if (!CheckEnvDirPathExists($"{Globals.L4D2MainDir}\\bin\\mdllib.dll"))
            return false;
        if (!CheckEnvDirPathExists($"{Globals.L4D2MainDir}\\bin\\studiorender.dll"))
            return false;
        if (!CheckEnvDirPathExists($"{Globals.L4D2MainDir}\\bin\\materialsystem.dll"))
            return false;
        if (!CheckEnvDirPathExists($"{Globals.L4D2MainDir}\\bin\\filesystem_stdio.dll"))
            return false;
        if (!CheckEnvDirPathExists($"{Globals.L4D2MainDir}\\bin\\vstdlib.dll"))
            return false;
        if (!CheckEnvDirPathExists($"{Globals.L4D2MainDir}\\bin\\tier0.dll"))
            return false;
        if (!CheckEnvDirPathExists($"{Globals.L4D2MainDir}\\left4dead2\\gameinfo.txt"))
            return false;

        return true;
    }

    /// <summary>
    /// 检测环境文件/文件夹是否存在
    /// </summary>
    /// <param name="envPath"></param>
    /// <returns></returns>
    private bool CheckEnvDirPathExists(string envPath)
    {
        if (FileUtil.IsDirectory(envPath))
        {
            if (Directory.Exists(envPath))
            {
                AddLogger($"✔ 已发现文件夹 {envPath}");
                return true;
            }
            else
            {
                AddLogger($"❌ 未发现文件夹 {envPath}");
                AddLogger("环境检查错误，操作结束");
                return false;
            }
        }
        else
        {
            if (File.Exists(envPath))
            {
                AddLogger($"✔ 已发现文件 {envPath}");
                return true;
            }
            else
            {
                AddLogger($"❌ 未发现文件 {envPath}");
                AddLogger("环境检查错误，操作结束");
                return false;
            }
        }
    }
}