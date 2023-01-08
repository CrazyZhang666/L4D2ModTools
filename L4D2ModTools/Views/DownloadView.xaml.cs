namespace L4D2ModTools.Views;

/// <summary>
/// DownloadView.xaml 的交互逻辑
/// </summary>
public partial class DownloadView : UserControl
{
    public DownloadView()
    {
        InitializeComponent();
        this.DataContext = this;
        MainWindow.WindowClosingEvent += MainWindow_WindowClosingEvent;
    }

    private void MainWindow_WindowClosingEvent()
    {

    }

    private void Button_DownloadWorkshopFIle_Click(object sender, RoutedEventArgs e)
    {

    }
}
