using System;
using System.Collections.Generic;
using LazyBearTechnology;

// Token: 0x0200021E RID: 542
[Serializable]
public class CustomInteraction
{
	// Token: 0x06000CE1 RID: 3297 RVA: 0x00040CD4 File Offset: 0x0003EED4
	public bool IsInteractable(WgoData data)
	{
		Item item;
		return this.TryGetInteractable(data, out item);
	}

	// Token: 0x06000CE2 RID: 3298 RVA: 0x00040CEC File Offset: 0x0003EEEC
	public bool TryGetInteractable(WgoData data, out Item candidate)
	{
		candidate = null;
		if (!this.condition.HasExpression)
		{
			return this.HasExecution(data);
		}
		PlayerData playerData = MainGame.PlayerData;
		if (playerData != null && playerData.HasMultipleOverheadItems)
		{
			IReadOnlyList<Item> overheadItems = playerData.OverheadItems;
			for (int i = overheadItems.Count - 1; i >= 0; i--)
			{
				Item item = overheadItems[i];
				if (item != null && !item.IsEmpty)
				{
					Item overheadCandidate = LazyExpressionEvaluationScope.OverheadCandidate;
					LazyExpressionEvaluationScope.OverheadCandidate = item;
					bool flag = this.condition.EvaluateBool(data);
					LazyExpressionEvaluationScope.OverheadCandidate = overheadCandidate;
					if (flag)
					{
						candidate = item;
						return true;
					}
				}
			}
			return false;
		}
		bool flag2 = this.condition.EvaluateBool(data);
		if (flag2 && playerData != null && playerData.HasOverheadItem)
		{
			candidate = playerData.overheadItem;
		}
		return flag2;
	}

	// Token: 0x06000CE3 RID: 3299 RVA: 0x00040D9A File Offset: 0x0003EF9A
	public bool HasExecution(WgoData data)
	{
		return this.execution.Count > 1 || (this.execution.Count == 1 && this.execution[0].HasExpression);
	}

	// Token: 0x06000CE4 RID: 3300 RVA: 0x00040DCD File Offset: 0x0003EFCD
	public override string ToString()
	{
		return this.condition.ToString();
	}

	// Token: 0x04000F73 RID: 3955
	public LazyExpression condition;

	// Token: 0x04000F74 RID: 3956
	public List<LazyExpression> execution;

	// Token: 0x04000F75 RID: 3957
	public GameKey gameKey = GameKey.Interaction;

	// Token: 0x04000F76 RID: 3958
	public string hint;
}
