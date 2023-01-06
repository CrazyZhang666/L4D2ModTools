namespace L4D2ModTools.Core;

public static class Globals
{
    public const string OutputDir = ".\\__输出目录";

    public const string AppDataDir = ".\\AppData";
    public const string ToolKitsDir = $"{AppDataDir}\\ToolKits";

    public const string AppSurvivorsDir = $"{AppDataDir}\\Survivors";
    public const string AppWeaponsDir = $"{AppDataDir}\\Weapons";

    public const string AppSurvivorsQc = $"{AppSurvivorsDir}\\__Main.qci";
    public const string AppWeaponsQc = $"{AppWeaponsDir}\\__Main.qci";

    /////////////////////////////////////////////////////////////

    public static bool IsConfigOk { get; set; } = false;
    public static bool IsReadyOk { get; set; } = false;

    /////////////////////////////////////////////////////////////

    public static string UnPackDir { get; set; } = string.Empty;

    public static string UnPackAddonImagePath
    {
        get { return $"{UnPackDir}\\addonimage.jpg"; }
    }

    public static string UnPackAddonInfoPath
    {
        get { return $"{UnPackDir}\\addoninfo.txt"; }
    }

    public static string UnPackMaterialsDir
    {
        get { return $"{UnPackDir}\\materials"; }
    }

    public static string UnPackVGUIDir
    {
        get { return $"{UnPackMaterialsDir}\\vgui"; }
    }

    public static string UnPackSurvivorsDecoDir
    {
        get { return $"{UnPackDir}\\models\\survivors\\decompiled 0.72"; }
    }

    public static string UnPackWeaponsDecoDir
    {
        get { return $"{UnPackDir}\\models\\weapons\\arms\\decompiled 0.72"; }
    }

    /////////////////////////////////////////////////////////////

    public static string L4D2MainDir { get; set; } = string.Empty;

    public static string StudiomdlExec
    {
        get { return $"{L4D2MainDir}\\bin\\studiomdl.exe"; }
    }

    public static string VPKExec
    {
        get { return $"{L4D2MainDir}\\bin\\vpk.exe"; }
    }

    public static string L4D2SurvivorsDir
    {
        get { return $"{L4D2MainDir}\\left4dead2\\models\\survivors"; }
    }

    public static string L4D2WeaponsDir
    {
        get { return $"{L4D2MainDir}\\left4dead2\\models\\weapons\\arms"; }
    }

    public static string L4D2AddonsDir
    {
        get { return $"{L4D2MainDir}\\left4dead2\\addons"; }
    }

    public static string L4D2ComplieArgs
    {
        get { return $"-game \"{L4D2MainDir}\\left4dead2\" -nop4"; }
    }
}