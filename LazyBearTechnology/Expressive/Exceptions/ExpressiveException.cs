using System;

namespace Expressive.Exceptions
{
	// Token: 0x020000BA RID: 186
	[Serializable]
	public sealed class ExpressiveException : Exception
	{
		// Token: 0x060002C1 RID: 705 RVA: 0x0000E9B6 File Offset: 0x0000CBB6
		internal ExpressiveException(string message)
			: base(message)
		{
		}

		// Token: 0x060002C2 RID: 706 RVA: 0x0000E9BF File Offset: 0x0000CBBF
		internal ExpressiveException(Exception innerException)
			: base(innerException.Message, innerException)
		{
		}
	}
}
