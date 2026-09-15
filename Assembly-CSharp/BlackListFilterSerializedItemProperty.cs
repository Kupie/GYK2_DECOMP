using System;
using UnityEngine;

// Token: 0x0200041F RID: 1055
[Serializable]
public sealed class BlackListFilterSerializedItemProperty : SerializedItemProperty
{
	// Token: 0x170004DE RID: 1246
	// (get) Token: 0x06001C02 RID: 7170 RVA: 0x00082D45 File Offset: 0x00080F45
	// (set) Token: 0x06001C03 RID: 7171 RVA: 0x00082D4D File Offset: 0x00080F4D
	public BlackListItemFilter BlackList
	{
		get
		{
			return this.blackList;
		}
		set
		{
			this.blackList = value;
		}
	}

	// Token: 0x06001C04 RID: 7172 RVA: 0x00082D56 File Offset: 0x00080F56
	public BlackListFilterSerializedItemProperty(BlackListItemFilter blackList)
	{
		this.blackList = blackList;
	}

	// Token: 0x06001C05 RID: 7173 RVA: 0x00082D65 File Offset: 0x00080F65
	public override SerializedItemProperty Clone()
	{
		return new BlackListFilterSerializedItemProperty(this.blackList);
	}

	// Token: 0x04001A8A RID: 6794
	[SerializeField]
	private BlackListItemFilter blackList;
}
