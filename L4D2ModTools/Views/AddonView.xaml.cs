using L4D2ModTools.Data;
using L4D2ModTools.Core;
using L4D2ModTools.Utils;
using L4D2ModTools.Steam;
using L4D2ModTools.Helper;

namespace L4D2ModTools.Views;

/// <summary>
/// AddonView.xaml 的交互逻辑
/// </summary>
public partial class AddonView : UserControl
{
    /// <summary>
    /// Addon配置文件
    /// </summary>
    private AddonInfo AddonInfo = new();
    /// <summary>
    /// Addon配置文件保存路径
    /// </summary>
    private const string savePath = $"{Globals.ConfigDir}\\addon.json";

    public AddonView()
    {
        InitializeComponent();
        this.DataContext = this;
        MainWindow.WindowClosingEvent += MainWindow_WindowClosingEvent;

        // 如果配置文件不存在就创建
        if (!File.Exists(savePath))
        {
            // 保存配置文件
            SaveConfig();
        }

        // 如果配置文件存在就读取
        if (File.Exists(savePath))
        {
            AddonInfo = JsonHelper.ReadFile<AddonInfo>(savePath);

            TextBox_addonImage.Text = AddonInfo.AddonImage;

            TextBox_addonTitle.Text = AddonInfo.AddonTitle;
            TextBox_addonAuthor.Text = AddonInfo.AddonAuthor;
            TextBox_addonURL0.Text = AddonInfo.AddonURL0;
            TextBox_addonDescription.Text = AddonInfo.AddonDescription;

            TextBox_Description.Text = AddonInfo.Description;
        }
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
        AddonInfo.AddonImage = TextBox_addonImage.Text;

        AddonInfo.AddonTitle = TextBox_addonTitle.Text;
        AddonInfo.AddonAuthor = TextBox_addonAuthor.Text;
        AddonInfo.AddonURL0 = TextBox_addonURL0.Text;
        AddonInfo.AddonDescription = TextBox_addonDescription.Text;

        AddonInfo.Description = TextBox_Description.Text;

        JsonHelper.WriteFile(savePath, AddonInfo);
    }

    /// <summary>
    /// VPK打包按钮点击事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Button_VPKPackage_Click(object sender, RoutedEventArgs e)
    {
        Button_VPKPackage.IsEnabled = false;

        AddonInfo.AddonImage = TextBox_addonImage.Text.Trim();

        AddonInfo.AddonTitle = TextBox_addonTitle.Text.Trim();
        AddonInfo.AddonAuthor = TextBox_addonAuthor.Text.Trim();
        AddonInfo.AddonURL0 = TextBox_addonURL0.Text.Trim();
        AddonInfo.AddonDescription = TextBox_addonDescription.Text.Trim();

        AddonInfo.Description = TextBox_Description.Text.Trim();

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
    /// <param name="outputDir"></param>
    private async void BuildVPK(string outputDir)
    {
        if (outputDir.Contains($"{Survivor.Bill}"))
        {
            await BuildAddonInfo(outputDir, Survivor.Bill);
            return;
        }

        if (outputDir.Contains($"{Survivor.Francis}"))
        {
            await BuildAddonInfo(outputDir, Survivor.Francis);
            return;
        }

        if (outputDir.Contains($"{Survivor.Louis}"))
        {
            await BuildAddonInfo(outputDir, Survivor.Louis);
            return;
        }

        if (outputDir.Contains($"{Survivor.Zoey}"))
        {
            await BuildAddonInfo(outputDir, Survivor.Zoey);
            return;
        }

        //////////////////////////////////////

        if (outputDir.Contains($"{Survivor.Coach}"))
        {
            await BuildAddonInfo(outputDir, Survivor.Coach);
            return;
        }

        if (outputDir.Contains($"{Survivor.Ellis}"))
        {
            await BuildAddonInfo(outputDir, Survivor.Ellis);
            return;
        }

        if (outputDir.Contains($"{Survivor.Nick}"))
        {
            await BuildAddonInfo(outputDir, Survivor.Nick);
            return;
        }

        if (outputDir.Contains($"{Survivor.Rochelle}"))
        {
            await BuildAddonInfo(outputDir, Survivor.Rochelle);
            return;
        }
    }

    /// <summary>
    /// 构建AddonInfo文本具体内容
    /// </summary>
    /// <param name="outputDir"></param>
    /// <param name="survivor"></param>
    private Task BuildAddonInfo(string outputDir, Survivor survivor)
    {
        return Task.Run(() =>
        {
            var info = new AddonInfo()
            {
                AddonImage = AddonInfo.AddonImage,
                AddonTitle = AddonInfo.AddonTitle.Replace("@@", $"{survivor}"),
                AddonAuthor = AddonInfo.AddonAuthor,
                AddonURL0 = AddonInfo.AddonURL0,
                AddonDescription = AddonInfo.AddonDescription.Replace("@@", $"{survivor}"),
                Survivor = survivor.ToString(),
                Description = AddonInfo.Description.Replace("@@", $"{survivor}")
            };

            var builder = new StringBuilder();
            builder.AppendLine("\"AddonInfo\"");
            builder.AppendLine("{");
            builder.AppendLine($"\taddonTitle                     \"{info.AddonTitle}\"");
            builder.AppendLine($"\taddonAuthor                    \"{info.AddonAuthor}\"");
            builder.AppendLine($"\taddonURL0                      \"{info.AddonURL0}\"");
            builder.AppendLine($"\taddonDescription               \"{info.AddonDescription}\"");
            builder.AppendLine("\taddonContent_Campaign          0");
            builder.AppendLine("\taddonContent_Map               0");
            builder.AppendLine("\taddonContent_Survivor          1");
            builder.AppendLine("\taddonContent_Skin              0");
            builder.AppendLine("\taddonContent_BossInfected      0");
            builder.AppendLine("\taddonContent_CommonInfected    0");
            builder.AppendLine("\taddonContent_Music             0");
            builder.AppendLine("\taddonContent_Sound             0");
            builder.AppendLine("\taddonContent_Prop              0");
            builder.AppendLine("\taddonContent_Weapon            0");
            builder.AppendLine("\taddonContent_Script            0");
            builder.Append("}");

            // 写入addoninfo文件
            FileUtil.WriteFileUTF8NoBOM($"{outputDir}\\addoninfo.txt", builder.ToString());

            // 如果图片存在，则复制预览图
            FileUtil.SafeCopy(info.AddonImage, $"{outputDir}\\addonimage.jpg");

            // 执行VPK打包命令
            Compile.RunL4D2DevExec(Globals.VPKExec, outputDir);

            // 上传工坊信息准备
            var path = $"{Globals.OutputDir}\\{outputDir.Replace(".\\__Output\\", "").ToLower()}";
            JsonHelper.WriteFile($"{path}.json", info);
            FileUtil.SafeCopy(info.AddonImage, $"{path}.jpg");
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

        TextBox_Description.Text = "DOAXVV Fiona - Endorphin Heart, Replace Survivor for @@";
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
        {
            TextBox_addonAuthor.Text = name;

            var steamId = Workshop.GetUserSteamId();
            if (steamId != 0)
                TextBox_addonURL0.Text = $"https://steamcommunity.com/profiles/{steamId}/";
            else
                TextBox_addonURL0.Text = "自动获取失败";
        }
        else
        {
            TextBox_addonAuthor.Text = "自动获取失败";
        }
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
        Client.RunL4D2Game();
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
}