using System;
using UnityEngine;

// Token: 0x0200013E RID: 318
[ExecuteInEditMode]
[RequireComponent(typeof(Collider))]
public class BuildArea : MonoBehaviour
{
	// Token: 0x17000126 RID: 294
	// (get) Token: 0x06000772 RID: 1906 RVA: 0x000233C0 File Offset: 0x000215C0
	public string Id
	{
		get
		{
			return this.id;
		}
	}

	// Token: 0x17000127 RID: 295
	// (get) Token: 0x06000773 RID: 1907 RVA: 0x000233C8 File Offset: 0x000215C8
	public Collider Collider
	{
		get
		{
			return this.collider;
		}
	}

	// Token: 0x17000128 RID: 296
	// (get) Token: 0x06000774 RID: 1908 RVA: 0x000233D0 File Offset: 0x000215D0
	public bool HasRotationRequirement
	{
		get
		{
			return this.hasRotationRequirement;
		}
	}

	// Token: 0x17000129 RID: 297
	// (get) Token: 0x06000775 RID: 1909 RVA: 0x000233D8 File Offset: 0x000215D8
	public int RotationRequirement
	{
		get
		{
			return this.rotationIndexRequirement;
		}
	}

	// Token: 0x04000960 RID: 2400
	[SerializeField]
	private Collider collider;

	// Token: 0x04000961 RID: 2401
	[SerializeField]
	private string id;

	// Token: 0x04000962 RID: 2402
	[SerializeField]
	private bool hasRotationRequirement;

	// Token: 0x04000963 RID: 2403
	[SerializeField]
	private int rotationIndexRequirement = -1;

	// Token: 0x04000964 RID: 2404
	public bool fullCoveringMode;

	// Token: 0x04000965 RID: 2405
	public bool ignoreForPointerPlacement;

	// Token: 0x04000966 RID: 2406
	public bool foprceShowAsBuffAreaForPointerPlacement;
}
