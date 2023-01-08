using L4D2ModTools.Core;
using L4D2ModTools.Data;
using L4D2ModTools.Utils;
using L4D2ModTools.Steam;
using L4D2ModTools.Helper;

using Steamworks;
using Steamworks.Ugc;
using Steamworks.Data;
using System.Security.Principal;
using System.Reflection.Metadata;
using System;

namespace L4D2ModTools.Windows;

/// <summary>
/// PublishWindow.xaml 的交互逻辑
/// </summary>
public partial class PublishWindow : Window
{
    private Progress<float> Progress;

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
        Progress = new Progress<float>(ReportProgress);

        ComboBox_ContentFile.Items.Add(NotUploadFile);
        ComboBox_ContentFile.SelectedIndex = 0;
        foreach (var item in Directory.GetFiles(Globals.OutputDir))
        {
            if (Path.GetExtension(item) == ".vpk")
                ComboBox_ContentFile.Items.Add(Path.GetFileNameWithoutExtension(item));
        }

        if (IsPublish)
        {
            Title = "发布L4D2创意工坊";
            Button_PublishMod.Content = "发布Mod";

            RadioButton_IsFriendsOnly.IsChecked = true;
        }
        else
        {
            Title = "更新选中Mod信息";
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
    private void ReportProgress(float progress)
    {
        this.ProgressBar_Publish.Value = progress;
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

        // 预览图二进制文件
        var imgData = await File.ReadAllBytesAsync(imgPath);
        // 上传预览图文件到Steam云存储
        if (!SteamRemoteStorage.FileWrite(imgName, imgData))
        {
            MsgBoxUtil.Error("上传预览图文件到Steam云存储失败");
            return;
        }

        // VPK二进制文件
        var vpkData = await File.ReadAllBytesAsync(vpkPath);
        // 上传VPK文件到Steam云存储
        if (!SteamRemoteStorage.FileWrite(vpkName, vpkData))
        {
            MsgBoxUtil.Error("上传VPK文件到Steam云存储失败");
            return;
        }

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
        // 标签格式转换
        using var a = SteamParamStringArray.From(tags.ToArray());
        var val = a.Value;

        // 从Steam云存储发布
        var fileId = await SteamRemoteStorage.PublishWorkshopFile(vpkName, imgName, title, description, visibility, val);
        if (fileId != 0)
            MsgBoxUtil.Information($"发布L4D2创意工坊成功，物品Id：{fileId}");
        else
            MsgBoxUtil.Error("发布L4D2创意工坊失败");
    }

    /// <summary>
    /// 更新选中Mod信息
    /// </summary>
    private async Task UpdateMod()
    {
        if (string.IsNullOrWhiteSpace(TextBlock_Id.Text))
        {
            MsgBoxUtil.Error("创意工坊物品Id不能为空，操作取消");
            return;
        }

        var editor = new Editor(new PublishedFileId
        {
            Value = ulong.Parse(TextBlock_Id.Text)
        });

        ///////////////////////////////////////////////////

        if (string.IsNullOrWhiteSpace(TextBox_Title.Text))
        {
            MsgBoxUtil.Error("Mod标题不能为空，操作取消");
            return;
        }

        // 标题
        editor.WithTitle(TextBox_Title.Text.Trim());
        // 描述
        editor.WithDescription(TextBox_Description.Text.Trim());
        // 更新日志
        editor.WithChangeLog(TextBox_ChangeLog.Text.Trim());

        // 可见性
        if (RadioButton_IsPublic.IsChecked == true)
            editor.WithPublicVisibility();
        else if (RadioButton_IsFriendsOnly.IsChecked == true)
            editor.WithFriendsOnlyVisibility();
        else if (RadioButton_IsPrivate.IsChecked == true)
            editor.WithPrivateVisibility();
        else if (RadioButton_IsUnlisted.IsChecked == true)
            editor.WithUnlistedVisibility();

        if (ComboBox_ContentFile.SelectedItem is string name)
        {
            if (name != NotUploadFile)
            {
                // VPK目录
                //editor.WithContent(Globals.FullPublishDir);

                // 预览图
                editor.WithPreviewFile(TextBox_PreviewImage.Text.Trim());
            }
        }

        // 标签
        var tag = TextBlock_Tags.Text.Replace("Survivors,", "").Trim();
        if (!string.IsNullOrWhiteSpace(tag))
        {
            editor.WithTag("Survivors");
            editor.WithTag(tag);
        }

        // 提交更新请求
        var result = await editor.SubmitAsync(Progress);
        if (result.Success)
        {
            MsgBoxUtil.Information($"更新选中Mod信息成功 {result.Result}");
        }
        else
        {
            MsgBoxUtil.Error($"更新选中Mod信息失败 {result.Result}");
        }
    }
}
