using System;
using UnityEngine;

// Token: 0x0200025F RID: 607
[Serializable]
public abstract class ColliderTriggerDataBase
{
	// Token: 0x06000F59 RID: 3929 RVA: 0x0004ED4C File Offset: 0x0004CF4C
	public void TrySetTrigger()
	{
		if (this.IsSetupCompleted() && this.chance >= global::UnityEngine.Random.Range(0f, 100f) && Time.time >= this.lastTriggerTime + this.rolledDelay)
		{
			this.lastTriggerTime = Time.time;
			this.rolledDelay = global::UnityEngine.Random.Range(this.delayFrom, this.delayTo);
			this.TriggerSetAction();
		}
	}

	// Token: 0x06000F5A RID: 3930 RVA: 0x0004EDB4 File Offset: 0x0004CFB4
	public void TryResetTrigger()
	{
		if (this.IsSetupCompleted())
		{
			this.TriggerResetAction();
		}
	}

	// Token: 0x06000F5B RID: 3931 RVA: 0x0003C7FE File Offset: 0x0003A9FE
	protected virtual bool IsSetupCompleted()
	{
		return true;
	}

	// Token: 0x06000F5C RID: 3932
	protected abstract void TriggerSetAction();

	// Token: 0x06000F5D RID: 3933
	protected abstract void TriggerResetAction();

	// Token: 0x0400123B RID: 4667
	[SerializeField]
	[Range(0f, 100f)]
	protected float chance;

	// Token: 0x0400123C RID: 4668
	[SerializeField]
	protected float delayFrom;

	// Token: 0x0400123D RID: 4669
	[SerializeField]
	protected float delayTo;

	// Token: 0x0400123E RID: 4670
	protected float lastTriggerTime = -1f;

	// Token: 0x0400123F RID: 4671
	protected float rolledDelay = -1f;
}
