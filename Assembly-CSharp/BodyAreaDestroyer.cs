using System;
using UnityEngine;

// Token: 0x020007CF RID: 1999
public class BodyAreaDestroyer : MonoBehaviour
{
	// Token: 0x0600336A RID: 13162 RVA: 0x000F8A11 File Offset: 0x000F6C11
	private void OnTriggerEnter(Collider other)
	{
		if (!other.GetComponentInParent<Rigidbody>().gameObject.name.Contains("Body_Trailer"))
		{
			return;
		}
		this.spawner.RemoveBody(other.gameObject);
	}

	// Token: 0x0400291D RID: 10525
	public AreaBodiesSpawner spawner;
}
