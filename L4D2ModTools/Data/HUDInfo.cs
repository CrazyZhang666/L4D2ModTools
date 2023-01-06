namespace L4D2ModTools.Data;

public class HUDInfo
{
    /// <summary>
    /// 游戏内HUD图标，s_panel_????.vtf
    /// </summary>
    public string HUD1 { get; set; }
    /// <summary>
    /// 游戏内倒地HUD图标，s_panel_????_incap.vtf
    /// </summary>
    public string HUD2 { get; set; }
    /// <summary>
    /// 大厅头像，一代是 select_????.vtf，二代是 s_panel_lobby_????.vtf
    /// </summary>
    public string HUD3 { get; set; }
}
