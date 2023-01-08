using L4D2ModTools.Data;
using L4D2ModTools.Steam;
using L4D2ModTools.Utils;

using Steamworks;

namespace L4D2ModTools.Windows;

/// <summary>
/// StorageWindow.xaml 的交互逻辑
/// </summary>
public partial class StorageWindow : Window
{
    public ObservableCollection<StorageInfo> StorageInfoLists { get; set; } = new();

    public StorageWindow()
    {
        InitializeComponent();
        this.DataContext = this;
    }

    private void Window_Storage_Loaded(object sender, RoutedEventArgs e)
    {
        RefreshList();
    }

    private void Window_Storage_Closing(object sender, CancelEventArgs e)
    {

    }

    /// <summary>
    /// 自动调整列宽
    /// </summary>
    private void AutoColumWidth()
    {
        lock (this)
        {
            if (ListView_RemoteStorage.View is GridView view)
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

            Border_QuotaUse.Width = 1.0 * quotaUsedBytes / quotaBytes * 300;
            TextBlock_QuotaInfo.Text = $"{MiscUtil.ByteConverterMB(quotaUsedBytes)} 已存储 / {MiscUtil.ByteConverterMB(quotaBytes)} 总大小";
        }
    }

    /// <summary>
    /// 刷新列表
    /// </summary>
    private async void RefreshList()
    {
        Button_RefreshList.IsEnabled = false;

        int index = 1;
        StorageInfoLists.Clear();
        foreach (var file in SteamRemoteStorage.Files)
        {
            this.Dispatcher.Invoke(DispatcherPriority.Background, () =>
            {
                StorageInfoLists.Add(new()
                {
                    Index = index++,
                    Name = file,
                    Size = MiscUtil.ByteConverterMB(SteamRemoteStorage.FileSize(file)),
                    Date = MiscUtil.FormatDateTime(SteamRemoteStorage.FileTime(file)),
                    Exists = MiscUtil.BoolToFlag(SteamRemoteStorage.FileExists(file)),
                    Persisted = MiscUtil.BoolToFlag(SteamRemoteStorage.FilePersisted(file))
                });
            });
        }

        RefushQuotaInfo();

        await Task.Delay(500);
        AutoColumWidth();

        Button_RefreshList.IsEnabled = true;
    }

    /// <summary>
    /// 刷新列表按钮点击事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Button_RefreshList_Click(object sender, RoutedEventArgs e)
    {
        RefreshList();
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

    /// <summary>
    /// 删除选中项按钮点击事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Button_DeleteRemoteStorage_Click(object sender, RoutedEventArgs e)
    {
        if (ListView_RemoteStorage.SelectedItem is StorageInfo info)
        {
            if (MessageBox.Show($"您确定要删除这个文件吗？此操作不可撤销！\n\n文件名：{info.Name}\n文件大小：{info.Size}",
                "删除文件", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
            {
                if (SteamRemoteStorage.FileDelete(info.Name))
                    MsgBoxUtil.Information($"删除文件 {info.Name} 成功");
                else
                    MsgBoxUtil.Error($"删除文件 {info.Name} 失败");

                RefreshList();
            }
        }
    }
}
