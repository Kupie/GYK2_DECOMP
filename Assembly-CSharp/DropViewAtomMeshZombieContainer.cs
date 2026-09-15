using System;
using UnityEngine;

// Token: 0x02000199 RID: 409
public class DropViewAtomMeshZombieContainer : MonoBehaviour
{
	// Token: 0x06000A65 RID: 2661 RVA: 0x00034930 File Offset: 0x00032B30
	private void Awake()
	{
		this.OnDisable();
	}

	// Token: 0x06000A66 RID: 2662 RVA: 0x00034938 File Offset: 0x00032B38
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

	// Token: 0x06000A67 RID: 2663 RVA: 0x0003495C File Offset: 0x00032B5C
	private void OnDisable()
	{
		if (this.colliderContainer)
		{
			this.colliderContainer.SetActive(false);
		}
	}

	// Token: 0x04000BD1 RID: 3025
	public Transform container;

	// Token: 0x04000BD2 RID: 3026
	public SpriteText spriteText;

	// Token: 0x04000BD3 RID: 3027
	public bool isPhysical = true;

	// Token: 0x04000BD4 RID: 3028
	public GameObject colliderContainer;
}
