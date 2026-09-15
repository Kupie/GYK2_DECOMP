using System;
using UnityEngine;

// Token: 0x020005E0 RID: 1504
[Serializable]
public class OrderBase
{
	// Token: 0x17000668 RID: 1640
	// (get) Token: 0x060027C2 RID: 10178 RVA: 0x000B9F18 File Offset: 0x000B8118
	public Item Item
	{
		get
		{
			return this.item;
		}
	}

	// Token: 0x17000669 RID: 1641
	// (get) Token: 0x060027C3 RID: 10179 RVA: 0x000B9F20 File Offset: 0x000B8120
	public SGuid TargetWgoUniqueId
	{
		get
		{
			return this.targetWgoUniqueId;
		}
	}

	// Token: 0x1700066A RID: 1642
	// (get) Token: 0x060027C4 RID: 10180 RVA: 0x000B9F28 File Offset: 0x000B8128
	// (set) Token: 0x060027C5 RID: 10181 RVA: 0x000B9F30 File Offset: 0x000B8130
	public SGuid ExecutorUniqueId
	{
		get
		{
			return this.executorUniqueId;
		}
		set
		{
			this.executorUniqueId = value;
		}
	}

	// Token: 0x1700066B RID: 1643
	// (get) Token: 0x060027C6 RID: 10182 RVA: 0x000B9F39 File Offset: 0x000B8139
	public ZombieWgoData ZombieWgoData
	{
		get
		{
			return MainGame.ZombieSystemData.GetZombie(this.targetWgoUniqueId);
		}
	}

	// Token: 0x1700066C RID: 1644
	// (get) Token: 0x060027C7 RID: 10183 RVA: 0x000B9F4B File Offset: 0x000B814B
	public SGuid UniqueId
	{
		get
		{
			return this.uniqueId;
		}
	}

	// Token: 0x060027C8 RID: 10184 RVA: 0x000B9F54 File Offset: 0x000B8154
	public OrderBase(SGuid targetWgoUniqueId, Item item)
	{
		this.uniqueId = new SGuid();
		this.targetWgoUniqueId.SetGuid(targetWgoUniqueId);
		this.item = new Item(item.id, item.Count);
	}

	// Token: 0x060027C9 RID: 10185 RVA: 0x00028294 File Offset: 0x00026494
	public virtual int GetPriority()
	{
		return 0;
	}

	// Token: 0x060027CA RID: 10186 RVA: 0x000B9FAB File Offset: 0x000B81AB
	public virtual bool TryExecuteOrder(IOrderExecutor orderExecutor, out string reasonIfNot)
	{
		if (this.CanOrderBeExecuted(orderExecutor, out reasonIfNot))
		{
			this.ExecuteOrder(orderExecutor);
			return true;
		}
		return false;
	}

	// Token: 0x060027CB RID: 10187 RVA: 0x000B9FC1 File Offset: 0x000B81C1
	public virtual void ExecuteOrder(IOrderExecutor orderExecutor)
	{
		Debug.LogError("ExecuteOrder is not implemented");
	}

	// Token: 0x060027CC RID: 10188 RVA: 0x000B9FCD File Offset: 0x000B81CD
	public virtual bool CanOrderBeExecuted(IOrderExecutor orderExecutor, out string reasonIfNot)
	{
		Debug.LogError("CanOrderBeExecuted is not implemented");
		reasonIfNot = "CanOrderBeExecuted is not implemented";
		return false;
	}

	// Token: 0x060027CD RID: 10189 RVA: 0x000B9FE1 File Offset: 0x000B81E1
	public virtual string GetInteractionHint()
	{
		Debug.LogError("GetInteractionHint is not implemented");
		return string.Empty;
	}

	// Token: 0x060027CE RID: 10190 RVA: 0x000B9FF2 File Offset: 0x000B81F2
	public virtual string GetStatusIcon()
	{
		Debug.LogError("GetStatusIcon is not implemented");
		return string.Empty;
	}

	// Token: 0x040021B2 RID: 8626
	[SerializeField]
	protected SGuid uniqueId;

	// Token: 0x040021B3 RID: 8627
	[SerializeField]
	protected SGuid targetWgoUniqueId = SGuid.Empty;

	// Token: 0x040021B4 RID: 8628
	[SerializeField]
	protected Item item;

	// Token: 0x040021B5 RID: 8629
	[SerializeField]
	protected SGuid executorUniqueId = SGuid.Empty;
}
