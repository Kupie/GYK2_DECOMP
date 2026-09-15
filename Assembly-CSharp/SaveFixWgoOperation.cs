using System;

// Token: 0x02000489 RID: 1161
[Serializable]
public abstract class SaveFixWgoOperation : SaveFixOperation
{
	// Token: 0x1700053A RID: 1338
	// (get) Token: 0x06001ECF RID: 7887 RVA: 0x0003C7FE File Offset: 0x0003A9FE
	public virtual bool OccupiesUniqueId
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06001ED0 RID: 7888 RVA: 0x00091BA0 File Offset: 0x0008FDA0
	public virtual bool TryGetTargetUniqueId(out SGuid uniqueId)
	{
		uniqueId = null;
		return false;
	}

	// Token: 0x06001ED1 RID: 7889 RVA: 0x00091BA8 File Offset: 0x0008FDA8
	public virtual bool ContainsUniqueId(SGuid uniqueId)
	{
		SGuid sguid;
		return this.TryGetTargetUniqueId(out sguid) && sguid == uniqueId;
	}
}
