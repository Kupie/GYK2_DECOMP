using System;
using UnityEngine;

// Token: 0x02000425 RID: 1061
[Serializable]
public sealed class WhiteListFilterSerializedItemProperty : SerializedItemProperty
{
	// Token: 0x170004E0 RID: 1248
	// (get) Token: 0x06001C11 RID: 7185 RVA: 0x00082DF0 File Offset: 0x00080FF0
	// (set) Token: 0x06001C12 RID: 7186 RVA: 0x00082DF8 File Offset: 0x00080FF8
	public WhiteListItemFilter WhiteList
	{
		get
		{
			return this.whiteList;
		}
		set
		{
			this.whiteList = value;
		}
	}

	// Token: 0x06001C13 RID: 7187 RVA: 0x00082E01 File Offset: 0x00081001
	public WhiteListFilterSerializedItemProperty(WhiteListItemFilter whiteList)
	{
		this.whiteList = whiteList;
	}

	// Token: 0x06001C14 RID: 7188 RVA: 0x00082E10 File Offset: 0x00081010
	public override SerializedItemProperty Clone()
	{
		return new WhiteListFilterSerializedItemProperty(this.whiteList);
	}

	// Token: 0x04001A91 RID: 6801
	[SerializeField]
	private WhiteListItemFilter whiteList;
}
