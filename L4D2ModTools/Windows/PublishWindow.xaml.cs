using L4D2ModTools.Core;
using L4D2ModTools.Data;
using L4D2ModTools.Utils;
using L4D2ModTools.Steam;
using L4D2ModTools.Helper;

using Steamworks;
using Steamworks.Ugc;
using Steamworks.Data;

namespace L4D2ModTools.Windows;

/// <summary>
/// PublishWindow.xaml 的交互逻辑
/// </summary>
public partial class PublishWindow : Window
{
    public bool IsPublish = true;

    private const string NotUploadFile = "不上传VPK文件";

    //////////////////////////////////////////////////

    public PublishWindow(ItemInfo itemInfo, bool isPublish)
    {
        InitializeComponent();
        this.DataContext = this;

        IsPublish = isPublish;
        if (!isPublish)
        {
            TextBox_Title.Text = itemInfo.Title;
            TextBox_PreviewImage.Text = itemInfo.PreviewImage;
            TextBlock_Tags.Text = itemInfo.TagsContent;
            TextBlock_Id.Text = itemInfo.Id.ToString();
            TextBox_Description.Text = itemInfo.Description;

            if (itemInfo.IsPublic)
                RadioButton_IsPublic.IsChecked = true;
            else if (itemInfo.IsFriendsOnly)
                RadioButton_IsFriendsOnly.IsChecked = true;
            else if (itemInfo.IsPrivate)
                RadioButton_IsPrivate.IsChecked = true;
            else if (itemInfo.IsUnlisted)
                RadioButton_IsUnlisted.IsChecked = true;
        }
    }

    private void Window_Publish_Loaded(object sender, RoutedEventArgs e)
    {
        ComboBox_ContentFile.Items.Add(NotUploadFile);
        ComboBox_ContentFile.SelectedIndex = 0;
        foreach (var item in Directory.GetFiles(Globals.OutputDir))
        {
            if (Path.GetExtension(item) == ".vpk")
                ComboBox_ContentFile.Items.Add(Path.GetFileNameWithoutExtension(item));
        }

        if (IsPublish)
        {
            Title = "求生之路2 发布创意工坊";
            Button_PublishMod.Content = "发布Mod";

            RadioButton_IsFriendsOnly.IsChecked = true;
        }
        else
        {
            Title = "求生之路2 更新Mod信息";
            Button_PublishMod.Content = "更新Mod";
        }
    }

    private void Window_Publish_Closing(object sender, CancelEventArgs e)
    {
        Image_PreviewImage.Source = null;
        ProcessUtil.ClearMemory();
    }

    /// <summary>
    /// 报告进度
    /// </summary>
    /// <param name="progress"></param>
    private void ReportProgress(double progress)
    {
        this.Dispatcher.BeginInvoke(DispatcherPriority.Background, () =>
        {
            this.ProgressBar_Publish.Value = progress;
        });
    }

    private void ComboBox_ContentFile_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (ComboBox_ContentFile.SelectedItem is string fileName)
        {
            if (fileName == NotUploadFile)
                return;

            var vpk = $"{Globals.FullOutputDir}\\{fileName}.vpk";
            var jpg = $"{Globals.FullOutputDir}\\{fileName}.jpg";
            var json = $"{Globals.FullOutputDir}\\{fileName}.json";

            TextBox_VPKPath.Text = vpk;
            TextBox_PreviewImage.Text = jpg;

            var addonInfo = JsonHelper.ReadFile<AddonInfo>(json);
            TextBox_Title.Text = addonInfo.AddonTitle;
            TextBlock_Tags.Text = $"Survivors, {addonInfo.Survivor}";
            TextBox_Description.Text = addonInfo.Description;
        }
    }

    private async void Button_PublishMod_Click(object sender, RoutedEventArgs e)
    {
        Button_PublishMod.IsEnabled = false;

        if (IsPublish)
            await PublishMod();
        else
            await UpdateMod();

        Button_PublishMod.IsEnabled = true;
    }

    /// <summary>
    /// 发布L4D2创意工坊
    /// </summary>
    private async Task PublishMod()
    {
        ReportProgress(0.0);

        if (!Workshop.Init())
            return;

        // 标题
        var title = TextBox_Title.Text.Trim();
        if (string.IsNullOrWhiteSpace(title))
        {
            MsgBoxUtil.Warning("Mod标题不能为空，操作取消");
            return;
        }

        // 预览图绝对路径
        var imgPath = TextBox_PreviewImage.Text.Trim();
        if (!File.Exists(imgPath))
        {
            MsgBoxUtil.Warning("Mod预览图文件为空或不存在，操作取消");
            return;
        }
        // 预览图名称
        var imgName = Path.GetFileName(imgPath);

        // VPK文件绝对路径
        var vpkPath = TextBox_VPKPath.Text.Trim();
        if (!File.Exists(vpkPath))
        {
            MsgBoxUtil.Warning("Mod主体VPK文件不能为空或不存在，操作取消");
            return;
        }
        // VPK文件名称
        var vpkName = Path.GetFileName(vpkPath);

        // Mod描述
        var description = TextBox_Description.Text.Trim();

        // 可见性
        var visibility = new RemoteStoragePublishedFileVisibility();
        if (RadioButton_IsPublic.IsChecked == true)
            visibility = RemoteStoragePublishedFileVisibility.Public;
        else if (RadioButton_IsFriendsOnly.IsChecked == true)
            visibility = RemoteStoragePublishedFileVisibility.FriendsOnly;
        else if (RadioButton_IsPrivate.IsChecked == true)
            visibility = RemoteStoragePublishedFileVisibility.Private;
        else if (RadioButton_IsUnlisted.IsChecked == true)
            visibility = RemoteStoragePublishedFileVisibility.Unlisted;

        // 标签
        var tags = new List<string> { "Survivors" };
        var tag = TextBlock_Tags.Text.Replace("Survivors,", "").Trim();
        if (!string.IsNullOrWhiteSpace(tag))
            tags.Add(tag);

        ReportProgress(0.2);

        // 预览图二进制文件
        var imgData = await File.ReadAllBytesAsync(imgPath);
        // 上传预览图文件到Steam云存储
        if (!SteamRemoteStorage.FileWrite(imgName, imgData))
        {
            MsgBoxUtil.Error("上传预览图文件到Steam云存储失败，操作取消");
            return;
        }

        ReportProgress(0.5);

        // VPK二进制文件
        var vpkData = await File.ReadAllBytesAsync(vpkPath);
        // 上传VPK文件到Steam云存储
        if (!SteamRemoteStorage.FileWrite(vpkName, vpkData))
        {
            MsgBoxUtil.Error("上传VPK文件到Steam云存储失败，操作取消");
            return;
        }

        ReportProgress(0.8);

        // 从Steam云存储发布
        var result_t = await SteamRemoteStorage.PublishWorkshopFile(vpkName, imgName, title, description, visibility, tags);
        if (result_t.Value.Result == Result.OK)
        {
            ReportProgress(1.0);
            MsgBoxUtil.Information($"发布L4D2创意工坊成功，物品Id：{result_t.Value.PublishedFileId.Value}");
        }
        else
        {
            MsgBoxUtil.Error($"发布L4D2创意工坊失败 {result_t.Value.Result}，操作取消");
            ReportProgress(0.0);
        }
    }

    /// <summary>
    /// 更新选中Mod信息
    /// </summary>
    private async Task UpdateMod()
    {
        // 物品Id
        var id = TextBlock_Id.Text.Trim();
        if (string.IsNullOrWhiteSpace(id))
        {
            MsgBoxUtil.Error("创意工坊物品Id不能为空，操作取消");
            return;
        }

        ///////////////////////////////////////////////////

        // 创建更新Mod请求
        var editor = new Editor(new PublishedFileId
        {
            Value = ulong.Parse(id)
        });

        // 标题
        var title = TextBox_Title.Text.Trim();
        if (string.IsNullOrWhiteSpace(title))
        {
            MsgBoxUtil.Warning("Mod标题不能为空，操作取消");
            return;
        }

        // 预览图绝对路径
        var imgPath = TextBox_PreviewImage.Text.Trim();
        if (!imgPath.StartsWith("http"))
        {
            if (!File.Exists(imgPath))
            {
                MsgBoxUtil.Warning("Mod预览图文件为空或不存在，操作取消");
                return;
            }
            else
            {
                // 预览图
                editor.WithPreviewFile(imgPath);
            }
        }

        ReportProgress(0.2);

        // Mod描述
        var description = TextBox_Description.Text.Trim();

        editor.WithTitle(title);
        editor.WithDescription(description);

        // 可见性
        if (RadioButton_IsPublic.IsChecked == true)
            editor.WithPublicVisibility();
        else if (RadioButton_IsFriendsOnly.IsChecked == true)
            editor.WithFriendsOnlyVisibility();
        else if (RadioButton_IsPrivate.IsChecked == true)
            editor.WithPrivateVisibility();
        else if (RadioButton_IsUnlisted.IsChecked == true)
            editor.WithUnlistedVisibility();

        // 标签
        var tag = TextBlock_Tags.Text.Replace("Survivors,", "").Trim();
        if (!string.IsNullOrWhiteSpace(tag))
        {
            editor.WithTag("Survivors");
            editor.WithTag(tag);
        }

        ReportProgress(0.3);

        // 提交更新请求
        var result = await editor.SubmitAsync();
        if (result.Success)
        {
            ReportProgress(0.5);

            // VPK文件绝对路径
            var vpkPath = TextBox_VPKPath.Text.Trim();
            if (string.IsNullOrWhiteSpace(vpkPath))
            {
                MsgBoxUtil.Information($"更新选中Mod信息成功 {result.Result}");
                ReportProgress(1.0);
                return;
            }

            if (File.Exists(vpkPath))
            {
                // VPK文件名称
                var vpkName = Path.GetFileName(vpkPath);
                // VPK二进制文件
                var vpkData = await File.ReadAllBytesAsync(vpkPath);

                ReportProgress(0.6);

                // 上传VPK文件到Steam云存储
                if (SteamRemoteStorage.FileWrite(vpkName, vpkData))
                {
                    ReportProgress(0.8);

                    var fileId = new PublishedFileId
                    {
                        Value = ulong.Parse(id)
                    };
                    var changeLog = TextBox_ChangeLog.Text.Trim();
                    // 更新VPK文件和分部分项日志
                    if (SteamRemoteStorage.UpdatePublished(fileId, changeLog, vpkName))
                    {
                        ReportProgress(1.0);
                        if (MessageBox.Show("更新已发布物品VPK文件成功，现在关闭窗口吗？", "更新成功",
                            MessageBoxButton.OKCancel, MessageBoxImage.Information) == MessageBoxResult.OK)
                        {
                            this.Close();
                        }
                    }
                    else
                    {
                        MsgBoxUtil.Error("更新已发布物品VPK文件失败，操作取消");
                        ReportProgress(0.0);
                        return;
                    }
                }
                else
                {
                    MsgBoxUtil.Error("上传VPK文件到Steam云存储失败，操作取消");
                    ReportProgress(0.0);
                    return;
                }
            }
            else
            {
                MsgBoxUtil.Warning("Mod主体VPK文件不能为空或不存在，操作取消");
                ReportProgress(0.0);
                return;
            }
        }
        else
        {
            MsgBoxUtil.Error($"更新选中Mod信息失败 {result.Result}，操作取消");
            ReportProgress(0.0);
        }
    }
}
