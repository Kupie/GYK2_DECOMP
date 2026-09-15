using System;

namespace Expressive.Exceptions
{
	// Token: 0x020000BC RID: 188
	[Serializable]
	public sealed class MissingParticipantException : Exception
	{
		// Token: 0x060002C6 RID: 710 RVA: 0x0000EA10 File Offset: 0x0000CC10
		internal MissingParticipantException(string message)
			: base(message)
		{
		}
	}
}
