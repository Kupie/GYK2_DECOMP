using System;
using UnityEngine;

// Token: 0x020001A6 RID: 422
public class Dummy : MonoBehaviour
{
	// Token: 0x06000AB1 RID: 2737 RVA: 0x000361F1 File Offset: 0x000343F1
	private void OnTriggerEnter(Collider other)
	{
		if (other.GetComponent<PikeHitBox>() != null)
		{
			this.worldFx.Play(null, null);
			this.animator.SetTrigger("Hit");
		}
	}

	// Token: 0x04000C2D RID: 3117
	[SerializeField]
	private WorldFX worldFx;

	// Token: 0x04000C2E RID: 3118
	[SerializeField]
	private Animator animator;
}
