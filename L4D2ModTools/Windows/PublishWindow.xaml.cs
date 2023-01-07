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

    /// <summary>
    /// 清空发布文件夹文件
    /// </summary>
    private void ClearPublish()
    {
        foreach (var item in Directory.GetFiles(Globals.PublishDir))
        {
            File.Delete(item);
        }
    }

    private void ComboBox_ContentFile_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (ComboBox_ContentFile.SelectedItem is string fileName)
        {
            ClearPublish();

            if (fileName == NotUploadFile)
                return;

            var vpk = $"{Globals.OutputDir}\\{fileName}.vpk";
            var jpg = $"{Globals.FullOutputDir}\\{fileName}.jpg";
            var json = $"{Globals.OutputDir}\\{fileName}.json";

            if (File.Exists(jpg))
                TextBox_PreviewImage.Text = jpg;

            if (File.Exists(json))
            {
                var addonInfo = JsonHelper.ReadFile<AddonInfo>(json);
                TextBox_Title.Text = addonInfo.AddonTitle;
                TextBlock_Tags.Text = $"Survivors, {addonInfo.Survivor}";
                TextBox_Description.Text = addonInfo.Description;
            }

            if (File.Exists(vpk))
                File.Copy(vpk, $"{Globals.PublishDir}\\{fileName}.vpk");
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

        if (string.IsNullOrWhiteSpace(TextBox_Title.Text))
        {
            MsgBoxUtil.Warning("Mod标题不能为空，操作取消");
            return;
        }

        if (string.IsNullOrWhiteSpace(TextBox_PreviewImage.Text))
        {
            MsgBoxUtil.Warning("Mod预览图文件不能为空，操作取消");
            return;
        }

        if (!Directory.Exists(Globals.PublishDir))
        {
            MsgBoxUtil.Warning("Mod标题上传VPK目录不存在，操作取消");
            return;
        }

        if (Directory.GetFiles(Globals.PublishDir).Length == 0)
        {
            MsgBoxUtil.Warning("Mod标题上传VPK目录文件为空，操作取消");
            return;
        }

        var editor = Editor.NewCommunityFile.WithTag("Survivors");

        // 标题
        editor.WithTitle(TextBox_Title.Text.Trim());
        // 描述
        editor.WithDescription(TextBox_Description.Text.Trim());
        // 更新日志
        editor.WithChangeLog(TextBox_ChangeLog.Text.Trim());

        // 预览图
        editor.WithPreviewFile(TextBox_PreviewImage.Text.Trim());

        // 标签
        var tag = TextBlock_Tags.Text.Replace("Survivors,", "").Trim();
        if (!string.IsNullOrWhiteSpace(tag))
            editor.WithTag(tag);

        // 可见性
        if (RadioButton_IsPublic.IsChecked == true)
            editor.WithPublicVisibility();
        else if (RadioButton_IsFriendsOnly.IsChecked == true)
            editor.WithFriendsOnlyVisibility();
        else if (RadioButton_IsPrivate.IsChecked == true)
            editor.WithPrivateVisibility();
        else if (RadioButton_IsUnlisted.IsChecked == true)
            editor.WithUnlistedVisibility();

        // VPK目录
        //editor.WithContent(Globals.FullPublishDir);

        // 提交更新请求
        var result = await editor.SubmitAsync(Progress);
        if (result.Success)
        {
            MsgBoxUtil.Information($"发布L4D2创意工坊成功 {result.Result}");
        }
        else
        {
            MsgBoxUtil.Error($"发布L4D2创意工坊失败 {result.Result}");
        }
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
