using L4D2ModTools.Core;
using L4D2ModTools.Data;
using L4D2ModTools.Utils;
using L4D2ModTools.Windows;

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
    /// 超链接请求导航事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
    {
        ProcessUtil.OpenLink(e.Uri.OriginalString);
        e.Handled = true;
    }

    /// <summary>
    /// 刷新Mod列表按钮点击事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void Button_RefushModList_Click(object sender, RoutedEventArgs e)
    {
        Button_RefushModList.IsEnabled = false;
        ClearLogger();

        ItemInfoLists.Clear();

        var itemInfos = await Workshop.GetUserPublished();
        itemInfos.ForEach(info =>
        {
            ItemInfoLists.Add(info);
            AddLogger($"{info.Index} {info.Title}");
        });

        Button_RefushModList.IsEnabled = true;
        AddLogger("刷新Mod列表完成");
    }

    /// <summary>
    /// 上传L4D2创意工坊按钮点击事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Button_PublishWorkshop_Click(object sender, RoutedEventArgs e)
    {
        var publishWindow = new PublishWindow
        {
            Owner = MainWindow.MainWindowInstance
        };
        publishWindow.ShowDialog();
    }

    /// <summary>
    /// 更新选中Mod信息钮点击事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Button_UpdateWorkshop_Click(object sender, RoutedEventArgs e)
    {
        if (ListView_WorkShops.SelectedItem is ItemInfo info)
        {
            var updateWindow = new UpdateWindow(info)
            {
                Owner = MainWindow.MainWindowInstance
            };
            updateWindow.ShowDialog();
        }
    }

    /// <summary>
    /// 重启工具按钮点击事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Button_RestarApp_Click(object sender, RoutedEventArgs e)
    {
        ProcessUtil.OpenExecWithArgs(FileUtil.CurrentAppPath);
        Application.Current.Shutdown();
    }

    /// <summary>
    /// 自动调整列宽按钮点击事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Button_AutoColumWidth_Click(object sender, RoutedEventArgs e)
    {
        lock (this)
        {
            if (ListView_WorkShops.View is GridView workshop)
            {
                foreach (GridViewColumn gvc in workshop.Columns)
                {
                    gvc.Width = 100;
                    gvc.Width = double.NaN;
                }
            }
        }
    }
}