using System;
using UnityEngine;

// Token: 0x02000427 RID: 1063
[Serializable]
public class SerializableCollision
{
	// Token: 0x06001C17 RID: 7191 RVA: 0x00082E2F File Offset: 0x0008102F
	public static SerializableCollision CreateFrom(Collision collision)
	{
		return new SerializableCollision
		{
			goHashCode = collision.gameObject.GetHashCode(),
			layer = collision.gameObject.layer,
			contactPoints = SerializableCollision.GetContactPoints(collision)
		};
	}

	// Token: 0x06001C18 RID: 7192 RVA: 0x00082E64 File Offset: 0x00081064
	public void UpdateContactPoints(Collision collision)
	{
		this.contactPoints = SerializableCollision.GetContactPoints(collision);
	}

	// Token: 0x06001C19 RID: 7193 RVA: 0x00082E74 File Offset: 0x00081074
	private static SerializableContactPoint[] GetContactPoints(Collision collision)
	{
		SerializableContactPoint[] array = new SerializableContactPoint[collision.contactCount];
		ContactPoint[] array2 = new ContactPoint[collision.contactCount];
		collision.GetContacts(array2);
		for (int i = 0; i < array2.Length; i++)
		{
			ContactPoint contactPoint = array2[i];
			array[i] = new SerializableContactPoint(contactPoint.point, contactPoint.normal);
		}
		return array;
	}

	// Token: 0x04001A92 RID: 6802
	public int goHashCode;

	// Token: 0x04001A93 RID: 6803
	public int layer;

	// Token: 0x04001A94 RID: 6804
	public SerializableContactPoint[] contactPoints;
}
