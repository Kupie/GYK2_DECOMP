using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020009BE RID: 2494
public class FightLoseWindow : LazyWindow<FightEndWindowData>
{
	// Token: 0x06004270 RID: 17008 RVA: 0x0013B6DC File Offset: 0x001398DC
	public override void Redraw()
	{
		base.Redraw();
		this.btnData = new UIDialogWindowData.ButtonData(new Action(this.Close), LLBase.L("btn_ok"), null, true, GameKey.Select, "");
		this.lazyButton.Draw(this.btnData);
		((RectTransform)base.transform).RefreshContentFitter();
	}

	// Token: 0x06004271 RID: 17009 RVA: 0x0013B740 File Offset: 0x00139940
	protected override void PrintTips()
	{
		List<LazyGameKeyTip> list = new List<LazyGameKeyTip>();
		list.Add(LazyGameKeyTip.Back(true, true, true));
		this.lazyButtonTips.Print(list, "  ");
	}

	// Token: 0x06004272 RID: 17010 RVA: 0x0013B772 File Offset: 0x00139972
	protected override Dictionary<GameKey, Func<bool>> GetGameKeyDelegates()
	{
		Dictionary<GameKey, Func<bool>> gameKeyDelegates = base.GetGameKeyDelegates();
		gameKeyDelegates.Add(GameKey.Select, () => false);
		return gameKeyDelegates;
	}

	// Token: 0x06004273 RID: 17011 RVA: 0x0013B678 File Offset: 0x00139878
	[LazyUITest]
	protected override void TestDraw()
	{
		this.Open(new FightEndWindowData(GameBalance.Me.GetData<FightDef>("fight_A1_1")));
	}

	// Token: 0x040033CC RID: 13260
	[SerializeField]
	private UIDialogWindowButton lazyButton;

	// Token: 0x040033CD RID: 13261
	private UIDialogWindowData.ButtonData btnData;
}
