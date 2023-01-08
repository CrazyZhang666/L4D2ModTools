using Steamworks.Data;

namespace Steamworks;

internal unsafe class ISteamRemoteStorage : SteamInterface
{
	internal ISteamRemoteStorage( bool IsGameServer )
	{
		SetupInterface( IsGameServer );
	}
	
	[DllImport( Platform.LibraryName, EntryPoint = "SteamAPI_SteamRemoteStorage_v016", CallingConvention = Platform.CC)]
	internal static extern IntPtr SteamAPI_SteamRemoteStorage_v016();
	public override IntPtr GetUserInterfacePointer() => SteamAPI_SteamRemoteStorage_v016();
	
	#region FunctionMeta
	[DllImport( Platform.LibraryName, EntryPoint = "SteamAPI_ISteamRemoteStorage_FileWrite", CallingConvention = Platform.CC)]
	[return: MarshalAs( UnmanagedType.I1 )]
	private static extern bool _FileWrite( IntPtr self, [MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( Utf8StringToNative ) )] string pchFile, IntPtr pvData, int cubData );
	#endregion
	
	internal bool FileWrite( [MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( Utf8StringToNative ) )] string pchFile, IntPtr pvData, int cubData )
	{
		var returnValue = _FileWrite( Self, pchFile, pvData, cubData );
		return returnValue;
	}
	
	#region FunctionMeta
	[DllImport( Platform.LibraryName, EntryPoint = "SteamAPI_ISteamRemoteStorage_FileRead", CallingConvention = Platform.CC)]
	private static extern int _FileRead( IntPtr self, [MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( Utf8StringToNative ) )] string pchFile, IntPtr pvData, int cubDataToRead );
	#endregion
	
	internal int FileRead( [MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( Utf8StringToNative ) )] string pchFile, IntPtr pvData, int cubDataToRead )
	{
		var returnValue = _FileRead( Self, pchFile, pvData, cubDataToRead );
		return returnValue;
	}
	
	#region FunctionMeta
	[DllImport( Platform.LibraryName, EntryPoint = "SteamAPI_ISteamRemoteStorage_FileWriteAsync", CallingConvention = Platform.CC)]
	private static extern SteamAPICall_t _FileWriteAsync( IntPtr self, [MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( Utf8StringToNative ) )] string pchFile, IntPtr pvData, uint cubData );
	#endregion
	
	internal CallResult<RemoteStorageFileWriteAsyncComplete_t> FileWriteAsync( [MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( Utf8StringToNative ) )] string pchFile, IntPtr pvData, uint cubData )
	{
		var returnValue = _FileWriteAsync( Self, pchFile, pvData, cubData );
		return new CallResult<RemoteStorageFileWriteAsyncComplete_t>( returnValue, IsServer );
	}
	
	#region FunctionMeta
	[DllImport( Platform.LibraryName, EntryPoint = "SteamAPI_ISteamRemoteStorage_FileReadAsync", CallingConvention = Platform.CC)]
	private static extern SteamAPICall_t _FileReadAsync( IntPtr self, [MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( Utf8StringToNative ) )] string pchFile, uint nOffset, uint cubToRead );
	#endregion
	
	internal CallResult<RemoteStorageFileReadAsyncComplete_t> FileReadAsync( [MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( Utf8StringToNative ) )] string pchFile, uint nOffset, uint cubToRead )
	{
		var returnValue = _FileReadAsync( Self, pchFile, nOffset, cubToRead );
		return new CallResult<RemoteStorageFileReadAsyncComplete_t>( returnValue, IsServer );
	}
	
	#region FunctionMeta
	[DllImport( Platform.LibraryName, EntryPoint = "SteamAPI_ISteamRemoteStorage_FileReadAsyncComplete", CallingConvention = Platform.CC)]
	[return: MarshalAs( UnmanagedType.I1 )]
	private static extern bool _FileReadAsyncComplete( IntPtr self, SteamAPICall_t hReadCall, IntPtr pvBuffer, uint cubToRead );
	#endregion
	
	internal bool FileReadAsyncComplete( SteamAPICall_t hReadCall, IntPtr pvBuffer, uint cubToRead )
	{
		var returnValue = _FileReadAsyncComplete( Self, hReadCall, pvBuffer, cubToRead );
		return returnValue;
	}
	
	#region FunctionMeta
	[DllImport( Platform.LibraryName, EntryPoint = "SteamAPI_ISteamRemoteStorage_FileForget", CallingConvention = Platform.CC)]
	[return: MarshalAs( UnmanagedType.I1 )]
	private static extern bool _FileForget( IntPtr self, [MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( Utf8StringToNative ) )] string pchFile );
	#endregion
	
	internal bool FileForget( [MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( Utf8StringToNative ) )] string pchFile )
	{
		var returnValue = _FileForget( Self, pchFile );
		return returnValue;
	}
	
	#region FunctionMeta
	[DllImport( Platform.LibraryName, EntryPoint = "SteamAPI_ISteamRemoteStorage_FileDelete", CallingConvention = Platform.CC)]
	[return: MarshalAs( UnmanagedType.I1 )]
	private static extern bool _FileDelete( IntPtr self, [MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( Utf8StringToNative ) )] string pchFile );
	#endregion
	
	internal bool FileDelete( [MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( Utf8StringToNative ) )] string pchFile )
	{
		var returnValue = _FileDelete( Self, pchFile );
		return returnValue;
	}
	
	#region FunctionMeta
	[DllImport( Platform.LibraryName, EntryPoint = "SteamAPI_ISteamRemoteStorage_FileShare", CallingConvention = Platform.CC)]
	private static extern SteamAPICall_t _FileShare( IntPtr self, [MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( Utf8StringToNative ) )] string pchFile );
	#endregion
	
	internal CallResult<RemoteStorageFileShareResult_t> FileShare( [MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( Utf8StringToNative ) )] string pchFile )
	{
		var returnValue = _FileShare( Self, pchFile );
		return new CallResult<RemoteStorageFileShareResult_t>( returnValue, IsServer );
	}
	
	#region FunctionMeta
	[DllImport( Platform.LibraryName, EntryPoint = "SteamAPI_ISteamRemoteStorage_SetSyncPlatforms", CallingConvention = Platform.CC)]
	[return: MarshalAs( UnmanagedType.I1 )]
	private static extern bool _SetSyncPlatforms( IntPtr self, [MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( Utf8StringToNative ) )] string pchFile, RemoteStoragePlatform eRemoteStoragePlatform );
	#endregion
	
	internal bool SetSyncPlatforms( [MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( Utf8StringToNative ) )] string pchFile, RemoteStoragePlatform eRemoteStoragePlatform )
	{
		var returnValue = _SetSyncPlatforms( Self, pchFile, eRemoteStoragePlatform );
		return returnValue;
	}
	
	#region FunctionMeta
	[DllImport( Platform.LibraryName, EntryPoint = "SteamAPI_ISteamRemoteStorage_FileWriteStreamOpen", CallingConvention = Platform.CC)]
	private static extern UGCFileWriteStreamHandle_t _FileWriteStreamOpen( IntPtr self, [MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( Utf8StringToNative ) )] string pchFile );
	#endregion
	
	internal UGCFileWriteStreamHandle_t FileWriteStreamOpen( [MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( Utf8StringToNative ) )] string pchFile )
	{
		var returnValue = _FileWriteStreamOpen( Self, pchFile );
		return returnValue;
	}
	
	#region FunctionMeta
	[DllImport( Platform.LibraryName, EntryPoint = "SteamAPI_ISteamRemoteStorage_FileWriteStreamWriteChunk", CallingConvention = Platform.CC)]
	[return: MarshalAs( UnmanagedType.I1 )]
	private static extern bool _FileWriteStreamWriteChunk( IntPtr self, UGCFileWriteStreamHandle_t writeHandle, IntPtr pvData, int cubData );
	#endregion
	
	internal bool FileWriteStreamWriteChunk( UGCFileWriteStreamHandle_t writeHandle, IntPtr pvData, int cubData )
	{
		var returnValue = _FileWriteStreamWriteChunk( Self, writeHandle, pvData, cubData );
		return returnValue;
	}
	
	#region FunctionMeta
	[DllImport( Platform.LibraryName, EntryPoint = "SteamAPI_ISteamRemoteStorage_FileWriteStreamClose", CallingConvention = Platform.CC)]
	[return: MarshalAs( UnmanagedType.I1 )]
	private static extern bool _FileWriteStreamClose( IntPtr self, UGCFileWriteStreamHandle_t writeHandle );
	#endregion
	
	internal bool FileWriteStreamClose( UGCFileWriteStreamHandle_t writeHandle )
	{
		var returnValue = _FileWriteStreamClose( Self, writeHandle );
		return returnValue;
	}
	
	#region FunctionMeta
	[DllImport( Platform.LibraryName, EntryPoint = "SteamAPI_ISteamRemoteStorage_FileWriteStreamCancel", CallingConvention = Platform.CC)]
	[return: MarshalAs( UnmanagedType.I1 )]
	private static extern bool _FileWriteStreamCancel( IntPtr self, UGCFileWriteStreamHandle_t writeHandle );
	#endregion
	
	internal bool FileWriteStreamCancel( UGCFileWriteStreamHandle_t writeHandle )
	{
		var returnValue = _FileWriteStreamCancel( Self, writeHandle );
		return returnValue;
	}
	
	#region FunctionMeta
	[DllImport( Platform.LibraryName, EntryPoint = "SteamAPI_ISteamRemoteStorage_FileExists", CallingConvention = Platform.CC)]
	[return: MarshalAs( UnmanagedType.I1 )]
	private static extern bool _FileExists( IntPtr self, [MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( Utf8StringToNative ) )] string pchFile );
	#endregion
	
	internal bool FileExists( [MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( Utf8StringToNative ) )] string pchFile )
	{
		var returnValue = _FileExists( Self, pchFile );
		return returnValue;
	}
	
	#region FunctionMeta
	[DllImport( Platform.LibraryName, EntryPoint = "SteamAPI_ISteamRemoteStorage_FilePersisted", CallingConvention = Platform.CC)]
	[return: MarshalAs( UnmanagedType.I1 )]
	private static extern bool _FilePersisted( IntPtr self, [MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( Utf8StringToNative ) )] string pchFile );
	#endregion
	
	internal bool FilePersisted( [MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( Utf8StringToNative ) )] string pchFile )
	{
		var returnValue = _FilePersisted( Self, pchFile );
		return returnValue;
	}
	
	#region FunctionMeta
	[DllImport( Platform.LibraryName, EntryPoint = "SteamAPI_ISteamRemoteStorage_GetFileSize", CallingConvention = Platform.CC)]
	private static extern int _GetFileSize( IntPtr self, [MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( Utf8StringToNative ) )] string pchFile );
	#endregion
	
	internal int GetFileSize( [MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( Utf8StringToNative ) )] string pchFile )
	{
		var returnValue = _GetFileSize( Self, pchFile );
		return returnValue;
	}
	
	#region FunctionMeta
	[DllImport( Platform.LibraryName, EntryPoint = "SteamAPI_ISteamRemoteStorage_GetFileTimestamp", CallingConvention = Platform.CC)]
	private static extern long _GetFileTimestamp( IntPtr self, [MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( Utf8StringToNative ) )] string pchFile );
	#endregion
	
	internal long GetFileTimestamp( [MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( Utf8StringToNative ) )] string pchFile )
	{
		var returnValue = _GetFileTimestamp( Self, pchFile );
		return returnValue;
	}
	
	#region FunctionMeta
	[DllImport( Platform.LibraryName, EntryPoint = "SteamAPI_ISteamRemoteStorage_GetSyncPlatforms", CallingConvention = Platform.CC)]
	private static extern RemoteStoragePlatform _GetSyncPlatforms( IntPtr self, [MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( Utf8StringToNative ) )] string pchFile );
	#endregion
	
	internal RemoteStoragePlatform GetSyncPlatforms( [MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( Utf8StringToNative ) )] string pchFile )
	{
		var returnValue = _GetSyncPlatforms( Self, pchFile );
		return returnValue;
	}
	
	#region FunctionMeta
	[DllImport( Platform.LibraryName, EntryPoint = "SteamAPI_ISteamRemoteStorage_GetFileCount", CallingConvention = Platform.CC)]
	private static extern int _GetFileCount( IntPtr self );
	#endregion
	
	internal int GetFileCount()
	{
		var returnValue = _GetFileCount( Self );
		return returnValue;
	}
	
	#region FunctionMeta
	[DllImport( Platform.LibraryName, EntryPoint = "SteamAPI_ISteamRemoteStorage_GetFileNameAndSize", CallingConvention = Platform.CC)]
	private static extern Utf8StringPointer _GetFileNameAndSize( IntPtr self, int iFile, ref int pnFileSizeInBytes );
	#endregion
	
	internal string GetFileNameAndSize( int iFile, ref int pnFileSizeInBytes )
	{
		var returnValue = _GetFileNameAndSize( Self, iFile, ref pnFileSizeInBytes );
		return returnValue;
	}
	
	#region FunctionMeta
	[DllImport( Platform.LibraryName, EntryPoint = "SteamAPI_ISteamRemoteStorage_GetQuota", CallingConvention = Platform.CC)]
	[return: MarshalAs( UnmanagedType.I1 )]
	private static extern bool _GetQuota( IntPtr self, ref ulong pnTotalBytes, ref ulong puAvailableBytes );
	#endregion
	
	internal bool GetQuota( ref ulong pnTotalBytes, ref ulong puAvailableBytes )
	{
		var returnValue = _GetQuota( Self, ref pnTotalBytes, ref puAvailableBytes );
		return returnValue;
	}
	
	#region FunctionMeta
	[DllImport( Platform.LibraryName, EntryPoint = "SteamAPI_ISteamRemoteStorage_IsCloudEnabledForAccount", CallingConvention = Platform.CC)]
	[return: MarshalAs( UnmanagedType.I1 )]
	private static extern bool _IsCloudEnabledForAccount( IntPtr self );
	#endregion
	
	internal bool IsCloudEnabledForAccount()
	{
		var returnValue = _IsCloudEnabledForAccount( Self );
		return returnValue;
	}
	
	#region FunctionMeta
	[DllImport( Platform.LibraryName, EntryPoint = "SteamAPI_ISteamRemoteStorage_IsCloudEnabledForApp", CallingConvention = Platform.CC)]
	[return: MarshalAs( UnmanagedType.I1 )]
	private static extern bool _IsCloudEnabledForApp( IntPtr self );
	#endregion
	
	internal bool IsCloudEnabledForApp()
	{
		var returnValue = _IsCloudEnabledForApp( Self );
		return returnValue;
	}
	
	#region FunctionMeta
	[DllImport( Platform.LibraryName, EntryPoint = "SteamAPI_ISteamRemoteStorage_SetCloudEnabledForApp", CallingConvention = Platform.CC)]
	private static extern void _SetCloudEnabledForApp( IntPtr self, [MarshalAs( UnmanagedType.U1 )] bool bEnabled );
	#endregion
	
	internal void SetCloudEnabledForApp( [MarshalAs( UnmanagedType.U1 )] bool bEnabled )
	{
		_SetCloudEnabledForApp( Self, bEnabled );
	}
	
	#region FunctionMeta
	[DllImport( Platform.LibraryName, EntryPoint = "SteamAPI_ISteamRemoteStorage_UGCDownload", CallingConvention = Platform.CC)]
	private static extern SteamAPICall_t _UGCDownload( IntPtr self, UGCHandle_t hContent, uint unPriority );
	#endregion
	
	internal CallResult<RemoteStorageDownloadUGCResult_t> UGCDownload( UGCHandle_t hContent, uint unPriority )
	{
		var returnValue = _UGCDownload( Self, hContent, unPriority );
		return new CallResult<RemoteStorageDownloadUGCResult_t>( returnValue, IsServer );
	}
	
	#region FunctionMeta
	[DllImport( Platform.LibraryName, EntryPoint = "SteamAPI_ISteamRemoteStorage_GetUGCDownloadProgress", CallingConvention = Platform.CC)]
	[return: MarshalAs( UnmanagedType.I1 )]
	private static extern bool _GetUGCDownloadProgress( IntPtr self, UGCHandle_t hContent, ref int pnBytesDownloaded, ref int pnBytesExpected );
	#endregion
	
	internal bool GetUGCDownloadProgress( UGCHandle_t hContent, ref int pnBytesDownloaded, ref int pnBytesExpected )
	{
		var returnValue = _GetUGCDownloadProgress( Self, hContent, ref pnBytesDownloaded, ref pnBytesExpected );
		return returnValue;
	}
	
	#region FunctionMeta
	[DllImport( Platform.LibraryName, EntryPoint = "SteamAPI_ISteamRemoteStorage_GetUGCDetails", CallingConvention = Platform.CC)]
	[return: MarshalAs( UnmanagedType.I1 )]
	private static extern bool _GetUGCDetails( IntPtr self, UGCHandle_t hContent, ref AppId pnAppID, [In,Out] ref char[]  ppchName, ref int pnFileSizeInBytes, ref SteamId pSteamIDOwner );
	#endregion
	
	internal bool GetUGCDetails( UGCHandle_t hContent, ref AppId pnAppID, [In,Out] ref char[]  ppchName, ref int pnFileSizeInBytes, ref SteamId pSteamIDOwner )
	{
		var returnValue = _GetUGCDetails( Self, hContent, ref pnAppID, ref ppchName, ref pnFileSizeInBytes, ref pSteamIDOwner );
		return returnValue;
	}
	
	#region FunctionMeta
	[DllImport( Platform.LibraryName, EntryPoint = "SteamAPI_ISteamRemoteStorage_UGCRead", CallingConvention = Platform.CC)]
	private static extern int _UGCRead( IntPtr self, UGCHandle_t hContent, IntPtr pvData, int cubDataToRead, uint cOffset, UGCReadAction eAction );
	#endregion
	
	internal int UGCRead( UGCHandle_t hContent, IntPtr pvData, int cubDataToRead, uint cOffset, UGCReadAction eAction )
	{
		var returnValue = _UGCRead( Self, hContent, pvData, cubDataToRead, cOffset, eAction );
		return returnValue;
	}
	
	#region FunctionMeta
	[DllImport( Platform.LibraryName, EntryPoint = "SteamAPI_ISteamRemoteStorage_GetCachedUGCCount", CallingConvention = Platform.CC)]
	private static extern int _GetCachedUGCCount( IntPtr self );
	#endregion
	
	internal int GetCachedUGCCount()
	{
		var returnValue = _GetCachedUGCCount( Self );
		return returnValue;
	}
	
	#region FunctionMeta
	[DllImport( Platform.LibraryName, EntryPoint = "SteamAPI_ISteamRemoteStorage_GetCachedUGCHandle", CallingConvention = Platform.CC)]
	private static extern UGCHandle_t _GetCachedUGCHandle( IntPtr self, int iCachedContent );
	#endregion
	
	internal UGCHandle_t GetCachedUGCHandle( int iCachedContent )
	{
		var returnValue = _GetCachedUGCHandle( Self, iCachedContent );
		return returnValue;
	}
	
	#region FunctionMeta
	[DllImport( Platform.LibraryName, EntryPoint = "SteamAPI_ISteamRemoteStorage_PublishWorkshopFile", CallingConvention = Platform.CC)]
	private static extern SteamAPICall_t _PublishWorkshopFile( IntPtr self, [MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( Utf8StringToNative ) )] string pchFile, [MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( Utf8StringToNative ) )] string pchPreviewFile, AppId nConsumerAppId, [MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( Utf8StringToNative ) )] string pchTitle, [MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( Utf8StringToNative ) )] string pchDescription, RemoteStoragePublishedFileVisibility eVisibility, ref SteamParamStringArray_t pTags, WorkshopFileType eWorkshopFileType );
	#endregion
	
	internal CallResult<RemoteStoragePublishFileProgress_t> PublishWorkshopFile( [MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( Utf8StringToNative ) )] string pchFile, [MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( Utf8StringToNative ) )] string pchPreviewFile, AppId nConsumerAppId, [MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( Utf8StringToNative ) )] string pchTitle, [MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( Utf8StringToNative ) )] string pchDescription, RemoteStoragePublishedFileVisibility eVisibility,  /* ref */ SteamParamStringArray_t pTags, WorkshopFileType eWorkshopFileType )
	{
		var returnValue = _PublishWorkshopFile( Self, pchFile, pchPreviewFile, nConsumerAppId, pchTitle, pchDescription, eVisibility, ref pTags, eWorkshopFileType );
		return new CallResult<RemoteStoragePublishFileProgress_t>( returnValue, IsServer );
	}
	
	#region FunctionMeta
	[DllImport( Platform.LibraryName, EntryPoint = "SteamAPI_ISteamRemoteStorage_CreatePublishedFileUpdateRequest", CallingConvention = Platform.CC)]
	private static extern PublishedFileUpdateHandle_t _CreatePublishedFileUpdateRequest( IntPtr self, PublishedFileId unPublishedFileId );
	#endregion
	
	internal PublishedFileUpdateHandle_t CreatePublishedFileUpdateRequest( PublishedFileId unPublishedFileId )
	{
		var returnValue = _CreatePublishedFileUpdateRequest( Self, unPublishedFileId );
		return returnValue;
	}
	
	#region FunctionMeta
	[DllImport( Platform.LibraryName, EntryPoint = "SteamAPI_ISteamRemoteStorage_UpdatePublishedFileFile", CallingConvention = Platform.CC)]
	[return: MarshalAs( UnmanagedType.I1 )]
	private static extern bool _UpdatePublishedFileFile( IntPtr self, PublishedFileUpdateHandle_t updateHandle, [MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( Utf8StringToNative ) )] string pchFile );
	#endregion
	
	internal bool UpdatePublishedFileFile( PublishedFileUpdateHandle_t updateHandle, [MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( Utf8StringToNative ) )] string pchFile )
	{
		var returnValue = _UpdatePublishedFileFile( Self, updateHandle, pchFile );
		return returnValue;
	}
	
	#region FunctionMeta
	[DllImport( Platform.LibraryName, EntryPoint = "SteamAPI_ISteamRemoteStorage_UpdatePublishedFilePreviewFile", CallingConvention = Platform.CC)]
	[return: MarshalAs( UnmanagedType.I1 )]
	private static extern bool _UpdatePublishedFilePreviewFile( IntPtr self, PublishedFileUpdateHandle_t updateHandle, [MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( Utf8StringToNative ) )] string pchPreviewFile );
	#endregion
	
	internal bool UpdatePublishedFilePreviewFile( PublishedFileUpdateHandle_t updateHandle, [MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( Utf8StringToNative ) )] string pchPreviewFile )
	{
		var returnValue = _UpdatePublishedFilePreviewFile( Self, updateHandle, pchPreviewFile );
		return returnValue;
	}
	
	#region FunctionMeta
	[DllImport( Platform.LibraryName, EntryPoint = "SteamAPI_ISteamRemoteStorage_UpdatePublishedFileTitle", CallingConvention = Platform.CC)]
	[return: MarshalAs( UnmanagedType.I1 )]
	private static extern bool _UpdatePublishedFileTitle( IntPtr self, PublishedFileUpdateHandle_t updateHandle, [MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( Utf8StringToNative ) )] string pchTitle );
	#endregion
	
	internal bool UpdatePublishedFileTitle( PublishedFileUpdateHandle_t updateHandle, [MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( Utf8StringToNative ) )] string pchTitle )
	{
		var returnValue = _UpdatePublishedFileTitle( Self, updateHandle, pchTitle );
		return returnValue;
	}
	
	#region FunctionMeta
	[DllImport( Platform.LibraryName, EntryPoint = "SteamAPI_ISteamRemoteStorage_UpdatePublishedFileDescription", CallingConvention = Platform.CC)]
	[return: MarshalAs( UnmanagedType.I1 )]
	private static extern bool _UpdatePublishedFileDescription( IntPtr self, PublishedFileUpdateHandle_t updateHandle, [MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( Utf8StringToNative ) )] string pchDescription );
	#endregion
	
	internal bool UpdatePublishedFileDescription( PublishedFileUpdateHandle_t updateHandle, [MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( Utf8StringToNative ) )] string pchDescription )
	{
		var returnValue = _UpdatePublishedFileDescription( Self, updateHandle, pchDescription );
		return returnValue;
	}
	
	#region FunctionMeta
	[DllImport( Platform.LibraryName, EntryPoint = "SteamAPI_ISteamRemoteStorage_UpdatePublishedFileVisibility", CallingConvention = Platform.CC)]
	[return: MarshalAs( UnmanagedType.I1 )]
	private static extern bool _UpdatePublishedFileVisibility( IntPtr self, PublishedFileUpdateHandle_t updateHandle, RemoteStoragePublishedFileVisibility eVisibility );
	#endregion
	
	internal bool UpdatePublishedFileVisibility( PublishedFileUpdateHandle_t updateHandle, RemoteStoragePublishedFileVisibility eVisibility )
	{
		var returnValue = _UpdatePublishedFileVisibility( Self, updateHandle, eVisibility );
		return returnValue;
	}
	
	#region FunctionMeta
	[DllImport( Platform.LibraryName, EntryPoint = "SteamAPI_ISteamRemoteStorage_UpdatePublishedFileTags", CallingConvention = Platform.CC)]
	[return: MarshalAs( UnmanagedType.I1 )]
	private static extern bool _UpdatePublishedFileTags( IntPtr self, PublishedFileUpdateHandle_t updateHandle, ref SteamParamStringArray_t pTags );
	#endregion
	
	internal bool UpdatePublishedFileTags( PublishedFileUpdateHandle_t updateHandle, ref SteamParamStringArray_t pTags )
	{
		var returnValue = _UpdatePublishedFileTags( Self, updateHandle, ref pTags );
		return returnValue;
	}
	
	#region FunctionMeta
	[DllImport( Platform.LibraryName, EntryPoint = "SteamAPI_ISteamRemoteStorage_CommitPublishedFileUpdate", CallingConvention = Platform.CC)]
	private static extern SteamAPICall_t _CommitPublishedFileUpdate( IntPtr self, PublishedFileUpdateHandle_t updateHandle );
	#endregion
	
	internal CallResult<RemoteStorageUpdatePublishedFileResult_t> CommitPublishedFileUpdate( PublishedFileUpdateHandle_t updateHandle )
	{
		var returnValue = _CommitPublishedFileUpdate( Self, updateHandle );
		return new CallResult<RemoteStorageUpdatePublishedFileResult_t>( returnValue, IsServer );
	}
	
	#region FunctionMeta
	[DllImport( Platform.LibraryName, EntryPoint = "SteamAPI_ISteamRemoteStorage_GetPublishedFileDetails", CallingConvention = Platform.CC)]
	private static extern SteamAPICall_t _GetPublishedFileDetails( IntPtr self, PublishedFileId unPublishedFileId, uint unMaxSecondsOld );
	#endregion
	
	internal CallResult<RemoteStorageGetPublishedFileDetailsResult_t> GetPublishedFileDetails( PublishedFileId unPublishedFileId, uint unMaxSecondsOld )
	{
		var returnValue = _GetPublishedFileDetails( Self, unPublishedFileId, unMaxSecondsOld );
		return new CallResult<RemoteStorageGetPublishedFileDetailsResult_t>( returnValue, IsServer );
	}
	
	#region FunctionMeta
	[DllImport( Platform.LibraryName, EntryPoint = "SteamAPI_ISteamRemoteStorage_DeletePublishedFile", CallingConvention = Platform.CC)]
	private static extern SteamAPICall_t _DeletePublishedFile( IntPtr self, PublishedFileId unPublishedFileId );
	#endregion
	
	internal CallResult<RemoteStorageDeletePublishedFileResult_t> DeletePublishedFile( PublishedFileId unPublishedFileId )
	{
		var returnValue = _DeletePublishedFile( Self, unPublishedFileId );
		return new CallResult<RemoteStorageDeletePublishedFileResult_t>( returnValue, IsServer );
	}
	
	#region FunctionMeta
	[DllImport( Platform.LibraryName, EntryPoint = "SteamAPI_ISteamRemoteStorage_EnumerateUserPublishedFiles", CallingConvention = Platform.CC)]
	private static extern SteamAPICall_t _EnumerateUserPublishedFiles( IntPtr self, uint unStartIndex );
	#endregion
	
	internal CallResult<RemoteStorageEnumerateUserPublishedFilesResult_t> EnumerateUserPublishedFiles( uint unStartIndex )
	{
		var returnValue = _EnumerateUserPublishedFiles( Self, unStartIndex );
		return new CallResult<RemoteStorageEnumerateUserPublishedFilesResult_t>( returnValue, IsServer );
	}
	
	#region FunctionMeta
	[DllImport( Platform.LibraryName, EntryPoint = "SteamAPI_ISteamRemoteStorage_SubscribePublishedFile", CallingConvention = Platform.CC)]
	private static extern SteamAPICall_t _SubscribePublishedFile( IntPtr self, PublishedFileId unPublishedFileId );
	#endregion
	
	internal CallResult<RemoteStorageSubscribePublishedFileResult_t> SubscribePublishedFile( PublishedFileId unPublishedFileId )
	{
		var returnValue = _SubscribePublishedFile( Self, unPublishedFileId );
		return new CallResult<RemoteStorageSubscribePublishedFileResult_t>( returnValue, IsServer );
	}
	
	#region FunctionMeta
	[DllImport( Platform.LibraryName, EntryPoint = "SteamAPI_ISteamRemoteStorage_EnumerateUserSubscribedFiles", CallingConvention = Platform.CC)]
	private static extern SteamAPICall_t _EnumerateUserSubscribedFiles( IntPtr self, uint unStartIndex );
	#endregion
	
	internal CallResult<RemoteStorageEnumerateUserSubscribedFilesResult_t> EnumerateUserSubscribedFiles( uint unStartIndex )
	{
		var returnValue = _EnumerateUserSubscribedFiles( Self, unStartIndex );
		return new CallResult<RemoteStorageEnumerateUserSubscribedFilesResult_t>( returnValue, IsServer );
	}
	
	#region FunctionMeta
	[DllImport( Platform.LibraryName, EntryPoint = "SteamAPI_ISteamRemoteStorage_UnsubscribePublishedFile", CallingConvention = Platform.CC)]
	private static extern SteamAPICall_t _UnsubscribePublishedFile( IntPtr self, PublishedFileId unPublishedFileId );
	#endregion
	
	internal CallResult<RemoteStorageUnsubscribePublishedFileResult_t> UnsubscribePublishedFile( PublishedFileId unPublishedFileId )
	{
		var returnValue = _UnsubscribePublishedFile( Self, unPublishedFileId );
		return new CallResult<RemoteStorageUnsubscribePublishedFileResult_t>( returnValue, IsServer );
	}
	
	#region FunctionMeta
	[DllImport( Platform.LibraryName, EntryPoint = "SteamAPI_ISteamRemoteStorage_UpdatePublishedFileSetChangeDescription", CallingConvention = Platform.CC)]
	[return: MarshalAs( UnmanagedType.I1 )]
	private static extern bool _UpdatePublishedFileSetChangeDescription( IntPtr self, PublishedFileUpdateHandle_t updateHandle, [MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( Utf8StringToNative ) )] string pchChangeDescription );
	#endregion
	
	internal bool UpdatePublishedFileSetChangeDescription( PublishedFileUpdateHandle_t updateHandle, [MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( Utf8StringToNative ) )] string pchChangeDescription )
	{
		var returnValue = _UpdatePublishedFileSetChangeDescription( Self, updateHandle, pchChangeDescription );
		return returnValue;
	}
	
	#region FunctionMeta
	[DllImport( Platform.LibraryName, EntryPoint = "SteamAPI_ISteamRemoteStorage_GetPublishedItemVoteDetails", CallingConvention = Platform.CC)]
	private static extern SteamAPICall_t _GetPublishedItemVoteDetails( IntPtr self, PublishedFileId unPublishedFileId );
	#endregion
	
	internal CallResult<RemoteStorageGetPublishedItemVoteDetailsResult_t> GetPublishedItemVoteDetails( PublishedFileId unPublishedFileId )
	{
		var returnValue = _GetPublishedItemVoteDetails( Self, unPublishedFileId );
		return new CallResult<RemoteStorageGetPublishedItemVoteDetailsResult_t>( returnValue, IsServer );
	}
	
	#region FunctionMeta
	[DllImport( Platform.LibraryName, EntryPoint = "SteamAPI_ISteamRemoteStorage_UpdateUserPublishedItemVote", CallingConvention = Platform.CC)]
	private static extern SteamAPICall_t _UpdateUserPublishedItemVote( IntPtr self, PublishedFileId unPublishedFileId, [MarshalAs( UnmanagedType.U1 )] bool bVoteUp );
	#endregion
	
	internal CallResult<RemoteStorageUpdateUserPublishedItemVoteResult_t> UpdateUserPublishedItemVote( PublishedFileId unPublishedFileId, [MarshalAs( UnmanagedType.U1 )] bool bVoteUp )
	{
		var returnValue = _UpdateUserPublishedItemVote( Self, unPublishedFileId, bVoteUp );
		return new CallResult<RemoteStorageUpdateUserPublishedItemVoteResult_t>( returnValue, IsServer );
	}
	
	#region FunctionMeta
	[DllImport( Platform.LibraryName, EntryPoint = "SteamAPI_ISteamRemoteStorage_GetUserPublishedItemVoteDetails", CallingConvention = Platform.CC)]
	private static extern SteamAPICall_t _GetUserPublishedItemVoteDetails( IntPtr self, PublishedFileId unPublishedFileId );
	#endregion
	
	internal CallResult<RemoteStorageGetPublishedItemVoteDetailsResult_t> GetUserPublishedItemVoteDetails( PublishedFileId unPublishedFileId )
	{
		var returnValue = _GetUserPublishedItemVoteDetails( Self, unPublishedFileId );
		return new CallResult<RemoteStorageGetPublishedItemVoteDetailsResult_t>( returnValue, IsServer );
	}
	
	#region FunctionMeta
	[DllImport( Platform.LibraryName, EntryPoint = "SteamAPI_ISteamRemoteStorage_EnumerateUserSharedWorkshopFiles", CallingConvention = Platform.CC)]
	private static extern SteamAPICall_t _EnumerateUserSharedWorkshopFiles( IntPtr self, SteamId steamId, uint unStartIndex, ref SteamParamStringArray_t pRequiredTags, ref SteamParamStringArray_t pExcludedTags );
	#endregion
	
	internal CallResult<RemoteStorageEnumerateUserPublishedFilesResult_t> EnumerateUserSharedWorkshopFiles( SteamId steamId, uint unStartIndex,  /* ref */ SteamParamStringArray_t pRequiredTags,  /* ref */ SteamParamStringArray_t pExcludedTags )
	{
		var returnValue = _EnumerateUserSharedWorkshopFiles( Self, steamId, unStartIndex, ref pRequiredTags, ref pExcludedTags );
		return new CallResult<RemoteStorageEnumerateUserPublishedFilesResult_t>( returnValue, IsServer );
	}
	
	#region FunctionMeta
	[DllImport( Platform.LibraryName, EntryPoint = "SteamAPI_ISteamRemoteStorage_PublishVideo", CallingConvention = Platform.CC)]
	private static extern SteamAPICall_t _PublishVideo( IntPtr self, WorkshopVideoProvider eVideoProvider, [MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( Utf8StringToNative ) )] string pchVideoAccount, [MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( Utf8StringToNative ) )] string pchVideoIdentifier, [MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( Utf8StringToNative ) )] string pchPreviewFile, AppId nConsumerAppId, [MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( Utf8StringToNative ) )] string pchTitle, [MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( Utf8StringToNative ) )] string pchDescription, RemoteStoragePublishedFileVisibility eVisibility, ref SteamParamStringArray_t pTags );
	#endregion
	
	internal CallResult<RemoteStoragePublishFileProgress_t> PublishVideo( WorkshopVideoProvider eVideoProvider, [MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( Utf8StringToNative ) )] string pchVideoAccount, [MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( Utf8StringToNative ) )] string pchVideoIdentifier, [MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( Utf8StringToNative ) )] string pchPreviewFile, AppId nConsumerAppId, [MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( Utf8StringToNative ) )] string pchTitle, [MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( Utf8StringToNative ) )] string pchDescription, RemoteStoragePublishedFileVisibility eVisibility,  /* ref */ SteamParamStringArray_t pTags )
	{
		var returnValue = _PublishVideo( Self, eVideoProvider, pchVideoAccount, pchVideoIdentifier, pchPreviewFile, nConsumerAppId, pchTitle, pchDescription, eVisibility, ref pTags );
		return new CallResult<RemoteStoragePublishFileProgress_t>( returnValue, IsServer );
	}
	
	#region FunctionMeta
	[DllImport( Platform.LibraryName, EntryPoint = "SteamAPI_ISteamRemoteStorage_SetUserPublishedFileAction", CallingConvention = Platform.CC)]
	private static extern SteamAPICall_t _SetUserPublishedFileAction( IntPtr self, PublishedFileId unPublishedFileId, WorkshopFileAction eAction );
	#endregion
	
	internal CallResult<RemoteStorageSetUserPublishedFileActionResult_t> SetUserPublishedFileAction( PublishedFileId unPublishedFileId, WorkshopFileAction eAction )
	{
		var returnValue = _SetUserPublishedFileAction( Self, unPublishedFileId, eAction );
		return new CallResult<RemoteStorageSetUserPublishedFileActionResult_t>( returnValue, IsServer );
	}
	
	#region FunctionMeta
	[DllImport( Platform.LibraryName, EntryPoint = "SteamAPI_ISteamRemoteStorage_EnumeratePublishedFilesByUserAction", CallingConvention = Platform.CC)]
	private static extern SteamAPICall_t _EnumeratePublishedFilesByUserAction( IntPtr self, WorkshopFileAction eAction, uint unStartIndex );
	#endregion
	
	internal CallResult<RemoteStorageEnumeratePublishedFilesByUserActionResult_t> EnumeratePublishedFilesByUserAction( WorkshopFileAction eAction, uint unStartIndex )
	{
		var returnValue = _EnumeratePublishedFilesByUserAction( Self, eAction, unStartIndex );
		return new CallResult<RemoteStorageEnumeratePublishedFilesByUserActionResult_t>( returnValue, IsServer );
	}
	
	#region FunctionMeta
	[DllImport( Platform.LibraryName, EntryPoint = "SteamAPI_ISteamRemoteStorage_EnumeratePublishedWorkshopFiles", CallingConvention = Platform.CC)]
	private static extern SteamAPICall_t _EnumeratePublishedWorkshopFiles( IntPtr self, WorkshopEnumerationType eEnumerationType, uint unStartIndex, uint unCount, uint unDays, ref SteamParamStringArray_t pTags, ref SteamParamStringArray_t pUserTags );
	#endregion
	
	internal CallResult<RemoteStorageEnumerateWorkshopFilesResult_t> EnumeratePublishedWorkshopFiles( WorkshopEnumerationType eEnumerationType, uint unStartIndex, uint unCount, uint unDays,  /* ref */ SteamParamStringArray_t pTags,  /* ref */ SteamParamStringArray_t pUserTags )
	{
		var returnValue = _EnumeratePublishedWorkshopFiles( Self, eEnumerationType, unStartIndex, unCount, unDays, ref pTags, ref pUserTags );
		return new CallResult<RemoteStorageEnumerateWorkshopFilesResult_t>( returnValue, IsServer );
	}
	
	#region FunctionMeta
	[DllImport( Platform.LibraryName, EntryPoint = "SteamAPI_ISteamRemoteStorage_UGCDownloadToLocation", CallingConvention = Platform.CC)]
	private static extern SteamAPICall_t _UGCDownloadToLocation( IntPtr self, UGCHandle_t hContent, [MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( Utf8StringToNative ) )] string pchLocation, uint unPriority );
	#endregion
	
	internal CallResult<RemoteStorageDownloadUGCResult_t> UGCDownloadToLocation( UGCHandle_t hContent, [MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( Utf8StringToNative ) )] string pchLocation, uint unPriority )
	{
		var returnValue = _UGCDownloadToLocation( Self, hContent, pchLocation, unPriority );
		return new CallResult<RemoteStorageDownloadUGCResult_t>( returnValue, IsServer );
	}
	
	#region FunctionMeta
	[DllImport( Platform.LibraryName, EntryPoint = "SteamAPI_ISteamRemoteStorage_GetLocalFileChangeCount", CallingConvention = Platform.CC)]
	private static extern int _GetLocalFileChangeCount( IntPtr self );
	#endregion
	
	internal int GetLocalFileChangeCount()
	{
		var returnValue = _GetLocalFileChangeCount( Self );
		return returnValue;
	}
	
	#region FunctionMeta
	[DllImport( Platform.LibraryName, EntryPoint = "SteamAPI_ISteamRemoteStorage_GetLocalFileChange", CallingConvention = Platform.CC)]
	private static extern Utf8StringPointer _GetLocalFileChange( IntPtr self, int iFile, ref RemoteStorageLocalFileChange pEChangeType, ref RemoteStorageFilePathType pEFilePathType );
	#endregion
	
	internal string GetLocalFileChange( int iFile, ref RemoteStorageLocalFileChange pEChangeType, ref RemoteStorageFilePathType pEFilePathType )
	{
		var returnValue = _GetLocalFileChange( Self, iFile, ref pEChangeType, ref pEFilePathType );
		return returnValue;
	}
	
	#region FunctionMeta
	[DllImport( Platform.LibraryName, EntryPoint = "SteamAPI_ISteamRemoteStorage_BeginFileWriteBatch", CallingConvention = Platform.CC)]
	[return: MarshalAs( UnmanagedType.I1 )]
	private static extern bool _BeginFileWriteBatch( IntPtr self );
	#endregion
	
	internal bool BeginFileWriteBatch()
	{
		var returnValue = _BeginFileWriteBatch( Self );
		return returnValue;
	}
	
	#region FunctionMeta
	[DllImport( Platform.LibraryName, EntryPoint = "SteamAPI_ISteamRemoteStorage_EndFileWriteBatch", CallingConvention = Platform.CC)]
	[return: MarshalAs( UnmanagedType.I1 )]
	private static extern bool _EndFileWriteBatch( IntPtr self );
	#endregion
	
	internal bool EndFileWriteBatch()
	{
		var returnValue = _EndFileWriteBatch( Self );
		return returnValue;
	}
	
}
