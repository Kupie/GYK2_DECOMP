using System;
using UnityEngine;
using UnityEngine.UI;

namespace SoftMasking.Samples
{
	// Token: 0x02000011 RID: 17
	public class SoftMaskToggler : MonoBehaviour
	{
		// Token: 0x0600004B RID: 75 RVA: 0x00002D18 File Offset: 0x00000F18
		public void Toggle(bool enabled)
		{
			if (this.mask)
			{
				this.mask.GetComponent<SoftMask>().enabled = enabled;
				this.mask.GetComponent<Mask>().enabled = !enabled;
				if (!this.doNotTouchImage)
				{
					this.mask.GetComponent<Image>().enabled = !enabled;
				}
			}
		}

		// Token: 0x04000042 RID: 66
		public GameObject mask;

		// Token: 0x04000043 RID: 67
		public bool doNotTouchImage;
	}
}
