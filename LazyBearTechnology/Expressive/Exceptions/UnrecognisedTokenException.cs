using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Expressive.Exceptions
{
	// Token: 0x020000C0 RID: 192
	[Serializable]
	public sealed class UnrecognisedTokenException : Exception
	{
		// Token: 0x17000088 RID: 136
		// (get) Token: 0x060002CF RID: 719 RVA: 0x0000EAA0 File Offset: 0x0000CCA0
		// (set) Token: 0x060002D0 RID: 720 RVA: 0x0000EAA8 File Offset: 0x0000CCA8
		public string Token { get; private set; }

		// Token: 0x060002D1 RID: 721 RVA: 0x0000EAB1 File Offset: 0x0000CCB1
		internal UnrecognisedTokenException(string token)
			: base("Unrecognised token '" + token + "'")
		{
			this.Token = token;
		}

		// Token: 0x060002D2 RID: 722 RVA: 0x0000EAD0 File Offset: 0x0000CCD0
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("Token", this.Token);
		}
	}
}
