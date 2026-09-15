using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x0200093B RID: 2363
public class PerksWidget : LazyWidget<PerksWidgetData>
{
	// Token: 0x140000BD RID: 189
	// (add) Token: 0x06003E54 RID: 15956 RVA: 0x001296F4 File Offset: 0x001278F4
	// (remove) Token: 0x06003E55 RID: 15957 RVA: 0x0012972C File Offset: 0x0012792C
	public event Action onContentChanged;

	// Token: 0x06003E56 RID: 15958 RVA: 0x00129761 File Offset: 0x00127961
	protected override void SetData(PerksWidgetData data)
	{
		base.SetData(data);
		data.OnPerkAdded += this.AddPerkWidget;
		data.OnPerkRemoved += this.RemovePerkWidget;
		data.OnPerkUpdated += this.UpdatePerk;
	}

	// Token: 0x06003E57 RID: 15959 RVA: 0x001297A0 File Offset: 0x001279A0
	public override void Redraw()
	{
		base.Redraw();
		int count = this.data.Perks.Count;
		for (int i = 0; i < count; i++)
		{
			PerkWidgetFull elementFromPool = UIPrefabsPooler.Instance.GetElementFromPool<PerkWidgetFull>(this.content.transform);
			elementFromPool.transform.SetAsLastSibling();
			PerkWidgetData perkWidgetData = new PerkWidgetData(this.data.Perks[i], true, null, null, null);
			elementFromPool.Draw(perkWidgetData);
			this.displayedPerks.Add(elementFromPool);
			if (i < count - 1)
			{
				PerksWidgetSeparator elementFromPool2 = UIPrefabsPooler.Instance.GetElementFromPool<PerksWidgetSeparator>(this.content.transform);
				elementFromPool2.gameObject.SetActive(true);
				elementFromPool2.transform.SetAsLastSibling();
				this.displayedSeparators.Add(elementFromPool2);
				elementFromPool.NextItemSeparator = elementFromPool2;
			}
		}
		this.emptyObject.SetActive(count <= 0);
	}

	// Token: 0x06003E58 RID: 15960 RVA: 0x00129884 File Offset: 0x00127A84
	public override void Hide()
	{
		if (this.data != null)
		{
			this.data.OnPerkAdded -= this.AddPerkWidget;
			this.data.OnPerkRemoved -= this.RemovePerkWidget;
			this.data.OnPerkUpdated -= this.UpdatePerk;
		}
		base.Hide();
		foreach (PerkWidgetFull perkWidgetFull in this.displayedPerks)
		{
			perkWidgetFull.NextItemSeparator = null;
			UIPrefabsPooler.Instance.ReleaseElementToPool<PerkWidgetFull>(perkWidgetFull);
		}
		foreach (PerksWidgetSeparator perksWidgetSeparator in this.displayedSeparators)
		{
			UIPrefabsPooler.Instance.ReleaseElementToPool<PerksWidgetSeparator>(perksWidgetSeparator);
		}
		this.displayedPerks.Clear();
		this.displayedSeparators.Clear();
	}

	// Token: 0x06003E59 RID: 15961 RVA: 0x00129994 File Offset: 0x00127B94
	public void DisableGamepadNavigation()
	{
		GamepadNavigationItem[] componentsInChildren = base.GetComponentsInChildren<GamepadNavigationItem>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].Active = false;
		}
	}

	// Token: 0x06003E5A RID: 15962 RVA: 0x001299C0 File Offset: 0x00127BC0
	public void EnableGamepadNavigation()
	{
		GamepadNavigationItem[] componentsInChildren = base.GetComponentsInChildren<GamepadNavigationItem>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].Active = true;
		}
	}

	// Token: 0x06003E5B RID: 15963 RVA: 0x001299EC File Offset: 0x00127BEC
	private void AddPerkWidget(PerkData perkData)
	{
		if (this.displayedPerks.Count > 0)
		{
			PerksWidgetSeparator elementFromPool = UIPrefabsPooler.Instance.GetElementFromPool<PerksWidgetSeparator>(this.content.transform);
			elementFromPool.gameObject.SetActive(true);
			elementFromPool.transform.SetParent(this.content.transform);
			elementFromPool.transform.SetAsLastSibling();
			this.displayedSeparators.Add(elementFromPool);
			List<PerkWidgetFull> list = this.displayedPerks;
			list[list.Count - 1].NextItemSeparator = elementFromPool;
		}
		PerkWidgetFull elementFromPool2 = UIPrefabsPooler.Instance.GetElementFromPool<PerkWidgetFull>(this.content.transform);
		elementFromPool2.transform.SetParent(this.content.transform);
		PerkWidgetData perkWidgetData = new PerkWidgetData(perkData, true, null, null, null);
		elementFromPool2.Draw(perkWidgetData);
		this.displayedPerks.Add(elementFromPool2);
		elementFromPool2.transform.SetAsLastSibling();
		this.emptyObject.SetActive(this.data.Perks.Count <= 0);
		Action action = this.onContentChanged;
		if (action == null)
		{
			return;
		}
		action();
	}

	// Token: 0x06003E5C RID: 15964 RVA: 0x00129AF8 File Offset: 0x00127CF8
	private void RemovePerkWidget(PerkData perkData)
	{
		PerkWidgetFull perkWidgetFull = this.displayedPerks.Find((PerkWidgetFull x) => x.PerkData == perkData);
		if (perkWidgetFull != null)
		{
			if (perkWidgetFull.NextItemSeparator != null)
			{
				this.displayedSeparators.Remove(perkWidgetFull.NextItemSeparator);
				UIPrefabsPooler.Instance.ReleaseElementToPool<PerksWidgetSeparator>(perkWidgetFull.NextItemSeparator);
			}
			UIPrefabsPooler.Instance.ReleaseElementToPool<PerkWidgetFull>(perkWidgetFull);
			this.displayedPerks.Remove(perkWidgetFull);
		}
		this.emptyObject.SetActive(this.data.Perks.Count <= 0);
		Action action = this.onContentChanged;
		if (action == null)
		{
			return;
		}
		action();
	}

	// Token: 0x06003E5D RID: 15965 RVA: 0x00129BAC File Offset: 0x00127DAC
	private void UpdatePerk(PerkData perkData)
	{
		PerkWidgetFull perkWidgetFull = this.displayedPerks.Find((PerkWidgetFull x) => x.PerkData == perkData);
		if (perkWidgetFull == null)
		{
			return;
		}
		perkWidgetFull.Redraw();
	}

	// Token: 0x06003E5E RID: 15966 RVA: 0x00129BE7 File Offset: 0x00127DE7
	[LazyUITest]
	protected override void TestDraw()
	{
		this.Draw(new PerksWidgetData());
	}

	// Token: 0x04003105 RID: 12549
	[SerializeField]
	private RectTransform content;

	// Token: 0x04003106 RID: 12550
	[SerializeField]
	private GameObject emptyObject;

	// Token: 0x04003107 RID: 12551
	private readonly List<PerkWidgetFull> displayedPerks = new List<PerkWidgetFull>();

	// Token: 0x04003108 RID: 12552
	private List<PerksWidgetSeparator> displayedSeparators = new List<PerksWidgetSeparator>();
}
