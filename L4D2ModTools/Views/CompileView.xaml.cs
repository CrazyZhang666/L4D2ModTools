using L4D2ModTools.Core;
using L4D2ModTools.Utils;
using L4D2ModTools.Helper;

namespace L4D2ModTools.Views;

/// <summary>
/// CompileView.xaml 的交互逻辑
/// </summary>
public partial class CompileView : UserControl
{
    public CompileView()
    {
        InitializeComponent();
        this.DataContext = this;
        MainWindow.WindowClosingEvent += MainWindow_WindowClosingEvent;

        // 读取对应配置文件
        TextBox_ModelName.Text = IniHelper.ReadValue("Compile", "ModelName");

        if (IniHelper.ReadValue("Compile", "AutoAnims") == "true")
            CheckBox_AutoAnims.IsChecked = true;

        var modelAnims = IniHelper.ReadValue("Compile", "ModelAnims");
        SelectModelAnims(modelAnims);

        if (IniHelper.ReadValue("Compile", "L4D1_Bill") == "true")
            CheckBox_L4D1_Bill.IsChecked = true;
        if (IniHelper.ReadValue("Compile", "L4D1_Francis") == "true")
            CheckBox_L4D1_Francis.IsChecked = true;
        if (IniHelper.ReadValue("Compile", "L4D1_Louis") == "true")
            CheckBox_L4D1_Louis.IsChecked = true;
        if (IniHelper.ReadValue("Compile", "L4D1_Zoey") == "true")
            CheckBox_L4D1_Zoey.IsChecked = true;

        if (IniHelper.ReadValue("Compile", "L4D2_Coach") == "true")
            CheckBox_L4D2_Coach.IsChecked = true;
        if (IniHelper.ReadValue("Compile", "L4D2_Ellis") == "true")
            CheckBox_L4D2_Ellis.IsChecked = true;
        if (IniHelper.ReadValue("Compile", "L4D2_Nick") == "true")
            CheckBox_L4D2_Nick.IsChecked = true;
        if (IniHelper.ReadValue("Compile", "L4D2_Rochelle") == "true")
            CheckBox_L4D2_Rochelle.IsChecked = true;
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
        IniHelper.WriteValue("Compile", "ModelName", TextBox_ModelName.Text);

        IniHelper.WriteValue("Compile", "ModelAnims", string.Empty);

        if (CheckBox_AutoAnims.IsChecked == true)
            IniHelper.WriteValue("Compile", "AutoAnims", "true");
        else
            IniHelper.WriteValue("Compile", "AutoAnims", "false");

        if (RadioButton_L4D1_Bill.IsChecked == true)
            IniHelper.WriteValue("Compile", "ModelAnims", "Bill");
        if (RadioButton_L4D1_Francis.IsChecked == true)
            IniHelper.WriteValue("Compile", "ModelAnims", "Francis");
        if (RadioButton_L4D1_Louis.IsChecked == true)
            IniHelper.WriteValue("Compile", "ModelAnims", "Louis");
        if (RadioButton_L4D1_Zoey.IsChecked == true)
            IniHelper.WriteValue("Compile", "ModelAnims", "Zoey");

        if (RadioButton_L4D2_Coach.IsChecked == true)
            IniHelper.WriteValue("Compile", "ModelAnims", "Coach");
        if (RadioButton_L4D2_Ellis.IsChecked == true)
            IniHelper.WriteValue("Compile", "ModelAnims", "Ellis");
        if (RadioButton_L4D2_Nick.IsChecked == true)
            IniHelper.WriteValue("Compile", "ModelAnims", "Nick");
        if (RadioButton_L4D2_Rochelle.IsChecked == true)
            IniHelper.WriteValue("Compile", "ModelAnims", "Rochelle");

        //////////////////////////////////////////////////////////////////

        IniHelper.WriteValue("Compile", "L4D1_Bill", "false");
        IniHelper.WriteValue("Compile", "L4D1_Francis", "false");
        IniHelper.WriteValue("Compile", "L4D1_Louis", "false");
        IniHelper.WriteValue("Compile", "L4D1_Zoey", "false");

        IniHelper.WriteValue("Compile", "L4D2_Coach", "false");
        IniHelper.WriteValue("Compile", "L4D2_Ellis", "false");
        IniHelper.WriteValue("Compile", "L4D2_Nick", "false");
        IniHelper.WriteValue("Compile", "L4D2_Rochelle", "false");

        if (CheckBox_L4D1_Bill.IsChecked == true)
            IniHelper.WriteValue("Compile", "L4D1_Bill", "true");
        if (CheckBox_L4D1_Francis.IsChecked == true)
            IniHelper.WriteValue("Compile", "L4D1_Francis", "true");
        if (CheckBox_L4D1_Louis.IsChecked == true)
            IniHelper.WriteValue("Compile", "L4D1_Louis", "true");
        if (CheckBox_L4D1_Zoey.IsChecked == true)
            IniHelper.WriteValue("Compile", "L4D1_Zoey", "true");

        if (CheckBox_L4D2_Coach.IsChecked == true)
            IniHelper.WriteValue("Compile", "L4D2_Coach", "true");
        if (CheckBox_L4D2_Ellis.IsChecked == true)
            IniHelper.WriteValue("Compile", "L4D2_Ellis", "true");
        if (CheckBox_L4D2_Nick.IsChecked == true)
            IniHelper.WriteValue("Compile", "L4D2_Nick", "true");
        if (CheckBox_L4D2_Rochelle.IsChecked == true)
            IniHelper.WriteValue("Compile", "L4D2_Rochelle", "true");
    }

    /// <summary>
    /// 清空日志
    /// </summary>
    private void ClearLogger()
    {
        this.Dispatcher.Invoke(() =>
        {
            TextBox_Logger.Clear();
        });
    }

    /// <summary>
    /// 增加日志信息
    /// </summary>
    /// <param name="log"></param>
    private void AddLogger(string log)
    {
        this.Dispatcher.BeginInvoke(DispatcherPriority.Background, () =>
        {
            TextBox_Logger.AppendText($"[{DateTime.Now:HH:mm:ss.fff}] {log}\n");
            TextBox_Logger.ScrollToEnd();
        });
    }

    /// <summary>
    /// 点击重编译按钮事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void Button_ReComplieModel_Click(object sender, RoutedEventArgs e)
    {
        if (!Globals.IsConfigOk || !Globals.IsReadyOk)
        {
            MsgBoxUtil.Warning("配置目录或准备工作环境检查未通过，操作取消");
            return;
        }

        ClearLogger();

        // 验证模型名称是否合法
        var modelName = TextBox_ModelName.Text.Trim();
        if (!MiscUtil.IsUserMatch(modelName))
        {
            AddLogger("❌ 重编译模型名称只能为英文和数字，不能以数字开头也不能有空格及其他特殊字符，字符范围3~20，操作取消");
            return;
        }
        // 打印合法的模型名称
        modelName = MiscUtil.UpperCaseFirstChar(modelName);
        TextBox_ModelName.Text = modelName;
        AddLogger($"重编译模型名称: {modelName}");

        // 原模型动作
        var modelAnims = Survivor.Null;
        if (CheckBox_AutoAnims.IsChecked == true)
        {
            modelAnims = Models.GetModelAnims();
            if (modelAnims != Survivor.Null)
                AddLogger($"原模型动作: 智能识别为 {modelAnims}");
            else
                AddLogger($"原模型动作: 智能识别出错，自动改为 {modelAnims}");

            SelectModelAnims($"{modelAnims}");
        }
        else
        {
            // 验证原模型动作是否正确选择
            if (RadioButton_L4D1_Bill.IsChecked == false &&
                RadioButton_L4D1_Francis.IsChecked == false &&
                RadioButton_L4D1_Louis.IsChecked == false &&
                RadioButton_L4D1_Zoey.IsChecked == false &&
                RadioButton_L4D2_Coach.IsChecked == false &&
                RadioButton_L4D2_Ellis.IsChecked == false &&
                RadioButton_L4D2_Nick.IsChecked == false &&
                RadioButton_L4D2_Rochelle.IsChecked == false)
            {
                AddLogger("❌ 请选择原模型动作，操作取消");
                return;
            }

            if (RadioButton_L4D1_Bill.IsChecked == true)
                modelAnims = Survivor.Bill;
            if (RadioButton_L4D1_Francis.IsChecked == true)
                modelAnims = Survivor.Francis;
            if (RadioButton_L4D1_Louis.IsChecked == true)
                modelAnims = Survivor.Louis;
            if (RadioButton_L4D1_Zoey.IsChecked == true)
                modelAnims = Survivor.Zoey;

            if (RadioButton_L4D2_Coach.IsChecked == true)
                modelAnims = Survivor.Coach;
            if (RadioButton_L4D2_Ellis.IsChecked == true)
                modelAnims = Survivor.Ellis;
            if (RadioButton_L4D2_Nick.IsChecked == true)
                modelAnims = Survivor.Nick;
            if (RadioButton_L4D2_Rochelle.IsChecked == true)
                modelAnims = Survivor.Rochelle;

            AddLogger($"原模型动作: {modelAnims}");
        }

        // 验证重编译目标模型是否正确选择
        if (CheckBox_L4D1_Bill.IsChecked == false &&
            CheckBox_L4D1_Francis.IsChecked == false &&
            CheckBox_L4D1_Louis.IsChecked == false &&
            CheckBox_L4D1_Zoey.IsChecked == false &&
            CheckBox_L4D2_Coach.IsChecked == false &&
            CheckBox_L4D2_Ellis.IsChecked == false &&
            CheckBox_L4D2_Nick.IsChecked == false &&
            CheckBox_L4D2_Rochelle.IsChecked == false)
        {
            AddLogger("❌ 请至少选择一个重编译目标模型，操作取消");
            return;
        }

        ////////////////////////////////////////////////////

        // 验证玩家准备工作是否完成
        if (string.IsNullOrEmpty(Globals.UnPackDir))
        {
            AddLogger("❌ 玩家未正确选择VPK解包目录，操作取消");
            return;
        }

        // 清空模型输出文件夹
        FileUtil.ClearDirectory(Globals.OutputDir);
        AddLogger($"✔ 清空 {Globals.OutputDir} 文件夹成功");

        // 复制角色模型反编译数据
        if (Models.Make())
            AddLogger($"✔ 复制角色模型反编译数据成功");
        else
            AddLogger($"❌ 复制角色模型反编译数据失败");

        // 复制角色手臂反编译数据
        if (Arms.Make())
            AddLogger($"✔ 复制角色手臂反编译数据成功");
        else
            AddLogger($"❌ 复制角色手臂反编译数据失败");

        ////////////////////////////////////////////////////

        Button_ReComplieModel.IsEnabled = false;
        ProgressBar_ReComplie.Value = 0;
        MainWindow.ActionTaskbarProgress(0);

        var stopwatch = new Stopwatch();
        stopwatch.Start();

        // 根据玩家选择重编译对应角色模型
        var survivorDic = new Dictionary<Survivor, string>();

        if (CheckBox_L4D1_Bill.IsChecked == true)
            survivorDic.Add(Survivor.Bill, $"\\{modelName}_L4D1_Bill");

        if (CheckBox_L4D1_Francis.IsChecked == true)
            survivorDic.Add(Survivor.Francis, $"\\{modelName}_L4D1_Francis");

        if (CheckBox_L4D1_Louis.IsChecked == true)
            survivorDic.Add(Survivor.Louis, $"\\{modelName}_L4D1_Louis");

        if (CheckBox_L4D1_Zoey.IsChecked == true)
            survivorDic.Add(Survivor.Zoey, $"\\{modelName}_L4D1_Zoey");

        if (CheckBox_L4D2_Coach.IsChecked == true)
            survivorDic.Add(Survivor.Coach, $"\\{modelName}_L4D2_Coach");

        if (CheckBox_L4D2_Ellis.IsChecked == true)
            survivorDic.Add(Survivor.Ellis, $"\\{modelName}_L4D2_Ellis");

        if (CheckBox_L4D2_Nick.IsChecked == true)
            survivorDic.Add(Survivor.Nick, $"\\{modelName}_L4D2_Nick");

        if (CheckBox_L4D2_Rochelle.IsChecked == true)
            survivorDic.Add(Survivor.Rochelle, $"\\{modelName}_L4D2_Rochelle");

        for (int i = 0; i < survivorDic.Count; i++)
        {
            var kv = survivorDic.ElementAt(i);
            await ReComplieModel(modelName, kv.Value, kv.Key, modelAnims);

            ProgressBar_ReComplie.Value = (i + 1) / (double)survivorDic.Count;
            MainWindow.ActionTaskbarProgress(ProgressBar_ReComplie.Value);
        }

        ////////////////////////////////////////////////////

        Compile.ClearCache();
        stopwatch.Stop();

        AddLogger($"全部编译完成，耗时: {stopwatch.Elapsed.TotalSeconds:0} 秒，请前往输出文件夹查看");
        MsgBoxUtil.Information($"全部编译完成，耗时: {stopwatch.Elapsed.TotalSeconds:0} 秒，请前往输出文件夹查看");

        Button_ReComplieModel.IsEnabled = true;
        ProgressBar_ReComplie.Value = 0;
        MainWindow.ActionTaskbarProgress(0);
    }

    /// <summary>
    /// 重编译角色模型
    /// </summary>
    /// <param name="outDirName"></param>
    /// <param name="survivor"></param>
    /// <param name="modelName"></param>
    /// <returns></returns>
    private Task ReComplieModel(string modelName, string outDirName, Survivor survivor, Survivor modelAnims)
    {
        return Task.Run(() =>
        {
            var outputDir = Globals.OutputDir + outDirName;

            // 创建输出文件夹
            Directory.CreateDirectory($"{outputDir}\\materials\\vgui");
            Directory.CreateDirectory($"{outputDir}\\models\\survivors");
            Directory.CreateDirectory($"{outputDir}\\models\\weapons\\arms");
            AddLogger($"✔ 创建输出文件夹成功 {outDirName} ");

            // 处理角色贴图
            if (Materials.Make(survivor, $"{outputDir}\\materials"))
                AddLogger($"✔ 处理角色贴图成功 {survivor} ");
            else
                AddLogger($"❌ 处理角色贴图失败 {survivor} ");

            // 处理角色头像
            if (VGUI.Make(modelName, survivor, $"{outputDir}\\materials\\vgui"))
                AddLogger($"✔ 处理角色头像成功 {survivor} ");
            else
                AddLogger($"❌ 处理角色头像失败 {survivor} ");

            // 处理角色模型Qc
            if (Models.MakeQc(survivor))
                AddLogger($"✔ 处理角色模型Qc成功 {survivor} ");
            else
                AddLogger($"❌ 处理角色模型Qc失败 {survivor} ");

            // 处理角色手臂Qc
            if (Arms.MakeQc(survivor))
                AddLogger($"✔ 处理角色手臂Qc成功 {survivor} ");
            else
                AddLogger($"❌ 处理角色手臂Qc失败 {survivor} ");

            ///////////////////////////////////////////////////////////

            Compile.ClearCache();
            AddLogger($"✔ {survivor} 清空 L4D2文件夹 .ani .vtx .mdl .vvd 编译缓存成功");

            AddLogger($"☛ 正在重编译模型 {survivor} 中...");

            // 编译角色模型
            Compile.StudiomdlExec(modelAnims, survivor, outputDir);
            AddLogger($"✔ 重编译角色模型成功 {outDirName}");

            // 编译角色Light模型
            if (Models.IsHaveLight(survivor))
            {
                Compile.StudiomdlExecLight(modelAnims, survivor, outputDir);
                AddLogger($"✔ 重编译角色Light模型模型成功 {outDirName}");
            }

            // 编译角色手臂模型
            Compile.StudiomdlExecArms(survivor, outputDir);
            AddLogger($"✔ 重编译角色手臂模型成功 {outDirName}");

            ///////////////////////////////////////////////////////////

            AddLogger($"✔ 重编译模型操作成功 {outDirName}");
        });
    }

    /// <summary>
    /// 更新UI模型动作按钮
    /// </summary>
    /// <param name="modelAnims"></param>
    private void SelectModelAnims(string modelAnims)
    {
        switch (modelAnims)
        {
            case "Bill":
                RadioButton_L4D1_Bill.IsChecked = true;
                break;

            case "Francis":
                RadioButton_L4D1_Francis.IsChecked = true;
                break;

            case "Louis":
                RadioButton_L4D1_Louis.IsChecked = true;
                break;

            case "Zoey":
                RadioButton_L4D1_Zoey.IsChecked = true;
                break;

            case "Coach":
                RadioButton_L4D2_Coach.IsChecked = true;
                break;

            case "Ellis":
                RadioButton_L4D2_Ellis.IsChecked = true;
                break;

            case "Nick":
                RadioButton_L4D2_Nick.IsChecked = true;
                break;

            case "Rochelle":
                RadioButton_L4D2_Rochelle.IsChecked = true;
                break;
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
    /// 重编译目标模型全选按钮点击事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Button_SelectAll_Click(object sender, RoutedEventArgs e)
    {
        CheckBox_L4D1_Bill.IsChecked = true;
        CheckBox_L4D1_Francis.IsChecked = true;
        CheckBox_L4D1_Louis.IsChecked = true;
        CheckBox_L4D1_Zoey.IsChecked = true;

        CheckBox_L4D2_Coach.IsChecked = true;
        CheckBox_L4D2_Ellis.IsChecked = true;
        CheckBox_L4D2_Nick.IsChecked = true;
        CheckBox_L4D2_Rochelle.IsChecked = true;
    }

    /// <summary>
    /// 重编译目标模型取消全选按钮点击事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Button_DeselectAll_Click(object sender, RoutedEventArgs e)
    {
        CheckBox_L4D1_Bill.IsChecked = false;
        CheckBox_L4D1_Francis.IsChecked = false;
        CheckBox_L4D1_Louis.IsChecked = false;
        CheckBox_L4D1_Zoey.IsChecked = false;

        CheckBox_L4D2_Coach.IsChecked = false;
        CheckBox_L4D2_Ellis.IsChecked = false;
        CheckBox_L4D2_Nick.IsChecked = false;
        CheckBox_L4D2_Rochelle.IsChecked = false;
    }
}