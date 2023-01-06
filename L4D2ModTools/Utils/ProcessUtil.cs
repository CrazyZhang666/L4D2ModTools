using L4D2ModTools.Core;

namespace L4D2ModTools.Utils;

public static class ProcessUtil
{
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
    public static void OpenExecWithArgs(string filePath, string args)
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
    /// 使用系统记事本打开指定文件
    /// </summary>
    /// <param name="args"></param>
    public static void OpenFileByNotepad(string args)
    {
        OpenExecWithArgs("notepad.exe", args);
    }

    /// <summary>
    /// 使用Notepad2打开指定文件
    /// </summary>
    /// <param name="args"></param>
    public static void OpenFileByNotepad2(string args)
    {
        OpenExecWithArgs(Globals.ToolKitsDir + "\\Notepad2.exe", args);
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

    /// <summary>
    /// 关闭全部第三方exe进程
    /// </summary>
    public static void CloseThirdProcess()
    {
        CloseProcess("Notepad2");
    }
}