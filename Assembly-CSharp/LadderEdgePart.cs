using System;
using UnityEngine;

// Token: 0x020001AD RID: 429
public class LadderEdgePart : MonoBehaviour
{
	// Token: 0x170001B3 RID: 435
	// (get) Token: 0x06000AD6 RID: 2774 RVA: 0x00036A21 File Offset: 0x00034C21
	public Part Part
	{
		get
		{
			return this.part;
		}
	}

	// Token: 0x170001B4 RID: 436
	// (get) Token: 0x06000AD7 RID: 2775 RVA: 0x00036A29 File Offset: 0x00034C29
	public BoxCollider BoxCollider
	{
		get
		{
			return this.boxCollider;
		}
	}

	// Token: 0x170001B5 RID: 437
	// (get) Token: 0x06000AD8 RID: 2776 RVA: 0x00036A31 File Offset: 0x00034C31
	public Transform TpPoint
	{
		get
		{
			return this.tpPoint;
		}
	}

	// Token: 0x170001B6 RID: 438
	// (get) Token: 0x06000AD9 RID: 2777 RVA: 0x00036A39 File Offset: 0x00034C39
	public Transform StartPoint
	{
		get
		{
			return this.startPoint;
		}
	}

	// Token: 0x170001B7 RID: 439
	// (get) Token: 0x06000ADA RID: 2778 RVA: 0x00036A41 File Offset: 0x00034C41
	public Transform BubblePointToDisplay
	{
		get
		{
			return this.bubblePointToDisplay;
		}
	}

	// Token: 0x170001B8 RID: 440
	// (get) Token: 0x06000ADB RID: 2779 RVA: 0x00036A49 File Offset: 0x00034C49
	// (set) Token: 0x06000ADC RID: 2780 RVA: 0x00036A51 File Offset: 0x00034C51
	public Ladder Ladder { get; private set; }

	// Token: 0x06000ADD RID: 2781 RVA: 0x00036A5A File Offset: 0x00034C5A
	public bool IsInLeaveRange(Vector3 position)
	{
		return Mathf.Abs(this.startPoint.position.y - position.y) < this.readyToLeaveLadderRange;
	}

	// Token: 0x06000ADE RID: 2782 RVA: 0x00036A80 File Offset: 0x00034C80
	private void Awake()
	{
		base.TryGetComponent<BoxCollider>(out this.boxCollider);
		this.Ladder = base.GetComponentInParent<Ladder>();
	}

	// Token: 0x06000ADF RID: 2783 RVA: 0x00036A9C File Offset: 0x00034C9C
	private void OnLeaveLadderRangeChange()
	{
		if (!this.boxCollider)
		{
			return;
		}
		int num = ((this.part == Part.Top) ? (-1) : 1);
		this.boxCollider.center = new Vector3(this.boxCollider.center.x, (float)num * this.readyToLeaveLadderRange / 2f, this.boxCollider.center.z);
		this.boxCollider.size = new Vector3(this.boxCollider.size.x, this.readyToLeaveLadderRange, this.boxCollider.size.z);
	}

	// Token: 0x06000AE0 RID: 2784 RVA: 0x00036B3C File Offset: 0x00034D3C
	private void OnDrawGizmos()
	{
		Color yellow = Color.yellow;
		Gizmos.color = new Color(0.9f, 0.7f, 0.3f, 0.6f);
		Gizmos.DrawCube(this.boxCollider.bounds.center, this.boxCollider.size);
		Gizmos.color = yellow;
		Gizmos.DrawWireCube(this.boxCollider.bounds.center, this.boxCollider.size);
		Gizmos.color = yellow;
		Gizmos.DrawSphere(this.TpPoint.position, 0.05f);
		Gizmos.color = yellow;
		Gizmos.DrawWireSphere(this.tpPoint.position, 0.2f);
	}

	// Token: 0x04000C48 RID: 3144
	[SerializeField]
	private Part part;

	// Token: 0x04000C49 RID: 3145
	[SerializeField]
	private BoxCollider boxCollider;

	// Token: 0x04000C4A RID: 3146
	[SerializeField]
	private Transform tpPoint;

	// Token: 0x04000C4B RID: 3147
	[SerializeField]
	private Transform startPoint;

	// Token: 0x04000C4C RID: 3148
	[SerializeField]
	private Transform bubblePointToDisplay;

	// Token: 0x04000C4D RID: 3149
	[SerializeField]
	[Range(0.01f, 1f)]
	private float readyToLeaveLadderRange = 0.1f;
}
