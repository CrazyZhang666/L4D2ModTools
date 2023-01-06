using L4D2ModTools.Utils;

namespace L4D2ModTools.Core;

public static class Materials
{
    /// <summary>
    /// 获取角色模型贴图主文件夹名称
    /// </summary>
    /// <returns></returns>
    public static string GetMaterialsName()
    {
        if (!Directory.Exists(Globals.UnPackMaterialsDir))
            return string.Empty;

        var dirs = Directory.GetDirectories(Globals.UnPackMaterialsDir);
        if (dirs.Length != 2)
            return string.Empty;

        foreach (var item in dirs)
        {
            var dir = new DirectoryInfo(item);

            if (dir.Name == "vgui")
                continue;
            else
                return dir.Name;
        }

        return string.Empty;
    }

    /// <summary>
    /// 处理角色模型贴图文件
    /// </summary>
    /// <param name="survivor">幸存者名称</param>
    /// <param name="outMaterialsDir">输出Materials文件夹路径</param>
    /// <returns></returns>
    public static bool Make(Survivor survivor, string outMaterialsDir)
    {
        if (!Directory.Exists(outMaterialsDir))
            return false;

        var oriMaterName = GetMaterialsName();
        if (string.IsNullOrEmpty(oriMaterName))
            return false;

        var newMaterName = $"{oriMaterName}_{survivor}";

        // 复制原始贴图文件夹到已重命名输出文件夹
        FileUtil.CopyDirectory($"{Globals.UnPackMaterialsDir}\\{oriMaterName}", $"{outMaterialsDir}");
        Directory.Move($"{outMaterialsDir}\\{oriMaterName}", $"{outMaterialsDir}\\{newMaterName}");

        // 贴图路径修改
        var files = FileUtil.GetDirectoryAllFiles($"{outMaterialsDir}\\{newMaterName}");
        foreach (var file in files)
        {
            if (Path.GetExtension(file) == ".vmt")
            {
                var temp = File.ReadAllText(file);
                temp = temp.Replace(oriMaterName, newMaterName);
                FileUtil.WriteFileUTF8NoBOM(file, temp);
            }
        }

        return true;
    }
}