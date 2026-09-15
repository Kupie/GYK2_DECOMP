using System;
using System.Collections.Generic;

// Token: 0x02000AF4 RID: 2804
[Serializable]
public class PhrasesByWgo
{
	// Token: 0x06004AE1 RID: 19169 RVA: 0x001616F2 File Offset: 0x0015F8F2
	public PhrasesByWgo(string wgoId, List<string> phrases)
	{
		this.wgoId = wgoId;
		this.phrases = phrases;
	}

	// Token: 0x06004AE2 RID: 19170 RVA: 0x00161708 File Offset: 0x0015F908
	public PhrasesByWgo(string wgoId)
	{
		this.wgoId = wgoId;
		this.phrases = new List<string>();
	}

	// Token: 0x04003C78 RID: 15480
	public string wgoId;

	// Token: 0x04003C79 RID: 15481
	public List<string> phrases;
}
