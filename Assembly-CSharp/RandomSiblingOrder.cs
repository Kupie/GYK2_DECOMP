using System;
using UnityEngine;

// Token: 0x02000B40 RID: 2880
public class RandomSiblingOrder : MonoBehaviour
{
	// Token: 0x06004C8B RID: 19595 RVA: 0x00169650 File Offset: 0x00167850
	public void RandomizeSiblingOrder()
	{
		if (this.parent == null)
		{
			Debug.LogWarning("parent was not specified. Using self.");
			this.parent = base.transform;
		}
		foreach (object obj in this.parent)
		{
			Transform transform = (Transform)obj;
			int num = global::UnityEngine.Random.Range(0, 1000);
			transform.SetSiblingIndex(num);
		}
	}

	// Token: 0x04003DAE RID: 15790
	[SerializeField]
	private Transform parent;
}
