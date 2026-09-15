using System;
using LazyBearTechnology;
using TMPro;
using UnityEngine;

// Token: 0x0200089F RID: 2207
public class UIMainMenuInfoPanel : MonoBehaviour, ILazyGUIElement
{
	// Token: 0x06003900 RID: 14592 RVA: 0x0011234B File Offset: 0x0011054B
	public void Init()
	{
		this.Draw();
	}

	// Token: 0x06003901 RID: 14593 RVA: 0x00112353 File Offset: 0x00110553
	public void ShowFullPanel()
	{
		base.gameObject.SetActive(true);
		this.SetMenuOnlyLabelsVisible(true);
		this.Draw();
	}

	// Token: 0x06003902 RID: 14594 RVA: 0x00027874 File Offset: 0x00025A74
	public void Hide()
	{
		base.gameObject.SetActive(false);
	}

	// Token: 0x06003903 RID: 14595 RVA: 0x00112370 File Offset: 0x00110570
	public void ShowVersionInGameIfNeeded()
	{
		bool showVersionInGame = LazySingletonSO<GameInfo>.Instance.ShowVersionInGame;
		base.gameObject.SetActive(showVersionInGame);
		this.SetMenuOnlyLabelsVisible(false);
	}

	// Token: 0x06003904 RID: 14596 RVA: 0x0011239B File Offset: 0x0011059B
	private void SetMenuOnlyLabelsVisible(bool isVisible)
	{
		this.developerNameLabel.gameObject.SetActive(isVisible);
		this.publisherNameLabel.gameObject.SetActive(isVisible);
		if (!isVisible)
		{
			this.xboxUserLabel.gameObject.SetActive(false);
		}
	}

	// Token: 0x06003905 RID: 14597 RVA: 0x001123D4 File Offset: 0x001105D4
	private void Draw()
	{
		this.versionLabel.text = LLBase.L("ui_ver") + this.rightSideStyle.ApplyStyleToString(LazySingletonSO<GameInfo>.Instance.Version, true, true);
		this.developerNameLabel.text = "game by: " + this.rightSideStyle.ApplyStyleToString("Lazy Bear Games", true, true);
		this.publisherNameLabel.text = "published by: " + this.rightSideStyle.ApplyStyleToString("tinyBuild", true, true);
		this.xboxUserLabel.gameObject.SetActive(false);
	}

	// Token: 0x04002D50 RID: 11600
	[SerializeField]
	private TextMeshProUGUI versionLabel;

	// Token: 0x04002D51 RID: 11601
	[SerializeField]
	private TextMeshProUGUI developerNameLabel;

	// Token: 0x04002D52 RID: 11602
	[SerializeField]
	private TextMeshProUGUI publisherNameLabel;

	// Token: 0x04002D53 RID: 11603
	[SerializeField]
	private TextMeshProUGUI xboxUserLabel;

	// Token: 0x04002D54 RID: 11604
	[SerializeField]
	private TextStyle rightSideStyle;
}
