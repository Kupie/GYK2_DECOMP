using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001A9 RID: 425
[Serializable]
public class IndoorAreaData
{
	// Token: 0x06000ABB RID: 2747 RVA: 0x000363D4 File Offset: 0x000345D4
	public bool ContainsXZ(Vector3 worldPos)
	{
		if (this.bounds == null)
		{
			return false;
		}
		for (int i = 0; i < this.bounds.Count; i++)
		{
			IndoorAreaBoundData indoorAreaBoundData = this.bounds[i];
			if (indoorAreaBoundData != null && indoorAreaBoundData.ContainsXZ(worldPos))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000ABC RID: 2748 RVA: 0x00036420 File Offset: 0x00034620
	public float GetXZArea()
	{
		float num = 0f;
		if (this.bounds == null)
		{
			return num;
		}
		for (int i = 0; i < this.bounds.Count; i++)
		{
			IndoorAreaBoundData indoorAreaBoundData = this.bounds[i];
			if (indoorAreaBoundData != null)
			{
				num += indoorAreaBoundData.GetXZArea();
			}
		}
		return num;
	}

	// Token: 0x04000C33 RID: 3123
	public string id;

	// Token: 0x04000C34 RID: 3124
	public List<IndoorAreaBoundData> bounds = new List<IndoorAreaBoundData>();
}
