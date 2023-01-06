using L4D2ModTools.Core;
using L4D2ModTools.Utils;
using L4D2ModTools.Helper;

namespace L4D2ModTools.Views;

/// <summary>
/// AddonView.xaml 的交互逻辑
/// </summary>
public partial class AddonView : UserControl
{
    private string addonImage = string.Empty;

    private string addonTitle = string.Empty;
    private string addonAuthor = string.Empty;
    private string addonURL0 = string.Empty;
    private string addonDescription = string.Empty;

    public AddonView()
    {
        InitializeComponent();
        this.DataContext = this;
        MainWindow.WindowClosingEvent += MainWindow_WindowClosingEvent;

        // 读取对应配置文件
        TextBox_addonImage.Text = IniHelper.ReadValue("Addon", "addonImage");

        TextBox_addonTitle.Text = IniHelper.ReadValue("Addon", "addonTitle");
        TextBox_addonAuthor.Text = IniHelper.ReadValue("Addon", "addonAuthor");
        TextBox_addonURL0.Text = IniHelper.ReadValue("Addon", "addonURL0");
        TextBox_addonDescription.Text = IniHelper.ReadValue("Addon", "addonDescription");
    }

    /// <summary>
    /// 主窗口关闭事件
    /// </summary>
    private void MainWindow_WindowClosingEvent()
    {
        SaveConfig();
    }

    /// <summary>
    /// 保存配置文件
    /// </summary>
    private void SaveConfig()
    {
        IniHelper.WriteValue("Addon", "addonImage", TextBox_addonImage.Text);

        IniHelper.WriteValue("Addon", "addonTitle", TextBox_addonTitle.Text);
        IniHelper.WriteValue("Addon", "addonAuthor", TextBox_addonAuthor.Text);
        IniHelper.WriteValue("Addon", "addonURL0", TextBox_addonURL0.Text);
        IniHelper.WriteValue("Addon", "addonDescription", TextBox_addonDescription.Text);
    }

    /// <summary>
    /// VPK打包按钮点击事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Button_VPKPackage_Click(object sender, RoutedEventArgs e)
    {
        Button_VPKPackage.IsEnabled = false;

        addonImage = TextBox_addonImage.Text.Trim();

        addonTitle = TextBox_addonTitle.Text.Trim();
        addonAuthor = TextBox_addonAuthor.Text.Trim();
        addonURL0 = TextBox_addonURL0.Text.Trim();
        addonDescription = TextBox_addonDescription.Text.Trim();

        var dirs = Directory.GetDirectories(Globals.OutputDir);
        if (dirs.Length == 0)
        {
            MsgBoxUtil.Warning("输出文件夹未发现需打包文件夹，操作取消");
            Button_VPKPackage.IsEnabled = true;
            return;
        }

        foreach (var dir in dirs)
        {
            BuildVPK(dir);
        }

        MsgBoxUtil.Information("VPK打包操作成功，请前往输出文件夹查看");

        Button_VPKPackage.IsEnabled = true;
    }

    /// <summary>
    /// 构建对应打包文件
    /// </summary>
    /// <param name="dirPath"></param>
    private async void BuildVPK(string dirPath)
    {
        if (dirPath.Contains($"{Survivor.Bill}"))
        {
            await BuildAddonInfo(dirPath, Survivor.Bill);
            return;
        }

        if (dirPath.Contains($"{Survivor.Francis}"))
        {
            await BuildAddonInfo(dirPath, Survivor.Francis);
            return;
        }

        if (dirPath.Contains($"{Survivor.Louis}"))
        {
            await BuildAddonInfo(dirPath, Survivor.Louis);
            return;
        }

        if (dirPath.Contains($"{Survivor.Zoey}"))
        {
            await BuildAddonInfo(dirPath, Survivor.Zoey);
            return;
        }

        //////////////////////////////////////

        if (dirPath.Contains($"{Survivor.Coach}"))
        {
            await BuildAddonInfo(dirPath, Survivor.Coach);
            return;
        }

        if (dirPath.Contains($"{Survivor.Ellis}"))
        {
            await BuildAddonInfo(dirPath, Survivor.Ellis);
            return;
        }

        if (dirPath.Contains($"{Survivor.Nick}"))
        {
            await BuildAddonInfo(dirPath, Survivor.Nick);
            return;
        }

        if (dirPath.Contains($"{Survivor.Rochelle}"))
        {
            await BuildAddonInfo(dirPath, Survivor.Rochelle);
            return;
        }
    }

    /// <summary>
    /// 构建AddonInfo文本具体内容
    /// </summary>
    /// <param name="dirPath"></param>
    /// <param name="survivor"></param>
    private Task BuildAddonInfo(string dirPath, Survivor survivor)
    {
        return Task.Run(() =>
        {
            var builder = new StringBuilder();
            builder.AppendLine("\"AddonInfo\"");
            builder.AppendLine("{");
            builder.AppendLine($"\taddonTitle \"{addonTitle}\"");
            builder.AppendLine($"\taddonAuthor \"{addonAuthor}\"");
            builder.AppendLine($"\taddonURL0 \"{addonURL0}\"");
            builder.AppendLine($"\taddonDescription \"{addonDescription}\"");
            builder.AppendLine("\taddonContent_Campaign 0");
            builder.AppendLine("\taddonContent_Map 0");
            builder.AppendLine("\taddonContent_Survivor 1");
            builder.AppendLine("\taddonContent_Skin 0");
            builder.AppendLine("\taddonContent_BossInfected 0");
            builder.AppendLine("\taddonContent_CommonInfected 0");
            builder.AppendLine("\taddonContent_Music 0");
            builder.AppendLine("\taddonContent_Sound 0");
            builder.AppendLine("\taddonContent_Prop 0");
            builder.AppendLine("\taddonContent_Weapon 0");
            builder.AppendLine("\taddonContent_Script 0");
            builder.AppendLine("}");

            // 写入addoninfo文件
            FileUtil.WriteFileUTF8NoBOM(dirPath + "\\addoninfo.txt", builder.Replace("@@", $"{survivor}").ToString());

            // 如果图片存在，则复制预览图
            FileUtil.SafeCopy(addonImage, dirPath + "\\addonimage.jpg");

            // 执行VPK打包命令
            Compile.RunL4D2DevExec(Globals.VPKExec, dirPath);
        });
    }

    /// <summary>
    /// 使用默认值按钮点击事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Button_UseDefault_Click(object sender, RoutedEventArgs e)
    {
        TextBox_addonImage.Text = Directory.GetCurrentDirectory() + "\\AppData\\Addons\\addonimage.jpg";

        TextBox_addonTitle.Text = "[@@] DOAXVV Fiona - Endorphin Heart";
        TextBox_addonAuthor.Text = "CrazyZhang";
        TextBox_addonURL0.Text = "https://steamcommunity.com/profiles/76561198293570981/";
        TextBox_addonDescription.Text = "DOAXVV Fiona - Endorphin Heart, Replace Survivor for @@";
    }

    /// <summary>
    /// 自动获取信息按钮点击事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Button_AutoGetInfo_Click(object sender, RoutedEventArgs e)
    {
        if (File.Exists(Globals.UnPackAddonImagePath))
            TextBox_addonImage.Text = Globals.UnPackAddonImagePath;

        var title = VPK.GetAddonTitle(Globals.UnPackAddonInfoPath);
        if (!string.IsNullOrWhiteSpace(title))
        {
            title = VPK.ReplaceSurvivorName(title);
            TextBox_addonTitle.Text = $"[@@] {title}";
            TextBox_addonDescription.Text = $"{title}, Replace Survivor for @@";
        }
        else
        {
            TextBox_addonTitle.Text = "自动获取失败";
            TextBox_addonDescription.Text = "自动获取失败";
        }

        var name = Workshop.GetUserName();
        if (!string.IsNullOrWhiteSpace(name))
            TextBox_addonAuthor.Text = name;
        else
            TextBox_addonAuthor.Text = "自动获取失败";

        var steamId = Workshop.GetUserSteamId();
        if (steamId != 0)
            TextBox_addonURL0.Text = $"https://steamcommunity.com/profiles/{steamId}/";
        else
            TextBox_addonURL0.Text = "自动获取失败";
    }

    /// <summary>
    /// 输出文件夹按钮点击事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Button_OutputDir_Click(object sender, RoutedEventArgs e)
    {
        ProcessUtil.OpenLink(Globals.OutputDir);
    }

    /// <summary>
    /// Addons文件夹按钮点击事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Button_AddonsDir_Click(object sender, RoutedEventArgs e)
    {
        ProcessUtil.OpenLink(Globals.L4D2AddonsDir);
    }

    /// <summary>
    /// 启动L4D2游戏按钮点击事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Button_RunL4D2_Click(object sender, RoutedEventArgs e)
    {
        ProcessUtil.OpenLink("steam://rungameid/550");
    }

    /// <summary>
    /// 发送打包文件到Addons按钮点击事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Button_SendAddons_Click(object sender, RoutedEventArgs e)
    {
        var files = Directory.GetFiles(Globals.OutputDir);
        if (files.Length == 0)
        {
            MsgBoxUtil.Warning("输出文件夹未发现VPK文件，操作取消");
            return;
        }

        foreach (var file in files)
        {
            if (Path.GetExtension(file) == ".vpk")
            {
                File.Copy(file, Path.Combine(Globals.L4D2AddonsDir, Path.GetFileName(file)), true);
            }
        }

        MsgBoxUtil.Information("发送VPK文件到Addons成功");
    }

    /// <summary>
    /// 图片拖拽事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Image_addonimage_DragEnter(object sender, DragEventArgs e)
    {
        if (e.Data.GetDataPresent(DataFormats.FileDrop))
            e.Effects = DragDropEffects.Copy;
    }

    /// <summary>
    /// 图片拖拽事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Image_addonimage_Drop(object sender, DragEventArgs e)
    {
        if (e.Data.GetDataPresent(DataFormats.FileDrop))
            TextBox_addonImage.Text = (e.Data.GetData(DataFormats.FileDrop) as Array).GetValue(0).ToString();
    }
}