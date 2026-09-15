using System;

// Token: 0x0200043D RID: 1085
public class PerkSystem : ICustomUpdatable
{
	// Token: 0x170004F0 RID: 1264
	// (get) Token: 0x06001CA3 RID: 7331 RVA: 0x00085510 File Offset: 0x00083710
	private PerkSystemData Data
	{
		get
		{
			return MainGame.Instance.GameSave.perkSystemData;
		}
	}

	// Token: 0x06001CA4 RID: 7332 RVA: 0x00085C20 File Offset: 0x00083E20
	public void CustomUpdate(float deltaTime)
	{
		for (int i = this.Data.activePerks.Count - 1; i >= 0; i--)
		{
			PerkData perkData = this.Data.activePerks[i];
			perkData.tickTimer += deltaTime;
			if (perkData.Definition.duration > 0f)
			{
				perkData.currentDuration -= deltaTime;
			}
			if (perkData.Definition.tickRate != 0f && perkData.tickTimer >= perkData.Definition.tickRate)
			{
				perkData.tickTimer = 0f;
				if (!perkData.Definition.addGameResPerTick.IsEmpty())
				{
					MainGame.PlayerData.AddRes(perkData.Definition.addGameResPerTick);
				}
				foreach (LazyExpression lazyExpression in perkData.Definition.onPerTickExpressions)
				{
					lazyExpression.Evaluate();
				}
			}
			if (perkData.Definition.duration > 0f && perkData.currentDuration <= 0f)
			{
				this.Data.RemovePerk(perkData, false);
			}
		}
		this.Data.NotifyUpdated();
	}
}
