namespace L4D2ModTools.Utils;

public static class ProcessUtil
{
    /// <summary>
    /// 判断程序是否运行
    /// </summary>
    /// <param name="appName">程序名称</param>
    /// <returns>正在运行返回true，未运行返回false</returns>
    public static bool IsAppRun(string appName)
    {
        return Process.GetProcessesByName(appName).Length > 0;
    }

    /// <summary>
    /// 垃圾回收
    /// </summary>
    public static void ClearMemory()
    {
        GC.Collect();
        GC.WaitForPendingFinalizers();
    }

    /// <summary>
    /// 打开指定文件夹路径或Web链接
    /// </summary>
    /// <param name="path"></param>
    public static void OpenLink(string path)
    {
        try
        {
            Process.Start(new ProcessStartInfo(path) { UseShellExecute = true });
        }
        catch (Exception ex)
        {
            MsgBoxUtil.Exception(ex);
        }
    }

    /// <summary>
    /// 打开指定程序并附带参数
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="args"></param>
    public static void OpenExecWithArgs(string filePath, string args = "")
    {
        try
        {
            Process.Start(filePath, args);
        }
        catch (Exception ex)
        {
            MsgBoxUtil.Exception(ex);
        }
    }

    /// <summary>
    /// 根据进程名字关闭指定程序
    /// </summary>
    /// <param name="processName">程序名字，不需要加.exe</param>
    public static void CloseProcess(string processName)
    {
        var appProcess = Process.GetProcessesByName(processName);
        foreach (var targetPro in appProcess)
            targetPro.Kill();
    }
}