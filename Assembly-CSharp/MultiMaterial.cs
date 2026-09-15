using System;
using UnityEngine;

// Token: 0x0200050B RID: 1291
public abstract class MultiMaterial : ScriptableObject
{
	// Token: 0x06002167 RID: 8551
	public abstract Material GetMaterial(PlatformSpecificMaterialType platform);
}
