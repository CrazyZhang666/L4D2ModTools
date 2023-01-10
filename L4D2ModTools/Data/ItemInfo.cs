namespace L4D2ModTools.Data;

public class ItemInfo
{
    public int Index { get; set; }
    public ulong Id { get; set; }
    public string PreviewImage { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string ChangeLog { get; set; }
    public string ContentFile { get; set; }
    public string Url { get; set; }
    public string FileSize { get; set; }
    public bool IsPublic { get; set; }
    public bool IsFriendsOnly { get; set; }
    public bool IsPrivate { get; set; }
    public bool IsUnlisted { get; set; }
    public string PublicState { get; set; }
    public string Updated { get; set; }
    public string Created { get; set; }
    public string[] Tags { get; set; }
    public string TagsContent { get; set; }
    public string Owner { get; set; }
    public ulong NumUniqueWebsiteViews { get; set; }
    public ulong NumSubscriptions { get; set; }
    public ulong NumFavorites { get; set; }
}
