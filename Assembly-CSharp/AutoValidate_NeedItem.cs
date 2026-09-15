using System;
using System.Diagnostics;
using LazyBearTechnology;

// Token: 0x0200023F RID: 575
[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
[Conditional("UNITY_EDITOR")]
[Conditional("BALANCE_PARSER")]
public class AutoValidate_NeedItem : AutoValidateAttribute
{
}
