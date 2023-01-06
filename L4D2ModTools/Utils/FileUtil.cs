namespace L4D2ModTools.Utils;

public static class FileUtil
{
    /// <summary>
    /// UTF-8 无BOM格式
    /// </summary>
    public static UTF8Encoding UTF8NoBOM = new UTF8Encoding(false);

    private static List<string> AllFileLists = new List<string>();

    private static object ObjLock = new object();

    /// <summary>
    /// 写入UTF8无BOM格式文本文件
    /// </summary>
    /// <param name="savePath"></param>
    /// <param name="content"></param>
    public static void WriteFileUTF8NoBOM(string savePath, string content)
    {
        try
        {
            File.WriteAllText(savePath, content, UTF8NoBOM);
        }
        catch (Exception ex)
        {
            MsgBoxUtil.Exception(ex);
        }
    }

    /// <summary>
    /// 写入UTF8无BOM格式文本文件
    /// </summary>
    /// <param name="savePath"></param>
    /// <param name="content"></param>
    public static void WriteFileUTF8NoBOM(string savePath, string[] content)
    {
        try
        {
            File.WriteAllLines(savePath, content, UTF8NoBOM);
        }
        catch (Exception ex)
        {
            MsgBoxUtil.Exception(ex);
        }
    }

    /// <summary>
    /// 判断目录是否为空文件夹
    /// </summary>
    /// <param name="dirPath"></param>
    /// <returns></returns>
    public static bool IsDirectoryEmpty(string dirPath)
    {
        return Directory.GetDirectories(dirPath).Length == 0 && Directory.GetFiles(dirPath).Length == 0;
    }

    /// <summary>
    /// 清空指定目录
    /// </summary>
    /// <param name="dirPath"></param>
    public static void ClearDirectory(string dirPath)
    {
        try
        {
            if (Directory.Exists(dirPath))
            {
                var dir = new DirectoryInfo(dirPath);
                dir.Delete(true);
            }

            Directory.CreateDirectory(dirPath);
        }
        catch (Exception ex)
        {
            MsgBoxUtil.Exception(ex);
        }
    }

    /// <summary>
    /// 递归复制文件夹
    /// </summary>
    /// <param name="sourcePath"></param>
    /// <param name="destPath"></param>
    public static void CopyDirectory(string sourcePath, string destPath)
    {
        var floderName = Path.GetFileName(sourcePath);
        var dir = Directory.CreateDirectory(Path.Combine(destPath, floderName));
        var files = Directory.GetFileSystemEntries(sourcePath);

        foreach (var file in files)
        {
            if (Directory.Exists(file))
                CopyDirectory(file, dir.FullName);
            else
                File.Copy(file, Path.Combine(dir.FullName, Path.GetFileName(file)), true);
        }
    }

    /// <summary>
    /// 删除指定扩展名文件，如：.smd
    /// </summary>
    /// <param name="dirPath"></param>
    /// <param name="exetension"></param>
    public static void DeleteFileByExtension(string dirPath, string exetension)
    {
        try
        {
            if (Directory.Exists(dirPath))
            {
                var files = Directory.GetFileSystemEntries(dirPath);
                foreach (var file in files)
                {
                    if (Path.GetExtension(file) == exetension)
                        File.Delete(file);
                }
            }
        }
        catch (Exception ex)
        {
            MsgBoxUtil.Exception(ex);
        }
    }

    /// <summary>
    /// 复制文件到指定文件夹
    /// </summary>
    /// <param name="dirPath"></param>
    /// <param name="targetPath"></param>
    public static void CopyFilesToTargetDir(string dirPath, string targetPath)
    {
        try
        {
            if (Directory.Exists(dirPath))
            {
                var files = Directory.GetFileSystemEntries(dirPath);
                foreach (var file in files)
                {
                    File.Copy(file, Path.Combine(targetPath, Path.GetFileName(file)), true);
                }
            }
        }
        catch (Exception ex)
        {
            MsgBoxUtil.Exception(ex);
        }
    }

    /// <summary>
    /// 获取目录下全部文件
    /// </summary>
    /// <param name="dirPath"></param>
    /// <returns></returns>
    public static List<string> GetDirectoryAllFiles(string dirPath)
    {
        lock (ObjLock)
        {
            AllFileLists.Clear();
            GetDirectoryFileList(dirPath);
            return AllFileLists;
        }
    }

    /// <summary>
    /// 获取一个文件夹下的所有文件集合
    /// </summary>
    /// <param name="path"></param>
    private static void GetDirectoryFileList(string path)
    {
        var dir = new DirectoryInfo(path);
        var fsInfos = dir.GetFileSystemInfos();
        foreach (var info in fsInfos)
        {
            if (info is DirectoryInfo)
                GetDirectoryFileList(info.FullName);
            else
                AllFileLists.Add(info.FullName);
        }
    }

    /// <summary>
    /// 判断目标是文件夹还是目录
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public static bool IsDirectory(string filePath)
    {
        var fInfo = new FileInfo(filePath);
        return fInfo.Attributes == FileAttributes.Directory;
    }

    /// <summary>
    /// 安全复制文件
    /// </summary>
    /// <param name="sourceFileName"></param>
    /// <param name="destFileName"></param>
    public static void SafeCopy(string sourceFileName, string destFileName)
    {
        if (File.Exists(sourceFileName))
            File.Copy(sourceFileName, destFileName, true);
    }
}
