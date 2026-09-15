using System;
using UnityEngine;

// Token: 0x0200049C RID: 1180
[Serializable]
public class CustomGameLogicData : GameLogicData
{
	// Token: 0x06001F62 RID: 8034 RVA: 0x00094A4D File Offset: 0x00092C4D
	public CustomGameLogicData()
	{
	}

	// Token: 0x06001F63 RID: 8035 RVA: 0x00094A55 File Offset: 0x00092C55
	public CustomGameLogicData(string id, float periodTime, Action executionLogic)
	{
		this.id = id;
		this.periodTime = periodTime;
		this.executionLogic = executionLogic;
	}

	// Token: 0x06001F64 RID: 8036 RVA: 0x00094A74 File Offset: 0x00092C74
	public override void Init()
	{
		this.envData = MainGame.Instance.GameSave.environmentData;
		float timeOfDay = this.envData.TimeOfDay;
		float num = timeOfDay + this.periodTime;
		num = MathF.Round(num, 3);
		if (this.execTime < 0f)
		{
			this.execDay = (int)num;
			this.execTime = num - (float)this.execDay;
			if (this.execDay == 0 && this.execTime < timeOfDay)
			{
				this.execDay++;
			}
		}
	}

	// Token: 0x06001F65 RID: 8037 RVA: 0x00094AF8 File Offset: 0x00092CF8
	protected override void UpdateTimer()
	{
		float num = (float)this.envData.Day + this.envData.TimeOfDay;
		this.execTime = num + this.periodTime;
		this.execTime = MathF.Round(this.execTime, 3);
		this.execDay += (int)this.execTime;
		this.execTime %= 1f;
	}

	// Token: 0x06001F66 RID: 8038 RVA: 0x00094B64 File Offset: 0x00092D64
	public override void TryExecute()
	{
		if (!base.TryConsumeFireSlot())
		{
			return;
		}
		this.UpdateTimer();
		Debug.Log("[CustomGameLogicData]: [" + this.id + "] executing");
		Action action = this.executionLogic;
		if (action == null)
		{
			return;
		}
		action();
	}

	// Token: 0x04001C22 RID: 7202
	private bool destroyAfterExecute;

	// Token: 0x04001C23 RID: 7203
	private float periodTime;

	// Token: 0x04001C24 RID: 7204
	private Action executionLogic;

	// Token: 0x04001C25 RID: 7205
	private EnvironmentData envData;
}
