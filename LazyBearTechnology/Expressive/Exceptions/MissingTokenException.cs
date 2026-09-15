using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Expressive.Exceptions
{
	// Token: 0x020000BD RID: 189
	[Serializable]
	public sealed class MissingTokenException : Exception
	{
		// Token: 0x17000086 RID: 134
		// (get) Token: 0x060002C7 RID: 711 RVA: 0x0000EA19 File Offset: 0x0000CC19
		// (set) Token: 0x060002C8 RID: 712 RVA: 0x0000EA21 File Offset: 0x0000CC21
		public char MissingToken { get; private set; }

		// Token: 0x060002C9 RID: 713 RVA: 0x0000EA2A File Offset: 0x0000CC2A
		internal MissingTokenException(string message, char missingToken)
			: base(message)
		{
			this.MissingToken = missingToken;
		}

		// Token: 0x060002CA RID: 714 RVA: 0x0000EA3A File Offset: 0x0000CC3A
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("MissingToken", this.MissingToken);
		}
	}
}
