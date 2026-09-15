using System;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020007E6 RID: 2022
public class ProgressCellCraft : MonoBehaviour
{
	// Token: 0x170007D5 RID: 2005
	// (get) Token: 0x06003410 RID: 13328 RVA: 0x000FB0FA File Offset: 0x000F92FA
	public GameObject PlusOneObject
	{
		get
		{
			return this.plusOneObject;
		}
	}

	// Token: 0x06003411 RID: 13329 RVA: 0x000FB104 File Offset: 0x000F9304
	public void Show(int currentIndex, int collectionCount, bool isEmpty, bool isFailed = false, int quality = -1, int autopsyQuality = -1, int gardenQuality = -1)
	{
		this.commonVar.gameObject.SetActive(false);
		this.starVar.gameObject.SetActive(false);
		this.autopsyVar.gameObject.SetActive(false);
		this.plusOneObject.SetActive(false);
		if (gardenQuality == 0)
		{
			this.plusOneObject.SetActive(true);
			this.autopsyFailIcon.gameObject.SetActive(false);
			this.autopsySuccess.gameObject.SetActive(false);
			this.currentVar = this.starVar;
		}
		else if (gardenQuality > 0)
		{
			this.starIcon.sprite = LazySingletonSO<EasySpritesCollection>.Instance.GetSprite("item_star_" + gardenQuality.ToString(), null);
			this.starIcon.gameObject.SetActive(true);
			this.autopsyFailIcon.gameObject.SetActive(false);
			this.autopsySuccess.gameObject.SetActive(false);
			this.currentVar = this.starVar;
		}
		else if (quality > 0)
		{
			this.starIcon.sprite = LazySingletonSO<EasySpritesCollection>.Instance.GetSprite("item_star_" + quality.ToString(), null);
			this.starIcon.gameObject.SetActive(true);
			this.autopsyFailIcon.gameObject.SetActive(false);
			this.autopsySuccess.gameObject.SetActive(false);
			this.currentVar = this.starVar;
		}
		else if (autopsyQuality > 0)
		{
			if (autopsyQuality == 1)
			{
				this.autopsyFailIcon.gameObject.SetActive(true);
				this.autopsySuccess.gameObject.SetActive(false);
			}
			else
			{
				this.autopsyFailIcon.gameObject.SetActive(false);
				this.autopsySuccess.gameObject.SetActive(true);
			}
			this.starIcon.gameObject.SetActive(false);
			this.currentVar = this.autopsyVar;
		}
		else
		{
			this.autopsyFailIcon.gameObject.SetActive(false);
			this.autopsySuccess.gameObject.SetActive(false);
			this.starIcon.gameObject.SetActive(false);
			this.currentVar = this.commonVar;
		}
		this.currentVar.gameObject.SetActive(true);
		this.currentVar.back.localScale = new Vector3(1f, 1f, 1f);
		this.currentVar.success.gameObject.SetActive(!isEmpty && !isFailed);
		this.currentVar.failed.gameObject.SetActive(!isEmpty && isFailed);
		((RectTransform)this.currentVar.transform).RefreshContentFitter();
	}

	// Token: 0x06003412 RID: 13330 RVA: 0x00027874 File Offset: 0x00025A74
	public void Hide()
	{
		base.gameObject.SetActive(false);
	}

	// Token: 0x04002988 RID: 10632
	[SerializeField]
	private ProgressCellVisualVariation commonVar;

	// Token: 0x04002989 RID: 10633
	[SerializeField]
	private ProgressCellVisualVariation starVar;

	// Token: 0x0400298A RID: 10634
	[SerializeField]
	private ProgressCellVisualVariation autopsyVar;

	// Token: 0x0400298B RID: 10635
	[SerializeField]
	private Image starIcon;

	// Token: 0x0400298C RID: 10636
	[SerializeField]
	private Image autopsyFailIcon;

	// Token: 0x0400298D RID: 10637
	[SerializeField]
	private Image autopsySuccess;

	// Token: 0x0400298E RID: 10638
	[SerializeField]
	private GameObject plusOneObject;

	// Token: 0x0400298F RID: 10639
	private ProgressCellVisualVariation currentVar;
}
