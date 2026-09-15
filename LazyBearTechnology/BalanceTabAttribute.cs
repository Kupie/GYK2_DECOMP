using System;

// Token: 0x02000003 RID: 3
[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public class BalanceTabAttribute : Attribute
{
	// Token: 0x17000001 RID: 1
	// (get) Token: 0x06000002 RID: 2 RVA: 0x000020BE File Offset: 0x000002BE
	public string TabName { get; }

	// Token: 0x17000002 RID: 2
	// (get) Token: 0x06000003 RID: 3 RVA: 0x000020C6 File Offset: 0x000002C6
	public int StartRow { get; }

	// Token: 0x06000004 RID: 4 RVA: 0x000020CE File Offset: 0x000002CE
	public BalanceTabAttribute(string tabName, int startRow = -1)
	{
		this.TabName = tabName;
		this.StartRow = startRow;
	}
}
