using System;
using System.Collections.Generic;
using LazyBearTechnology;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200099B RID: 2459
public class UIProgressCellsInfoWidget : LazyWidget<UIProgressCellsInfoWidgetData>
{
	// Token: 0x060041B0 RID: 16816 RVA: 0x00139108 File Offset: 0x00137308
	public override void Init()
	{
		base.Init();
		for (int i = 0; i < ConstDef.Get("max_cells_per_one_hit").IntValue; i++)
		{
			this.backgroundCellImages.Add(global::UnityEngine.Object.Instantiate<GameObject>(this.backgroundCellImagePrefab, this.backGroupParent));
		}
		this.backgroundCellImagePrefab.gameObject.SetActive(false);
	}

	// Token: 0x060041B1 RID: 16817 RVA: 0x00139162 File Offset: 0x00137362
	public override void Redraw()
	{
		base.Redraw();
		this.DrawCells();
		this.AttachTickTooltip();
	}

	// Token: 0x060041B2 RID: 16818 RVA: 0x00139178 File Offset: 0x00137378
	public override void Hide()
	{
		base.Hide();
		foreach (GameObject gameObject in this.backgroundCellImages)
		{
			gameObject.gameObject.SetActive(false);
		}
		this.HideCells();
	}

	// Token: 0x060041B3 RID: 16819 RVA: 0x001391DC File Offset: 0x001373DC
	private void HideCells()
	{
		foreach (UIProgressCellsInfoWidgetCell uiprogressCellsInfoWidgetCell in this.progressCells)
		{
			UIPrefabsPooler.Instance.ReleaseElementToPool<UIProgressCellsInfoWidgetCell>(uiprogressCellsInfoWidgetCell);
		}
		this.progressCells.Clear();
	}

	// Token: 0x060041B4 RID: 16820 RVA: 0x00139240 File Offset: 0x00137440
	private void DrawCells()
	{
		int masteryValue = this.data.MasteryValue;
		int masteryLock = this.data.MasteryLock;
		float num = 0f;
		this.chanceLabel.gameObject.SetActive(false);
		int num2;
		if (!this.data.IsStarCraft)
		{
			if (masteryValue < masteryLock)
			{
				num2 = 0;
			}
			else
			{
				num2 = Math.Clamp(masteryValue / masteryLock, 0, ConstDef.Get("max_cells_per_one_hit").IntValue);
				num = (float)masteryValue % (float)masteryLock / (float)masteryLock;
			}
		}
		else
		{
			if (masteryValue < masteryLock)
			{
				UIProgressCellsInfoWidgetCell elementFromPool = UIPrefabsPooler.Instance.GetElementFromPool<UIProgressCellsInfoWidgetCell>(this.cellsParent);
				elementFromPool.Show(false, true, 1f);
				this.progressCells.Add(elementFromPool);
				this.chanceLabel.text = string.Format("{0}%", (int)(100f * ((float)masteryValue / (float)masteryLock)));
				this.chanceLabel.gameObject.SetActive(true);
				this.RefreshBackgroundCells();
				((RectTransform)this.cellsParent).RefreshContentFitter();
				return;
			}
			num2 = Math.Clamp(masteryValue / masteryLock, 0, ConstDef.Get("max_cells_per_one_hit").IntValue);
			num = (float)masteryValue % (float)masteryLock / (float)masteryLock;
		}
		if (num2 <= 0)
		{
			this.Hide();
			return;
		}
		for (int i = 0; i < ConstDef.Get("max_cells_per_one_hit").IntValue; i++)
		{
			UIProgressCellsInfoWidgetCell elementFromPool2 = UIPrefabsPooler.Instance.GetElementFromPool<UIProgressCellsInfoWidgetCell>(this.cellsParent);
			if (num2 - 1 >= i)
			{
				elementFromPool2.Show(false, false, 1f);
			}
			else if (num2 == i && num > 0f)
			{
				elementFromPool2.Show(false, false, num);
			}
			else
			{
				elementFromPool2.Show(true, false, 1f);
			}
			this.progressCells.Add(elementFromPool2);
		}
		this.RefreshBackgroundCells();
		((RectTransform)this.cellsParent).RefreshContentFitter();
	}

	// Token: 0x060041B5 RID: 16821 RVA: 0x001393FC File Offset: 0x001375FC
	private void RefreshBackgroundCells()
	{
		foreach (GameObject gameObject in this.backgroundCellImages)
		{
			gameObject.gameObject.SetActive(false);
		}
		for (int i = 0; i < this.progressCells.Count; i++)
		{
			this.backgroundCellImages[i].gameObject.SetActive(true);
		}
	}

	// Token: 0x060041B6 RID: 16822 RVA: 0x00139480 File Offset: 0x00137680
	private void AttachTickTooltip()
	{
		if (base.gameObject.activeSelf)
		{
			UIProgressCellsInfoWidgetData data = this.data;
			if (((data != null) ? data.TalentDef : null) != null)
			{
				UIMouseTooltip.AttachCustom(base.gameObject, new Action(this.ShowTickTooltip), true, true, default(UIMouseTooltipEdges), default(Vector2));
				return;
			}
		}
	}

	// Token: 0x060041B7 RID: 16823 RVA: 0x001394DC File Offset: 0x001376DC
	private void ShowTickTooltip()
	{
		UIProgressCellsInfoWidgetData data = this.data;
		if (((data != null) ? data.TalentDef : null) == null)
		{
			return;
		}
		string text = this.data.TalentDef.id.FontIcon();
		string text2 = string.Format("{0}<space=2px>{1} / {2}<space=2px>{3}", new object[]
		{
			text,
			this.data.FormatPlayerMasteryValue(),
			text,
			this.data.MasteryLock
		}).NOBR();
		string text3 = null;
		if (!string.IsNullOrEmpty(this.data.TooltipHeaderLngId) && LL.HasLocalizedValueForCurrentLang(this.data.TooltipHeaderLngId))
		{
			text3 = LLBase.L(this.data.TooltipHeaderLngId);
		}
		Vector2 vector = Vector2.zero;
		UIMouseTooltip component = base.GetComponent<UIMouseTooltip>();
		if (component != null)
		{
			vector = component.GetResolvedAppearOffset();
		}
		UITooltip.ShowSimpleInfo(base.transform, text2, vector, text3);
	}

	// Token: 0x060041B8 RID: 16824 RVA: 0x00002318 File Offset: 0x00000518
	public void OnOver()
	{
	}

	// Token: 0x060041B9 RID: 16825 RVA: 0x00002318 File Offset: 0x00000518
	public void OnOut()
	{
	}

	// Token: 0x060041BA RID: 16826 RVA: 0x001395B8 File Offset: 0x001377B8
	public void ShowUITooltip()
	{
		this.isHovered = true;
		UITooltip.ShowProgressTicksInfo(this, this.data.MasteryValue, this.data.MasteryLock, this.data.IsStarCraft, this.data.TalentDef);
	}

	// Token: 0x060041BB RID: 16827 RVA: 0x001395F3 File Offset: 0x001377F3
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

	// Token: 0x060041BC RID: 16828 RVA: 0x00002318 File Offset: 0x00000518
	protected override void TestDraw()
	{
	}

	// Token: 0x04003353 RID: 13139
	[SerializeField]
	public Transform cellsParent;

	// Token: 0x04003354 RID: 13140
	[SerializeField]
	private TextMeshProUGUI chanceLabel;

	// Token: 0x04003355 RID: 13141
	[SerializeField]
	private GameObject backgroundCellImagePrefab;

	// Token: 0x04003356 RID: 13142
	[SerializeField]
	private RectTransform backGroupParent;

	// Token: 0x04003357 RID: 13143
	private List<UIProgressCellsInfoWidgetCell> progressCells = new List<UIProgressCellsInfoWidgetCell>();

	// Token: 0x04003358 RID: 13144
	private List<GameObject> backgroundCellImages = new List<GameObject>();

	// Token: 0x04003359 RID: 13145
	private bool isHovered;
}
