using System;
using System.Diagnostics;

namespace LazyBearTechnology
{
	// Token: 0x020000E8 RID: 232
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
	[Conditional("UNITY_EDITOR")]
	[Conditional("BALANCE_PARSER")]
	public abstract class AutoValidateAttribute : Attribute
	{
	}
}
