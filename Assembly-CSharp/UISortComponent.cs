using System;
using UnityEngine;

// Token: 0x02000841 RID: 2113
[DisallowMultipleComponent]
public class UISortComponent : MonoBehaviour
{
	// Token: 0x1700080A RID: 2058
	// (get) Token: 0x060035EF RID: 13807 RVA: 0x0010307A File Offset: 0x0010127A
	public float FloorLine
	{
		get
		{
			return this.floorLine;
		}
	}

	// Token: 0x060035F0 RID: 13808 RVA: 0x00103084 File Offset: 0x00101284
	private void OnDrawGizmosSelected()
	{
		Vector3 position = base.transform.position;
		SpriteRenderer component = base.GetComponent<SpriteRenderer>();
		float num = 100f;
		if (component != null)
		{
			num = component.sprite.bounds.size.x / 2f;
		}
		Gizmos.color = Color.magenta;
		Gizmos.DrawLine(new Vector3(position.x - num, position.y + this.floorLine, position.z), new Vector3(position.x + num, position.y + this.floorLine, position.z));
	}

	// Token: 0x04002B36 RID: 11062
	[SerializeField]
	private float floorLine;
}
