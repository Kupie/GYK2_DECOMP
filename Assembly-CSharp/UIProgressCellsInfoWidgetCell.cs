using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200099C RID: 2460
public class UIProgressCellsInfoWidgetCell : MonoBehaviour
{
	// Token: 0x060041BE RID: 16830 RVA: 0x00139628 File Offset: 0x00137828
	public void Show(bool isEmpty, bool isChance, float fillValue = 1f)
	{
		this.commonVarGreen.gameObject.SetActive(false);
		this.commonVarEmpty.gameObject.SetActive(false);
		this.chanceVar.gameObject.SetActive(false);
		this.percentVar.gameObject.SetActive(false);
		if (isEmpty)
		{
			this.currentVar = this.commonVarEmpty;
		}
		else if (isChance)
		{
			this.currentVar = this.chanceVar;
		}
		else if (fillValue >= 1f)
		{
			this.currentVar = this.commonVarGreen;
		}
		else
		{
			this.currentVar = this.percentVar;
			this.percentProgressBar.fillAmount = fillValue;
		}
		this.currentVar.gameObject.SetActive(true);
		((RectTransform)this.currentVar.transform).RefreshContentFitter();
	}

	// Token: 0x060041BF RID: 16831 RVA: 0x00027874 File Offset: 0x00025A74
	public void Hide()
	{
		base.gameObject.SetActive(false);
	}

	// Token: 0x0400335A RID: 13146
	[SerializeField]
	private UIProgressCellsInfoWidgetCellVariation commonVarEmpty;

	// Token: 0x0400335B RID: 13147
	[SerializeField]
	private UIProgressCellsInfoWidgetCellVariation commonVarGreen;

	// Token: 0x0400335C RID: 13148
	[SerializeField]
	private UIProgressCellsInfoWidgetCellVariation percentVar;

	// Token: 0x0400335D RID: 13149
	[SerializeField]
	private Image percentProgressBar;

	// Token: 0x0400335E RID: 13150
	[SerializeField]
	private UIProgressCellsInfoWidgetCellVariation chanceVar;

	// Token: 0x0400335F RID: 13151
	private UIProgressCellsInfoWidgetCellVariation currentVar;
}
