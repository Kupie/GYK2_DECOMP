using System;
using UnityEngine;

// Token: 0x02000737 RID: 1847
public class RecastFloor : MonoBehaviour
{
	// Token: 0x06003039 RID: 12345 RVA: 0x000E7877 File Offset: 0x000E5A77
	public void UpdateParameters(Vector3 position, Vector2 size)
	{
		base.transform.position = position;
		base.transform.localScale = new Vector3(size.x, 0f, size.y);
	}
}
