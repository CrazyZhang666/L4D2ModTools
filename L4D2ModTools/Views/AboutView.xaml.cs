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

    /// <summary>
    /// 超链接请求导航事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
    {
        ProcessUtil.OpenLink(e.Uri.OriginalString);
        e.Handled = true;
    }
}