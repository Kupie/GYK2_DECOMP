using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000508 RID: 1288
public class GroundTilesLayout : MonoBehaviour
{
	// Token: 0x0600215A RID: 8538 RVA: 0x0009D598 File Offset: 0x0009B798
	private void Redraw()
	{
		this.meshFilter.gameObject.SetActive(false);
		for (int i = 0; i < this.createdList.Count; i++)
		{
			if (this.createdList[i] == null)
			{
				this.createdList.RemoveAt(i);
				i--;
			}
			else
			{
				this.createdList[i].SetActive(false);
			}
		}
		Bounds bounds = this.meshFilter.GetComponent<Renderer>().bounds;
		float x = bounds.size.x;
		float z = bounds.size.z;
		int num = 0;
		int count = this.createdList.Count;
		for (int j = this.nzTiles; j < this.zTiles; j++)
		{
			for (int k = this.nxTiles; k < this.xTiles; k++)
			{
				float num2 = (float)k * x;
				float num3 = (float)j * z;
				Vector3 vector = base.transform.position + new Vector3(num2, 0f, num3);
				GameObject gameObject;
				if (num < count)
				{
					gameObject = this.createdList[num];
					gameObject.transform.position = vector;
				}
				else
				{
					gameObject = global::UnityEngine.Object.Instantiate<GameObject>(this.meshFilter.gameObject, vector, base.transform.rotation);
					this.createdList.Add(gameObject);
				}
				gameObject.transform.parent = base.transform;
				gameObject.SetActive(true);
				num++;
			}
		}
	}

	// Token: 0x04001DEE RID: 7662
	[SerializeField]
	private MeshFilter meshFilter;

	// Token: 0x04001DEF RID: 7663
	[Space]
	[SerializeField]
	private int xTiles = 1;

	// Token: 0x04001DF0 RID: 7664
	[SerializeField]
	private int nxTiles;

	// Token: 0x04001DF1 RID: 7665
	[SerializeField]
	private int zTiles = 1;

	// Token: 0x04001DF2 RID: 7666
	[SerializeField]
	private int nzTiles;

	// Token: 0x04001DF3 RID: 7667
	[SerializeField]
	[HideInInspector]
	private List<GameObject> createdList = new List<GameObject>();
}
