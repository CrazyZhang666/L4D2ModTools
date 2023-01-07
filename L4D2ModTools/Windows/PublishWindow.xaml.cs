using L4D2ModTools.Data;
using L4D2ModTools.Utils;

using Steamworks.Ugc;
using Steamworks.Data;
using L4D2ModTools.Core;

namespace L4D2ModTools.Windows;

/// <summary>
/// PublishWindow.xaml 的交互逻辑
/// </summary>
public partial class PublishWindow : Window
{
    public ItemInfo ItemInfo { get; set; }
    public bool IsPublish { get; set; }

    //////////////////////////////////////////////////

    private Progress<float> Progress { get; set; }

    public PublishWindow(ItemInfo itemInfo, bool isPublish)
    {
        InitializeComponent();
        this.DataContext = this;

        ItemInfo = itemInfo;
        IsPublish = isPublish;
    }

    private void Window_Publish_Loaded(object sender, RoutedEventArgs e)
    {
        Progress = new Progress<float>(ReportProgress);

        if (IsPublish)
        {
            Title = "发布L4D2创意工坊";
            Button_PublishMod.Content = "发布Mod";

            RadioButton_IsFriendsOnly.IsChecked = true;

            foreach (var item in Directory.GetFiles(Globals.OutputDir))
            {
                if (Path.GetExtension(item) == ".vpk")
                    ComboBox_ContentFile.Items.Add(Path.GetFileName(item));
            }
        }
        else
        {
            Title = "更新选中Mod信息";
            Button_PublishMod.Content = "更新Mod";

            ComboBox_ContentFile.Items.Add("保持默认");
            foreach (var item in Directory.GetFiles(Globals.OutputDir))
            {
                if (Path.GetExtension(item) == ".vpk")
                    ComboBox_ContentFile.Items.Add(Path.GetFileName(item));
            }
        }
    }

    private void Window_Publish_Closing(object sender, CancelEventArgs e)
    {

    }

    /// <summary>
    /// 报告进度
    /// </summary>
    /// <param name="progress"></param>
    private void ReportProgress(float progress)
    {
        this.ProgressBar_Publish.Value = progress;
    }

    private void Button_PublishMod_Click(object sender, RoutedEventArgs e)
    {
        if (IsPublish)
            PublishMod();
        else
            UpdateMod();
    }

    /// <summary>
    /// 发布新的Mod
    /// </summary>
    private void PublishMod()
    {

    }

    /// <summary>
    /// 更新选中Mod信息
    /// </summary>
    private async void UpdateMod()
    {
        if (ItemInfo.Id == 0)
        {
            MsgBoxUtil.Error("创意工坊项目ID为空，操作取消");
            return;
        }

        var editor = new Editor(new PublishedFileId
        {
            Value = ItemInfo.Id
        });

        ///////////////////////////////////////////////////

        if (!string.IsNullOrWhiteSpace(ItemInfo.Title))
        {
            editor.WithTitle(ItemInfo.Title);
        }
        else
        {
            MsgBoxUtil.Error("Mod标题为空，操作取消");
            return;
        }

        if (!string.IsNullOrWhiteSpace(ItemInfo.ContentFile))
        {
            if (Directory.Exists(ItemInfo.ContentFile))
            {
                if (Directory.GetFiles(ItemInfo.ContentFile).Length != 0)
                {
                    editor.WithContent(new DirectoryInfo(ItemInfo.ContentFile));
                }
            }
        }

        editor.WithDescription(ItemInfo.Description);
        editor.WithChangeLog(ItemInfo.ChangeLog);

        //if (string.IsNullOrWhiteSpace(ItemInfo.PreviewImage))
        //{
        //    editor.WithPreviewFile(ItemInfo.PreviewImage);
        //}

        //foreach (var tag in ItemInfo.Tags)
        //{
        //    editor.WithTag(tag);
        //}

        //editor.WithFriendsOnlyVisibility();

        await editor.SubmitAsync(Progress);
    }
}
