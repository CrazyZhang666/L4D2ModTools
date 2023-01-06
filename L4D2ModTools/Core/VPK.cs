namespace L4D2ModTools.Core;

public static class VPK
{
    /// <summary>
    /// 获取 addoninfo.txt 标题内容
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public static string GetAddonTitle(string filePath)
    {
        if (File.Exists(filePath))
        {
            foreach (var item in File.ReadAllLines(filePath))
            {
                if (item.Trim().ToLower().StartsWith("addontitle"))
                {
                    return item.Replace("addontitle", "", StringComparison.OrdinalIgnoreCase).Replace("\"", "").Trim();
                }
            }
        }

        return string.Empty;
    }

    /// <summary>
    /// 移除字符串中生还者名称
    /// </summary>
    /// <param name="originStr"></param>
    /// <returns></returns>
    public static string ReplaceSurvivorName(string originStr)
    {
        foreach (var item in Enum.GetNames(typeof(Survivor)))
        {
            if (item == "Null")
                continue;

            originStr = originStr.Replace(item, "", StringComparison.OrdinalIgnoreCase);
        }

        return originStr;
    }
}
