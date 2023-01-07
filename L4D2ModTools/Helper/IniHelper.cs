using L4D2ModTools.Core;

namespace L4D2ModTools.Helper;

public static class IniHelper
{
    /// <summary>
    /// 默认配置文件路径
    /// </summary>
    private const string IniPath = $"{Globals.ConfigDir}.\\config.ini";

    [DllImport("kernel32", CharSet = CharSet.Unicode)]
    private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

    [DllImport("kernel32", CharSet = CharSet.Unicode)]
    private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

    /// <summary>
    /// 创建INI配置文件
    /// </summary>
    public static void Create()
    {
        if (!File.Exists(IniPath))
            File.Create(IniPath).Close();
    }

    /// <summary>
    /// 读取节点值
    /// </summary>
    /// <param name="section"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static string ReadValue(string section, string key)
    {
        var temp = new StringBuilder(1024);
        _ = GetPrivateProfileString(section, key, string.Empty, temp, temp.Capacity, IniPath);
        return temp.ToString();
    }

    /// <summary>
    /// 写入节点值
    /// </summary>
    /// <param name="section"></param>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public static void WriteValue(string section, string key, string value)
    {
        WritePrivateProfileString(section, key, value, IniPath);
    }
}