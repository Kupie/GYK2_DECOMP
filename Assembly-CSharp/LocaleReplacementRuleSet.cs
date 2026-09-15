using System;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x02000832 RID: 2098
public class LocaleReplacementRuleSet : BaseReplacementRuleSet
{
	// Token: 0x060035AF RID: 13743 RVA: 0x00102254 File Offset: 0x00100454
	public override string Replace(string input)
	{
		if (!input.StartsWith("GameKey"))
		{
			return base.Replace(input);
		}
		GameKey byStaticFieldName = Enumeration.GetByStaticFieldName<GameKey>(input.Replace("GameKey", ""));
		if (byStaticFieldName == null)
		{
			Debug.LogError("#icon# Can't parse LocaleReplacementRuleSet value:[" + input + "]");
			return string.Empty;
		}
		return ControllerIconLibrary.GetIconId(byStaticFieldName, null, false);
	}

	// Token: 0x04002B01 RID: 11009
	private const string GAME_KEY_PREFIX = "GameKey";
}
