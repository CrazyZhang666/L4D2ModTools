using L4D2ModTools.Data;
using L4D2ModTools.Utils;

namespace L4D2ModTools;

/// <summary>
/// MainWindow.xaml 的交互逻辑
/// </summary>
public partial class MainWindow : Window
{
    /// <summary>
    /// 导航菜单
    /// </summary>
    public List<NavMenu> NavMenus { get; set; } = new();
    /// <summary>
    /// 导航字典
    /// </summary>
    private Dictionary<string, UserControl> NavDictionary = new();

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
        this.DataContext = this;

        MainWindowInstance = this;
        this.Title = $"求生之路2 Mod工具箱 v{MiscUtil.VersionInfo} - 编译时间 {MiscUtil.BuildTime}";

        CreateNavMenu();
        Navigate(NavDictionary.First().Key);

        ActionTaskbarProgress = TaskbarProgress;
    }

    private void Window_Main_Loaded(object sender, RoutedEventArgs e)
    {

    }

    private void Window_Main_Closing(object sender, CancelEventArgs e)
    {
        WindowClosingEvent();
    }

    /// <summary>
    /// 创建导航菜单
    /// </summary>
    private void CreateNavMenu()
    {
        NavMenus.Add(new() { Icon = "\xe60f", Title = "目录配置", ViewName = "ConfigView" });
        NavMenus.Add(new() { Icon = "\xe606", Title = "VPK处理", ViewName = "ReadyView" });
        NavMenus.Add(new() { Icon = "\xe78e", Title = "QC重编译", ViewName = "CompileView" });
        NavMenus.Add(new() { Icon = "\xec89", Title = "VPK打包", ViewName = "AddonView" });
        NavMenus.Add(new() { Icon = "\xe652", Title = "发布工坊", ViewName = "PublishView" });
        NavMenus.Add(new() { Icon = "\xe603", Title = "关于", ViewName = "AboutView" });

        NavMenus.ForEach(menu =>
        {
            var type = Type.GetType($"L4D2ModTools.Views.{menu.ViewName}");
            NavDictionary.Add(menu.ViewName, Activator.CreateInstance(type) as UserControl);
        });
    }

    /// <summary>
    /// 页面导航
    /// </summary>
    /// <param name="menu"></param>
    [RelayCommand]
    private void Navigate(string viewName)
    {
        if (NavDictionary.ContainsKey(viewName))
        {
            ContentControl_NavRegion.Content = NavDictionary[viewName];
        }
    }

    private void TaskbarProgress(double value)
    {
        TaskbarItemInfo.ProgressValue = value;
    }
}
