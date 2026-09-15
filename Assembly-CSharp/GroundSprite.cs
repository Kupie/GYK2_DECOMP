using System;
using UnityEngine;

// Token: 0x02000507 RID: 1287
[RequireComponent(typeof(SpriteRenderer))]
public class GroundSprite : MonoBehaviour
{
	// Token: 0x06002156 RID: 8534 RVA: 0x0009D51C File Offset: 0x0009B71C
	private void OnDrawGizmosSelected()
	{
		this.EnsureCorrectScaleForHorizontalSprite(base.transform);
	}

	// Token: 0x06002157 RID: 8535 RVA: 0x0009D52C File Offset: 0x0009B72C
	private void EnsureCorrectScaleForHorizontalSprite(Transform t)
	{
		Vector3 localScale = t.localScale;
		if (localScale.y > 0f || localScale.z > 0f)
		{
			this.SetCorrectLocalScaleForHorizontalSprite(t);
		}
	}

	// Token: 0x06002158 RID: 8536 RVA: 0x0009D564 File Offset: 0x0009B764
	private void SetCorrectLocalScaleForHorizontalSprite(Transform t)
	{
		Vector3 localScale = t.localScale;
		localScale.y = 1.25f;
		localScale.z = 1f;
		t.localScale = localScale;
	}
}
