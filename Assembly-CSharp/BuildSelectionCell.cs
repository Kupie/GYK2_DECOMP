using System;
using UnityEngine;

// Token: 0x0200014E RID: 334
public class BuildSelectionCell : MonoBehaviour
{
	// Token: 0x17000149 RID: 329
	// (get) Token: 0x060007FC RID: 2044 RVA: 0x00027470 File Offset: 0x00025670
	// (set) Token: 0x060007FD RID: 2045 RVA: 0x00027478 File Offset: 0x00025678
	public bool IsAvailableForBuild
	{
		get
		{
			return this.isAvailableForBuild;
		}
		set
		{
			this.isAvailableForBuild = value;
			this.spriteRenderer.color = (this.isAvailableForBuild ? this.availableColor : this.unavailableColor);
		}
	}

	// Token: 0x1700014A RID: 330
	// (get) Token: 0x060007FE RID: 2046 RVA: 0x000274A2 File Offset: 0x000256A2
	public Bounds SpriteBounds
	{
		get
		{
			return this.spriteRenderer.bounds;
		}
	}

	// Token: 0x060007FF RID: 2047 RVA: 0x000274AF File Offset: 0x000256AF
	public int OverlapBoxNonAlloc(Collider[] overlapColliders, int mask)
	{
		return Physics.OverlapBoxNonAlloc(base.transform.position, BuildConsts.CASTING_BOX_HALF_EXTENTS, overlapColliders, Quaternion.identity, mask);
	}

	// Token: 0x06000800 RID: 2048 RVA: 0x000274D0 File Offset: 0x000256D0
	public void SetVariation(BuildSelectionCellVariation variation)
	{
		this.spriteRenderer.sprite = this.spriteVariations[(int)variation];
		this.spriteRenderer.transform.localPosition = Vector3.Scale(new Vector3((float)this.spriteXZPixelShifts[(int)variation].x, 0f, (float)this.spriteXZPixelShifts[(int)variation].y), VisualConsts.XYZ_STEP);
	}

	// Token: 0x040009E6 RID: 2534
	[SerializeField]
	private SpriteRenderer spriteRenderer;

	// Token: 0x040009E7 RID: 2535
	[Space]
	[SerializeField]
	private Color availableColor = Color.green;

	// Token: 0x040009E8 RID: 2536
	[SerializeField]
	private Color unavailableColor = Color.red;

	// Token: 0x040009E9 RID: 2537
	[Space]
	[SerializeField]
	private Sprite[] spriteVariations = new Sprite[13];

	// Token: 0x040009EA RID: 2538
	[SerializeField]
	private Vector2Int[] spriteXZPixelShifts = new Vector2Int[13];

	// Token: 0x040009EB RID: 2539
	private Vector3 localPos;

	// Token: 0x040009EC RID: 2540
	private bool isAvailableForBuild;
}
