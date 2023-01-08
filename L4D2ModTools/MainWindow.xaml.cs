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
    }

    private void Window_Main_Closing(object sender, CancelEventArgs e)
    {
        WindowClosingEvent();
    }

    private void TaskbarProgress(double value)
    {
        TaskbarItemInfo.ProgressValue = value;
    }
}
