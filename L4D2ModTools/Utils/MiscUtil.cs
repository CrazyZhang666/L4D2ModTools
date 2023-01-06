namespace L4D2ModTools.Utils;

public static class MiscUtil
{
    private const double MB = 1024.0 * 1024.0;

    /// <summary>
    /// 程序客户端版本号，如：1.2.3.4
    /// </summary>
    public static Version VersionInfo = Application.ResourceAssembly.GetName().Version;

    /// <summary>
    /// 程序客户端最后编译时间
    /// </summary>
    public static DateTime BuildTime = File.GetLastWriteTime(Environment.ProcessPath);

    /// <summary>
    /// 用户名是否合法
    /// </summary>
    /// <param name="userName"></param>
    /// <returns></returns>
    public static bool IsUserMatch(string userName)
    {
        return Regex.IsMatch(userName, "^[a-zA-Z][a-zA-Z0-9]{3,20}$");
    }

    /// <summary>
    /// 首字母大写
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static string UpperCaseFirstChar(string text)
    {
        return Regex.Replace(text, "^[a-z]", m => m.Value.ToUpper());
    }

    /// <summary>
    /// 字节转MB单位，保留3位小数
    /// </summary>
    /// <param name="size"></param>
    /// <returns></returns>
    public static string ByteConverterMB(int size)
    {
        return $"{size / MB:0.000} MB";
    }

    /// <summary>
    /// 从资源文件中抽取资源文件
    /// </summary>
    /// <param name="resFileName"></param>
    /// <param name="outputFile"></param>
    public static void ExtractResFile(string resFileName, string outputFile)
    {
        BufferedStream inStream = null;
        FileStream outStream = null;
        try
        {
            var asm = Assembly.GetExecutingAssembly();
            inStream = new BufferedStream(asm.GetManifestResourceStream(resFileName));
            outStream = new FileStream(outputFile, FileMode.Create, FileAccess.Write);

            var buffer = new byte[1024];
            int length;

            while ((length = inStream.Read(buffer, 0, buffer.Length)) > 0)
                outStream.Write(buffer, 0, length);

            outStream.Flush();
        }
        finally
        {
            outStream?.Close();
            inStream?.Close();
        }
    }
}
