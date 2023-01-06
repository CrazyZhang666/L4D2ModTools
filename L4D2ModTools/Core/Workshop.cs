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
    /// 获取Mod访问状态
    /// </summary>
    /// <param name="isPublic"></param>
    /// <param name="isFriendsOnly"></param>
    /// <param name="isPrivate"></param>
    /// <returns></returns>
    public static string GetPublicState(bool isPublic, bool isFriendsOnly, bool isPrivate)
    {
        if (isPublic)
            return "公开";
        if (isFriendsOnly)
            return "仅好友可见";
        if (isPrivate)
            return "私有";

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
    /// 结束SteamAPI
    /// </summary>
    public static void ShutDown()
    {
        SteamClient.Shutdown();
    }

    /// <summary>
    /// 获取求生之路2玩家创意工坊物品列表
    /// </summary>
    public static async Task<List<ItemInfo>> GetUserPublished()
    {
        var itemInfos = new List<ItemInfo>();

        try
        {
            SteamClient.Init(AppID);

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
                    PublicState = GetPublicState(item.IsPublic, item.IsFriendsOnly, item.IsPrivate),
                    Updated = item.Updated.ToString(),
                    Created = item.Created.ToString(),
                    Tags = GetTags(item.Tags),
                    Owner = item.Owner.Name,
                });
            }
        }
        catch (Exception ex)
        {
            MsgBoxUtil.Exception(ex);
        }
        finally
        {
            SteamClient.Shutdown();
        }

        return itemInfos;
    }
}
