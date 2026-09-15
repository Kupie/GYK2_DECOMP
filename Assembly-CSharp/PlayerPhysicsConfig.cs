using System;
using UnityEngine;

// Token: 0x0200039E RID: 926
[CreateAssetMenu(menuName = "GK2/Player Physics Config", fileName = "PlayerPhysicsConfig")]
[Serializable]
public class PlayerPhysicsConfig : ScriptableObject
{
	// Token: 0x0400185F RID: 6239
	public float speed = 5f;

	// Token: 0x04001860 RID: 6240
	public float jumpForce = 100f;

	// Token: 0x04001861 RID: 6241
	public float groundRaycastLength = 0.5f;

	// Token: 0x04001862 RID: 6242
	public float gravityFallScale = 1f;

	// Token: 0x04001863 RID: 6243
	public float maxGroundAngle = 60f;

	// Token: 0x04001864 RID: 6244
	public float contactForceMult = 1f;

	// Token: 0x04001865 RID: 6245
	public float stickForceMult = 10f;

	// Token: 0x04001866 RID: 6246
	[Range(0f, 10f)]
	public float slopeGravityMovementSlowdown = 1f;

	// Token: 0x04001867 RID: 6247
	[Space]
	[Header("Attack Dash Settings")]
	public float attackDashForce = 15f;

	// Token: 0x04001868 RID: 6248
	public float attackDashDuration = 0.2f;

	// Token: 0x04001869 RID: 6249
	public float attackDashCooldown = 0.5f;
}
