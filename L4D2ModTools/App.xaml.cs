using L4D2ModTools.Utils;

namespace L4D2ModTools;

/// <summary>
/// App.xaml 的交互逻辑
/// </summary>
public partial class App : Application
{
    /// <summary>
    /// 主程序互斥体
    /// </summary>
    public static Mutex AppMainMutex;
    /// <summary>
    /// 应用程序名称
    /// </summary>
    private string AppName = ResourceAssembly.GetName().Name;

    /// <summary>
    /// 保证程序只能同时启动一个
    /// </summary>
    /// <param name="e"></param>
    protected override void OnStartup(StartupEventArgs e)
    {
        AppMainMutex = new Mutex(true, AppName, out var createdNew);

        if (createdNew)
        {
            base.OnStartup(e);
        }
        else
        {
            MsgBoxUtil.Warning($"请不要重复打开，程序已经运行\n如果一直提示，请到\"任务管理器-详细信息（win7为进程）\"里\n强制结束 \"{AppName}.exe\" 程序"
                , "重复运行警告");
            Current.Shutdown();
        }
    }
}
