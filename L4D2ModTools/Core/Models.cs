using L4D2ModTools.Utils;

namespace L4D2ModTools.Core;

public static class Models
{
    /// <summary>
    /// 清空 .\\AppData\\Survivors 文件夹缓存
    /// </summary>
    public static void ClearCache()
    {
        FileUtil.DeleteFileByExtension(Globals.AppSurvivorsDir, ".smd");
        FileUtil.DeleteFileByExtension(Globals.AppSurvivorsDir, ".vrd");
        FileUtil.DeleteFileByExtension(Globals.AppSurvivorsDir, ".vta");

        FileUtil.WriteFileUTF8NoBOM(Globals.AppSurvivorsQc, string.Empty);
    }

    /// <summary>
    /// 根据qc文件内 $includemodel 判断具体生还者
    /// </summary>
    /// <param name="includemodelStr">例如：$includemodel "survivors/anim_teenangst.mdl"</param>
    /// <returns>生还者枚举</returns>
    private static Survivor GetSurvivorByQc(string includemodelStr)
    {
        // 一代生还者
        if (includemodelStr.Contains("namvet", StringComparison.OrdinalIgnoreCase))
            return Survivor.Bill;
        if (includemodelStr.Contains("biker", StringComparison.OrdinalIgnoreCase))
            return Survivor.Francis;
        if (includemodelStr.Contains("manager", StringComparison.OrdinalIgnoreCase))
            return Survivor.Louis;
        if (includemodelStr.Contains("teenangst", StringComparison.OrdinalIgnoreCase))
            return Survivor.Zoey;
        // 二代生还者
        if (includemodelStr.Contains("coach", StringComparison.OrdinalIgnoreCase))
            return Survivor.Coach;
        if (includemodelStr.Contains("mechanic", StringComparison.OrdinalIgnoreCase))
            return Survivor.Ellis;
        if (includemodelStr.Contains("gambler", StringComparison.OrdinalIgnoreCase))
            return Survivor.Nick;
        if (includemodelStr.Contains("producer", StringComparison.OrdinalIgnoreCase))
            return Survivor.Rochelle;
        // 其他情况
        return Survivor.Null;
    }

    /// <summary>
    /// 根据qc文件内判断生还者模型动作
    /// </summary>
    /// <returns>生还者枚举</returns>
    public static Survivor GetModelAnims()
    {
        if (!Directory.Exists(Globals.UnPackSurvivorsDecoDir))
            return Survivor.Null;

        var files = FileUtil.GetDirectoryAllFiles(Globals.UnPackSurvivorsDecoDir);
        foreach (var file in files)
        {
            if (Path.GetExtension(file) == ".qc")
            {
                // 从qc文件中判断使用模型动作
                foreach (var item in File.ReadAllLines(file))
                {
                    if (item.StartsWith("$includemodel"))
                        return GetSurvivorByQc(item);
                }
            }
        }

        return Survivor.Null;
    }

    /// <summary>
    /// 判断幸存者是否有Light模型
    /// </summary>
    /// <param name="survivor"></param>
    /// <returns></returns>
    public static bool IsHaveLight(Survivor survivor)
    {
        return survivor switch
        {
            Survivor.Francis or Survivor.Zoey => true,
            _ => false,
        };
    }

    /// <summary>
    /// 处理角色模型
    /// </summary>
    /// <returns></returns>
    public static bool Make()
    {
        if (!Directory.Exists(Globals.UnPackSurvivorsDecoDir))
            return false;

        ClearCache();

        var files = FileUtil.GetDirectoryAllFiles(Globals.UnPackSurvivorsDecoDir);
        foreach (var file in files)
        {
            if (Path.GetExtension(file) != ".qc")
                File.Copy(file, Path.Combine(Globals.AppSurvivorsDir, Path.GetFileName(file)), true);
        }

        return true;
    }

    /// <summary>
    /// 处理角色模型Qc文件
    /// </summary>
    /// <param name="survivor"></param>
    /// <returns></returns>
    public static bool MakeQc(Survivor survivor)
    {
        if (!Directory.Exists(Globals.UnPackSurvivorsDecoDir))
            return false;

        var oriMaterName = Materials.GetMaterialsName();
        if (string.IsNullOrEmpty(oriMaterName))
            return false;

        var newMaterName = $"{oriMaterName}_{survivor}";

        var files = FileUtil.GetDirectoryAllFiles(Globals.UnPackSurvivorsDecoDir);
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

                    if (item.StartsWith("$animation"))
                        break;

                    builder.AppendLine(item);
                }

                FileUtil.WriteFileUTF8NoBOM(Globals.AppSurvivorsQc, builder.ToString().Replace(oriMaterName, newMaterName));
                break;
            }
        }

        return true;
    }
}
