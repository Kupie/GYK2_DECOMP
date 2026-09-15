using System;
using System.Collections.Generic;
using UnityEngine;

namespace LazyBearTechnology
{
	// Token: 0x02000166 RID: 358
	public abstract class LazyWidgetBase : MonoBehaviour, ILazyGUIElement
	{
		// Token: 0x060007B2 RID: 1970 RVA: 0x000271BF File Offset: 0x000253BF
		public virtual void Init()
		{
		}

		// Token: 0x060007B3 RID: 1971 RVA: 0x000271C1 File Offset: 0x000253C1
		public virtual void DeInit()
		{
		}

		// Token: 0x060007B4 RID: 1972 RVA: 0x000271C3 File Offset: 0x000253C3
		public virtual void Draw()
		{
			base.gameObject.SetActive(true);
		}

		// Token: 0x060007B5 RID: 1973
		public abstract void Draw(LazyWidgetDataBase data);

		// Token: 0x060007B6 RID: 1974
		public abstract void Redraw();

		// Token: 0x060007B7 RID: 1975
		public abstract Type GetDataType();

		// Token: 0x060007B8 RID: 1976 RVA: 0x000271D1 File Offset: 0x000253D1
		public virtual void Hide()
		{
			base.gameObject.SetActive(false);
		}

		// Token: 0x060007B9 RID: 1977 RVA: 0x000271DF File Offset: 0x000253DF
		public virtual void CustomUpdate()
		{
		}

		// Token: 0x060007BA RID: 1978 RVA: 0x000271E1 File Offset: 0x000253E1
		public virtual List<LazyGameKeyTip> GetTips(GamepadNavigationItem gamepadNavigationItem)
		{
			return new List<LazyGameKeyTip>();
		}

		// Token: 0x060007BB RID: 1979
		[LazyUITest]
		protected abstract void TestDraw();
	}
}
