using L4D2ModTools.Core;
using L4D2ModTools.Data;

namespace L4D2ModTools.Views;

/// <summary>
/// PublishView.xaml 的交互逻辑
/// </summary>
public partial class PublishView : UserControl
{
    public ObservableCollection<ItemInfo> ItemInfoLists { get; set; } = new();

    public PublishView()
    {
        InitializeComponent();
        this.DataContext = this;
        MainWindow.WindowClosingEvent += MainWindow_WindowClosingEvent;
    }

    private void MainWindow_WindowClosingEvent()
    {
        Workshop.ShutDown();
    }

    /// <summary>
    /// 清空日志
    /// </summary>
    private void ClearLogger()
    {
        this.Dispatcher.Invoke(() =>
        {
            TextBox_Logger.Clear();
        });
    }

    /// <summary>
    /// 增加日志信息
    /// </summary>
    /// <param name="log"></param>
    private void AddLogger(string log)
    {
        this.Dispatcher.Invoke(() =>
        {
            TextBox_Logger.AppendText($"[{DateTime.Now:T}]  {log}\n");
            TextBox_Logger.ScrollToEnd();
        });
    }

    /// <summary>
    /// 刷新Mod列表按钮点击事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void Button_RefushModList_Click(object sender, RoutedEventArgs e)
    {
        Button_RefushModList.IsEnabled = false;

        ItemInfoLists.Clear();

        var itemInfos = await Workshop.GetUserPublished();
        itemInfos.ForEach(info =>
        {
            ItemInfoLists.Add(info);
        });

        Button_RefushModList.IsEnabled = true;
    }

    /// <summary>
    /// 上传L4D2创意工坊按钮点击事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Button_PublishWorkshop_Click(object sender, RoutedEventArgs e)
    {
    }
}