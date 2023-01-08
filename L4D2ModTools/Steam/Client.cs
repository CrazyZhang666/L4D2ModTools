using L4D2ModTools.Utils;

namespace L4D2ModTools.Steam;

public static class Client
{
    /// <summary>
    /// 判断Steam客户端是否运行
    /// </summary>
    /// <returns></returns>
    public static bool IsRun()
    {
        return ProcessUtil.IsAppRun("steam");
    }

    /// <summary>
    /// 判断求生之路2是否运行
    /// </summary>
    /// <returns></returns>
    public static bool IsL4D2Run()
    {
        return ProcessUtil.IsAppRun("left4dead2");
    }

    /// <summary>
    /// 获取Steam位置
    /// </summary>
    /// <returns></returns>
    public static string GetMainPath()
    {
        if (IsRun())
        {
            var steam = Process.GetProcessesByName("steam")[0];
            return steam.MainModule.FileName;
        }

        return string.Empty;
    }

    /// <summary>
    /// 运行Steam客户端
    /// </summary>
    public static void Run()
    {
        if (!IsRun())
            ProcessUtil.OpenLink("steam://rungameid/#");
        else
            MsgBoxUtil.Warning("Steam客户端已经在运行了，请不要重复启动");
    }

    /// <summary>
    /// 运行求生之路2游戏
    /// </summary>
    public static void RunL4D2Game()
    {
        if (!IsL4D2Run())
            ProcessUtil.OpenLink("steam://rungameid/550");
        else
            MsgBoxUtil.Warning("求生之路2游戏已经在运行了，请不要重复启动");
    }
}
