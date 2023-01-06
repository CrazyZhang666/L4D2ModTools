using L4D2ModTools.Utils;

namespace L4D2ModTools.Core;

public static class Arms
{
    /// <summary>
    /// 清空 .\\AppData\\Weapons 文件夹缓存
    /// </summary>
    public static void ClearCache()
    {
        FileUtil.DeleteFileByExtension(Globals.AppWeaponsDir, ".smd");

        FileUtil.WriteFileUTF8NoBOM(Globals.AppWeaponsQc, string.Empty);
    }

    /// <summary>
    /// 处理角色手臂
    /// </summary>
    /// <returns></returns>
    public static bool Make()
    {
        if (!Directory.Exists(Globals.UnPackWeaponsDecoDir))
            return false;

        ClearCache();

        var files = FileUtil.GetDirectoryAllFiles(Globals.UnPackWeaponsDecoDir);
        foreach (var file in files)
        {
            if (Path.GetExtension(file) != ".qc")
                File.Copy(file, Path.Combine(Globals.AppWeaponsDir, Path.GetFileName(file)), true);
        }

        return true;
    }

    /// <summary>
    /// 处理角色手臂Qc文件
    /// </summary>
    /// <param name="survivor"></param>
    /// <returns></returns>
    public static bool MakeQc(Survivor survivor)
    {
        if (!Directory.Exists(Globals.UnPackWeaponsDecoDir))
            return false;

        var oriMaterName = Materials.GetMaterialsName();
        if (string.IsNullOrEmpty(oriMaterName))
            return false;

        var newMaterName = $"{oriMaterName}_{survivor}";

        var files = FileUtil.GetDirectoryAllFiles(Globals.UnPackWeaponsDecoDir);
        foreach (var file in files)
        {
            if (Path.GetExtension(file) == ".qc")
            {
                // 贴图qc文件处理
                var builder = new StringBuilder();
                foreach (var item in File.ReadAllLines(file))
                {
                    if (item.StartsWith("$modelname"))
                        continue;

                    if (item.StartsWith("$sequence"))
                        break;

                    builder.AppendLine(item);
                }

                FileUtil.WriteFileUTF8NoBOM(Globals.AppWeaponsQc, builder.ToString().Replace(oriMaterName, newMaterName));
                break;
            }
        }

        return true;
    }
}