using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Expressive.Exceptions
{
	// Token: 0x020000BE RID: 190
	[Serializable]
	public sealed class OperatorNameAlreadyRegisteredException : Exception
	{
		// Token: 0x17000087 RID: 135
		// (get) Token: 0x060002CB RID: 715 RVA: 0x0000EA55 File Offset: 0x0000CC55
		public string Tag { get; }

		// Token: 0x060002CC RID: 716 RVA: 0x0000EA5D File Offset: 0x0000CC5D
		internal OperatorNameAlreadyRegisteredException(string tag)
			: base("An operator has already been registered '" + tag + "'")
		{
			this.Tag = tag;
		}

		// Token: 0x060002CD RID: 717 RVA: 0x0000EA7C File Offset: 0x0000CC7C
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("Tag", this.Tag);
		}
	}
}
