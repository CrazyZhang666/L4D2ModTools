using L4D2ModTools.Core;
using L4D2ModTools.Data;
using L4D2ModTools.Utils;

using Steamworks;
using Steamworks.Ugc;

namespace L4D2ModTools.Steam;

public static class Workshop
{
    /// <summary>
    /// 是否初始化成功
    /// </summary>
    private static bool IsInitSuccess = false;

    /// <summary>
    /// 锁标志
    /// </summary>
    private static readonly object ObjLock = new();

    /// <summary>
    /// 初始化Steamworks
    /// </summary>
    /// <returns></returns>
    public static bool Init()
    {
        lock (ObjLock)
        {
            if (Client.IsRun())
            {
                try
                {
                    if (!IsInitSuccess)
                        SteamClient.Init(Globals.AppID);

                    IsInitSuccess = true;
                    return true;
                }
                catch (Exception ex)
                {
                    MsgBoxUtil.Exception($"Steamworks初始化失败，请重启程序并检查Steam状态\n\n异常信息 : \n{ex.Message}", "初始化失败");

                    IsInitSuccess = false;
                    return false;
                }
            }
            else
            {
                MsgBoxUtil.Warning("未发现Steam进程，请先启动Steam客户端");

                IsInitSuccess = false;
                return false;
            }
        }
    }

    /// <summary>
    /// 结束Steamworks
    /// </summary>
    public static void ShutDown()
    {
        if (IsInitSuccess)
            SteamClient.Shutdown();
    }

    /// <summary>
    /// 获取Mod可见性
    /// </summary>
    /// <param name="isPublic"></param>
    /// <param name="isFriendsOnly"></param>
    /// <param name="isPrivate"></param>
    /// <param name="isUnlisted"></param>
    /// <returns></returns>
    public static string GetPublicState(bool isPublic, bool isFriendsOnly, bool isPrivate, bool isUnlisted)
    {
        // 当前可见性： 公开
        if (isPublic)
            return "公开";
        // 当前可见性： 仅限好友该物品仅对您、您的好友和管理员可见。
        if (isFriendsOnly)
            return "仅限好友";
        // 当前可见性： 隐藏该物品仅对您、管理员和被标记为创作者的用户可见。
        if (isPrivate)
            return "隐藏";
        // 当前可见性： 非公开此物品对所有人可见，但不会在搜索中或您的个人资料里显示。
        if (isUnlisted)
            return "非公开";

        return "其他";
    }

    /// <summary>
    /// 获取Mod标志字符串
    /// </summary>
    /// <param name="tags"></param>
    /// <returns></returns>
    public static string GetTagsContent(string[] tags)
    {
        for (int i = 0; i < tags.Length; i++)
            tags[i] = MiscUtil.UpperCaseFirstChar(tags[i]);

        return string.Join(", ", tags);
    }

    /////////////////////////////////////////////////////////////////////////////////

    /// <summary>
    /// 获取玩家Steam昵称
    /// </summary>
    /// <returns></returns>
    public static string GetUserName()
    {
        try
        {
            if (Init())
            {
                return SteamClient.Name;
            }
        }
        catch (Exception ex)
        {
            MsgBoxUtil.Exception(ex);
        }

        return string.Empty;
    }

    /// <summary>
    /// 获取玩家Steam数字Id
    /// </summary>
    /// <returns></returns>
    public static ulong GetUserSteamId()
    {
        try
        {
            if (Init())
            {
                return SteamClient.SteamId.Value;
            }
        }
        catch (Exception ex)
        {
            MsgBoxUtil.Exception(ex);
        }

        return 0;
    }

    /// <summary>
    /// 获取求生之路2安装目录
    /// </summary>
    /// <returns></returns>
    public static string GetL4D2InstallDir()
    {
        try
        {
            if (Init())
            {
                return SteamApps.AppInstallDir(Globals.AppID);
            }
        }
        catch (Exception ex)
        {
            MsgBoxUtil.Exception(ex);
        }

        return string.Empty;
    }

    /// <summary>
    /// 获取求生之路2玩家创意工坊物品列表
    /// </summary>
    public static async Task<List<ItemInfo>> GetUserPublished()
    {
        var itemInfos = new List<ItemInfo>();

        try
        {
            if (Init())
            {
                var result = await Query.All.WhereUserPublished().GetPageAsync(1);

                int index = 1;
                foreach (var item in result.Value.Entries)
                {
                    itemInfos.Add(new()
                    {
                        Index = index++,
                        Id = item.Id.Value,
                        PreviewImage = item.PreviewImageUrl,
                        Title = item.Title.Replace("\n", ""),
                        Description = item.Description,
                        Url = item.Url,
                        FileSize = MiscUtil.ByteConverterMB(item.FileSize),
                        IsPublic = item.IsPublic,
                        IsFriendsOnly = item.IsFriendsOnly,
                        IsPrivate = item.IsPrivate,
                        IsUnlisted = item.IsUnlisted,
                        PublicState = GetPublicState(item.IsPublic, item.IsFriendsOnly, item.IsPrivate, item.IsUnlisted),
                        Updated = item.Updated.ToString(),
                        Created = item.Created.ToString(),
                        Tags = item.Tags,
                        TagsContent = GetTagsContent(item.Tags),
                        Owner = item.Owner.Name,
                    });
                }
            }
        }
        catch (Exception ex)
        {
            MsgBoxUtil.Exception(ex);
        }

        return itemInfos;
    }
}