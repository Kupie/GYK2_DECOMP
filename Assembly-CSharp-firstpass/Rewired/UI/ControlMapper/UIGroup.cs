using System;
using TMPro;
using UnityEngine;

namespace Rewired.UI.ControlMapper
{
	// Token: 0x020000DD RID: 221
	[AddComponentMenu("")]
	public class UIGroup : MonoBehaviour
	{
		// Token: 0x17000482 RID: 1154
		// (get) Token: 0x06000B72 RID: 2930 RVA: 0x0001F114 File Offset: 0x0001D314
		// (set) Token: 0x06000B73 RID: 2931 RVA: 0x0001F135 File Offset: 0x0001D335
		public string labelText
		{
			get
			{
				if (!(this._label != null))
				{
					return string.Empty;
				}
				return this._label.text;
			}
			set
			{
				if (this._label == null)
				{
					return;
				}
				this._label.text = value;
			}
		}

		// Token: 0x17000483 RID: 1155
		// (get) Token: 0x06000B74 RID: 2932 RVA: 0x0001F152 File Offset: 0x0001D352
		public Transform content
		{
			get
			{
				return this._content;
			}
		}

		// Token: 0x06000B75 RID: 2933 RVA: 0x0001F15A File Offset: 0x0001D35A
		public void SetLabelActive(bool state)
		{
			if (this._label == null)
			{
				return;
			}
			this._label.gameObject.SetActive(state);
		}

		// Token: 0x040005B8 RID: 1464
		[SerializeField]
		private TMP_Text _label;

		// Token: 0x040005B9 RID: 1465
		[SerializeField]
		private Transform _content;
	}
}
