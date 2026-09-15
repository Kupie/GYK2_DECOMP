using System;

// Token: 0x0200035E RID: 862
public interface IWorkActivity
{
	// Token: 0x060016DA RID: 5850
	bool IsEnoughDurability(Item tool);

	// Token: 0x060016DB RID: 5851
	int GetActionDamage(Item item);

	// Token: 0x060016DC RID: 5852
	bool CanStartActivity();

	// Token: 0x060016DD RID: 5853
	void OnStartActivity();

	// Token: 0x060016DE RID: 5854
	bool IsEnoughMastery();

	// Token: 0x060016DF RID: 5855
	bool CanUseTool(Item tool);

	// Token: 0x060016E0 RID: 5856
	void UseTool(Item tool, int deltaTick);

	// Token: 0x060016E1 RID: 5857
	bool IsEnoughEnergy(Item tool, float energyPerTick);

	// Token: 0x060016E2 RID: 5858
	void ConsumeEnergy(Item tool, float energyPerTick);

	// Token: 0x060016E3 RID: 5859
	float GetEnergyCostPerTick(Item tool);

	// Token: 0x060016E4 RID: 5860
	bool CanChangeInsanity(Item tool, float insanityPerTick);

	// Token: 0x060016E5 RID: 5861
	void ChangeInsanity(Item tool, float insanityPerTick);

	// Token: 0x060016E6 RID: 5862
	float GetInsanityCostPerTick(Item tool);
}
