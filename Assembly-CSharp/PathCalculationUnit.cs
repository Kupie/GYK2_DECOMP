using System;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

// Token: 0x02000736 RID: 1846
public class PathCalculationUnit : MonoBehaviour
{
	// Token: 0x1700076E RID: 1902
	// (set) Token: 0x06003036 RID: 12342 RVA: 0x000E77E2 File Offset: 0x000E59E2
	public List<Vector3> VectorPath
	{
		set
		{
			this.vectorPath = value;
		}
	}

	// Token: 0x06003037 RID: 12343 RVA: 0x000E77EC File Offset: 0x000E59EC
	private void OnDrawGizmos()
	{
		if (this.vectorPath == null || this.vectorPath.Count < 2)
		{
			return;
		}
		int num = this.vectorPath.Count - 1;
		for (int i = 0; i < num; i++)
		{
			float num2 = (float)i / (float)num;
			Gizmos.color = Color.Lerp(Color.red, Color.green, num2);
			Gizmos.DrawLine(this.vectorPath[i], this.vectorPath[i + 1]);
		}
	}

	// Token: 0x0400270E RID: 9998
	public Seeker seeker;

	// Token: 0x0400270F RID: 9999
	private List<Vector3> vectorPath = new List<Vector3>();
}
