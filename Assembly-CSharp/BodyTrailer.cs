using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020007D0 RID: 2000
public class BodyTrailer : MonoBehaviour
{
	// Token: 0x0600336C RID: 13164 RVA: 0x000F8A41 File Offset: 0x000F6C41
	public void RollAndSetTexture()
	{
		this.bodyObject3D.modelsData[0].texturesData[0].texture = this.textures.GetRandom<Texture2D>();
	}

	// Token: 0x0400291E RID: 10526
	public List<Texture2D> textures = new List<Texture2D>();

	// Token: 0x0400291F RID: 10527
	public Object3D bodyObject3D;
}
