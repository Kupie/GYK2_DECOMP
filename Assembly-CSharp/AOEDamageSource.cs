using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020002A6 RID: 678
public class AOEDamageSource : MonoBehaviour
{
	// Token: 0x06001151 RID: 4433 RVA: 0x000576C6 File Offset: 0x000558C6
	private void OnEnable()
	{
		this.Activate();
	}

	// Token: 0x06001152 RID: 4434 RVA: 0x000576D0 File Offset: 0x000558D0
	private void Activate()
	{
		HashSet<ICombatEntity> hashSet = new HashSet<ICombatEntity>();
		int num;
		switch (this.colliderType)
		{
		case AOEDamageSource.ColliderType.Box:
			num = Physics.OverlapBoxNonAlloc(this.col.bounds.center, this.col.bounds.size, this.colliders, this.col.transform.rotation, -1, QueryTriggerInteraction.Ignore);
			break;
		case AOEDamageSource.ColliderType.Sphere:
			num = Physics.OverlapSphereNonAlloc(this.col.bounds.center, Mathf.Max(new float[]
			{
				this.col.bounds.extents.x,
				this.col.bounds.extents.y,
				this.col.bounds.extents.z
			}), this.colliders, -1, QueryTriggerInteraction.Ignore);
			break;
		case AOEDamageSource.ColliderType.Capsule:
			num = this.GetCapsuleOverlapCount();
			break;
		default:
			num = 0;
			break;
		}
		int num2 = num;
		for (int i = 0; i < num2; i++)
		{
			ICombatEntity componentInParent = this.colliders[i].GetComponentInParent<ICombatEntity>();
			if (componentInParent != null && componentInParent.IsActiveCombatant && componentInParent.TeamType == this.damagingTeamType && !hashSet.Contains(componentInParent))
			{
				Debug.Log(string.Format("Applying damage to {0}, damage: {1}", componentInParent.CombatEntityUID, this.damage));
				componentInParent.CombatEntityHpComponent.ApplyDamage(this.damage);
				hashSet.Add(componentInParent);
			}
		}
	}

	// Token: 0x06001153 RID: 4435 RVA: 0x00057860 File Offset: 0x00055A60
	private int GetCapsuleOverlapCount()
	{
		CapsuleCollider capsuleCollider = (CapsuleCollider)this.col;
		int num = capsuleCollider.direction;
		Vector3 vector;
		if (num != 0)
		{
			if (num != 1)
			{
				vector = Vector3.forward;
			}
			else
			{
				vector = Vector3.up;
			}
		}
		else
		{
			vector = Vector3.right;
		}
		Vector3 vector2 = vector;
		float num2 = capsuleCollider.height * 0.5f - capsuleCollider.radius;
		Vector3 vector3 = this.col.transform.TransformPoint(capsuleCollider.center - vector2 * num2);
		Vector3 vector4 = this.col.transform.TransformPoint(capsuleCollider.center + vector2 * num2);
		Vector3 lossyScale = this.col.transform.lossyScale;
		num = capsuleCollider.direction;
		float num3;
		if (num != 0)
		{
			if (num != 1)
			{
				num3 = Mathf.Max(lossyScale.x, lossyScale.y);
			}
			else
			{
				num3 = Mathf.Max(lossyScale.x, lossyScale.z);
			}
		}
		else
		{
			num3 = Mathf.Max(lossyScale.y, lossyScale.z);
		}
		float num4 = num3;
		return Physics.OverlapCapsuleNonAlloc(vector3, vector4, capsuleCollider.radius * num4, this.colliders, -1, QueryTriggerInteraction.Ignore);
	}

	// Token: 0x0400134D RID: 4941
	public Collider col;

	// Token: 0x0400134E RID: 4942
	public AOEDamageSource.ColliderType colliderType;

	// Token: 0x0400134F RID: 4943
	public LazyConsts.Fighting.TeamType damagingTeamType;

	// Token: 0x04001350 RID: 4944
	public int damage;

	// Token: 0x04001351 RID: 4945
	private Collider[] colliders = new Collider[50];

	// Token: 0x020002A7 RID: 679
	public enum ColliderType
	{
		// Token: 0x04001353 RID: 4947
		Box,
		// Token: 0x04001354 RID: 4948
		Sphere,
		// Token: 0x04001355 RID: 4949
		Capsule
	}
}
