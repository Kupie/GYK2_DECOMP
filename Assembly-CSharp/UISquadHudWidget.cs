using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020009D2 RID: 2514
public class UISquadHudWidget : LazyWidget<UISquadHudWidgetData>
{
	// Token: 0x0600431B RID: 17179 RVA: 0x0013ECED File Offset: 0x0013CEED
	public override void Hide()
	{
		base.Hide();
		this.Clear();
	}

	// Token: 0x0600431C RID: 17180 RVA: 0x0013ECFC File Offset: 0x0013CEFC
	public override void Redraw()
	{
		base.Redraw();
		this.Clear();
		if (this.data == null)
		{
			return;
		}
		if (this.bannerIcon != null)
		{
			this.bannerIcon.sprite = this.data.Banner;
			this.bannerIcon.gameObject.SetActive(this.data.Banner != null);
		}
		if (this.data.AlliesSpawn == null)
		{
			return;
		}
		Transform transform = ((this.elementsParent != null) ? this.elementsParent : base.transform);
		foreach (WgoData wgoData in this.data.AlliesSpawn.Fighters)
		{
			if (wgoData != null)
			{
				UISquadHudElementWidget elementFromPool = UIPrefabsPooler.Instance.GetElementFromPool<UISquadHudElementWidget>(transform);
				this.drawnElements.Add(elementFromPool);
				elementFromPool.Draw(new UISquadHudElementWidgetData(wgoData));
				elementFromPool.transform.SetAsLastSibling();
			}
		}
	}

	// Token: 0x0600431D RID: 17181 RVA: 0x0013EE0C File Offset: 0x0013D00C
	private void Clear()
	{
		foreach (UISquadHudElementWidget uisquadHudElementWidget in this.drawnElements)
		{
			if (!(uisquadHudElementWidget == null))
			{
				uisquadHudElementWidget.Hide();
				UIPrefabsPooler.Instance.ReleaseElementToPool<UISquadHudElementWidget>(uisquadHudElementWidget);
			}
		}
		this.drawnElements.Clear();
	}

	// Token: 0x0600431E RID: 17182 RVA: 0x00002318 File Offset: 0x00000518
	protected override void TestDraw()
	{
	}

	// Token: 0x04003457 RID: 13399
	[SerializeField]
	private Image bannerIcon;

	// Token: 0x04003458 RID: 13400
	[SerializeField]
	private RectTransform elementsParent;

	// Token: 0x04003459 RID: 13401
	private List<UISquadHudElementWidget> drawnElements = new List<UISquadHudElementWidget>();
}
