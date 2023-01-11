using L4D2ModTools.Core;
using L4D2ModTools.Utils;
using L4D2ModTools.Helper;

namespace L4D2ModTools.Views;

/// <summary>
/// FontView.xaml 的交互逻辑
/// </summary>
public partial class FontView : UserControl
{
    private const string fontINI = $"{Globals.MacTypeDir}\\L4D2.ini";

    public FontView()
    {
        InitializeComponent();
        this.DataContext = this;
        MainWindow.WindowClosingEvent += MainWindow_WindowClosingEvent;

        // 读取对应配置文件
        TextBox_CustomFontName.Text = IniHelper.ReadValue("Font", "CustomFontName");
        TextBox_CustomRunArgs.Text = IniHelper.ReadValue("Font", "CustomRunArgs");

        if (string.IsNullOrWhiteSpace(TextBox_CustomRunArgs.Text.Trim()))
            TextBox_CustomRunArgs.Text = "-steam -novid -language schinese";

        Task.Run(() =>
        {
            var fonts = new List<string>();
            // 获取系统所有字体
            foreach (var family in Fonts.SystemFontFamilies)
            {
                foreach (var item in family.FamilyNames)
                {
                    fonts.Add(item.Value);
                }
            }

            fonts.Sort();
            fonts.ForEach(font =>
            {
                this.Dispatcher.BeginInvoke(DispatcherPriority.Background, () =>
                {
                    ListBox_Fonts.Items.Add(font);
                });
            });
        });
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
        IniHelper.WriteValue("Font", "CustomFontName", TextBox_CustomFontName.Text.Trim());
        IniHelper.WriteValue("Font", "CustomRunArgs", TextBox_CustomRunArgs.Text.Trim());
    }

    private void WriteCustonFontName(string fontName)
    {
        IniHelper.WriteValue("FontSubstitutes@L4D2", "SimSun", fontName, fontINI);
        IniHelper.WriteValue("FontSubstitutes@L4D2", "NSimSun", fontName, fontINI);
        IniHelper.WriteValue("FontSubstitutes@L4D2", "Tahoma", fontName, fontINI);
    }

    private void Button_RunL4D2ByMacLoader_Click(object sender, RoutedEventArgs e)
    {
        var fontName = TextBox_CustomFontName.Text.Trim();
        if (!string.IsNullOrWhiteSpace(fontName))
        {
            WriteCustonFontName(fontName);
        }

        ProcessUtil.OpenExecWithArgs(Globals.FontLoaderExec, $"\"{Globals.L4D2MainExec}\" {TextBox_CustomRunArgs.Text.Trim()}");
    }
}
