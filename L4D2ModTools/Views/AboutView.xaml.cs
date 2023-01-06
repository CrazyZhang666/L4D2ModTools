using L4D2ModTools.Utils;

namespace L4D2ModTools.Views;

/// <summary>
/// AboutView.xaml 的交互逻辑
/// </summary>
public partial class AboutView : UserControl
{
    public AboutView()
    {
        InitializeComponent();
        this.DataContext = this;
        MainWindow.WindowClosingEvent += MainWindow_WindowClosingEvent;

        TextBlock_VersionInfo.Text = $"版本 v{MiscUtil.VersionInfo}";
        TextBlock_BuildTime.Text = $"编译时间 {MiscUtil.BuildTime}";
    }

    private void MainWindow_WindowClosingEvent()
    {
        
    }
}
