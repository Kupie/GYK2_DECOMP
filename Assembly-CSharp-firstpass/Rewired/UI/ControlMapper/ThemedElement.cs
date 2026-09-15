using System;
using UnityEngine;

namespace Rewired.UI.ControlMapper
{
	// Token: 0x020000CB RID: 203
	[AddComponentMenu("")]
	public class ThemedElement : MonoBehaviour
	{
		// Token: 0x06000AFC RID: 2812 RVA: 0x0001E1D4 File Offset: 0x0001C3D4
		private void Start()
		{
			this.ApplyTheme();
		}

		// Token: 0x06000AFD RID: 2813 RVA: 0x0001E1DC File Offset: 0x0001C3DC
		private void OnEnable()
		{
			ControlMapper.Register(this);
		}

		// Token: 0x06000AFE RID: 2814 RVA: 0x0001E1E4 File Offset: 0x0001C3E4
		private void OnDisable()
		{
			ControlMapper.Unregister(this);
		}

		// Token: 0x06000AFF RID: 2815 RVA: 0x0001E1EC File Offset: 0x0001C3EC
		public void ApplyTheme()
		{
			ControlMapper.ApplyTheme(this._elements);
		}

		// Token: 0x04000561 RID: 1377
		[SerializeField]
		private ThemedElement.ElementInfo[] _elements;

		// Token: 0x020000CC RID: 204
		[Serializable]
		public class ElementInfo
		{
			// Token: 0x1700044F RID: 1103
			// (get) Token: 0x06000B01 RID: 2817 RVA: 0x0001E1F9 File Offset: 0x0001C3F9
			public string themeClass
			{
				get
				{
					return this._themeClass;
				}
			}

			// Token: 0x17000450 RID: 1104
			// (get) Token: 0x06000B02 RID: 2818 RVA: 0x0001E201 File Offset: 0x0001C401
			public Component component
			{
				get
				{
					return this._component;
				}
			}

			// Token: 0x04000562 RID: 1378
			[SerializeField]
			private string _themeClass;

			// Token: 0x04000563 RID: 1379
			[SerializeField]
			private Component _component;
		}
	}
}
