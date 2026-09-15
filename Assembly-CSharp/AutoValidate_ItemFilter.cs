using System;
using System.Diagnostics;
using LazyBearTechnology;

// Token: 0x02000242 RID: 578
[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
[Conditional("UNITY_EDITOR")]
[Conditional("BALANCE_PARSER")]
public class AutoValidate_ItemFilter : AutoValidateAttribute
{
}
