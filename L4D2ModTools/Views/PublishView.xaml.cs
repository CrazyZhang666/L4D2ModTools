using L4D2ModTools.Data;
using L4D2ModTools.Utils;
using L4D2ModTools.Steam;
using L4D2ModTools.Windows;

using Steamworks;

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
        this.Dispatcher.BeginInvoke(DispatcherPriority.Background, () =>
        {
            TextBox_Logger.AppendText($"[{DateTime.Now:HH:mm:ss.fff}] {log}\n");
            TextBox_Logger.ScrollToEnd();
        });
    }

    /// <summary>
    /// 自动调整列宽
    /// </summary>
    private void AutoColumWidth()
    {
        lock (this)
        {
            if (ListView_WorkShops.View is GridView view)
            {
                foreach (GridViewColumn gvc in view.Columns)
                {
                    gvc.Width = 100;
                    gvc.Width = double.NaN;
                }
            }
        }
    }

    /// <summary>
    /// 刷新配额信息
    /// </summary>
    private void RefushQuotaInfo()
    {
        if (Workshop.Init())
        {
            var quotaBytes = SteamRemoteStorage.QuotaBytes;
            var quotaUsedBytes = SteamRemoteStorage.QuotaUsedBytes;

            Border_QuotaUse.Width = 1.0 * quotaUsedBytes / quotaBytes * 200;
            TextBlock_QuotaInfo.Text = $"{MiscUtil.ByteConverterMB(quotaUsedBytes)} / {MiscUtil.ByteConverterMB(quotaBytes)}";
        }
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
    /// 启动Steam按钮点击事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Button_RunSteam_Click(object sender, RoutedEventArgs e)
    {
        Client.Run();
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
        if (itemInfos.Count > 0)
        {
            itemInfos.ForEach(info =>
            {
                this.Dispatcher.BeginInvoke(DispatcherPriority.Background, () =>
                {
                    ItemInfoLists.Add(info);
                });
                AddLogger($"{info.Index} {info.Title}");
            });

            AddLogger("刷新Mod列表完成");
        }

        RefushQuotaInfo();

        Button_RefushModList.IsEnabled = true;

        await Task.Delay(500);
        AutoColumWidth();
    }

    /// <summary>
    /// 管理STEAM云按钮点击事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Button_SteamCloud_Click(object sender, RoutedEventArgs e)
    {
        if (Workshop.Init())
        {
            var storageWindow = new StorageWindow()
            {
                Owner = MainWindow.MainWindowInstance
            };
            storageWindow.ShowDialog();
        }
    }

    /// <summary>
    /// 发布L4D2创意工坊按钮点击事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Button_PublishWorkshop_Click(object sender, RoutedEventArgs e)
    {
        if (Workshop.Init())
        {
            var publishWindow = new PublishWindow(new ItemInfo(), true)
            {
                Owner = MainWindow.MainWindowInstance
            };
            publishWindow.ShowDialog();
        }
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
            var publishWindow = new PublishWindow(info, false)
            {
                Owner = MainWindow.MainWindowInstance
            };
            publishWindow.ShowDialog();
        }
    }

    /// <summary>
    /// 删除选中Mod
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Button_DeleteWorkshop_Click(object sender, RoutedEventArgs e)
    {
        if (ListView_WorkShops.SelectedItem is ItemInfo info)
        {
            if (MessageBox.Show($"您确定要删除这件物品吗？此操作不可撤销！\n\n标题：{info.Title}\n物品ID：{info.Id}",
                "删除物品", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
            {
                Workshop.DeletePublishedFile(info.Id);
            }
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
        AutoColumWidth();
    }
}