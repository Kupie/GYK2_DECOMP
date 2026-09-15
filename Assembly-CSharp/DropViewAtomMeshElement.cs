using System;
using UnityEngine;

// Token: 0x02000198 RID: 408
public class DropViewAtomMeshElement : MonoBehaviour
{
	// Token: 0x06000A61 RID: 2657 RVA: 0x000348DA File Offset: 0x00032ADA
	private void Awake()
	{
		this.OnDisable();
	}

	// Token: 0x06000A62 RID: 2658 RVA: 0x000348E2 File Offset: 0x00032AE2
	private void OnEnable()
	{
		if (!this.isPhysical)
		{
			return;
		}
		if (this.colliderContainer)
		{
			this.colliderContainer.SetActive(true);
		}
	}

	// Token: 0x06000A63 RID: 2659 RVA: 0x00034906 File Offset: 0x00032B06
	private void OnDisable()
	{
		if (this.colliderContainer)
		{
			this.colliderContainer.SetActive(false);
		}
	}

	// Token: 0x04000BCC RID: 3020
	public Object3D object3D;

	// Token: 0x04000BCD RID: 3021
	public SpriteText spriteText;

	// Token: 0x04000BCE RID: 3022
	public bool isPhysical = true;

	// Token: 0x04000BCF RID: 3023
	public GameObject colliderContainer;

	// Token: 0x04000BD0 RID: 3024
	public Collider physicsCollider;
}
