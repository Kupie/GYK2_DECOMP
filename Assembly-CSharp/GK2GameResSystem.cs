using System;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x02000434 RID: 1076
public class GK2GameResSystem : GameResSystemBase
{
	// Token: 0x170004E9 RID: 1257
	// (get) Token: 0x06001C77 RID: 7287 RVA: 0x0008522C File Offset: 0x0008342C
	protected static PlayerData PlayerData
	{
		get
		{
			return MainGame.PlayerData;
		}
	}

	// Token: 0x06001C78 RID: 7288 RVA: 0x00085233 File Offset: 0x00083433
	public GK2GameResSystem(string gameResAtomName, GameRes gameRes)
		: base(gameResAtomName, gameRes)
	{
	}

	// Token: 0x170004EA RID: 1258
	// (get) Token: 0x06001C79 RID: 7289 RVA: 0x0008523D File Offset: 0x0008343D
	private GameResSystemDef Def
	{
		get
		{
			return GameBalance.Me.GetData<GameResSystemDef>(this.gameResAtomName);
		}
	}

	// Token: 0x170004EB RID: 1259
	// (get) Token: 0x06001C7A RID: 7290 RVA: 0x0008524F File Offset: 0x0008344F
	public float Max
	{
		get
		{
			return this.Def.max.EvaluateFloat();
		}
	}

	// Token: 0x170004EC RID: 1260
	// (get) Token: 0x06001C7B RID: 7291 RVA: 0x00085261 File Offset: 0x00083461
	public float Min
	{
		get
		{
			return this.Def.min.EvaluateFloat();
		}
	}

	// Token: 0x06001C7C RID: 7292 RVA: 0x00085273 File Offset: 0x00083473
	public bool IsEnoughValue(float value)
	{
		return (this.resForChanges.Get(this.gameResAtomName, 0f) - value).EqualsOrMore(0f, 0.001f);
	}

	// Token: 0x06001C7D RID: 7293 RVA: 0x000852A1 File Offset: 0x000834A1
	public bool HasMax()
	{
		return GK2GameResSystem.PlayerData.GetRes(this.gameResAtomName, 0f).EqualsTo(this.Max, 1E-05f);
	}

	// Token: 0x06001C7E RID: 7294 RVA: 0x000852C8 File Offset: 0x000834C8
	public bool CanAddValue(float value)
	{
		float num = this.resForChanges.Get(this.gameResAtomName, 0f);
		if (num + value > this.Max || num < this.Min)
		{
			Debug.Log(string.Format("[{0}] typed:[{1}]: Cannot add more {2} [current maximum = {3}].", new object[] { "GameResSystemBase", this.gameResAtomName, this.gameResAtomName, this.Max }));
			return false;
		}
		return true;
	}

	// Token: 0x06001C7F RID: 7295 RVA: 0x00085340 File Offset: 0x00083540
	public override void Add(float value, bool silent = false)
	{
		float num = this.resForChanges.Get(this.gameResAtomName, 0f);
		float num2 = num;
		num += value;
		num = Mathf.Clamp(num, this.Min, this.Max);
		this.resForChanges.SetWithoutSystemsCheck(this.gameResAtomName, num);
		if (!silent)
		{
			Action<float> onValueChanged = this.onValueChanged;
			if (onValueChanged != null)
			{
				onValueChanged(num);
			}
			Action<float> action = this.onValueDeltaChanged;
			if (action != null)
			{
				action(num - num2);
			}
		}
		int num3 = (int)num - (int)num2;
		this.EvaluateGameResExpressions(num3);
	}

	// Token: 0x06001C80 RID: 7296 RVA: 0x000853C8 File Offset: 0x000835C8
	public override void Set(float value, bool silent = false)
	{
		float num = this.resForChanges.Get(this.gameResAtomName, 0f);
		float num2 = Mathf.Clamp(value, this.Min, this.Max);
		this.resForChanges.SetWithoutSystemsCheck(this.gameResAtomName, num2);
		if (!silent)
		{
			Action<float> onValueChanged = this.onValueChanged;
			if (onValueChanged != null)
			{
				onValueChanged(num2);
			}
			Action<float> action = this.onValueDeltaChanged;
			if (action != null)
			{
				action(num2 - num);
			}
		}
		int num3 = (int)num2 - (int)num;
		this.EvaluateGameResExpressions(num3);
	}

	// Token: 0x06001C81 RID: 7297 RVA: 0x00085448 File Offset: 0x00083648
	protected void EvaluateGameResExpressions(int valueDelta)
	{
		if (valueDelta > 0)
		{
			foreach (LazyExpression lazyExpression in this.Def.expressionsOnIncrease)
			{
				lazyExpression.EvaluateValueDelta(valueDelta);
			}
		}
		if (valueDelta < 0)
		{
			foreach (LazyExpression lazyExpression2 in this.Def.expressionsOnDecrease)
			{
				lazyExpression2.EvaluateValueDelta(Mathf.Abs(valueDelta));
			}
		}
	}

	// Token: 0x06001C82 RID: 7298 RVA: 0x000854F4 File Offset: 0x000836F4
	public static GK2GameResSystem GetSystem(string type)
	{
		return GK2GameResSystem.PlayerData.GetResSystem(type) as GK2GameResSystem;
	}

	// Token: 0x04001AB8 RID: 6840
	public Action<float> onValueDeltaChanged;

	// Token: 0x04001AB9 RID: 6841
	private const float EPSILON = 0.001f;
}
