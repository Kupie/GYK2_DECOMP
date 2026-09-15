using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000200 RID: 512
[Serializable]
public class QuestCheck : IEventTrigerrable
{
	// Token: 0x17000228 RID: 552
	// (get) Token: 0x06000C92 RID: 3218 RVA: 0x0003F646 File Offset: 0x0003D846
	public string TriggerableId
	{
		get
		{
			return this.triggerId;
		}
	}

	// Token: 0x17000229 RID: 553
	// (get) Token: 0x06000C93 RID: 3219 RVA: 0x0003F64E File Offset: 0x0003D84E
	public GlobalEventsSystem.Event.Type Type
	{
		get
		{
			return this.triggerType;
		}
	}

	// Token: 0x1700022A RID: 554
	// (get) Token: 0x06000C94 RID: 3220 RVA: 0x0003F656 File Offset: 0x0003D856
	// (set) Token: 0x06000C95 RID: 3221 RVA: 0x0003F65E File Offset: 0x0003D85E
	public SGuid UniqueId
	{
		get
		{
			return this.uniqueId;
		}
		set
		{
			this.uniqueId = value;
		}
	}

	// Token: 0x06000C96 RID: 3222 RVA: 0x0003F668 File Offset: 0x0003D868
	public bool OnTriggerPassed()
	{
		using (List<LazyExpression>.Enumerator enumerator = this.condExpressions.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (!enumerator.Current.EvaluateBool())
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x04000EA6 RID: 3750
	public string questId;

	// Token: 0x04000EA7 RID: 3751
	public GlobalEventsSystem.Event.Type triggerType;

	// Token: 0x04000EA8 RID: 3752
	public string triggerId;

	// Token: 0x04000EA9 RID: 3753
	public bool hasTrigger;

	// Token: 0x04000EAA RID: 3754
	public List<LazyExpression> condExpressions = new List<LazyExpression>();

	// Token: 0x04000EAB RID: 3755
	public QuestCheck.RunModificator runModificator;

	// Token: 0x04000EAC RID: 3756
	[SerializeField]
	private SGuid uniqueId;

	// Token: 0x02000201 RID: 513
	public enum RunModificator
	{
		// Token: 0x04000EAE RID: 3758
		None,
		// Token: 0x04000EAF RID: 3759
		TriggerSolo
	}
}
