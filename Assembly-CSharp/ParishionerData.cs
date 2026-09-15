using System;

// Token: 0x02000A0F RID: 2575
[Serializable]
public class ParishionerData
{
	// Token: 0x06004545 RID: 17733 RVA: 0x00147ABB File Offset: 0x00145CBB
	public ParishionerData(SermonDef sermonDef, int faith)
	{
		this.sermonDef = sermonDef;
		this.faithToDrop = faith;
	}

	// Token: 0x04003618 RID: 13848
	public SermonDef sermonDef;

	// Token: 0x04003619 RID: 13849
	public readonly int faithToDrop;
}
