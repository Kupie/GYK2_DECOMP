using System;
using UnityEngine;

namespace SoftMasking.Samples
{
	// Token: 0x02000006 RID: 6
	[RequireComponent(typeof(Camera))]
	public class HorizontalFovSetter : MonoBehaviour
	{
		// Token: 0x0600000C RID: 12 RVA: 0x00002219 File Offset: 0x00000419
		public void Awake()
		{
			this._camera = base.GetComponent<Camera>();
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002227 File Offset: 0x00000427
		public void Update()
		{
			this._camera.fieldOfView = this.horizontalFov / this._camera.aspect;
		}

		// Token: 0x0400000B RID: 11
		public float horizontalFov;

		// Token: 0x0400000C RID: 12
		private Camera _camera;
	}
}
