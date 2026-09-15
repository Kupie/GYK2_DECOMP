using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020009C0 RID: 2496
public class FightWinWindow : LazyWindow<FightEndWindowData>
{
	// Token: 0x06004278 RID: 17016 RVA: 0x0013B7B0 File Offset: 0x001399B0
	public override void Redraw()
	{
		base.Redraw();
		this.btnData = new UIDialogWindowData.ButtonData(new Action(this.Close), LLBase.L("btn_ok"), null, true, GameKey.Select, "");
		this.lazyButton.Draw(this.btnData);
		UIItemCell[] array = this.rewardCells;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].gameObject.SetActive(false);
		}
		if (this.data.FightDefinition.rewards.Count > 0)
		{
			for (int j = 0; j < this.data.FightDefinition.rewards.Count; j++)
			{
				this.rewardCells[j].gameObject.SetActive(true);
				this.rewardCells[j].Draw(new Item(this.data.FightDefinition.rewards[j].id, this.data.FightDefinition.rewards[j].GetCount(null)), false, -1, false, 1, false, 0, true, false, false, ItemRelatedWidgetState.NotSet, false);
			}
			this.noRewardsObj.SetActive(false);
			this.rewardsObj.SetActive(true);
		}
		else
		{
			this.noRewardsObj.SetActive(true);
			this.rewardsObj.SetActive(false);
		}
		if (LazyInput.IsGamepadActive)
		{
			base.GamepadNavigationController.ReinitItems(true, null, null);
		}
		((RectTransform)base.transform).RefreshContentFitter();
	}

	// Token: 0x06004279 RID: 17017 RVA: 0x0013B924 File Offset: 0x00139B24
	protected override void PrintTips()
	{
		List<LazyGameKeyTip> list = new List<LazyGameKeyTip>();
		list.Add(LazyGameKeyTip.Back(true, true, true));
		this.lazyButtonTips.Print(list, "  ");
	}

	// Token: 0x0600427A RID: 17018 RVA: 0x0013B956 File Offset: 0x00139B56
	protected override Dictionary<GameKey, Func<bool>> GetGameKeyDelegates()
	{
		Dictionary<GameKey, Func<bool>> gameKeyDelegates = base.GetGameKeyDelegates();
		gameKeyDelegates.Add(GameKey.Select, () => false);
		return gameKeyDelegates;
	}

	// Token: 0x0600427B RID: 17019 RVA: 0x0013B678 File Offset: 0x00139878
	[LazyUITest]
	protected override void TestDraw()
	{
		this.Open(new FightEndWindowData(GameBalance.Me.GetData<FightDef>("fight_A1_1")));
	}

	// Token: 0x040033D0 RID: 13264
	[SerializeField]
	private GameObject noRewardsObj;

	// Token: 0x040033D1 RID: 13265
	[SerializeField]
	private GameObject rewardsObj;

	// Token: 0x040033D2 RID: 13266
	[SerializeField]
	private UIDialogWindowButton lazyButton;

	// Token: 0x040033D3 RID: 13267
	[SerializeField]
	private UIItemCell[] rewardCells;

	// Token: 0x040033D4 RID: 13268
	private UIDialogWindowData.ButtonData btnData;
}
