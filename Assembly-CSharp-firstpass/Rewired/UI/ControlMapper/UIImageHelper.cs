using System;
using UnityEngine;
using UnityEngine.UI;

namespace Rewired.UI.ControlMapper
{
	// Token: 0x020000DE RID: 222
	[AddComponentMenu("")]
	[RequireComponent(typeof(Image))]
	public class UIImageHelper : MonoBehaviour
	{
		// Token: 0x06000B77 RID: 2935 RVA: 0x0001F17C File Offset: 0x0001D37C
		public void SetEnabledState(bool newState)
		{
			this.currentState = newState;
			UIImageHelper.State state = (newState ? this.enabledState : this.disabledState);
			if (state == null)
			{
				return;
			}
			Image component = base.gameObject.GetComponent<Image>();
			if (component == null)
			{
				Debug.LogError("Image is missing!");
				return;
			}
			state.Set(component);
		}

		// Token: 0x06000B78 RID: 2936 RVA: 0x0001F1CD File Offset: 0x0001D3CD
		public void SetEnabledStateColor(Color color)
		{
			this.enabledState.color = color;
		}

		// Token: 0x06000B79 RID: 2937 RVA: 0x0001F1DB File Offset: 0x0001D3DB
		public void SetDisabledStateColor(Color color)
		{
			this.disabledState.color = color;
		}

		// Token: 0x06000B7A RID: 2938 RVA: 0x0001F1EC File Offset: 0x0001D3EC
		public void Refresh()
		{
			UIImageHelper.State state = (this.currentState ? this.enabledState : this.disabledState);
			Image component = base.gameObject.GetComponent<Image>();
			if (component == null)
			{
				return;
			}
			state.Set(component);
		}

		// Token: 0x040005BA RID: 1466
		[SerializeField]
		private UIImageHelper.State enabledState;

		// Token: 0x040005BB RID: 1467
		[SerializeField]
		private UIImageHelper.State disabledState;

		// Token: 0x040005BC RID: 1468
		private bool currentState;

		// Token: 0x020000DF RID: 223
		[Serializable]
		private class State
		{
			// Token: 0x06000B7C RID: 2940 RVA: 0x0001F22D File Offset: 0x0001D42D
			public void Set(Image image)
			{
				if (image == null)
				{
					return;
				}
				image.color = this.color;
			}

			// Token: 0x040005BD RID: 1469
			[SerializeField]
			public Color color;
		}
	}
}
