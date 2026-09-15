using System;
using TMPro;
using UnityEngine;

namespace Rewired.UI.ControlMapper
{
	// Token: 0x020000C4 RID: 196
	[AddComponentMenu("")]
	public class InputRow : MonoBehaviour
	{
		// Token: 0x170003F4 RID: 1012
		// (get) Token: 0x06000A61 RID: 2657 RVA: 0x0001D5E1 File Offset: 0x0001B7E1
		// (set) Token: 0x06000A62 RID: 2658 RVA: 0x0001D5E9 File Offset: 0x0001B7E9
		public ButtonInfo[] buttons { get; private set; }

		// Token: 0x06000A63 RID: 2659 RVA: 0x0001D5F2 File Offset: 0x0001B7F2
		public void Initialize(int rowIndex, string label, Action<int, ButtonInfo> inputFieldActivatedCallback)
		{
			this.rowIndex = rowIndex;
			this.label.text = label;
			this.inputFieldActivatedCallback = inputFieldActivatedCallback;
			this.buttons = base.transform.GetComponentsInChildren<ButtonInfo>(true);
		}

		// Token: 0x06000A64 RID: 2660 RVA: 0x0001D620 File Offset: 0x0001B820
		public void OnButtonActivated(ButtonInfo buttonInfo)
		{
			if (this.inputFieldActivatedCallback == null)
			{
				return;
			}
			this.inputFieldActivatedCallback(this.rowIndex, buttonInfo);
		}

		// Token: 0x04000515 RID: 1301
		public TMP_Text label;

		// Token: 0x04000517 RID: 1303
		private int rowIndex;

		// Token: 0x04000518 RID: 1304
		private Action<int, ButtonInfo> inputFieldActivatedCallback;
	}
}
