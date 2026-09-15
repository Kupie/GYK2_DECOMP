using System;
using LazyBearTechnology;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020009E1 RID: 2529
public class UIGraveElementWidget : LazyWidget<UIGraveElementWidgetData>
{
	// Token: 0x060043C5 RID: 17349 RVA: 0x00142314 File Offset: 0x00140514
	public void StartItemCellSelectionBlinking()
	{
		this.insertItemCell.UIItemCell.StartSelectionBlinking();
	}

	// Token: 0x060043C6 RID: 17350 RVA: 0x00142328 File Offset: 0x00140528
	public override void Redraw()
	{
		base.Redraw();
		this.description.text = "";
		if (!this.data.IsEmpty)
		{
			this.insertItemCell.Draw(this.data.GraveElementItem, this.data.HasRequiredTool, this.data.RequiredTool);
			this.description.text = LLBase.L(this.data.GraveElementItem.id);
			this.activeTextStyleText.ApplyStyle(this.description, false, null, null, null);
			this.activeTextStyleValue.ApplyStyle(this.qualityValue, false, null, null, null);
			this.qualityType.gameObject.SetActive(true);
			this.qualityValue.text = this.data.Quality.ToString();
		}
		else
		{
			this.insertItemCell.DrawEmpty(this.data.HasRequiredTool, this.data.RequiredTool);
			this.inactiveTextStyleText.ApplyStyle(this.description, false, null, null, null);
			this.inactiveTextStyleValue.ApplyStyle(this.qualityValue, false, null, null, null);
			this.qualityType.gameObject.SetActive(false);
			this.description.text = LLBase.L(this.data.EmptyDescriptionLocale);
			this.qualityValue.text = "0";
		}
		this.insertItemCell.UIItemCell.OnItemCellPress = new Action<UIItemCell>(this.OnElementClicked);
	}

	// Token: 0x060043C7 RID: 17351 RVA: 0x00142508 File Offset: 0x00140708
	private void OnElementClicked(UIItemCell itemCell)
	{
		Action onElementClicked = this.data.onElementClicked;
		if (onElementClicked == null)
		{
			return;
		}
		onElementClicked();
	}

	// Token: 0x060043C8 RID: 17352 RVA: 0x00002318 File Offset: 0x00000518
	protected override void TestDraw()
	{
	}

	// Token: 0x040034E2 RID: 13538
	[SerializeField]
	private UIInsertItemCell insertItemCell;

	// Token: 0x040034E3 RID: 13539
	[SerializeField]
	private TextMeshProUGUI description;

	// Token: 0x040034E4 RID: 13540
	[SerializeField]
	private TextMeshProUGUI qualityValue;

	// Token: 0x040034E5 RID: 13541
	[SerializeField]
	private Image qualityType;

	// Token: 0x040034E6 RID: 13542
	[SerializeField]
	private TextStyle activeTextStyleValue;

	// Token: 0x040034E7 RID: 13543
	[SerializeField]
	private TextStyle inactiveTextStyleValue;

	// Token: 0x040034E8 RID: 13544
	[SerializeField]
	private TextStyle activeTextStyleText;

	// Token: 0x040034E9 RID: 13545
	[SerializeField]
	private TextStyle inactiveTextStyleText;
}
