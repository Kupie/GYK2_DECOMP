using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200001B RID: 27
public class ImageCollidable : Image
{
	// Token: 0x06000076 RID: 118 RVA: 0x00003F34 File Offset: 0x00002134
	public override bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
	{
		bool flag = !(this.collider2D != null) || this.collider2D.OverlapPoint(screenPoint);
		return base.IsRaycastLocationValid(screenPoint, eventCamera) && flag;
	}

	// Token: 0x0400005C RID: 92
	[SerializeField]
	[Space]
	public Collider2D collider2D;
}
