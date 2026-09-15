using System;
using System.Collections.Generic;
using LazyBearTechnology;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020009F8 RID: 2552
public class UILobbyWindow : LazyWindow<UILobbyWidgetData>
{
	// Token: 0x060044CB RID: 17611 RVA: 0x00146002 File Offset: 0x00144202
	private void Awake()
	{
		this.startBtn.onClick.AddListener(new UnityAction(this.OnStartBtnClicked));
		this.syncBtn.onClick.AddListener(delegate
		{
			this.startBtn.interactable = false;
			LobbyHelper.Host_SyncGameSaves();
			foreach (KeyValuePair<ulong, UIUserInfo> keyValuePair in this.userInfoObjects)
			{
				if (keyValuePair.Key != LazyNetwork.NetworkManager.ServerClientId)
				{
					keyValuePair.Value.Deactivate();
				}
			}
		});
	}

	// Token: 0x060044CC RID: 17612 RVA: 0x0014603C File Offset: 0x0014423C
	public override void Open(UILobbyWidgetData data)
	{
		base.Open(data);
		this.isHost = LazyNetwork.NetworkManager.IsHost;
		this.syncBtn.gameObject.SetActive(this.isHost);
		this.startBtn.interactable = false;
		this.startBtnLabel.text = (this.isHost ? "Wait For Sync" : "Wait For Host");
		if (!this.isHost)
		{
			return;
		}
		if (data.isNewGame)
		{
			ulong clientId = NetworkManager.Singleton.LocalClient.ClientId;
			this.AddUserInfo(clientId);
			this.userInfoObjects[clientId].Activate();
		}
		LobbyHelper.OnAllClientsSynced = (Action)Delegate.Combine(LobbyHelper.OnAllClientsSynced, new Action(this.AllowToStartGame));
		LobbyHelper.OnClientAdded = (Action<ulong>)Delegate.Combine(LobbyHelper.OnClientAdded, new Action<ulong>(this.AddUserInfo));
		LobbyHelper.OnClientSynced = (Action<ulong>)Delegate.Combine(LobbyHelper.OnClientSynced, new Action<ulong>(this.UpdateUserInfo));
	}

	// Token: 0x060044CD RID: 17613 RVA: 0x0014613A File Offset: 0x0014433A
	private void AllowToStartGame()
	{
		this.startBtn.interactable = true;
		this.startBtnLabel.text = "Start Game";
	}

	// Token: 0x060044CE RID: 17614 RVA: 0x00146158 File Offset: 0x00144358
	private void UpdateUserInfo(ulong id)
	{
		this.userInfoObjects[id].Activate();
	}

	// Token: 0x060044CF RID: 17615 RVA: 0x0014616C File Offset: 0x0014436C
	private void AddUserInfo(ulong id)
	{
		UIUserInfo uiuserInfo = this.userInfoPrefab.Copy(null, true, "");
		uiuserInfo.Init(id.ToString(), "");
		uiuserInfo.Deactivate();
		this.userInfoObjects.Add(id, uiuserInfo);
	}

	// Token: 0x060044D0 RID: 17616 RVA: 0x001461B1 File Offset: 0x001443B1
	private void OnStartBtnClicked()
	{
		if (this.isHost)
		{
			this.Close();
			LobbyHelper.Host_StartGame();
		}
	}

	// Token: 0x060044D1 RID: 17617 RVA: 0x001461C8 File Offset: 0x001443C8
	public override void Close()
	{
		if (this.isHost)
		{
			foreach (KeyValuePair<ulong, UIUserInfo> keyValuePair in this.userInfoObjects)
			{
				global::UnityEngine.Object.Destroy(keyValuePair.Value.gameObject);
			}
			this.userInfoObjects.Clear();
			LobbyHelper.OnAllClientsSynced = (Action)Delegate.Remove(LobbyHelper.OnAllClientsSynced, new Action(this.AllowToStartGame));
			LobbyHelper.OnClientAdded = (Action<ulong>)Delegate.Remove(LobbyHelper.OnClientAdded, new Action<ulong>(this.AddUserInfo));
			LobbyHelper.OnClientSynced = (Action<ulong>)Delegate.Remove(LobbyHelper.OnClientSynced, new Action<ulong>(this.UpdateUserInfo));
		}
		base.Close();
	}

	// Token: 0x060044D2 RID: 17618 RVA: 0x001462A4 File Offset: 0x001444A4
	protected override void TestDraw()
	{
		this.Open(new UILobbyWidgetData());
	}

	// Token: 0x040035B3 RID: 13747
	[SerializeField]
	private LazyButton syncBtn;

	// Token: 0x040035B4 RID: 13748
	[SerializeField]
	private LazyButton startBtn;

	// Token: 0x040035B5 RID: 13749
	[SerializeField]
	private TextMeshProUGUI startBtnLabel;

	// Token: 0x040035B6 RID: 13750
	[SerializeField]
	private UIUserInfo userInfoPrefab;

	// Token: 0x040035B7 RID: 13751
	private Dictionary<ulong, UIUserInfo> userInfoObjects = new Dictionary<ulong, UIUserInfo>();

	// Token: 0x040035B8 RID: 13752
	private bool isHost;
}
