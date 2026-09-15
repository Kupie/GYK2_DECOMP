using System;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x0200049E RID: 1182
[Serializable]
public class GameLogicData : ObjectLinkedToDefinition<GameLogicDef>
{
	// Token: 0x06001F76 RID: 8054 RVA: 0x00094E8F File Offset: 0x0009308F
	public GameLogicData()
	{
	}

	// Token: 0x06001F77 RID: 8055 RVA: 0x00094EBB File Offset: 0x000930BB
	public GameLogicData(string id)
		: base(id)
	{
	}

	// Token: 0x06001F78 RID: 8056 RVA: 0x00094EE8 File Offset: 0x000930E8
	public virtual void Init()
	{
		if (this.execTime < 0f)
		{
			EnvironmentData environmentData = MainGame.Instance.GameSave.environmentData;
			this.execTime = base.Definition.startTime;
			this.execDay = 1 + (int)this.execTime;
			this.execTime %= 1f;
			if (this.execDay == 1 && this.execTime < environmentData.TimeOfDay)
			{
				this.execDay++;
			}
			this.SkipOverduePeriods(environmentData);
		}
	}

	// Token: 0x06001F79 RID: 8057 RVA: 0x00094F70 File Offset: 0x00093170
	private void SkipOverduePeriods(EnvironmentData envData)
	{
		float periodTime = base.Definition.periodTime;
		if (periodTime <= 0f)
		{
			return;
		}
		float num = (float)envData.Day + envData.TimeOfDay;
		float num2 = (float)this.execDay + this.execTime;
		if (num < num2)
		{
			return;
		}
		float num3 = MathF.Floor((num - num2) / periodTime) + 1f;
		num2 = MathF.Round(num2 + num3 * periodTime, 3);
		this.execDay = (int)num2;
		this.execTime = num2 - (float)this.execDay;
	}

	// Token: 0x06001F7A RID: 8058 RVA: 0x00094FEC File Offset: 0x000931EC
	protected virtual void UpdateTimer()
	{
		this.execTime += base.Definition.periodTime;
		this.execTime = MathF.Round(this.execTime, 3);
		this.execDay += (int)this.execTime;
		this.execTime %= 1f;
	}

	// Token: 0x06001F7B RID: 8059 RVA: 0x0009504C File Offset: 0x0009324C
	protected bool TryConsumeFireSlot()
	{
		EnvironmentData environmentData = MainGame.Instance.GameSave.environmentData;
		if ((environmentData.TimeOfDay - this.lastFireTimeOfDay).EqualsTo(0f, 1E-05f) && environmentData.Day == this.lastFireDay)
		{
			return false;
		}
		this.lastFireTimeOfDay = environmentData.TimeOfDay;
		this.lastFireDay = environmentData.Day;
		return true;
	}

	// Token: 0x06001F7C RID: 8060 RVA: 0x000950B0 File Offset: 0x000932B0
	public virtual void TryExecute()
	{
		if (!this.TryConsumeFireSlot())
		{
			return;
		}
		this.UpdateTimer();
		if (!base.Definition.condition.EvaluateChance())
		{
			return;
		}
		Debug.Log("[GameLogicData]: [" + this.id + "] executing");
		foreach (LazyExpression lazyExpression in base.Definition.execExpressions)
		{
			lazyExpression.Evaluate();
		}
		if (!string.IsNullOrEmpty(base.Definition.execFlowscriptName))
		{
			GameScriptUtility.RunGlobalScript(base.Definition.execFlowscriptName, null);
		}
	}

	// Token: 0x04001C31 RID: 7217
	[SerializeField]
	public float execTime = -1f;

	// Token: 0x04001C32 RID: 7218
	[SerializeField]
	public int execDay = 1;

	// Token: 0x04001C33 RID: 7219
	public int lastExecDay;

	// Token: 0x04001C34 RID: 7220
	private float lastFireTimeOfDay = -1f;

	// Token: 0x04001C35 RID: 7221
	private int lastFireDay = -1;
}
