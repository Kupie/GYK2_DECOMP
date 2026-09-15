using System;

// Token: 0x020005CF RID: 1487
public class WgoPartStateData
{
	// Token: 0x06002712 RID: 10002 RVA: 0x000B7B64 File Offset: 0x000B5D64
	public WgoPartStateData(string id, int rotationIndex, bool mirror)
	{
		this.id = id;
		this.rotationIndex = rotationIndex;
		this.mirror = mirror;
	}

	// Token: 0x04002183 RID: 8579
	public string id;

	// Token: 0x04002184 RID: 8580
	public int rotationIndex;

	// Token: 0x04002185 RID: 8581
	public bool mirror;
}
