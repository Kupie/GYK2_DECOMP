using System;

// Token: 0x02000488 RID: 1160
[Serializable]
public abstract class SaveFixOperation
{
	// Token: 0x17000537 RID: 1335
	// (get) Token: 0x06001EC9 RID: 7881 RVA: 0x0003C7FE File Offset: 0x0003A9FE
	public virtual int InfoCount
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x06001ECA RID: 7882
	public abstract string Info();

	// Token: 0x17000538 RID: 1336
	// (get) Token: 0x06001ECB RID: 7883 RVA: 0x00091B4C File Offset: 0x0008FD4C
	public virtual string Summary
	{
		get
		{
			string summaryBody = this.SummaryBody;
			string text = this.Info();
			if (!string.IsNullOrEmpty(summaryBody))
			{
				return "[" + text + "]  " + summaryBody;
			}
			return "[" + text + "]";
		}
	}

	// Token: 0x17000539 RID: 1337
	// (get) Token: 0x06001ECC RID: 7884 RVA: 0x00054915 File Offset: 0x00052B15
	protected virtual string SummaryBody
	{
		get
		{
			return null;
		}
	}

	// Token: 0x06001ECD RID: 7885
	public abstract void Apply(SaveFixContext ctx);

	// Token: 0x04001BCF RID: 7119
	public bool isEnabled = true;
}
