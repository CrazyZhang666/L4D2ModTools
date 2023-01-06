using L4D2ModTools.Data;

namespace L4D2ModTools.Windows;

/// <summary>
/// UpdateWindow.xaml 的交互逻辑
/// </summary>
public partial class UpdateWindow : Window
{
    public ItemInfo ItemInfo { get; }

    public UpdateWindow(ItemInfo itemInfo)
    {
        InitializeComponent();
        this.DataContext = this;

        ItemInfo = itemInfo;
    }

    private void Window_Update_Loaded(object sender, RoutedEventArgs e)
    {

    }

    private void Window_Update_Closing(object sender, CancelEventArgs e)
    {

    }
}
