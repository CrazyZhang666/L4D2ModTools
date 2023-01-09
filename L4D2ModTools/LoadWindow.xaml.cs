using L4D2ModTools.Core;
using L4D2ModTools.Utils;
using L4D2ModTools.Helper;

namespace L4D2ModTools;

/// <summary>
/// LoadWindow.xaml 的交互逻辑
/// </summary>
public partial class LoadWindow : Window
{
    public LoadWindow()
    {
        InitializeComponent();
    }

    private void Window_Load_Loaded(object sender, RoutedEventArgs e)
    {
        this.DataContext = this;

        Task.Run(async () =>
        {
            try
            {
                // 创建文件夹
                Directory.CreateDirectory(Globals.OutputDir);
                Directory.CreateDirectory(Globals.ConfigDir);

                // 创建INI配置文件
                IniHelper.Create();

                // 释放数据文件
                MiscUtil.ExtractResFile("L4D2ModTools.Files.AppData.zip", ".\\AppData.bin");
                MiscUtil.ExtractResFile("L4D2ModTools.Files.steam_api64.dll", ".\\steam_api64.dll");

                // 解压数据文件
                if (File.Exists(".\\AppData.bin"))
                {
                    if (!Directory.Exists(Globals.AppDataDir))
                    {
                        using var archive = ZipFile.OpenRead(".\\AppData.bin");
                        archive.ExtractToDirectory(Globals.AppDataDir);
                    }
                }
                else
                {
                    MsgBoxUtil.Error("未发现AppData.bin，请更新工具版本");
                    Application.Current.Shutdown();
                }

                /////////////////////////////////////////////////////////////////////

                await Task.Delay(500);

                this.Dispatcher.Invoke(() =>
                {
                    var mainWindow = new MainWindow();
                    // 转移主程序控制权
                    Application.Current.MainWindow = mainWindow;
                    // 显示主窗口
                    mainWindow.Show();

                    // 关闭初始化窗口
                    this.Close();
                });
            }
            catch (Exception ex)
            {
                MsgBoxUtil.Exception(ex);
                Application.Current.Shutdown();
            }
        });
    }
}
