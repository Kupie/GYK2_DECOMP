using System;
using TMPro;
using UnityEngine;

namespace Rewired.UI.ControlMapper
{
	// Token: 0x020000D9 RID: 217
	[AddComponentMenu("")]
	public class UIControl : MonoBehaviour
	{
		// Token: 0x1700047F RID: 1151
		// (get) Token: 0x06000B5F RID: 2911 RVA: 0x0001EE55 File Offset: 0x0001D055
		public int id
		{
			get
			{
				return this._id;
			}
		}

		// Token: 0x06000B60 RID: 2912 RVA: 0x0001EE5D File Offset: 0x0001D05D
		private void Awake()
		{
			this._id = UIControl.GetNextUid();
		}

		// Token: 0x17000480 RID: 1152
		// (get) Token: 0x06000B61 RID: 2913 RVA: 0x0001EE6A File Offset: 0x0001D06A
		// (set) Token: 0x06000B62 RID: 2914 RVA: 0x0001EE72 File Offset: 0x0001D072
		public bool showTitle
		{
			get
			{
				return this._showTitle;
			}
			set
			{
				if (this.title == null)
				{
					return;
				}
				this.title.gameObject.SetActive(value);
				this._showTitle = value;
			}
		}

		// Token: 0x06000B63 RID: 2915 RVA: 0x00003466 File Offset: 0x00001666
		public virtual void SetCancelCallback(Action cancelCallback)
		{
		}

		// Token: 0x06000B64 RID: 2916 RVA: 0x0001EE9B File Offset: 0x0001D09B
		private static int GetNextUid()
		{
			if (UIControl._uidCounter == 2147483647)
			{
				UIControl._uidCounter = 0;
			}
			int uidCounter = UIControl._uidCounter;
			UIControl._uidCounter++;
			return uidCounter;
		}

		// Token: 0x040005AB RID: 1451
		public TMP_Text title;

		// Token: 0x040005AC RID: 1452
		private int _id;

		// Token: 0x040005AD RID: 1453
		private bool _showTitle;

		// Token: 0x040005AE RID: 1454
		private static int _uidCounter;
	}
}
