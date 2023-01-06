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

    /// <summary>
    /// 报告进度
    /// </summary>
    public static Action<double> ActionTaskbarProgress;

    public MainWindow()
    {
        InitializeComponent();
    }

    private void Window_Main_Loaded(object sender, RoutedEventArgs e)
    {
        this.DataContext = this;

        this.Title = $"求生之路2 Mod工具箱 v{MiscUtil.VersionInfo} - 编译时间 {MiscUtil.BuildTime}";

        ActionTaskbarProgress = TaskbarProgress;

        try
        {
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
                // 创建输出文件夹
                Directory.CreateDirectory(Globals.OutputDir);
            }
            else
            {
                MsgBoxUtil.Error("未发现AppData.bin，请更新工具版本");
            }
        }
        catch (Exception ex)
        {
            MsgBoxUtil.Exception(ex);
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
