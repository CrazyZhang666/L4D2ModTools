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
}
