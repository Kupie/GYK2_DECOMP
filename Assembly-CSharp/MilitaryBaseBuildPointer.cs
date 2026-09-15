using System;

// Token: 0x0200015D RID: 349
public class MilitaryBaseBuildPointer : WgoBuildPointer
{
	// Token: 0x06000855 RID: 2133 RVA: 0x000290FC File Offset: 0x000272FC
	public override bool TryDoBuildAction()
	{
		if (this.shownAsActive)
		{
			Action takeResourcesAction = this.takeResourcesAction;
			if (takeResourcesAction != null)
			{
				takeResourcesAction();
			}
			Wgo wgo = this.gameScene.AddWgoData(this.buildData.WgoId, this.target.Data.Position);
			if (this.target.CanBeRotated() && this.target.MainWgoPart.WgoPartData.rotationIndex != -1)
			{
				wgo.MainWgoPart.ApplyWgoPartState(this.target.MainWgoPart.WgoPartData.variationId, this.target.MainWgoPart.WgoPartData.rotationIndex);
			}
			if (this.buildData.Definition != null)
			{
				foreach (LazyExpression lazyExpression in this.buildData.Definition.expressionAfterBuilding)
				{
					lazyExpression.EvaluateBool(wgo.Data);
				}
			}
			MainGame.Instance.GameSave.militaryBaseData.AddBaseBuilding(wgo.Data);
			return true;
		}
		return false;
	}
}
