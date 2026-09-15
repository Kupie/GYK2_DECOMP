using System;
using UnityEngine;

// Token: 0x020001AB RID: 427
public class Ladder : MonoBehaviour
{
	// Token: 0x170001AB RID: 427
	// (get) Token: 0x06000AC0 RID: 2752 RVA: 0x00036493 File Offset: 0x00034693
	public LadderEdgePart TopPart
	{
		get
		{
			return this.topPart;
		}
	}

	// Token: 0x170001AC RID: 428
	// (get) Token: 0x06000AC1 RID: 2753 RVA: 0x0003649B File Offset: 0x0003469B
	public LadderEdgePart BotPart
	{
		get
		{
			return this.botPart;
		}
	}

	// Token: 0x06000AC2 RID: 2754 RVA: 0x000364A4 File Offset: 0x000346A4
	public LadderEdgePart GetNearestLadderPart(Vector3 position)
	{
		float magnitude = (position - this.TopPart.StartPoint.position).magnitude;
		if ((position - this.BotPart.StartPoint.position).magnitude >= magnitude)
		{
			return this.TopPart;
		}
		return this.BotPart;
	}

	// Token: 0x06000AC3 RID: 2755 RVA: 0x00036500 File Offset: 0x00034700
	private void OnDrawGizmos()
	{
		Gizmos.color = Color.cyan;
		LadderEdgePart ladderEdgePart = this.topPart;
		if ((ladderEdgePart != null) ? ladderEdgePart.StartPoint : null)
		{
			LadderEdgePart ladderEdgePart2 = this.botPart;
			if ((ladderEdgePart2 != null) ? ladderEdgePart2.StartPoint : null)
			{
				Gizmos.DrawLine(this.topPart.StartPoint.position, this.botPart.StartPoint.position);
			}
		}
	}

	// Token: 0x04000C36 RID: 3126
	[SerializeField]
	private LadderEdgePart topPart;

	// Token: 0x04000C37 RID: 3127
	[SerializeField]
	private LadderEdgePart botPart;
}
