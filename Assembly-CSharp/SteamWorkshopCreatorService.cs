using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Steamworks;
using UnityEngine;

// Token: 0x020007AB RID: 1963
public class SteamWorkshopCreatorService
{
	// Token: 0x140000B1 RID: 177
	// (add) Token: 0x0600324E RID: 12878 RVA: 0x000F0F68 File Offset: 0x000EF168
	// (remove) Token: 0x0600324F RID: 12879 RVA: 0x000F0FA0 File Offset: 0x000EF1A0
	public event Action<string> Logged;

	// Token: 0x140000B2 RID: 178
	// (add) Token: 0x06003250 RID: 12880 RVA: 0x000F0FD8 File Offset: 0x000EF1D8
	// (remove) Token: 0x06003251 RID: 12881 RVA: 0x000F1010 File Offset: 0x000EF210
	public event Action ItemsChanged;

	// Token: 0x170007AB RID: 1963
	// (get) Token: 0x06003252 RID: 12882 RVA: 0x000F1045 File Offset: 0x000EF245
	// (set) Token: 0x06003253 RID: 12883 RVA: 0x000F104D File Offset: 0x000EF24D
	public bool IsBusy { get; private set; }

	// Token: 0x06003254 RID: 12884 RVA: 0x000F1058 File Offset: 0x000EF258
	public SteamWorkshopCreatorService()
	{
		this.items = SteamWorkshopCreatorStore.Load();
		this.createItemResult = CallResult<CreateItemResult_t>.Create(new CallResult<CreateItemResult_t>.APIDispatchDelegate(this.OnCreateItem));
		this.submitItemResult = CallResult<SubmitItemUpdateResult_t>.Create(new CallResult<SubmitItemUpdateResult_t>.APIDispatchDelegate(this.OnSubmitItem));
		this.deleteItemResult = CallResult<DeleteItemResult_t>.Create(new CallResult<DeleteItemResult_t>.APIDispatchDelegate(this.OnDeleteItem));
	}

	// Token: 0x170007AC RID: 1964
	// (get) Token: 0x06003255 RID: 12885 RVA: 0x000F10D8 File Offset: 0x000EF2D8
	public IReadOnlyList<SteamWorkshopItemRecord> Items
	{
		get
		{
			return this.items;
		}
	}

	// Token: 0x06003256 RID: 12886 RVA: 0x000F10E0 File Offset: 0x000EF2E0
	public void Dispose()
	{
		this.CleanupStaging();
		CallResult<CreateItemResult_t> callResult = this.createItemResult;
		if (callResult != null)
		{
			callResult.Dispose();
		}
		CallResult<SubmitItemUpdateResult_t> callResult2 = this.submitItemResult;
		if (callResult2 != null)
		{
			callResult2.Dispose();
		}
		CallResult<DeleteItemResult_t> callResult3 = this.deleteItemResult;
		if (callResult3 != null)
		{
			callResult3.Dispose();
		}
		this.createItemResult = null;
		this.submitItemResult = null;
		this.deleteItemResult = null;
	}

	// Token: 0x06003257 RID: 12887 RVA: 0x000F113C File Offset: 0x000EF33C
	public void Tick()
	{
		if (!this.IsBusy)
		{
			return;
		}
		this.busyElapsed += Time.unscaledDeltaTime;
		if (this.busyElapsed - this.lastHeartbeat >= 2f)
		{
			this.lastHeartbeat = this.busyElapsed;
			this.Log(string.Format("Waiting for Steam... {0:0}s", this.busyElapsed));
		}
		if (this.busyElapsed > 120f)
		{
			this.Fail("Timed out waiting for Steam after 120 seconds. The previous submit callback may have been dropped; try Upload again.");
			return;
		}
		if (this.queuedSubmit)
		{
			this.queuedSubmit = false;
			this.Log("Continuing submit on the next frame...");
			this.SubmitContent(this.queuedSubmitId);
			return;
		}
		if (this.updateHandle != UGCUpdateHandle_t.Invalid)
		{
			ulong num;
			ulong num2;
			EItemUpdateStatus itemUpdateProgress = SteamUGC.GetItemUpdateProgress(this.updateHandle, out num, out num2);
			if (itemUpdateProgress != EItemUpdateStatus.k_EItemUpdateStatusInvalid)
			{
				int num3 = ((num2 > 0UL) ? ((int)(num * 100UL / num2)) : 0);
				if (itemUpdateProgress != this.lastProgressStatus || num3 != this.lastProgressPercent)
				{
					this.lastProgressStatus = itemUpdateProgress;
					this.lastProgressPercent = num3;
					this.Log(string.Format("{0} ({1}/{2})", SteamWorkshopCreatorService.DescribeStatus(itemUpdateProgress), num, num2));
				}
			}
		}
	}

	// Token: 0x06003258 RID: 12888 RVA: 0x000F1260 File Offset: 0x000EF460
	public void CreateAndUpload(string title, string tag, string localFolder)
	{
		if (!this.TryBeginBusy())
		{
			return;
		}
		if (string.IsNullOrWhiteSpace(title))
		{
			this.Fail("Title is empty.");
			return;
		}
		if (string.IsNullOrWhiteSpace(tag))
		{
			this.Fail("Tag is empty.");
			return;
		}
		if (tag.IndexOf(',') >= 0)
		{
			this.Fail("Tag contains a comma, which Steam rejects: '" + tag + "'");
			return;
		}
		string text;
		string text2;
		if (!SteamWorkshopCreatorService.TryNormalizeFolder(localFolder, out text, out text2))
		{
			this.Fail(text2);
			return;
		}
		this.EnsureThumbnail(text);
		this.pendingRecord = new SteamWorkshopItemRecord
		{
			title = title.Trim(),
			tag = tag.Trim(),
			localFolder = text
		};
		this.pendingChangeNote = "Initial upload";
		if (!SteamManager.Initialized)
		{
			this.Fail("Steam is not initialized.");
			return;
		}
		this.Log("Creating workshop item '" + this.pendingRecord.title + "'...");
		AppId_t appID = SteamUtils.GetAppID();
		this.LogAppId(appID);
		SteamAPICall_t steamAPICall_t = SteamUGC.CreateItem(appID, EWorkshopFileType.k_EWorkshopFileTypeFirst);
		this.createItemResult.Set(steamAPICall_t, null);
	}

	// Token: 0x06003259 RID: 12889 RVA: 0x000F1368 File Offset: 0x000EF568
	public void Upload(SteamWorkshopItemRecord record)
	{
		if (!this.TryBeginBusy())
		{
			return;
		}
		if (record == null)
		{
			this.Fail("No item selected.");
			return;
		}
		if (record.publishedFileId == 0UL)
		{
			this.Fail("Item has no Steam published file id. Create it first.");
			return;
		}
		string text;
		string text2;
		if (!SteamWorkshopCreatorService.TryNormalizeFolder(record.localFolder, out text, out text2))
		{
			this.Fail(text2);
			return;
		}
		record.localFolder = text;
		this.EnsureThumbnail(text);
		this.pendingRecord = record;
		this.pendingChangeNote = "Updated";
		if (!SteamManager.Initialized)
		{
			this.Fail("Steam is not initialized.");
			return;
		}
		this.SubmitContent(new PublishedFileId_t(record.publishedFileId));
	}

	// Token: 0x0600325A RID: 12890 RVA: 0x000F1400 File Offset: 0x000EF600
	public void Delete(SteamWorkshopItemRecord record)
	{
		if (!this.TryBeginBusy())
		{
			return;
		}
		if (record == null)
		{
			this.Fail("No item selected.");
			return;
		}
		this.pendingRecord = record;
		if (record.publishedFileId == 0UL)
		{
			this.RemoveLocal(record);
			this.Log("Removed local creator config. Nothing to delete on Steam.");
			this.FinishBusy();
			Action itemsChanged = this.ItemsChanged;
			if (itemsChanged == null)
			{
				return;
			}
			itemsChanged();
			return;
		}
		else
		{
			if (!SteamManager.Initialized)
			{
				this.Fail("Steam is not initialized.");
				return;
			}
			this.Log(string.Format("Deleting Steam item {0}...", record.publishedFileId));
			SteamAPICall_t steamAPICall_t = SteamUGC.DeleteItem(new PublishedFileId_t(record.publishedFileId));
			this.deleteItemResult.Set(steamAPICall_t, null);
			return;
		}
	}

	// Token: 0x0600325B RID: 12891 RVA: 0x000F14AC File Offset: 0x000EF6AC
	private bool TryBeginBusy()
	{
		if (this.IsBusy)
		{
			this.Log("Busy: wait for the current Steam operation to finish.");
			return false;
		}
		this.IsBusy = true;
		this.lastProgressPercent = -1;
		this.omitTags = false;
		this.retryWithoutTagsOnInvalidParam = true;
		this.queuedSubmit = false;
		this.busyElapsed = 0f;
		this.lastHeartbeat = 0f;
		this.lastSubmitDetails = "";
		this.lastProgressStatus = EItemUpdateStatus.k_EItemUpdateStatusInvalid;
		this.updateHandle = UGCUpdateHandle_t.Invalid;
		return true;
	}

	// Token: 0x0600325C RID: 12892 RVA: 0x000F1525 File Offset: 0x000EF725
	private void FinishBusy()
	{
		this.CleanupStaging();
		this.IsBusy = false;
		this.pendingRecord = null;
		this.pendingChangeNote = null;
		this.omitTags = false;
		this.retryWithoutTagsOnInvalidParam = true;
		this.queuedSubmit = false;
		this.updateHandle = UGCUpdateHandle_t.Invalid;
	}

	// Token: 0x0600325D RID: 12893 RVA: 0x000F1562 File Offset: 0x000EF762
	private void Fail(string message)
	{
		this.Log("Error: " + message);
		if (!string.IsNullOrEmpty(this.lastSubmitDetails))
		{
			this.Log(this.lastSubmitDetails);
		}
		this.FinishBusy();
	}

	// Token: 0x0600325E RID: 12894 RVA: 0x000F1594 File Offset: 0x000EF794
	private void Log(string message)
	{
		Debug.Log("[SteamWorkshopCreator] " + message);
		Action<string> logged = this.Logged;
		if (logged == null)
		{
			return;
		}
		logged(message);
	}

	// Token: 0x0600325F RID: 12895 RVA: 0x000F15B8 File Offset: 0x000EF7B8
	private void OnCreateItem(CreateItemResult_t result, bool ioFailure)
	{
		if (ioFailure || result.m_eResult != EResult.k_EResultOK)
		{
			this.Fail("CreateItem failed: " + this.DescribeResult(ioFailure, result.m_eResult, result.m_nPublishedFileId.m_PublishedFileId, result.m_bUserNeedsToAcceptWorkshopLegalAgreement));
			return;
		}
		ulong publishedFileId = result.m_nPublishedFileId.m_PublishedFileId;
		this.pendingRecord.publishedFileId = publishedFileId;
		this.UpsertLocal(this.pendingRecord);
		SteamWorkshopCreatorStore.Save(this.items);
		Action itemsChanged = this.ItemsChanged;
		if (itemsChanged != null)
		{
			itemsChanged();
		}
		this.Log(string.Format("Created item id {0}.", publishedFileId));
		if (result.m_bUserNeedsToAcceptWorkshopLegalAgreement)
		{
			this.OpenLegalAgreement(result.m_nPublishedFileId);
		}
		this.queuedSubmitId = result.m_nPublishedFileId;
		this.queuedSubmit = true;
		this.Log("Submit queued for the next frame (Steam rejects SubmitItemUpdate from inside CreateItem).");
	}

	// Token: 0x06003260 RID: 12896 RVA: 0x000F1688 File Offset: 0x000EF888
	private void SubmitContent(PublishedFileId_t publishedFileId)
	{
		AppId_t appID = SteamUtils.GetAppID();
		string localFolder = this.pendingRecord.localFolder;
		string text;
		int num;
		string text2;
		if (!this.TryCreateUploadStaging(localFolder, out text, out num, out text2))
		{
			this.Fail(text2);
			return;
		}
		int num2;
		long num3;
		string text3;
		SteamWorkshopCreatorService.DescribeFolder(text, out num2, out num3, out text3);
		if (num > 0)
		{
			this.Log(string.Format("Skipping {0} file(s) whose names start with '_'.", num));
		}
		bool flag = false;
		this.Log(string.Format("Starting content update. App ID {0}, item {1}, files {2}, bytes {3}.", new object[] { appID.m_AppId, publishedFileId.m_PublishedFileId, num2, num3 }));
		this.updateHandle = SteamUGC.StartItemUpdate(appID, publishedFileId);
		if (this.updateHandle == UGCUpdateHandle_t.Invalid)
		{
			this.lastSubmitDetails = this.BuildSubmitDetails(appID, publishedFileId, text, num2, num3, text3, false, false, false, false, false, false, "StartItemUpdate returned invalid handle");
			this.Fail("StartItemUpdate returned an invalid handle.");
			return;
		}
		bool flag2 = SteamUGC.SetItemTitle(this.updateHandle, this.pendingRecord.title);
		if (!flag2)
		{
			this.Log("Warning: SetItemTitle failed.");
		}
		bool flag3 = SteamUGC.SetItemDescription(this.updateHandle, this.pendingRecord.title);
		if (!flag3)
		{
			this.Log("Warning: SetItemDescription failed.");
		}
		bool flag4;
		if (!this.omitTags)
		{
			flag4 = SteamUGC.SetItemTags(this.updateHandle, new List<string> { this.pendingRecord.tag }, false);
			if (!flag4)
			{
				this.Log("Warning: SetItemTags failed for Type='" + this.pendingRecord.tag + "'.");
			}
			else
			{
				this.Log("Set workshop tag Type='" + this.pendingRecord.tag + "'.");
			}
		}
		else
		{
			this.Log("Submitting without workshop tags.");
			flag4 = true;
		}
		bool flag5 = SteamUGC.SetItemVisibility(this.updateHandle, ERemoteStoragePublishedFileVisibility.k_ERemoteStoragePublishedFileVisibilityUnlisted);
		if (!flag5)
		{
			this.Log("Warning: SetItemVisibility failed.");
		}
		bool flag6 = SteamUGC.SetItemContent(this.updateHandle, text);
		if (!flag6)
		{
			this.lastSubmitDetails = this.BuildSubmitDetails(appID, publishedFileId, text, num2, num3, text3, flag2, flag3, flag4, flag5, false, false, "SetItemContent returned false");
			this.Fail("SetItemContent failed. Steam rejected the folder path.");
			return;
		}
		string text4 = SteamWorkshopCreatorService.FindThumbnailPath(localFolder);
		if (!string.IsNullOrEmpty(text4))
		{
			flag = SteamUGC.SetItemPreview(this.updateHandle, text4);
			if (!flag)
			{
				this.Log("Warning: SetItemPreview failed for '" + text4 + "'.");
			}
			else
			{
				this.Log("Set preview image '" + Path.GetFileName(text4) + "'.");
			}
		}
		else
		{
			this.Log("Warning: no Thumbnail.png/.jpg/.jpeg in the folder; submitting without a preview.");
		}
		this.lastSubmitDetails = this.BuildSubmitDetails(appID, publishedFileId, text, num2, num3, text3, flag2, flag3, flag4, flag5, flag6, flag, text4);
		SteamAPICall_t steamAPICall_t = SteamUGC.SubmitItemUpdate(this.updateHandle, this.pendingChangeNote ?? "Updated");
		this.submitItemResult.Set(steamAPICall_t, null);
		this.Log("SubmitItemUpdate started.");
	}

	// Token: 0x06003261 RID: 12897 RVA: 0x000F1978 File Offset: 0x000EFB78
	private void OnSubmitItem(SubmitItemUpdateResult_t result, bool ioFailure)
	{
		this.updateHandle = UGCUpdateHandle_t.Invalid;
		if (ioFailure || result.m_eResult != EResult.k_EResultOK)
		{
			ulong num = result.m_nPublishedFileId.m_PublishedFileId;
			if (num == 0UL && this.pendingRecord != null)
			{
				num = this.pendingRecord.publishedFileId;
			}
			if (!ioFailure && result.m_eResult == EResult.k_EResultInvalidParam && this.retryWithoutTagsOnInvalidParam && !this.omitTags && this.pendingRecord != null)
			{
				this.retryWithoutTagsOnInvalidParam = false;
				this.omitTags = true;
				this.Log(this.DescribeResult(false, result.m_eResult, num, result.m_bUserNeedsToAcceptWorkshopLegalAgreement));
				this.Log("Retrying the same upload without workshop tags on the next frame. InvalidParam often means the tag is not listed in Steamworks App Admin > Workshop, or a required tag group is missing.");
				this.queuedSubmitId = new PublishedFileId_t(this.pendingRecord.publishedFileId);
				this.queuedSubmit = true;
				return;
			}
			this.Fail("SubmitItemUpdate failed: " + this.DescribeResult(ioFailure, result.m_eResult, num, result.m_bUserNeedsToAcceptWorkshopLegalAgreement));
			if (!ioFailure && result.m_eResult == EResult.k_EResultInvalidParam)
			{
				this.Log("Download will keep failing until this upload succeeds. Check Steamworks App Admin > Workshop: user tags must include this exact tag (and any Required tag group must be filled). File transfers can be on and Steam still returns InvalidParam for a bad/required tag. Re-upload from Shift+F11 after that, then subscribe again.");
			}
			return;
		}
		else
		{
			if (result.m_bUserNeedsToAcceptWorkshopLegalAgreement)
			{
				this.OpenLegalAgreement(result.m_nPublishedFileId);
			}
			this.Log(string.Format("Upload complete. Item {0}", result.m_nPublishedFileId));
			SteamWorkshopCreatorService.OpenItemPage(result.m_nPublishedFileId);
			SteamWorkshopInstalledItems.PendingRescan = true;
			this.FinishBusy();
			Action itemsChanged = this.ItemsChanged;
			if (itemsChanged == null)
			{
				return;
			}
			itemsChanged();
			return;
		}
	}

	// Token: 0x06003262 RID: 12898 RVA: 0x000F1ACC File Offset: 0x000EFCCC
	private void OnDeleteItem(DeleteItemResult_t result, bool ioFailure)
	{
		if (ioFailure || (result.m_eResult != EResult.k_EResultOK && result.m_eResult != EResult.k_EResultFileNotFound))
		{
			this.Fail("DeleteItem failed: " + this.DescribeResult(ioFailure, result.m_eResult, result.m_nPublishedFileId.m_PublishedFileId, false));
			return;
		}
		this.RemoveLocal(this.pendingRecord);
		this.Log("Deleted on Steam and removed from the local creator list. Linked folder files were left intact.");
		this.FinishBusy();
		Action itemsChanged = this.ItemsChanged;
		if (itemsChanged == null)
		{
			return;
		}
		itemsChanged();
	}

	// Token: 0x06003263 RID: 12899 RVA: 0x000F1B46 File Offset: 0x000EFD46
	private void OpenLegalAgreement(PublishedFileId_t id)
	{
		this.Log("Steam Workshop legal agreement must be accepted. Opening overlay...");
		SteamWorkshopCreatorService.OpenItemPage(id);
	}

	// Token: 0x06003264 RID: 12900 RVA: 0x000F1B59 File Offset: 0x000EFD59
	private static void OpenItemPage(PublishedFileId_t id)
	{
		SteamFriends.ActivateGameOverlayToWebPage(string.Format("steam://url/CommunityFilePage/{0}", id), EActivateGameOverlayToWebPageMode.k_EActivateGameOverlayToWebPageMode_Default);
	}

	// Token: 0x06003265 RID: 12901 RVA: 0x000F1B74 File Offset: 0x000EFD74
	private static string DescribeStatus(EItemUpdateStatus status)
	{
		string text;
		switch (status)
		{
		case EItemUpdateStatus.k_EItemUpdateStatusPreparingConfig:
			text = "Preparing config";
			break;
		case EItemUpdateStatus.k_EItemUpdateStatusPreparingContent:
			text = "Preparing content";
			break;
		case EItemUpdateStatus.k_EItemUpdateStatusUploadingContent:
			text = "Uploading content";
			break;
		case EItemUpdateStatus.k_EItemUpdateStatusUploadingPreviewFile:
			text = "Uploading preview";
			break;
		case EItemUpdateStatus.k_EItemUpdateStatusCommittingChanges:
			text = "Committing changes";
			break;
		default:
			text = status.ToString();
			break;
		}
		return text;
	}

	// Token: 0x06003266 RID: 12902 RVA: 0x000F1BD8 File Offset: 0x000EFDD8
	private string DescribeResult(bool ioFailure, EResult result, ulong publishedFileId, bool needsLegal)
	{
		if (ioFailure)
		{
			return "IO failure talking to Steam.";
		}
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(result);
		stringBuilder.Append(". ");
		stringBuilder.Append(SteamWorkshopCreatorService.ExplainResult(result));
		if (publishedFileId != 0UL)
		{
			stringBuilder.Append(" publishedFileId=").Append(publishedFileId);
		}
		if (needsLegal)
		{
			stringBuilder.Append(" legalAgreementRequired=true");
		}
		if (SteamManager.Initialized)
		{
			stringBuilder.Append(" steamUser=").Append(SteamUser.GetSteamID());
			stringBuilder.Append(" persona=").Append(SteamFriends.GetPersonaName());
		}
		return stringBuilder.ToString();
	}

	// Token: 0x06003267 RID: 12903 RVA: 0x000F1C80 File Offset: 0x000EFE80
	private static string ExplainResult(EResult result)
	{
		if (result <= EResult.k_EResultTimeout)
		{
			if (result == EResult.k_EResultFail)
			{
				return "Generic Steam failure. For DownloadItem this usually means the workshop item has no committed files.";
			}
			switch (result)
			{
			case EResult.k_EResultInvalidParam:
				return "A parameter Steam rejected at submit time: a workshop tag that is not in the Steamworks tag list, a missing required tag group, empty/invalid title or description, a content folder Steam cannot use, or SubmitItemUpdate called from inside another Steam callback.";
			case EResult.k_EResultFileNotFound:
				return "Steam could not read the content folder or item metadata.";
			case EResult.k_EResultDuplicateName:
				return "Duplicate name.";
			case EResult.k_EResultAccessDenied:
				return "This Steam account does not own a license for this App ID.";
			case EResult.k_EResultTimeout:
				return "Steam timed out.";
			}
		}
		else
		{
			if (result == EResult.k_EResultLimitExceeded)
			{
				return "Preview/content too large, or Steam Cloud quota exceeded.";
			}
			if (result == EResult.k_EResultLockingFailed)
			{
				return "Failed to acquire the UGC lock. Try again.";
			}
		}
		return result.ToString();
	}

	// Token: 0x06003268 RID: 12904 RVA: 0x000F1D20 File Offset: 0x000EFF20
	private void LogAppId(AppId_t appId)
	{
		if (appId.m_AppId == 480U)
		{
			this.Log("Warning: Steam App ID is 480 (Spacewar). Check steam_appid.txt next to the executable / project root.");
		}
		string text = Path.Combine(Directory.GetCurrentDirectory(), "steam_appid.txt");
		if (File.Exists(text))
		{
			this.Log(string.Concat(new string[]
			{
				"steam_appid.txt at ",
				text,
				": '",
				File.ReadAllText(text).Trim(),
				"'"
			}));
			return;
		}
		this.Log("steam_appid.txt not found at " + text + ".");
	}

	// Token: 0x06003269 RID: 12905 RVA: 0x000F1DB0 File Offset: 0x000EFFB0
	private string BuildSubmitDetails(AppId_t appId, PublishedFileId_t publishedFileId, string folder, int fileCount, long totalBytes, string fileSample, bool titleOk, bool descOk, bool tagsOk, bool visOk, bool contentOk, bool previewOk, string extra)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("Submit payload: appId=").Append(appId.m_AppId);
		stringBuilder.Append(" publishedFileId=").Append(publishedFileId.m_PublishedFileId);
		StringBuilder stringBuilder2 = stringBuilder.Append(" title='");
		SteamWorkshopItemRecord steamWorkshopItemRecord = this.pendingRecord;
		StringBuilder stringBuilder3 = stringBuilder2.Append((steamWorkshopItemRecord != null) ? steamWorkshopItemRecord.title : null).Append("' len=");
		SteamWorkshopItemRecord steamWorkshopItemRecord2 = this.pendingRecord;
		int? num;
		if (steamWorkshopItemRecord2 == null)
		{
			num = null;
		}
		else
		{
			string title = steamWorkshopItemRecord2.title;
			num = ((title != null) ? new int?(title.Length) : null);
		}
		int? num2 = num;
		stringBuilder3.Append(num2.GetValueOrDefault());
		StringBuilder stringBuilder4 = stringBuilder.Append(" tag='");
		SteamWorkshopItemRecord steamWorkshopItemRecord3 = this.pendingRecord;
		stringBuilder4.Append((steamWorkshopItemRecord3 != null) ? steamWorkshopItemRecord3.tag : null).Append("' omitTags=").Append(this.omitTags);
		stringBuilder.Append(" visibility=Private");
		stringBuilder.Append(" changeNote='").Append(this.pendingChangeNote).Append("'");
		stringBuilder.Append(" folder='").Append(folder).Append("'");
		stringBuilder.Append(" files=").Append(fileCount);
		stringBuilder.Append(" bytes=").Append(totalBytes);
		stringBuilder.Append(" SetItemTitle=").Append(titleOk);
		stringBuilder.Append(" SetItemDescription=").Append(descOk);
		stringBuilder.Append(" SetItemTags=").Append(tagsOk);
		stringBuilder.Append(" SetItemVisibility=").Append(visOk);
		stringBuilder.Append(" SetItemContent=").Append(contentOk);
		stringBuilder.Append(" SetItemPreview=").Append(previewOk);
		if (!string.IsNullOrEmpty(fileSample))
		{
			stringBuilder.Append(" sample=[").Append(fileSample).Append(']');
		}
		if (!string.IsNullOrEmpty(extra))
		{
			stringBuilder.Append(" extra=").Append(extra);
		}
		stringBuilder.Append(" Check: Steamworks App Admin > Workshop tags must include this exact tag; Workshop > General must allow ISteamUGC file transfers.");
		return stringBuilder.ToString();
	}

	// Token: 0x0600326A RID: 12906 RVA: 0x000F1FC8 File Offset: 0x000F01C8
	private static bool TryNormalizeFolder(string localFolder, out string folder, out string error)
	{
		folder = null;
		error = null;
		if (string.IsNullOrWhiteSpace(localFolder))
		{
			error = "Folder path is empty.";
			return false;
		}
		try
		{
			folder = Path.GetFullPath(localFolder.Trim());
		}
		catch (Exception ex)
		{
			error = string.Concat(new string[] { "Folder path is invalid: ", localFolder, " (", ex.Message, ")" });
			return false;
		}
		if (!Directory.Exists(folder))
		{
			error = "Folder does not exist: " + folder;
			return false;
		}
		string[] files;
		try
		{
			files = Directory.GetFiles(folder, "*", SearchOption.AllDirectories);
		}
		catch (Exception ex2)
		{
			error = "Cannot read folder " + folder + ": " + ex2.Message;
			return false;
		}
		if (files.Length == 0)
		{
			error = "Content folder has no files: " + folder;
			return false;
		}
		if (!SteamWorkshopCreatorService.HasUploadableFiles(folder))
		{
			error = "No files to upload in " + folder + ". Names starting with '_' are skipped.";
			return false;
		}
		return true;
	}

	// Token: 0x0600326B RID: 12907 RVA: 0x000F20CC File Offset: 0x000F02CC
	private static bool HasUploadableFiles(string folder)
	{
		try
		{
			string[] files = Directory.GetFiles(folder, "*", SearchOption.AllDirectories);
			for (int i = 0; i < files.Length; i++)
			{
				if (!SteamWorkshopCreatorService.ShouldSkipUploadFile(files[i]))
				{
					return true;
				}
			}
		}
		catch
		{
		}
		return false;
	}

	// Token: 0x0600326C RID: 12908 RVA: 0x000F211C File Offset: 0x000F031C
	private static bool ShouldSkipUploadFile(string path)
	{
		string fileName = Path.GetFileName(path);
		return string.IsNullOrEmpty(fileName) || fileName[0] == '_' || (fileName.Equals("Thumbnail.png", StringComparison.OrdinalIgnoreCase) || fileName.Equals("Thumbnail.jpg", StringComparison.OrdinalIgnoreCase) || fileName.Equals("Thumbnail.jpeg", StringComparison.OrdinalIgnoreCase));
	}

	// Token: 0x0600326D RID: 12909 RVA: 0x000F2174 File Offset: 0x000F0374
	private bool TryCreateUploadStaging(string sourceFolder, out string stagedFolder, out int skippedCount, out string error)
	{
		stagedFolder = null;
		skippedCount = 0;
		error = null;
		this.CleanupStaging();
		string text = Path.Combine(Application.persistentDataPath, "SteamWorkshop", "UploadStaging", Guid.NewGuid().ToString("N"));
		int num = 0;
		int num2 = 0;
		try
		{
			SteamWorkshopCreatorService.CopyUploadTree(sourceFolder, text, ref num, ref num2);
		}
		catch (Exception ex)
		{
			SteamWorkshopCreatorService.TryDeleteDirectory(text);
			error = "Failed to stage upload files: " + ex.Message;
			return false;
		}
		if (num == 0)
		{
			SteamWorkshopCreatorService.TryDeleteDirectory(text);
			error = "No files to upload after skipping names that start with '_'.";
			return false;
		}
		this.stagingFolder = text;
		stagedFolder = text;
		skippedCount = num2;
		return true;
	}

	// Token: 0x0600326E RID: 12910 RVA: 0x000F2220 File Offset: 0x000F0420
	private static void CopyUploadTree(string source, string dest, ref int copied, ref int skipped)
	{
		Directory.CreateDirectory(dest);
		string[] files = Directory.GetFiles(source);
		for (int i = 0; i < files.Length; i++)
		{
			if (SteamWorkshopCreatorService.ShouldSkipUploadFile(files[i]))
			{
				skipped++;
			}
			else
			{
				File.Copy(files[i], Path.Combine(dest, Path.GetFileName(files[i])), true);
				copied++;
			}
		}
		string[] directories = Directory.GetDirectories(source);
		for (int j = 0; j < directories.Length; j++)
		{
			string fileName = Path.GetFileName(directories[j]);
			if (string.IsNullOrEmpty(fileName) || fileName[0] == '_' || fileName[0] == '~')
			{
				skipped++;
			}
			else
			{
				SteamWorkshopCreatorService.CopyUploadTree(directories[j], Path.Combine(dest, fileName), ref copied, ref skipped);
			}
		}
	}

	// Token: 0x0600326F RID: 12911 RVA: 0x000F22D2 File Offset: 0x000F04D2
	private void CleanupStaging()
	{
		if (string.IsNullOrEmpty(this.stagingFolder))
		{
			return;
		}
		SteamWorkshopCreatorService.TryDeleteDirectory(this.stagingFolder);
		this.stagingFolder = null;
	}

	// Token: 0x06003270 RID: 12912 RVA: 0x000F22F4 File Offset: 0x000F04F4
	private static void TryDeleteDirectory(string path)
	{
		if (string.IsNullOrEmpty(path) || !Directory.Exists(path))
		{
			return;
		}
		try
		{
			Directory.Delete(path, true);
		}
		catch
		{
		}
	}

	// Token: 0x06003271 RID: 12913 RVA: 0x000F2330 File Offset: 0x000F0530
	private static void DescribeFolder(string folder, out int fileCount, out long totalBytes, out string fileSample)
	{
		fileCount = 0;
		totalBytes = 0L;
		fileSample = "";
		if (string.IsNullOrEmpty(folder) || !Directory.Exists(folder))
		{
			return;
		}
		StringBuilder stringBuilder = new StringBuilder();
		try
		{
			string[] files = Directory.GetFiles(folder, "*", SearchOption.AllDirectories);
			fileCount = files.Length;
			for (int i = 0; i < files.Length; i++)
			{
				try
				{
					totalBytes += new FileInfo(files[i]).Length;
				}
				catch
				{
				}
				if (i < 8)
				{
					if (stringBuilder.Length > 0)
					{
						stringBuilder.Append(", ");
					}
					stringBuilder.Append(Path.GetRelativePath(folder, files[i]));
				}
			}
			if (files.Length > 8)
			{
				stringBuilder.Append(", ...");
			}
		}
		catch (Exception ex)
		{
			stringBuilder.Append("error: ").Append(ex.Message);
		}
		fileSample = stringBuilder.ToString();
	}

	// Token: 0x06003272 RID: 12914 RVA: 0x000F2418 File Offset: 0x000F0618
	public string EnsureThumbnail(string folder)
	{
		string text = SteamWorkshopCreatorService.FindThumbnailPath(folder);
		if (!string.IsNullOrEmpty(text))
		{
			return text;
		}
		string text2 = Path.Combine(folder, "Thumbnail.png");
		string text3;
		try
		{
			Texture2D texture2D = new Texture2D(256, 256, TextureFormat.RGB24, false);
			Color32[] array = new Color32[65536];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = new Color32(0, 0, 0, byte.MaxValue);
			}
			texture2D.SetPixels32(array);
			File.WriteAllBytes(text2, texture2D.EncodeToPNG());
			global::UnityEngine.Object.Destroy(texture2D);
			this.Log("Created empty 256x256 Thumbnail.png in '" + folder + "'.");
			text3 = text2;
		}
		catch (Exception ex)
		{
			this.Log("Failed to create Thumbnail.png in '" + folder + "': " + ex.Message);
			text3 = null;
		}
		return text3;
	}

	// Token: 0x06003273 RID: 12915 RVA: 0x000F24F0 File Offset: 0x000F06F0
	public static string FindThumbnailPath(string folder)
	{
		if (string.IsNullOrEmpty(folder) || !Directory.Exists(folder))
		{
			return null;
		}
		string[] array = new string[] { "Thumbnail.png", "Thumbnail.jpg", "Thumbnail.jpeg" };
		for (int i = 0; i < array.Length; i++)
		{
			string text = Path.Combine(folder, array[i]);
			if (File.Exists(text))
			{
				return text;
			}
		}
		try
		{
			string[] files = Directory.GetFiles(folder, "Thumbnail.*");
			for (int j = 0; j < files.Length; j++)
			{
				string extension = Path.GetExtension(files[j]);
				if (extension.Equals(".png", StringComparison.OrdinalIgnoreCase) || extension.Equals(".jpg", StringComparison.OrdinalIgnoreCase) || extension.Equals(".jpeg", StringComparison.OrdinalIgnoreCase))
				{
					return files[j];
				}
			}
		}
		catch
		{
		}
		return null;
	}

	// Token: 0x06003274 RID: 12916 RVA: 0x000F25C8 File Offset: 0x000F07C8
	private void UpsertLocal(SteamWorkshopItemRecord record)
	{
		for (int i = 0; i < this.items.Count; i++)
		{
			if (this.items[i].publishedFileId == record.publishedFileId && record.publishedFileId != 0UL)
			{
				this.items[i] = record;
				SteamWorkshopCreatorStore.Save(this.items);
				return;
			}
		}
		this.items.Add(record);
		SteamWorkshopCreatorStore.Save(this.items);
	}

	// Token: 0x06003275 RID: 12917 RVA: 0x000F263C File Offset: 0x000F083C
	private void RemoveLocal(SteamWorkshopItemRecord record)
	{
		if (record == null)
		{
			return;
		}
		this.items.RemoveAll((SteamWorkshopItemRecord item) => (record.publishedFileId != 0UL && item.publishedFileId == record.publishedFileId) || (record.publishedFileId == 0UL && item == record));
		SteamWorkshopCreatorStore.Save(this.items);
	}

	// Token: 0x04002867 RID: 10343
	private List<SteamWorkshopItemRecord> items;

	// Token: 0x04002868 RID: 10344
	private SteamWorkshopItemRecord pendingRecord;

	// Token: 0x04002869 RID: 10345
	private string pendingChangeNote;

	// Token: 0x0400286A RID: 10346
	private int lastProgressPercent = -1;

	// Token: 0x0400286B RID: 10347
	private bool omitTags;

	// Token: 0x0400286C RID: 10348
	private bool retryWithoutTagsOnInvalidParam;

	// Token: 0x0400286D RID: 10349
	private bool queuedSubmit;

	// Token: 0x0400286E RID: 10350
	private float busyElapsed;

	// Token: 0x0400286F RID: 10351
	private float lastHeartbeat;

	// Token: 0x04002870 RID: 10352
	private string lastSubmitDetails = "";

	// Token: 0x04002871 RID: 10353
	private string stagingFolder;

	// Token: 0x04002872 RID: 10354
	private EItemUpdateStatus lastProgressStatus;

	// Token: 0x04002873 RID: 10355
	private CallResult<CreateItemResult_t> createItemResult;

	// Token: 0x04002874 RID: 10356
	private CallResult<SubmitItemUpdateResult_t> submitItemResult;

	// Token: 0x04002875 RID: 10357
	private CallResult<DeleteItemResult_t> deleteItemResult;

	// Token: 0x04002876 RID: 10358
	private UGCUpdateHandle_t updateHandle = UGCUpdateHandle_t.Invalid;

	// Token: 0x04002877 RID: 10359
	private PublishedFileId_t queuedSubmitId;
}
