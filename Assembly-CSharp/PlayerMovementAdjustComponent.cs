using System;
using UnityEngine;

// Token: 0x0200036E RID: 878
public class PlayerMovementAdjustComponent : MovementAdjustComponentBase
{
	// Token: 0x06001757 RID: 5975 RVA: 0x0006F090 File Offset: 0x0006D290
	public override void Init(IMovable movable)
	{
		this.player = movable as PlayerController;
		if (this.player == null)
		{
			Debug.LogError("Wrong IMovable type");
			return;
		}
		this.player.PlayerData.position.ValueChanged += this.UpdatePosIfMoving;
	}

	// Token: 0x06001758 RID: 5976 RVA: 0x0006F0E4 File Offset: 0x0006D2E4
	public override void DeInit()
	{
		if (this.player != null)
		{
			this.player.PlayerData.position.ValueChanged -= this.UpdatePosIfMoving;
			return;
		}
		Debug.LogWarning("MovementAdjustComponent: Data is null");
	}

	// Token: 0x06001759 RID: 5977 RVA: 0x0006F121 File Offset: 0x0006D321
	protected override void UpdatePosIfMoving(Vector3 newPos)
	{
		if (this.player != null && this.player.MovementComponent.IsMoving)
		{
			base.UpdatePos(newPos);
		}
	}

	// Token: 0x0600175A RID: 5978 RVA: 0x0006F14A File Offset: 0x0006D34A
	private void OnDestroy()
	{
		this.DeInit();
	}

	// Token: 0x04001746 RID: 5958
	private PlayerController player;
}
