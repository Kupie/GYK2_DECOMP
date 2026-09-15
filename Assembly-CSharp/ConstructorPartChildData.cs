using System;
using UnityEngine;

// Token: 0x02000A9F RID: 2719
[Serializable]
public class ConstructorPartChildData
{
	// Token: 0x0400396E RID: 14702
	public bool canNotBeBaked;

	// Token: 0x0400396F RID: 14703
	public string pathToObject;

	// Token: 0x04003970 RID: 14704
	public Vector3 localPosition;

	// Token: 0x04003971 RID: 14705
	public Vector3 localScale = Vector3.one;

	// Token: 0x04003972 RID: 14706
	public Quaternion rotation;

	// Token: 0x04003973 RID: 14707
	public BurstableChunkBoundsPair chunkBounds;

	// Token: 0x04003974 RID: 14708
	[NonSerialized]
	public ConstructorPartChildObject view;

	// Token: 0x04003975 RID: 14709
	public Texture2D lut;
}
