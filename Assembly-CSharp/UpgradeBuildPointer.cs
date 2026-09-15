using System;

// Token: 0x0200015F RID: 351
public class UpgradeBuildPointer : WgoBuildPointer
{
	// Token: 0x06000878 RID: 2168 RVA: 0x0002A204 File Offset: 0x00028404
	public override bool TryDoBuildAction()
	{
		if (this.shownAsActive)
		{
			Action takeResourcesAction = this.takeResourcesAction;
			if (takeResourcesAction != null)
			{
				takeResourcesAction();
			}
			Wgo componentInParent = BuildController.Instance.CurrentFullCoverSoftHintArea.GetComponentInParent<Wgo>();
			if (this.buildData.Definition != null)
			{
				foreach (LazyExpression lazyExpression in this.buildData.Definition.expressionAfterBuilding)
				{
					lazyExpression.EvaluateBool(componentInParent.Data);
				}
			}
			return true;
		}
		return false;
	}
}
