using System;

namespace Expressive.Helpers
{
	// Token: 0x02000056 RID: 86
	public static class TypeHelper
	{
		// Token: 0x060001A7 RID: 423 RVA: 0x0000C098 File Offset: 0x0000A298
		public static TypeCode GetTypeCode(object value)
		{
			return Type.GetTypeCode((value != null) ? value.GetType() : null);
		}
	}
}
