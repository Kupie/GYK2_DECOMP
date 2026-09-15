using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x020009C7 RID: 2503
public class UIFightingSquadHudGroupWidget : LazyWidget<UIFightingSquadHudGroupWidgetData>
{
	// Token: 0x0600429D RID: 17053 RVA: 0x0013C4FF File Offset: 0x0013A6FF
	public override void Hide()
	{
		base.Hide();
		this.Clear();
	}

	// Token: 0x0600429E RID: 17054 RVA: 0x0013C510 File Offset: 0x0013A710
	public override void Redraw()
	{
		base.Redraw();
		this.Clear();
		UIFightingSquadHudGroupWidgetData data = this.data;
		if (((data != null) ? data.FightingLevel : null) == null)
		{
			return;
		}
		Transform transform = ((this.widgetsParent != null) ? this.widgetsParent : base.transform);
		List<AlliesSpawn> alliesSpawns = this.data.FightingLevel.AlliesSpawns;
		List<AlliesSpawn> list = new List<AlliesSpawn>();
		for (int i = 0; i < alliesSpawns.Count; i++)
		{
			if (alliesSpawns[i] != null && alliesSpawns[i].Fighters.Count > 0)
			{
				list.Add(alliesSpawns[i]);
			}
		}
		for (int j = 0; j < list.Count; j++)
		{
			if (j > 0)
			{
				UISquadHudSeparator elementFromPool = UIPrefabsPooler.Instance.GetElementFromPool<UISquadHudSeparator>(transform);
				this.drawnSeparators.Add(elementFromPool);
				elementFromPool.transform.SetAsLastSibling();
			}
			UISquadHudWidget elementFromPool2 = UIPrefabsPooler.Instance.GetElementFromPool<UISquadHudWidget>(transform);
			this.drawnSquads.Add(elementFromPool2);
			elementFromPool2.Draw(new UISquadHudWidgetData(list[j], this.GetBanner(list[j].SquadSlotIndex)));
			elementFromPool2.transform.SetAsLastSibling();
		}
	}

	// Token: 0x0600429F RID: 17055 RVA: 0x0013C649 File Offset: 0x0013A849
	private Sprite GetBanner(int index)
	{
		if (this.banners == null || index < 0 || index >= this.banners.Length)
		{
			return null;
		}
		return this.banners[index];
	}

	// Token: 0x060042A0 RID: 17056 RVA: 0x0013C66C File Offset: 0x0013A86C
	private void Clear()
	{
		foreach (UISquadHudWidget uisquadHudWidget in this.drawnSquads)
		{
			uisquadHudWidget.Hide();
			UIPrefabsPooler.Instance.ReleaseElementToPool<UISquadHudWidget>(uisquadHudWidget);
		}
		this.drawnSquads.Clear();
		foreach (UISquadHudSeparator uisquadHudSeparator in this.drawnSeparators)
		{
			UIPrefabsPooler.Instance.ReleaseElementToPool<UISquadHudSeparator>(uisquadHudSeparator);
		}
		this.drawnSeparators.Clear();
	}

	// Token: 0x060042A1 RID: 17057 RVA: 0x00002318 File Offset: 0x00000518
	protected override void TestDraw()
	{
	}

	// Token: 0x04003401 RID: 13313
	[SerializeField]
	private Sprite[] banners;

	// Token: 0x04003402 RID: 13314
	[SerializeField]
	private RectTransform widgetsParent;

	// Token: 0x04003403 RID: 13315
	private List<UISquadHudWidget> drawnSquads = new List<UISquadHudWidget>();

	// Token: 0x04003404 RID: 13316
	private List<UISquadHudSeparator> drawnSeparators = new List<UISquadHudSeparator>();
}
