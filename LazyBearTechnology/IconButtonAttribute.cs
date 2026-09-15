using System;
using Sirenix.OdinInspector;

namespace LazyBearTechnology
{
	// Token: 0x0200016D RID: 365
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
	public class IconButtonAttribute : ShowInInspectorAttribute
	{
		// Token: 0x06000802 RID: 2050 RVA: 0x00028011 File Offset: 0x00026211
		public IconButtonAttribute(string iconResource)
		{
			this.iconResource = iconResource;
		}

		// Token: 0x06000803 RID: 2051 RVA: 0x00028028 File Offset: 0x00026228
		public IconButtonAttribute(string iconResource, ButtonSizes buttonSize)
		{
			this.iconResource = iconResource;
			this.buttonHeight = (int)buttonSize;
		}

		// Token: 0x040004D9 RID: 1241
		public string iconResource;

		// Token: 0x040004DA RID: 1242
		public int buttonHeight = 31;
	}
}
