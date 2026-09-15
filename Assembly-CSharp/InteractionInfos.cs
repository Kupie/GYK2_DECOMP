using System;
using System.Collections.Generic;

// Token: 0x02000632 RID: 1586
public class InteractionInfos
{
	// Token: 0x170006C1 RID: 1729
	// (get) Token: 0x06002A3D RID: 10813 RVA: 0x000C759E File Offset: 0x000C579E
	public bool IsEmpty
	{
		get
		{
			return this.list.Count == 0;
		}
	}

	// Token: 0x06002A3E RID: 10814 RVA: 0x000C75AE File Offset: 0x000C57AE
	public InteractionInfos()
	{
	}

	// Token: 0x06002A3F RID: 10815 RVA: 0x000C75C1 File Offset: 0x000C57C1
	public InteractionInfos(InteractionInfo interactionInfo)
	{
		this.list = new List<InteractionInfo>();
		this.Add(interactionInfo);
	}

	// Token: 0x06002A40 RID: 10816 RVA: 0x000C75E6 File Offset: 0x000C57E6
	public void Add(InteractionInfo interactionInfo)
	{
		this.list.Add(interactionInfo);
	}

	// Token: 0x04002322 RID: 8994
	public List<InteractionInfo> list = new List<InteractionInfo>();
}
