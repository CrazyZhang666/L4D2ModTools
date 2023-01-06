using L4D2ModTools.Core;
using L4D2ModTools.Helper;
using L4D2ModTools.Utils;

namespace L4D2ModTools.Views;

/// <summary>
/// ReadyView.xaml 的交互逻辑
/// </summary>
public partial class ReadyView : UserControl
{
    public ReadyView()
    {
        InitializeComponent();
        this.DataContext = this;
        MainWindow.WindowClosingEvent += MainWindow_WindowClosingEvent;

        // 读取对应配置文件
        Globals.UnPackDir = IniHelper.ReadValue("Config", "UnPackDir");

        TextBox_UnPackDir.Text = Globals.UnPackDir;

        if (!string.IsNullOrEmpty(Globals.UnPackDir))
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
        IniHelper.WriteValue("Config", "UnPackDir", Globals.UnPackDir);
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

    /// <summary>
    /// 清空缓存文件夹按钮点击事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Button_ClearCacheDir_Click(object sender, RoutedEventArgs e)
    {
        Models.ClearCache();
        Arms.ClearCache();
        Compile.ClearCache();

        MsgBoxUtil.Information("清空缓存文件夹成功");
    }

    private void Button_L4D2SurvivorsDir_Click(object sender, RoutedEventArgs e)
    {
        ProcessUtil.OpenLink(Globals.L4D2SurvivorsDir);
    }

    private void Button_L4D2WeaponsDir_Click(object sender, RoutedEventArgs e)
    {
        ProcessUtil.OpenLink(Globals.L4D2WeaponsDir);
    }

    private void Button_RunCrowbar_Click(object sender, RoutedEventArgs e)
    {
        Process.Start(Globals.ToolKitsDir + "\\Crowbar.exe");
    }

    private void Border_DropVPKUnPack_DragEnter(object sender, DragEventArgs e)
    {
        if (e.Data.GetDataPresent(DataFormats.FileDrop))
            e.Effects = DragDropEffects.Copy;
    }

    private void Border_DropVPKUnPack_Drop(object sender, DragEventArgs e)
    {
        if (e.Data.GetDataPresent(DataFormats.FileDrop))
        {
            foreach (string item in e.Data.GetData(DataFormats.FileDrop) as Array)
            {
                if (Path.GetExtension(item) == ".vpk")
                    Compile.RunL4D2DevExec(Globals.VPKExec, $"\"{item}\"");
            }
        }
    }

    private void Border_DropVPKPack_DragEnter(object sender, DragEventArgs e)
    {
        if (e.Data.GetDataPresent(DataFormats.FileDrop))
            e.Effects = DragDropEffects.Copy;
    }

    private void Border_DropVPKPack_Drop(object sender, DragEventArgs e)
    {
        if (e.Data.GetDataPresent(DataFormats.FileDrop))
        {
            foreach (string item in e.Data.GetData(DataFormats.FileDrop) as Array)
            {
                if (FileUtil.IsDirectory(item) && File.Exists(item + "\\addoninfo.txt"))
                    Compile.RunL4D2DevExec(Globals.VPKExec, $"\"{item}\"");
            }
        }
    }

    /////////////////////////////////////////////////////////////////

    private void Button_UnPackDir_Click(object sender, RoutedEventArgs e)
    {
        SelectUnPackDir();
    }

    private void TextBox_UnPackDir_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        SelectUnPackDir();
    }

    private void SelectUnPackDir()
    {
        var folder = new OpenFileDialog
        {
            Title = "VPK解包文件夹 addoninfo.txt",
            RestoreDirectory = true,
            Multiselect = false,
            DefaultExt = ".exe",
            Filter = "文本文件 (*.txt)|*.txt",
            FileName = "addoninfo.txt"
        };

        if (!string.IsNullOrEmpty(Globals.UnPackDir))
            folder.InitialDirectory = Globals.UnPackDir;

        if (folder.ShowDialog() == true)
        {
            Globals.UnPackDir = Path.GetDirectoryName(folder.FileName);

            CheckEnv();
        }
    }

    /// <summary>
    /// 检测环境
    /// </summary>
    private void CheckEnv()
    {
        ClearLogger();
        Globals.IsReadyOk = false;

        TextBox_UnPackDir.Text = Globals.UnPackDir;

        if (CheckEnvDirPath())
        {
            AddLogger("✔ VPK解包文件夹环境检查通过");

            AddLogger($"VPK解包 根目录路径: {Globals.UnPackDir}");

            AddLogger($"VPK解包 addonimage.jpg路径: {Globals.UnPackAddonImagePath}");
            AddLogger($"VPK解包 addoninfo.txt路径: {Globals.UnPackAddonInfoPath}");
            AddLogger($"VPK解包 materials路径: {Globals.UnPackMaterialsDir}");
            AddLogger($"VPK解包 vgui路径: {Globals.UnPackVGUIDir}");

            AddLogger($"VPK解包 survivors反编译路径: {Globals.UnPackSurvivorsDecoDir}");
            AddLogger($"VPK解包 arms反编译路径: {Globals.UnPackWeaponsDecoDir}");

            Globals.IsReadyOk = true;
        }
    }

    /// <summary>
    /// 检测环境文件/文件夹
    /// </summary>
    /// <returns></returns>
    private bool CheckEnvDirPath()
    {
        if (!CheckEnvDirPathExists(Globals.UnPackDir))
            return false;
        if (!CheckEnvDirPathExists(Globals.UnPackAddonImagePath))
            return false;
        if (!CheckEnvDirPathExists(Globals.UnPackAddonInfoPath))
            return false;
        if (!CheckEnvDirPathExists(Globals.UnPackMaterialsDir))
            return false;
        if (!CheckEnvDirPathExists(Globals.UnPackVGUIDir))
            return false;
        if (!CheckEnvDirPathExists(Globals.UnPackSurvivorsDecoDir))
            return false;
        if (!CheckEnvDirPathExists(Globals.UnPackWeaponsDecoDir))
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