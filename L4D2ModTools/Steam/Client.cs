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
        if (!ProcessUtil.IsAppRun("steam"))
        {
            MsgBoxUtil.Warning("未发现Steam进程，请先启动Steam客户端");
            return false;
        }

        return true;
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
        if (!IsRun())
            ProcessUtil.OpenLink("steam://rungameid/550");
        else
            MsgBoxUtil.Warning("求生之路2游戏已经在运行了，请不要重复启动");
    }
}
