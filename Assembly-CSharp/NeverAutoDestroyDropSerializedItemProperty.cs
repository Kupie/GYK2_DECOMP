using System;

// Token: 0x02000424 RID: 1060
[Serializable]
public sealed class NeverAutoDestroyDropSerializedItemProperty : SerializedItemProperty
{
	// Token: 0x06001C0F RID: 7183 RVA: 0x00082DE9 File Offset: 0x00080FE9
	public override SerializedItemProperty Clone()
	{
		return new NeverAutoDestroyDropSerializedItemProperty();
	}
}
