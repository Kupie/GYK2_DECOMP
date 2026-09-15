using System;

// Token: 0x02000378 RID: 888
public abstract class PlayerActivity : IWorkActivity
{
	// Token: 0x170003FD RID: 1021
	// (get) Token: 0x0600178F RID: 6031 RVA: 0x0006FD6A File Offset: 0x0006DF6A
	public WgoData WgoData
	{
		get
		{
			return this.wgoData;
		}
	}

	// Token: 0x06001790 RID: 6032
	public abstract bool IsEnoughDurability(Item tool);

	// Token: 0x06001791 RID: 6033
	public abstract int GetActionDamage(Item item);

	// Token: 0x06001792 RID: 6034
	public abstract bool CanStartActivity();

	// Token: 0x06001793 RID: 6035
	public abstract void OnStartActivity();

	// Token: 0x06001794 RID: 6036
	public abstract bool IsEnoughMastery();

	// Token: 0x06001795 RID: 6037
	public abstract bool CanUseTool(Item tool);

	// Token: 0x06001796 RID: 6038
	public abstract void UseTool(Item tool, int deltaTick);

	// Token: 0x06001797 RID: 6039
	public abstract bool IsEnoughEnergy(Item tool, float energyPerTick);

	// Token: 0x06001798 RID: 6040
	public abstract void ConsumeEnergy(Item tool, float energyPerTick);

	// Token: 0x06001799 RID: 6041
	public abstract float GetEnergyCostPerTick(Item tool);

	// Token: 0x0600179A RID: 6042
	public abstract bool CanChangeInsanity(Item tool, float insanityPerTick);

	// Token: 0x0600179B RID: 6043
	public abstract void ChangeInsanity(Item tool, float insanityPerTick);

	// Token: 0x0600179C RID: 6044
	public abstract float GetInsanityCostPerTick(Item tool);

	// Token: 0x04001769 RID: 5993
	protected PlayerData playerData;

	// Token: 0x0400176A RID: 5994
	protected WgoData wgoData;
}
