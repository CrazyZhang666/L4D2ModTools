using L4D2ModTools.Core;
using L4D2ModTools.Steam;
using L4D2ModTools.Utils;

using Steamworks;
using Steamworks.Data;

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

    /// <summary>
    /// 报告进度
    /// </summary>
    /// <param name="progress"></param>
    private void ReportProgress(float progress)
    {
        this.Dispatcher.BeginInvoke(DispatcherPriority.Background, () =>
        {
            this.ProgressBar_Download.Value = progress;
        });
    }

    private async void Button_DownloadWorkshopFile_Click(object sender, RoutedEventArgs e)
    {
        var link = TextBox_WorkshopFIleLink.Text.Trim();
        if (string.IsNullOrWhiteSpace(link))
        {
            MsgBoxUtil.Warning("创意工坊物品下载链接不能为空");
            return;
        }

        link = link.Replace("https://steamcommunity.com/sharedfiles/filedetails/?id=", "");
        if (!Regex.IsMatch(link, "^[0-9]{10}$"))
        {
            MsgBoxUtil.Warning("请输入正确的创意工坊物品下载链接");
            return;
        }

        if (!Workshop.Init())
        {
            MsgBoxUtil.Warning("未发现Steam进程，请先启动Steam客户端");
            return;
        }

        var fileId = new PublishedFileId
        {
            Value = ulong.Parse(link)
        };
        var result = await SteamUGC.DownloadAsync(fileId, ReportProgress);
        if (!result)
        {
            MsgBoxUtil.Error($"请物品 {fileId} 下载失败");
            return;
        }

        var path = Client.GetMainPath();
        if (string.IsNullOrEmpty(path))
            return;

        var dir = $"{Path.GetDirectoryName(path)}\\steamapps\\workshop\\content\\550\\{link}";
        if (!Directory.Exists(dir))
            return;

        foreach (var file in Directory.GetFiles(dir))
        {
            if (Path.GetExtension(file) != ".bin")
                continue;

            File.Copy(file, $"{Globals.VPKDir}\\{fileId}.vpk", true);
        }

        MsgBoxUtil.Information($"物品 {fileId} 下载完成");
    }

    private void Button_VPKDir_Click(object sender, RoutedEventArgs e)
    {
        ProcessUtil.OpenLink(Globals.VPKDir);
    }
}
