using System;
using TMPro;
using UnityEngine;

namespace LazyBearTechnology
{
	// Token: 0x02000196 RID: 406
	[RequireComponent(typeof(TMP_Text))]
	public class LocalizedRtlAlign : MonoBehaviour
	{
		// Token: 0x17000144 RID: 324
		// (get) Token: 0x0600092A RID: 2346 RVA: 0x0002C550 File Offset: 0x0002A750
		public bool DontChangeAlignOnRtl
		{
			get
			{
				return this.dontChangeAlignOnRtl;
			}
		}

		// Token: 0x0400057C RID: 1404
		[SerializeField]
		[InspectorName("Don't change align on RTL")]
		[Tooltip("If enabled, right-to-left still applies but the horizontal alignment is not flipped.")]
		private bool dontChangeAlignOnRtl = true;
	}
}
