using System;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x02000125 RID: 293
[RequireComponent(typeof(AudioListener))]
public class Microphone : LazySingleton<global::Microphone>
{
	// Token: 0x06000719 RID: 1817 RVA: 0x00021ECE File Offset: 0x000200CE
	public void SetTarget(Transform target)
	{
		this.target = target;
	}

	// Token: 0x0600071A RID: 1818 RVA: 0x00021ED7 File Offset: 0x000200D7
	private void Update()
	{
		if (this.target != null)
		{
			base.transform.position = this.target.position;
		}
	}

	// Token: 0x04000904 RID: 2308
	[SerializeField]
	private Transform target;
}
