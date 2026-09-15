using System;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x02000797 RID: 1943
[RequireComponent(typeof(SpriteRenderer))]
public class SpriteFlipHandler : MonoBehaviour
{
	// Token: 0x06003210 RID: 12816 RVA: 0x000F03AF File Offset: 0x000EE5AF
	private void Awake()
	{
		if (this.rend == null)
		{
			this.rend = base.GetComponent<SpriteRenderer>();
		}
	}

	// Token: 0x06003211 RID: 12817 RVA: 0x000F03CC File Offset: 0x000EE5CC
	private void LateUpdate()
	{
		if (base.transform.lossyScale.x < 0f)
		{
			base.transform.localScale = new Vector3(-base.transform.localScale.x, base.transform.localScale.y, base.transform.localScale.z);
			this.rend.flipX = !this.rend.flipX;
		}
	}

	// Token: 0x0400283B RID: 10299
	[SerializeField]
	[FormerlySerializedAs("renderer")]
	private SpriteRenderer rend;
}
