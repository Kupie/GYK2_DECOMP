using System;
using LazyBearTechnology;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000A4A RID: 2634
public class UIConnectToHostGameWindow : LazyWindow<LazyWidgetDataBase>
{
	// Token: 0x06004703 RID: 18179 RVA: 0x0014FD6C File Offset: 0x0014DF6C
	public override void Init()
	{
		this.startButton.onClick.AddListener(new UnityAction(this.OnStartButtonClicked));
		this.closeButton.onClick.AddListener(new UnityAction(this.Close));
		this.autoFillIpBtn.onClick.AddListener(delegate
		{
			this.ipAddressInputField.text = LobbyHelper.GetLocalIpAddress();
		});
		base.Init();
	}

	// Token: 0x06004704 RID: 18180 RVA: 0x0014FDD4 File Offset: 0x0014DFD4
	public void OnStartButtonClicked()
	{
		string text = this.ipAddressInputField.text;
		ushort num = ushort.Parse(this.portInputField.text);
		if (LazyNetwork.NetworkManager.ConnectToHost(text, num))
		{
			LazyUI.GetWindow<UIMainMenuWindow>().Close();
		}
		LazyUI.GetWindow<UILobbyWindow>().Open(new UILobbyWidgetData
		{
			isNewGame = true
		});
		this.Close();
	}

	// Token: 0x06004705 RID: 18181 RVA: 0x0014FE32 File Offset: 0x0014E032
	public override void Close()
	{
		base.Close();
		LazyUI.GetWindow<UIMainMenuWindow>().Open(null);
	}

	// Token: 0x06004706 RID: 18182 RVA: 0x00002318 File Offset: 0x00000518
	protected override void TestDraw()
	{
	}

	// Token: 0x04003761 RID: 14177
	[SerializeField]
	private LazyButton autoFillIpBtn;

	// Token: 0x04003762 RID: 14178
	[SerializeField]
	private TMP_InputField ipAddressInputField;

	// Token: 0x04003763 RID: 14179
	[SerializeField]
	private TMP_InputField portInputField;

	// Token: 0x04003764 RID: 14180
	[SerializeField]
	private LazyButton startButton;
}
