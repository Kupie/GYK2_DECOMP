using System;

// Token: 0x0200035D RID: 861
public interface IDamageDealer
{
	// Token: 0x14000038 RID: 56
	// (add) Token: 0x060016D4 RID: 5844
	// (remove) Token: 0x060016D5 RID: 5845
	event Action<ICombatEntity, AttackContext> OnHit;

	// Token: 0x14000039 RID: 57
	// (add) Token: 0x060016D6 RID: 5846
	// (remove) Token: 0x060016D7 RID: 5847
	event Action OnMiss;

	// Token: 0x060016D8 RID: 5848
	void Activate(AttackContext ctx);

	// Token: 0x060016D9 RID: 5849
	void Cancel();
}
