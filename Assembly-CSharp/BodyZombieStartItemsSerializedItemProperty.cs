using System;

// Token: 0x02000421 RID: 1057
[Serializable]
public sealed class BodyZombieStartItemsSerializedItemProperty : SerializedItemProperty
{
	// Token: 0x06001C08 RID: 7176 RVA: 0x00082D9D File Offset: 0x00080F9D
	public override SerializedItemProperty Clone()
	{
		return new BodyZombieStartItemsSerializedItemProperty
		{
			armorId = this.armorId,
			handsId = this.handsId
		};
	}

	// Token: 0x04001A8E RID: 6798
	public string armorId;

	// Token: 0x04001A8F RID: 6799
	public string handsId;
}
