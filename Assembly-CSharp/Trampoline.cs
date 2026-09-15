using System;
using UnityEngine;

// Token: 0x02000B43 RID: 2883
public class Trampoline : MonoBehaviour
{
	// Token: 0x06004C93 RID: 19603 RVA: 0x00169728 File Offset: 0x00167928
	private void OnCollisionEnter(Collision other)
	{
		Rigidbody component = other.gameObject.GetComponent<Rigidbody>();
		if (component != null)
		{
			Vector3 vector = base.transform.up * this.bounceForce;
			component.linearVelocity = vector;
		}
	}

	// Token: 0x04003DB0 RID: 15792
	[SerializeField]
	private float bounceForce = 100f;
}
