using System;

// Token: 0x02000420 RID: 1056
[Serializable]
public sealed class BodyZombieSkinSerializedItemProperty : SerializedItemProperty
{
	// Token: 0x06001C06 RID: 7174 RVA: 0x00082D72 File Offset: 0x00080F72
	public override SerializedItemProperty Clone()
	{
		return new BodyZombieSkinSerializedItemProperty
		{
			body = this.body,
			head = this.head,
			headLut = this.headLut
		};
	}

	// Token: 0x04001A8B RID: 6795
	public int body;

	// Token: 0x04001A8C RID: 6796
	public int head;

	// Token: 0x04001A8D RID: 6797
	public string headLut;
}
