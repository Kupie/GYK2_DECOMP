using System;
using UnityEngine;

// Token: 0x02000742 RID: 1858
public class PlayerUniqueCommandHolder : UniqueCommandHolderDataWrapper<NetworkPlayer>
{
	// Token: 0x0600305A RID: 12378 RVA: 0x000E7D44 File Offset: 0x000E5F44
	public override void RegisterData(NetworkPlayer data)
	{
		this.data = data;
		data.playerData.position.ValueChanged += this.UpdatePosition;
		data.playerData.AddDirectionListener(new Action<Vector2>(this.UpdateDirection));
		data.playerData.charState.ValueChanged += this.UpdateAnimState;
	}

	// Token: 0x0600305B RID: 12379 RVA: 0x000E7DA8 File Offset: 0x000E5FA8
	public override void UnregisterData()
	{
		this.data.playerData.position.ValueChanged -= this.UpdatePosition;
		this.data.playerData.RemoveDirectionListener(new Action<Vector2>(this.UpdateDirection));
		this.data.playerData.charState.ValueChanged -= this.UpdateAnimState;
		this.data = null;
	}

	// Token: 0x0600305C RID: 12380 RVA: 0x000E7E1A File Offset: 0x000E601A
	public override Command GetCommand()
	{
		return new CharMoveCommand
		{
			playerId = this.data.clientId,
			position = this.position,
			direction = this.direction,
			animState = this.animState
		};
	}

	// Token: 0x0600305D RID: 12381 RVA: 0x000E7E56 File Offset: 0x000E6056
	private void UpdatePosition(Vector3 position)
	{
		this.position = position;
		this.PrepareDataForSending();
	}

	// Token: 0x0600305E RID: 12382 RVA: 0x000E7E65 File Offset: 0x000E6065
	private void UpdateDirection(Vector2 direction)
	{
		this.direction = direction;
		this.PrepareDataForSending();
	}

	// Token: 0x0600305F RID: 12383 RVA: 0x000E7E74 File Offset: 0x000E6074
	private void UpdateAnimState(global::AnimationState animState)
	{
		this.animState = animState;
		this.PrepareDataForSending();
	}

	// Token: 0x06003060 RID: 12384 RVA: 0x000E7E83 File Offset: 0x000E6083
	private void PrepareDataForSending()
	{
		LazyNetwork.ConnectionManager.CurrentState.SendNetworkData<PlayerUniqueCommandHolder>(this);
	}

	// Token: 0x04002726 RID: 10022
	private NetworkPlayer data;

	// Token: 0x04002727 RID: 10023
	private Vector3 position;

	// Token: 0x04002728 RID: 10024
	private Vector2 direction;

	// Token: 0x04002729 RID: 10025
	private global::AnimationState animState;
}
