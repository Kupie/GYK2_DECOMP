using System;
using LazyBearTechnology;

// Token: 0x0200015A RID: 346
public class FightingBuildPointer : WgoBuildPointer
{
	// Token: 0x0600084A RID: 2122 RVA: 0x00028F74 File Offset: 0x00027174
	public override bool TryDoBuildAction()
	{
		if (this.shownAsActive)
		{
			Action takeResourcesAction = this.takeResourcesAction;
			if (takeResourcesAction != null)
			{
				takeResourcesAction();
			}
			WgoData wgoData = new WgoData(this.buildData.WgoId, this.target.Data.Position, this.gameScene.Id);
			if (this.target.CanBeRotated() && this.target.MainWgoPart.WgoPartData.rotationIndex != -1)
			{
				wgoData.MainWgoPartData.variationId = this.target.MainWgoPart.WgoPartData.variationId;
				wgoData.MainWgoPartData.rotationIndex = this.target.MainWgoPart.WgoPartData.rotationIndex;
			}
			Wgo wgo = this.gameScene.AddWgoData(wgoData, false);
			if (this.buildData.Definition != null)
			{
				foreach (LazyExpression lazyExpression in this.buildData.Definition.expressionAfterBuilding)
				{
					lazyExpression.EvaluateBool(wgo.Data);
				}
			}
			MainGame.Instance.GameSave.militaryBaseData.AddFightBuilding(wgo.Data);
			if (LazySingleton<FightingGameController>.Instance.CurrentFightState != FightState.Disabled)
			{
				wgo.IsActiveCombatant = GameBalance.Me.fighterWgoIdsCache.Contains(wgo.Data.id);
				if (wgo.IsActiveCombatant)
				{
					LazySingleton<FightingGameController>.Instance.RegisterCombatantTarget(wgo, false);
				}
			}
			return true;
		}
		return false;
	}
}
