using System;
using System.Diagnostics;

// Token: 0x02000240 RID: 576
[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
[Conditional("UNITY_EDITOR")]
[Conditional("BALANCE_PARSER")]
public class AutoValidate_NeedItemsList : AutoValidate_NeedItem
{
}
