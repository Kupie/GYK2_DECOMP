using System;
using Rewired.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Rewired.UI.ControlMapper
{
	// Token: 0x02000093 RID: 147
	[RequireComponent(typeof(CanvasScalerExt))]
	public class CanvasScalerFitter : MonoBehaviour
	{
		// Token: 0x06000743 RID: 1859 RVA: 0x000129B8 File Offset: 0x00010BB8
		private void OnEnable()
		{
			this.canvasScaler = base.GetComponent<CanvasScalerExt>();
			this.Update();
			this.canvasScaler.ForceRefresh();
		}

		// Token: 0x06000744 RID: 1860 RVA: 0x000129D7 File Offset: 0x00010BD7
		private void Update()
		{
			if (Screen.width != this.screenWidth || Screen.height != this.screenHeight)
			{
				this.screenWidth = Screen.width;
				this.screenHeight = Screen.height;
				this.UpdateSize();
			}
		}

		// Token: 0x06000745 RID: 1861 RVA: 0x00012A10 File Offset: 0x00010C10
		private void UpdateSize()
		{
			if (this.canvasScaler.uiScaleMode != CanvasScaler.ScaleMode.ScaleWithScreenSize)
			{
				return;
			}
			if (this.breakPoints == null)
			{
				return;
			}
			float num = (float)Screen.width / (float)Screen.height;
			float num2 = float.PositiveInfinity;
			int num3 = 0;
			for (int i = 0; i < this.breakPoints.Length; i++)
			{
				float num4 = Mathf.Abs(num - this.breakPoints[i].screenAspectRatio);
				if ((num4 <= this.breakPoints[i].screenAspectRatio || MathTools.IsNear(this.breakPoints[i].screenAspectRatio, 0.01f)) && num4 < num2)
				{
					num2 = num4;
					num3 = i;
				}
			}
			this.canvasScaler.referenceResolution = this.breakPoints[num3].referenceResolution;
		}

		// Token: 0x040003AB RID: 939
		[SerializeField]
		private CanvasScalerFitter.BreakPoint[] breakPoints;

		// Token: 0x040003AC RID: 940
		private CanvasScalerExt canvasScaler;

		// Token: 0x040003AD RID: 941
		private int screenWidth;

		// Token: 0x040003AE RID: 942
		private int screenHeight;

		// Token: 0x040003AF RID: 943
		private Action ScreenSizeChanged;

		// Token: 0x02000094 RID: 148
		[Serializable]
		private class BreakPoint
		{
			// Token: 0x040003B0 RID: 944
			[SerializeField]
			public string name;

			// Token: 0x040003B1 RID: 945
			[SerializeField]
			public float screenAspectRatio;

			// Token: 0x040003B2 RID: 946
			[SerializeField]
			public Vector2 referenceResolution;
		}
	}
}
