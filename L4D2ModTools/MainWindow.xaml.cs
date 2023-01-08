using L4D2ModTools.Core;
using L4D2ModTools.Utils;
using L4D2ModTools.Helper;

namespace L4D2ModTools;

/// <summary>
/// MainWindow.xaml 的交互逻辑
/// </summary>
public partial class MainWindow : Window
{
    /// <summary>
    /// 主窗口关闭委托
    /// </summary>
    public delegate void WindowClosingDelegate();
    /// <summary>
    /// 主窗口关闭事件
    /// </summary>
    public static event WindowClosingDelegate WindowClosingEvent;

    ///////////////////////////////////////////////////////

    /// <summary>
    /// 向外暴露主窗口实例
    /// </summary>
    public static Window MainWindowInstance { get; private set; }

    /// <summary>
    /// 报告状态栏进度委托
    /// </summary>
    public static Action<double> ActionTaskbarProgress;

    public MainWindow()
    {
        InitializeComponent();
    }

    private void Window_Main_Loaded(object sender, RoutedEventArgs e)
    {
        this.DataContext = this;
        MainWindowInstance = this;

        this.Title = $"求生之路2 Mod工具箱 v{MiscUtil.VersionInfo} - 编译时间 {MiscUtil.BuildTime}";

        ActionTaskbarProgress = TaskbarProgress;

        try
        {
            // 创建文件夹
            Directory.CreateDirectory(Globals.OutputDir);
            Directory.CreateDirectory(Globals.ConfigDir);

            // 创建INI配置文件
            IniHelper.Create();

            // 释放数据文件
            MiscUtil.ExtractResFile("L4D2ModTools.Files.AppData.zip", ".\\AppData.bin");
            MiscUtil.ExtractResFile("L4D2ModTools.Files.steam_api.dll", ".\\steam_api.dll");

            // 解压数据文件
            if (File.Exists(".\\AppData.bin"))
            {
                if (!Directory.Exists(Globals.AppDataDir))
                {
                    using var archive = ZipFile.OpenRead(".\\AppData.bin");
                    archive.ExtractToDirectory(Globals.AppDataDir);
                }
            }
            else
            {
                MsgBoxUtil.Error("未发现AppData.bin，请更新工具版本");
                Application.Current.Shutdown();
            }
        }
        catch (Exception ex)
        {
            MsgBoxUtil.Exception(ex);
            Application.Current.Shutdown();
        }
    }

    private void Window_Main_Closing(object sender, CancelEventArgs e)
    {
        WindowClosingEvent();
        ProcessUtil.CloseThirdProcess();
    }

    private void TaskbarProgress(double value)
    {
        TaskbarItemInfo.ProgressValue = value;
    }
}
