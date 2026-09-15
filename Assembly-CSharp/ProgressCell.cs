using System;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020007E4 RID: 2020
public class ProgressCell : MonoBehaviour, IPoolable
{
	// Token: 0x06003404 RID: 13316 RVA: 0x000FAE44 File Offset: 0x000F9044
	public bool Show(int currentIndex, int collectionCount, bool isEmpty, bool isFailed = false, int quality = -1, PerkDef perkDef = null, ItemDef itemDef = null)
	{
		ProgressCell.ProgressCellType progressCellType = this.GetProgressCellType(currentIndex, collectionCount);
		ProgressCellVisualVariation progressCellVisualVariation = this.currentVar;
		bool flag = false;
		this.currentVar = this.GetProgressCellVariation(progressCellType);
		if (progressCellVisualVariation != this.currentVar)
		{
			if (progressCellVisualVariation != null)
			{
				progressCellVisualVariation.gameObject.SetActive(false);
			}
			flag = true;
		}
		if (!this.currentVar.gameObject.activeSelf)
		{
			this.currentVar.gameObject.SetActive(true);
			flag = true;
		}
		this.currentVar.back.localScale = new Vector3(1f, 1f, 1f);
		this.currentVar.success.gameObject.SetActive(!isEmpty && !isFailed);
		this.currentVar.failed.gameObject.SetActive(!isEmpty && isFailed);
		((RectTransform)this.currentVar.transform).RefreshContentFitter();
		this.UpdateStarIcon(quality);
		this.perkDefBonus = perkDef;
		this.itemDefBonus = itemDef;
		return flag;
	}

	// Token: 0x06003405 RID: 13317 RVA: 0x000FAF48 File Offset: 0x000F9148
	public void Hide()
	{
		this.DeactivateAllVariations();
		this.currentVar = null;
		base.gameObject.SetActive(false);
		this.perkDefBonus = null;
		this.itemDefBonus = null;
	}

	// Token: 0x06003406 RID: 13318 RVA: 0x000FAF71 File Offset: 0x000F9171
	public void OnPoolableObjReleased()
	{
		this.Hide();
	}

	// Token: 0x06003407 RID: 13319 RVA: 0x00002318 File Offset: 0x00000518
	public void OnOver()
	{
	}

	// Token: 0x06003408 RID: 13320 RVA: 0x00002318 File Offset: 0x00000518
	public void OnOut()
	{
	}

	// Token: 0x06003409 RID: 13321 RVA: 0x000FAF79 File Offset: 0x000F9179
	public void ShowUITooltip()
	{
		this.isHovered = true;
		UITooltip.ShowProgressTickBonus(this, this.perkDefBonus, this.itemDefBonus);
	}

	// Token: 0x0600340A RID: 13322 RVA: 0x000FAF94 File Offset: 0x000F9194
	public void HideUITooltip(bool immediately = false)
	{
		this.isHovered = false;
		if (immediately)
		{
			UITooltip.HideImmediately();
			return;
		}
		UITooltip.Hide();
	}

	// Token: 0x0600340B RID: 13323 RVA: 0x000FAFAC File Offset: 0x000F91AC
	private void UpdateStarIcon(int quality)
	{
		if (quality > 0)
		{
			this.starIcon.sprite = LazySingletonSO<EasySpritesCollection>.Instance.GetSprite("item_star_" + quality.ToString(), null);
			this.starIcon.gameObject.SetActive(true);
			this.plusOneObject.SetActive(false);
			return;
		}
		if (quality == 0)
		{
			this.starIcon.gameObject.SetActive(false);
			this.plusOneObject.SetActive(true);
			return;
		}
		this.starIcon.gameObject.SetActive(false);
		this.plusOneObject.SetActive(false);
	}

	// Token: 0x0600340C RID: 13324 RVA: 0x000FB040 File Offset: 0x000F9240
	private ProgressCell.ProgressCellType GetProgressCellType(int currentIndex, int collectionCount = 1)
	{
		if (collectionCount == 1)
		{
			return ProgressCell.ProgressCellType.Single;
		}
		if (currentIndex == 0)
		{
			return ProgressCell.ProgressCellType.LeftCorner;
		}
		if (currentIndex == collectionCount - 1)
		{
			return ProgressCell.ProgressCellType.RightCorner;
		}
		return ProgressCell.ProgressCellType.Middle;
	}

	// Token: 0x0600340D RID: 13325 RVA: 0x000FB058 File Offset: 0x000F9258
	private void DeactivateAllVariations()
	{
		this.middleVar.gameObject.SetActive(false);
		this.singleVar.gameObject.SetActive(false);
		this.sideLVar.gameObject.SetActive(false);
		this.sideRVar.gameObject.SetActive(false);
	}

	// Token: 0x0600340E RID: 13326 RVA: 0x000FB0AC File Offset: 0x000F92AC
	private ProgressCellVisualVariation GetProgressCellVariation(ProgressCell.ProgressCellType progressCellType)
	{
		ProgressCellVisualVariation progressCellVisualVariation = null;
		switch (progressCellType)
		{
		case ProgressCell.ProgressCellType.LeftCorner:
			progressCellVisualVariation = this.sideLVar;
			break;
		case ProgressCell.ProgressCellType.RightCorner:
			progressCellVisualVariation = this.sideRVar;
			break;
		case ProgressCell.ProgressCellType.Middle:
			progressCellVisualVariation = this.middleVar;
			break;
		case ProgressCell.ProgressCellType.Single:
			progressCellVisualVariation = this.singleVar;
			break;
		}
		return progressCellVisualVariation;
	}

	// Token: 0x04002979 RID: 10617
	[SerializeField]
	private ProgressCellVisualVariation middleVar;

	// Token: 0x0400297A RID: 10618
	[SerializeField]
	private ProgressCellVisualVariation singleVar;

	// Token: 0x0400297B RID: 10619
	[SerializeField]
	private ProgressCellVisualVariation sideLVar;

	// Token: 0x0400297C RID: 10620
	[SerializeField]
	private ProgressCellVisualVariation sideRVar;

	// Token: 0x0400297D RID: 10621
	[SerializeField]
	private Image starIcon;

	// Token: 0x0400297E RID: 10622
	[SerializeField]
	private GameObject plusOneObject;

	// Token: 0x0400297F RID: 10623
	private ProgressCellVisualVariation currentVar;

	// Token: 0x04002980 RID: 10624
	private bool isHovered;

	// Token: 0x04002981 RID: 10625
	private PerkDef perkDefBonus;

	// Token: 0x04002982 RID: 10626
	private ItemDef itemDefBonus;

	// Token: 0x020007E5 RID: 2021
	private enum ProgressCellType
	{
		// Token: 0x04002984 RID: 10628
		LeftCorner,
		// Token: 0x04002985 RID: 10629
		RightCorner,
		// Token: 0x04002986 RID: 10630
		Middle = 3,
		// Token: 0x04002987 RID: 10631
		Single
	}
}
