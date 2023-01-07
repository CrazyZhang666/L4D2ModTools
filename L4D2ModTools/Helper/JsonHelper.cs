namespace L4D2ModTools.Helper;

public static class JsonHelper
{
    private static readonly JsonSerializerOptions OptionsDese = new()
    {
        IncludeFields = true,
        NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    private static readonly JsonSerializerOptions OptionsSeri = new()
    {
        WriteIndented = true,
        IncludeFields = true,
        NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
    };

    /// <summary>
    /// 反序列化，将json字符串转换成json类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="result"></param>
    /// <returns></returns>
    public static T JsonDese<T>(string result)
    {
        return JsonSerializer.Deserialize<T>(result, OptionsDese);
    }

    /// <summary>
    /// 序列化，将json类转换成json字符串
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="jsonClass"></param>
    /// <returns></returns>
    public static string JsonSeri<T>(T jsonClass)
    {
        return JsonSerializer.Serialize(jsonClass, OptionsSeri);
    }

    /// <summary>
    /// 利用序列化和反序列号深拷贝
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <returns></returns>
    public static T DeepClone<T>(T source)
    {
        return JsonDese<T>(JsonSeri(source));
    }
}