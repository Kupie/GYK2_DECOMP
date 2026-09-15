using System;
using UnityEngine;

// Token: 0x0200036F RID: 879
public class WgoMovementAdjustComponent : MovementAdjustComponentBase
{
	// Token: 0x0600175C RID: 5980 RVA: 0x0006F15A File Offset: 0x0006D35A
	public override void Init(IMovable movable)
	{
		this.wgoData = movable as WgoData;
		if (this.wgoData == null)
		{
			Debug.LogError("Wrong IMovable type");
			return;
		}
		this.wgoData.OnPositionChanged += this.UpdatePosIfMoving;
	}

	// Token: 0x0600175D RID: 5981 RVA: 0x0006F193 File Offset: 0x0006D393
	public override void DeInit()
	{
		if (this.wgoData != null)
		{
			this.wgoData.OnPositionChanged -= this.UpdatePosIfMoving;
			return;
		}
		Debug.LogWarning("MovementAdjustComponent: Data is null");
	}

	// Token: 0x0600175E RID: 5982 RVA: 0x0006F1C0 File Offset: 0x0006D3C0
	public override void SetAdjustmentActive(bool active)
	{
		if (active == base.enabled)
		{
			return;
		}
		if (!active)
		{
			if (this.wgoData != null)
			{
				this.wgoData.OnPositionChanged -= this.UpdatePosIfMoving;
			}
		}
		else if (this.wgoData != null)
		{
			this.wgoData.OnPositionChanged += this.UpdatePosIfMoving;
		}
		base.enabled = active;
	}

	// Token: 0x0600175F RID: 5983 RVA: 0x0006F223 File Offset: 0x0006D423
	protected override void UpdatePosIfMoving(Vector3 newPos)
	{
		if (this.wgoData != null && this.wgoData.MovementComponent.IsMoving)
		{
			base.UpdatePos(newPos);
		}
	}

	// Token: 0x06001760 RID: 5984 RVA: 0x0006F14A File Offset: 0x0006D34A
	private void OnDestroy()
	{
		this.DeInit();
	}

	// Token: 0x04001747 RID: 5959
	private WgoData wgoData;
}
