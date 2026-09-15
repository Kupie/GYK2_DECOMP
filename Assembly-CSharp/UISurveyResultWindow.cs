using System;
using System.Collections.Generic;
using LazyBearTechnology;
using TMPro;
using UnityEngine;

// Token: 0x02000A3E RID: 2622
public class UISurveyResultWindow : LazyWindow<UISurveyResultWindowData>
{
	// Token: 0x060046BF RID: 18111 RVA: 0x0014EF94 File Offset: 0x0014D194
	public override void Redraw()
	{
		base.Redraw();
		this.itemCell.Draw(new Item(this.data.ItemDef.id, 1), false, -1, false, 1, false, 0, true, false, false, ItemRelatedWidgetState.NotSet, false);
		this.itemNameLabel.text = this.data.ItemDef.GetHeader();
		string text = this.data.ItemDef.GetRunesAsString();
		if (string.IsNullOrEmpty(text))
		{
			text = "-";
		}
		this.runesLabel.text = LLBase.L("ui_survey_complete_runes") + ": " + this.runesStyle.ApplyStyleToString(text, false, true);
		this.windowButton.Draw(new UIDialogWindowData.ButtonData(new Action(this.OnBtnPressed), LLBase.L("btn_ok"), null, true, GameKey.Select, ""));
	}

	// Token: 0x060046C0 RID: 18112 RVA: 0x0014F06C File Offset: 0x0014D26C
	private void OnBtnPressed()
	{
		this.Close();
	}

	// Token: 0x060046C1 RID: 18113 RVA: 0x0014F074 File Offset: 0x0014D274
	protected override void PrintTips()
	{
		this.lazyButtonTips.Clear();
	}

	// Token: 0x060046C2 RID: 18114 RVA: 0x0014F081 File Offset: 0x0014D281
	protected override Dictionary<GameKey, Func<bool>> GetGameKeyDelegates()
	{
		Dictionary<GameKey, Func<bool>> gameKeyDelegates = base.GetGameKeyDelegates();
		gameKeyDelegates.Add(GameKey.Select, () => false);
		return gameKeyDelegates;
	}

	// Token: 0x060046C3 RID: 18115 RVA: 0x0014F0B4 File Offset: 0x0014D2B4
	[LazyUITest]
	protected override void TestDraw()
	{
		UISurveyResultWindowData uisurveyResultWindowData = new UISurveyResultWindowData(GameBalance.Me.GetData<ItemDef>("flesh"));
		this.Open(uisurveyResultWindowData);
	}

	// Token: 0x04003738 RID: 14136
	[SerializeField]
	private UIItemCell itemCell;

	// Token: 0x04003739 RID: 14137
	[SerializeField]
	private TextMeshProUGUI itemNameLabel;

	// Token: 0x0400373A RID: 14138
	[SerializeField]
	private TextMeshProUGUI runesLabel;

	// Token: 0x0400373B RID: 14139
	[SerializeField]
	private TextStyle runesStyle;

	// Token: 0x0400373C RID: 14140
	[SerializeField]
	private UIDialogWindowButton windowButton;
}
