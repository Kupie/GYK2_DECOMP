using System;
using UnityEngine;

// Token: 0x02000422 RID: 1058
[Serializable]
public sealed class DurabilitySerializedItemProperty : SerializedItemProperty
{
	// Token: 0x170004DF RID: 1247
	// (get) Token: 0x06001C0A RID: 7178 RVA: 0x00082DBC File Offset: 0x00080FBC
	// (set) Token: 0x06001C0B RID: 7179 RVA: 0x00082DC4 File Offset: 0x00080FC4
	public float Durability
	{
		get
		{
			return this.durability;
		}
		set
		{
			this.durability = value;
		}
	}

	// Token: 0x06001C0C RID: 7180 RVA: 0x00082DCD File Offset: 0x00080FCD
	public DurabilitySerializedItemProperty(float durability)
	{
		this.durability = durability;
	}

	// Token: 0x06001C0D RID: 7181 RVA: 0x00082DDC File Offset: 0x00080FDC
	public override SerializedItemProperty Clone()
	{
		return new DurabilitySerializedItemProperty(this.durability);
	}

	// Token: 0x04001A90 RID: 6800
	[SerializeField]
	private float durability;
}
