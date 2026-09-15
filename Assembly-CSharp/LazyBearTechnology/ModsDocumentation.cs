using System;
using UnityEngine;

namespace LazyBearTechnology
{
	// Token: 0x02000C53 RID: 3155
	public static class ModsDocumentation
	{
		// Token: 0x06005071 RID: 20593 RVA: 0x0017CD58 File Offset: 0x0017AF58
		public static string Load(string resourcePath)
		{
			TextAsset textAsset = Resources.Load<TextAsset>(resourcePath);
			if (textAsset == null)
			{
				Debug.LogWarning("[Mods] Missing documentation resource '" + resourcePath + "'.");
				return string.Empty;
			}
			return textAsset.text;
		}

		// Token: 0x040041EE RID: 16878
		public const string GlobalReadmeResource = "Mods/README";

		// Token: 0x040041EF RID: 16879
		public const string LanguagesReadmeResource = "Mods/Languages/README";

		// Token: 0x040041F0 RID: 16880
		public const string ExampleLanguageJsonResource = "Mods/Languages/language";
	}
}
