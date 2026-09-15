using System;
using UnityEngine;

// Token: 0x02000379 RID: 889
public class PlayerColliderTester : MonoBehaviour
{
	// Token: 0x0600179E RID: 6046 RVA: 0x0006FD72 File Offset: 0x0006DF72
	private void OnEnable()
	{
		PlayerColliderTester.neighbours = new Collider[10];
		PlayerColliderTester.thisCollider = base.GetComponent<Collider>();
		if (!PlayerColliderTester.thisCollider)
		{
			Debug.LogWarning("PlayerColliderTester requires a Collider component.", this);
		}
	}

	// Token: 0x0600179F RID: 6047 RVA: 0x0006FDA4 File Offset: 0x0006DFA4
	public static bool IsPositionReachable(Vector3 position)
	{
		if (PlayerColliderTester.thisCollider == null)
		{
			return false;
		}
		position = new Vector3(position.x, PlayerColliderTester.thisCollider.transform.position.y, position.z);
		int num = Physics.OverlapSphereNonAlloc(position, 0.5f, PlayerColliderTester.neighbours, 256);
		for (int i = 0; i < num; i++)
		{
			Collider collider = PlayerColliderTester.neighbours[i];
			Vector3 vector;
			float num2;
			if (collider && !(collider == PlayerColliderTester.thisCollider) && Physics.ComputePenetration(PlayerColliderTester.thisCollider, position, PlayerColliderTester.thisCollider.transform.rotation, collider, collider.transform.position, collider.transform.rotation, out vector, out num2) && num2 > 0.05f)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x0400176B RID: 5995
	private const float MAX_PENETRATION_DISTANCE = 0.05f;

	// Token: 0x0400176C RID: 5996
	private static Collider[] neighbours;

	// Token: 0x0400176D RID: 5997
	private static Collider thisCollider;
}
