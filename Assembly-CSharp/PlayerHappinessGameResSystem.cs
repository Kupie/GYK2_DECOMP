using System;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x02000436 RID: 1078
public class PlayerHappinessGameResSystem : GK2GameResSystem
{
	// Token: 0x06001C87 RID: 7303 RVA: 0x00085506 File Offset: 0x00083706
	public PlayerHappinessGameResSystem(string gameResAtomName, GameRes gameRes)
		: base(gameResAtomName, gameRes)
	{
	}

	// Token: 0x06001C88 RID: 7304 RVA: 0x00085598 File Offset: 0x00083798
	public override void Add(float value, bool silent = false)
	{
		float num = this.resForChanges.Get(this.gameResAtomName, 0f);
		float num2 = num;
		if (MainGame.Instance.GameSave.townSystem.Quality > 0)
		{
			if (value >= 0f)
			{
				if (num < (float)MainGame.Instance.GameSave.townSystem.Quality)
				{
					num += value;
					num = Mathf.Clamp(num, base.Min, (float)MainGame.Instance.GameSave.townSystem.Quality);
					this.resForChanges.SetWithoutSystemsCheck(this.gameResAtomName, num);
				}
			}
			else if (num >= (float)MainGame.Instance.GameSave.townSystem.Quality)
			{
				num += value;
				num = Mathf.Clamp(num, base.Min, num2);
				this.resForChanges.SetWithoutSystemsCheck(this.gameResAtomName, num);
			}
			else
			{
				num += value;
				num = Mathf.Clamp(num, base.Min, (float)MainGame.Instance.GameSave.townSystem.Quality);
				this.resForChanges.SetWithoutSystemsCheck(this.gameResAtomName, num);
			}
		}
		else
		{
			num += value;
			num = Mathf.Clamp(num, base.Min, base.Max);
			this.resForChanges.SetWithoutSystemsCheck(this.gameResAtomName, num);
		}
		if (!silent)
		{
			Action<float> onValueChanged = this.onValueChanged;
			if (onValueChanged != null)
			{
				onValueChanged(num);
			}
			if (!Mathf.Approximately(num, num2))
			{
				Action<float> onValueDeltaChanged = this.onValueDeltaChanged;
				if (onValueDeltaChanged != null)
				{
					onValueDeltaChanged(num - num2);
				}
			}
		}
		int num3 = (int)num - (int)num2;
		base.EvaluateGameResExpressions(num3);
	}
}
