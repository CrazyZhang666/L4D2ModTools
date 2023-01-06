using L4D2ModTools.Data;
using L4D2ModTools.Utils;

using Steamworks;
using Steamworks.Ugc;

namespace L4D2ModTools.Core;

public static class Workshop
{
    /// <summary>
    /// 求生之路2 AppID
    /// </summary>
    private const int AppID = 550;
    /// <summary>
    /// 是否初始化成功
    /// </summary>
    private static bool IsInitSuccess = false;

    /// <summary>
    /// 锁标志
    /// </summary>
    private static readonly object ObjLock = new();

    /// <summary>
    /// 初始化
    /// </summary>
    /// <returns></returns>
    public static bool Init()
    {
        lock (ObjLock)
        {
            try
            {
                if (!IsInitSuccess)
                    SteamClient.Init(AppID);

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
    public static string GetTags(string[] tags)
    {
        for (int i = 0; i < tags.Length; i++)
            tags[i] = MiscUtil.UpperCaseFirstChar(tags[i]);

        return string.Join(", ", tags);
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
                        PreviewImageUrl = item.PreviewImageUrl,
                        Title = item.Title.Replace("\n", ""),
                        FileSize = MiscUtil.ByteConverterMB(item.FileSize),
                        PublicState = GetPublicState(item.IsPublic, item.IsFriendsOnly, item.IsPrivate, item.IsUnlisted),
                        Updated = item.Updated.ToString(),
                        Created = item.Created.ToString(),
                        Tags = GetTags(item.Tags),
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