using System;
using LazyBearTechnology;

// Token: 0x02000438 RID: 1080
public class PlayerMoneyGameResSystem : GK2GameResSystem
{
	// Token: 0x06001C8F RID: 7311 RVA: 0x00085506 File Offset: 0x00083706
	public PlayerMoneyGameResSystem(string gameResAtomName, GameRes gameRes)
		: base(gameResAtomName, gameRes)
	{
	}

	// Token: 0x06001C90 RID: 7312 RVA: 0x00085857 File Offset: 0x00083A57
	public static PlayerMoneyGameResSystem GetSystem()
	{
		return GK2GameResSystem.PlayerData.GetResSystem("money") as PlayerMoneyGameResSystem;
	}

	// Token: 0x06001C91 RID: 7313 RVA: 0x00085870 File Offset: 0x00083A70
	public override void Set(float value, bool silent = false)
	{
		float num = this.resForChanges.Get(this.gameResAtomName, 0f);
		base.Set(value, silent);
		Action<float> action = this.onValueChangedDiff;
		if (action == null)
		{
			return;
		}
		action(value - num);
	}

	// Token: 0x06001C92 RID: 7314 RVA: 0x000858AF File Offset: 0x00083AAF
	public override void Add(float value, bool silent = false)
	{
		base.Add(value, silent);
		Action<float> action = this.onValueChangedDiff;
		if (action == null)
		{
			return;
		}
		action(value);
	}

	// Token: 0x04001ABA RID: 6842
	public Action<float> onValueChangedDiff;
}
