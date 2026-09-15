using System;

namespace Expressive.Exceptions
{
	// Token: 0x020000BF RID: 191
	[Serializable]
	public sealed class ParameterCountMismatchException : Exception
	{
		// Token: 0x060002CE RID: 718 RVA: 0x0000EA97 File Offset: 0x0000CC97
		internal ParameterCountMismatchException(string message)
			: base(message)
		{
		}
	}
}
