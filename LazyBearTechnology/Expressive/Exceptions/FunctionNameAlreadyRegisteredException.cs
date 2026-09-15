using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Expressive.Exceptions
{
	// Token: 0x020000BB RID: 187
	[Serializable]
	public sealed class FunctionNameAlreadyRegisteredException : Exception
	{
		// Token: 0x17000085 RID: 133
		// (get) Token: 0x060002C3 RID: 707 RVA: 0x0000E9CE File Offset: 0x0000CBCE
		public string Name { get; }

		// Token: 0x060002C4 RID: 708 RVA: 0x0000E9D6 File Offset: 0x0000CBD6
		internal FunctionNameAlreadyRegisteredException(string name)
			: base("A function has already been registered '" + name + "'")
		{
			this.Name = name;
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x0000E9F5 File Offset: 0x0000CBF5
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("Name", this.Name);
		}
	}
}
