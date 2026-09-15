using System;
using LazyBearTechnology;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000A69 RID: 2665
public class UIStartHostGameWindow : LazyWindow<LazyWidgetDataBase>
{
	// Token: 0x06004844 RID: 18500 RVA: 0x00156A88 File Offset: 0x00154C88
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

	// Token: 0x06004845 RID: 18501 RVA: 0x00156AF0 File Offset: 0x00154CF0
	public void OnStartButtonClicked()
	{
		string text = this.ipAddressInputField.text;
		ushort num = ushort.Parse(this.portInputField.text);
		if (LazyNetwork.NetworkManager.StartHostGame(text, num))
		{
			LazyUI.GetWindow<UIMainMenuWindow>().Close();
		}
		LobbyHelper.Host_Init();
		LazyUI.GetWindow<UILobbyWindow>().Open(new UILobbyWidgetData
		{
			isNewGame = true
		});
		this.Close();
	}

	// Token: 0x06004846 RID: 18502 RVA: 0x0014FE32 File Offset: 0x0014E032
	public override void Close()
	{
		base.Close();
		LazyUI.GetWindow<UIMainMenuWindow>().Open(null);
	}

	// Token: 0x06004847 RID: 18503 RVA: 0x00002318 File Offset: 0x00000518
	protected override void TestDraw()
	{
	}

	// Token: 0x04003865 RID: 14437
	[SerializeField]
	private LazyButton autoFillIpBtn;

	// Token: 0x04003866 RID: 14438
	[SerializeField]
	private TMP_InputField ipAddressInputField;

	// Token: 0x04003867 RID: 14439
	[SerializeField]
	private TMP_InputField portInputField;

	// Token: 0x04003868 RID: 14440
	[SerializeField]
	private LazyButton startButton;
}
