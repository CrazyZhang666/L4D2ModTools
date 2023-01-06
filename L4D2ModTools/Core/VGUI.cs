using L4D2ModTools.Data;
using L4D2ModTools.Utils;

namespace L4D2ModTools.Core;

public static class VGUI
{
    /// <summary>
    /// 根据HUD文件名称判断具体生还者，一般为.vtf文件
    /// </summary>
    /// <param name="vtfFileName">.vtf文件名称</param>
    /// <returns>生还者枚举</returns>
    public static Survivor GetSurvivorByVtf(string vtfFileName)
    {
        return vtfFileName switch
        {
            // 一代生还者
            "s_panel_namvet" or "s_panel_namvet_incap" or "select_bill" => Survivor.Bill,
            "s_panel_biker" or "s_panel_biker_incap" or "select_francis" => Survivor.Francis,
            "s_panel_manager" or "s_panel_manager_incap" or "select_louis" => Survivor.Louis,
            "s_panel_teenangst" or "s_panel_teenangst_incap" or "select_zoey" => Survivor.Zoey,
            // 二代生还者
            "s_panel_coach" or "s_panel_coach_incap" or "s_panel_lobby_coach" => Survivor.Coach,
            "s_panel_mechanic" or "s_panel_mechanic_incap" or "s_panel_lobby_mechanic" => Survivor.Ellis,
            "s_panel_gambler" or "s_panel_gambler_incap" or "s_panel_lobby_gambler" => Survivor.Nick,
            "s_panel_producer" or "s_panel_producer_incap" or "s_panel_lobby_producer" => Survivor.Rochelle,
            // 其他情况
            _ => Survivor.Null,
        };
    }

    /// <summary>
    /// 获取生还者HUD信息
    /// </summary>
    /// <param name="survivor"></param>
    /// <returns></returns>
    public static HUDInfo GetHUDInfo(Survivor survivor)
    {
        var hud = new HUDInfo();

        switch (survivor)
        {
            // 一代生还者
            case Survivor.Bill:
                hud.HUD1 = "s_panel_namvet";
                hud.HUD2 = "s_panel_namvet_incap";
                hud.HUD3 = "select_bill";
                break;
            case Survivor.Francis:
                hud.HUD1 = "s_panel_biker";
                hud.HUD2 = "s_panel_biker_incap";
                hud.HUD3 = "select_francis";
                break;
            case Survivor.Louis:
                hud.HUD1 = "s_panel_manager";
                hud.HUD2 = "s_panel_manager_incap";
                hud.HUD3 = "select_louis";
                break;
            case Survivor.Zoey:
                hud.HUD1 = "s_panel_teenangst";
                hud.HUD2 = "s_panel_teenangst_incap";
                hud.HUD3 = "select_zoey";
                break;
            // 二代生还者
            case Survivor.Coach:
                hud.HUD1 = "s_panel_coach";
                hud.HUD2 = "s_panel_coach_incap";
                hud.HUD3 = "s_panel_lobby_coach";
                break;
            case Survivor.Ellis:
                hud.HUD1 = "s_panel_mechanic";
                hud.HUD2 = "s_panel_mechanic_incap";
                hud.HUD3 = "s_panel_lobby_mechanic";
                break;
            case Survivor.Nick:
                hud.HUD1 = "s_panel_gambler";
                hud.HUD2 = "s_panel_gambler_incap";
                hud.HUD3 = "s_panel_lobby_gambler";
                break;
            case Survivor.Rochelle:
                hud.HUD1 = "s_panel_producer";
                hud.HUD2 = "s_panel_producer_incap";
                hud.HUD3 = "s_panel_lobby_producer";
                break;
        }

        return hud;
    }

    /// <summary>
    /// 创建角色头像.vmt文本文件
    /// </summary>
    /// <param name="newHUDName"></param>
    /// <param name="savePath"></param>
    public static void CreateHUDVmt(string newHUDName, string savePath)
    {
        var builder = new StringBuilder();

        builder.AppendLine("\"UnlitGeneric\"");
        builder.AppendLine("{");
        builder.AppendLine($"\t\"$basetexture\" \"VGUI\\{newHUDName}\"");
        builder.AppendLine($"\t\"$translucent\" \"1\"");
        builder.AppendLine($"\t\"$vertexcolor\" \"1\"");
        builder.AppendLine($"\t\"$vertexalpha\" \"1\"");
        builder.AppendLine($"\t\"$no_fullbright\" \"1\"");
        builder.AppendLine($"\t\"$ignorez\" \"1\"");
        builder.AppendLine($"\t\"$additive\" \"0\"");
        builder.AppendLine("}");

        FileUtil.WriteFileUTF8NoBOM(savePath, builder.ToString());
    }

    /// <summary>
    /// 处理角色HUD贴图文件
    /// </summary>
    /// <param name="modelName">角色模型名称</param>
    /// <param name="survivor">当前生还者</param>
    /// <param name="outVguiDir">输出VGUI文件夹路径</param>
    /// <returns></returns>
    public static bool Make(string modelName, Survivor survivor, string outVguiDir)
    {
        if (!Directory.Exists(Globals.UnPackVGUIDir) || !Directory.Exists(outVguiDir))
            return false;

        var files = Directory.GetFiles(Globals.UnPackVGUIDir);
        if (files.Length == 0)
            return false;

        var tempSurvivor = GetSurvivorByVtf(Path.GetFileNameWithoutExtension(files[0]));
        if (tempSurvivor == Survivor.Null)
            return false;

        var hud = GetHUDInfo(tempSurvivor);
        foreach (var item in files)
        {
            if (Path.GetFileNameWithoutExtension(item) == hud.HUD1)
            {
                File.Copy(item, $"{outVguiDir}\\{modelName}_{survivor}.vtf");
                CreateHUDVmt($"{modelName}_{survivor}", $"{outVguiDir}\\{GetHUDInfo(survivor).HUD1}.vmt");
                continue;
            }

            if (Path.GetFileNameWithoutExtension(item) == hud.HUD2)
            {
                File.Copy(item, $"{outVguiDir}\\{modelName}_{survivor}_Incap.vtf");
                CreateHUDVmt($"{modelName}_{survivor}_Incap", $"{outVguiDir}\\{GetHUDInfo(survivor).HUD2}.vmt");
                continue;
            }

            if (Path.GetFileNameWithoutExtension(item) == hud.HUD3)
            {
                File.Copy(item, $"{outVguiDir}\\{modelName}_{survivor}_Lobby.vtf");
                CreateHUDVmt($"{modelName}_{survivor}_Lobby", $"{outVguiDir}\\{GetHUDInfo(survivor).HUD3}.vmt");
                continue;
            }
        }

        return true;
    }
}