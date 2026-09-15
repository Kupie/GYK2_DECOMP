using System;
using Pathfinding;
using UnityEngine;

// Token: 0x02000585 RID: 1413
[Serializable]
public class DockPointData
{
	// Token: 0x170005D9 RID: 1497
	// (get) Token: 0x0600242C RID: 9260 RVA: 0x000AA4E6 File Offset: 0x000A86E6
	// (set) Token: 0x0600242D RID: 9261 RVA: 0x000AA4EE File Offset: 0x000A86EE
	public DockPointData.Baked BakedData { get; set; }

	// Token: 0x1400005E RID: 94
	// (add) Token: 0x0600242E RID: 9262 RVA: 0x000AA4F8 File Offset: 0x000A86F8
	// (remove) Token: 0x0600242F RID: 9263 RVA: 0x000AA530 File Offset: 0x000A8730
	public event Action OnOccupiedStatusChanged;

	// Token: 0x170005DA RID: 1498
	// (get) Token: 0x06002430 RID: 9264 RVA: 0x000AA565 File Offset: 0x000A8765
	public Direction Direction
	{
		get
		{
			return this.BakedData.Direction;
		}
	}

	// Token: 0x170005DB RID: 1499
	// (get) Token: 0x06002431 RID: 9265 RVA: 0x000AA572 File Offset: 0x000A8772
	// (set) Token: 0x06002432 RID: 9266 RVA: 0x000AA57A File Offset: 0x000A877A
	public SGuid OccupiedBy
	{
		get
		{
			return this.occupiedBy;
		}
		set
		{
			this.occupiedBy = value;
		}
	}

	// Token: 0x170005DC RID: 1500
	// (get) Token: 0x06002433 RID: 9267 RVA: 0x000AA583 File Offset: 0x000A8783
	public bool IsOccupied
	{
		get
		{
			return this.occupiedBy != SGuid.Empty;
		}
	}

	// Token: 0x06002434 RID: 9268 RVA: 0x000AA595 File Offset: 0x000A8795
	public Vector3 GetPosFrom(Vector3 pos)
	{
		return pos + this.BakedData.Position;
	}

	// Token: 0x06002435 RID: 9269 RVA: 0x000AA5A8 File Offset: 0x000A87A8
	public void Occupy(SGuid occupantId)
	{
		this.occupiedBy = occupantId;
		Action onOccupiedStatusChanged = this.OnOccupiedStatusChanged;
		if (onOccupiedStatusChanged == null)
		{
			return;
		}
		onOccupiedStatusChanged();
	}

	// Token: 0x06002436 RID: 9270 RVA: 0x000AA5C1 File Offset: 0x000A87C1
	public void UnOccupy()
	{
		this.occupiedBy = SGuid.Empty;
		Action onOccupiedStatusChanged = this.OnOccupiedStatusChanged;
		if (onOccupiedStatusChanged == null)
		{
			return;
		}
		onOccupiedStatusChanged();
	}

	// Token: 0x06002437 RID: 9271 RVA: 0x000AA5DE File Offset: 0x000A87DE
	public bool IsOccupiedBy(SGuid sGuid)
	{
		return this.occupiedBy == sGuid && this.occupiedBy != SGuid.Empty;
	}

	// Token: 0x06002438 RID: 9272 RVA: 0x000AA600 File Offset: 0x000A8800
	public Vector3 GetDropPos(ItemSize itemSize, float dropOffsetRight, float dropOffsetForward, float playerBackOffset)
	{
		Direction direction = this.BakedData.Direction;
		Vector3 vector = this.BakedData.Position;
		if (direction == Direction.Up && itemSize == ItemSize.Big)
		{
			vector += new Vector3(0f, 0f, -playerBackOffset);
		}
		else
		{
			Vector2 vector2;
			switch (direction)
			{
			case Direction.Right:
				vector2 = new Vector2(dropOffsetRight, dropOffsetForward);
				break;
			case Direction.Up:
				vector2 = new Vector2(playerBackOffset, dropOffsetRight);
				break;
			case Direction.Left:
				vector2 = new Vector2(-dropOffsetRight, playerBackOffset);
				break;
			case Direction.Down:
				vector2 = new Vector2(dropOffsetForward, dropOffsetRight);
				break;
			default:
				vector2 = Vector2.zero;
				break;
			}
			Vector2 vector3 = vector2;
			Direction direction2 = direction.OppositeDir();
			vector += direction2.ConvertToVector3() * vector3.x + direction2.ClockwiseDir().ConvertToVector3() * vector3.y;
		}
		vector.y = this.BakedData.Position.y;
		return vector;
	}

	// Token: 0x06002439 RID: 9273 RVA: 0x000AA6F0 File Offset: 0x000A88F0
	public bool IsOnRecast(Vector3 parentPos, RecastGraph recastGraph)
	{
		return recastGraph.IsPointOnNavmesh(this.GetPosFrom(parentPos.XZ()));
	}

	// Token: 0x04002021 RID: 8225
	[SerializeField]
	private SGuid occupiedBy = SGuid.Empty;

	// Token: 0x02000586 RID: 1414
	public enum Availability
	{
		// Token: 0x04002025 RID: 8229
		All,
		// Token: 0x04002026 RID: 8230
		OnlyOccupied,
		// Token: 0x04002027 RID: 8231
		OnlyNotOccupied
	}

	// Token: 0x02000587 RID: 1415
	public enum Filter
	{
		// Token: 0x04002029 RID: 8233
		All,
		// Token: 0x0400202A RID: 8234
		OnlyZombie,
		// Token: 0x0400202B RID: 8235
		OnlyNotZombie
	}

	// Token: 0x02000588 RID: 1416
	[Serializable]
	public class Baked
	{
		// Token: 0x170005DD RID: 1501
		// (get) Token: 0x0600243B RID: 9275 RVA: 0x000AA71C File Offset: 0x000A891C
		public Vector3 Position
		{
			get
			{
				return this.position;
			}
		}

		// Token: 0x170005DE RID: 1502
		// (get) Token: 0x0600243C RID: 9276 RVA: 0x000AA724 File Offset: 0x000A8924
		public Direction Direction
		{
			get
			{
				return this.direction;
			}
		}

		// Token: 0x170005DF RID: 1503
		// (get) Token: 0x0600243D RID: 9277 RVA: 0x000AA72C File Offset: 0x000A892C
		public bool IsForZombie
		{
			get
			{
				return this.isForZombie;
			}
		}

		// Token: 0x170005E0 RID: 1504
		// (get) Token: 0x0600243E RID: 9278 RVA: 0x000AA734 File Offset: 0x000A8934
		public bool HideInFighting
		{
			get
			{
				return this.hideInFighting;
			}
		}

		// Token: 0x170005E1 RID: 1505
		// (get) Token: 0x0600243F RID: 9279 RVA: 0x000AA73C File Offset: 0x000A893C
		public DockPointTag DockPointTag
		{
			get
			{
				return this.dockPointTag;
			}
		}

		// Token: 0x170005E2 RID: 1506
		// (get) Token: 0x06002440 RID: 9280 RVA: 0x000AA744 File Offset: 0x000A8944
		public bool DisableTargetingForCaretaker
		{
			get
			{
				return this.disableTargetingForCaretaker;
			}
		}

		// Token: 0x170005E3 RID: 1507
		// (get) Token: 0x06002441 RID: 9281 RVA: 0x000AA74C File Offset: 0x000A894C
		public bool DontUseForWorkerPlacement
		{
			get
			{
				return this.dontUseForWorkerPlacement;
			}
		}

		// Token: 0x06002442 RID: 9282 RVA: 0x00021B94 File Offset: 0x0001FD94
		public Baked()
		{
		}

		// Token: 0x06002443 RID: 9283 RVA: 0x000AA754 File Offset: 0x000A8954
		public Baked(DockPoint dockPoint)
		{
			WgoPart componentInParent = dockPoint.GetComponentInParent<WgoPart>(true);
			this.position = componentInParent.transform.InverseTransformPoint(dockPoint.transform.position);
			this.direction = dockPoint.Direction;
			this.isForZombie = dockPoint.IsForZombie;
			this.hideInFighting = dockPoint.HideInFighting;
			this.dockPointTag = dockPoint.DockPointTag;
			this.disableTargetingForCaretaker = dockPoint.DisableTargetingForCaretaker;
			this.dontUseForWorkerPlacement = dockPoint.DontUseForWorkerPlacement;
		}

		// Token: 0x0400202C RID: 8236
		[SerializeField]
		private Vector3 position;

		// Token: 0x0400202D RID: 8237
		[SerializeField]
		private Direction direction;

		// Token: 0x0400202E RID: 8238
		[SerializeField]
		private bool isForZombie;

		// Token: 0x0400202F RID: 8239
		[SerializeField]
		private bool hideInFighting;

		// Token: 0x04002030 RID: 8240
		[SerializeField]
		private DockPointTag dockPointTag;

		// Token: 0x04002031 RID: 8241
		[SerializeField]
		private bool disableTargetingForCaretaker;

		// Token: 0x04002032 RID: 8242
		[SerializeField]
		private bool dontUseForWorkerPlacement;
	}
}
