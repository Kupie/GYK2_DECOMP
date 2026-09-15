using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020009BB RID: 2491
public class FightDeadWindow : LazyWindow<FightEndWindowData>
{
	// Token: 0x06004264 RID: 16996 RVA: 0x0013B5A0 File Offset: 0x001397A0
	public override void Redraw()
	{
		base.Redraw();
		this.btnData = new UIDialogWindowData.ButtonData(new Action(this.Close), LLBase.L("btn_ok"), null, true, GameKey.Select, "");
		this.lazyButton.Draw(this.btnData);
		this.animator.SetTrigger(FightDeadWindow.start);
		((RectTransform)base.transform).RefreshContentFitter();
	}

	// Token: 0x06004265 RID: 16997 RVA: 0x0013B614 File Offset: 0x00139814
	protected override void PrintTips()
	{
		List<LazyGameKeyTip> list = new List<LazyGameKeyTip>();
		list.Add(LazyGameKeyTip.Back(true, true, true));
		this.lazyButtonTips.Print(list, "  ");
	}

	// Token: 0x06004266 RID: 16998 RVA: 0x0013B646 File Offset: 0x00139846
	protected override Dictionary<GameKey, Func<bool>> GetGameKeyDelegates()
	{
		Dictionary<GameKey, Func<bool>> gameKeyDelegates = base.GetGameKeyDelegates();
		gameKeyDelegates.Add(GameKey.Select, () => false);
		return gameKeyDelegates;
	}

	// Token: 0x06004267 RID: 16999 RVA: 0x0013B678 File Offset: 0x00139878
	[LazyUITest]
	protected override void TestDraw()
	{
		this.Open(new FightEndWindowData(GameBalance.Me.GetData<FightDef>("fight_A1_1")));
	}

	// Token: 0x040033C5 RID: 13253
	private static readonly int start = Animator.StringToHash("start");

	// Token: 0x040033C6 RID: 13254
	[SerializeField]
	private UIDialogWindowButton lazyButton;

	// Token: 0x040033C7 RID: 13255
	[SerializeField]
	private Animator animator;

	// Token: 0x040033C8 RID: 13256
	private UIDialogWindowData.ButtonData btnData;
}
