using System;
using System.Diagnostics;
using LazyBearTechnology;

// Token: 0x02000241 RID: 577
[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
[Conditional("UNITY_EDITOR")]
[Conditional("BALANCE_PARSER")]
public class AutoValidate_OutputItems : AutoValidateAttribute
{
}
