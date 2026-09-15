using System;
using UnityEngine;

namespace LazyBearTechnology
{
	// Token: 0x02000146 RID: 326
	public class LazyGameKeyTip
	{
		// Token: 0x060006AC RID: 1708 RVA: 0x00022416 File Offset: 0x00020616
		public static void InitSelectAndBackLocales(string selectLocale, string backLocale)
		{
			if (!LazyGameKeyTip.selectAndBackLocalesInitialized)
			{
				LazyGameKeyTip.selectAndBackLocalesInitialized = true;
				LazyGameKeyTip.uiSelectLocale = selectLocale;
				LazyGameKeyTip.uiBackLocale = backLocale;
			}
		}

		// Token: 0x060006AD RID: 1709 RVA: 0x00022431 File Offset: 0x00020631
		public static string Get(GameKey key, string text, bool active = true, bool gamepadOnly = true, bool translate = true)
		{
			return new LazyGameKeyTip(key, text, active, gamepadOnly, translate).ToString();
		}

		// Token: 0x060006AE RID: 1710 RVA: 0x00022443 File Offset: 0x00020643
		public LazyGameKeyTip(GameKey key, string text, bool active = true, bool gamepadOnly = true, bool translate = true)
		{
			this.key = key;
			this.text = text;
			this.active = active;
			this.translate = translate;
			this.gamepadOnly = gamepadOnly;
		}

		// Token: 0x060006AF RID: 1711 RVA: 0x00022470 File Offset: 0x00020670
		public override string ToString()
		{
			if (this.gamepadOnly && !LazyInput.IsGamepadActive)
			{
				return string.Empty;
			}
			string icon = this.GetIcon(this.key, this.active ? GameKeyIconType.Default : GameKeyIconType.Inactive);
			if (string.IsNullOrEmpty(icon))
			{
				return string.Empty;
			}
			string text = (this.translate ? LLBase.L(this.text) : this.text);
			return icon + (this.active ? (text ?? "") : ("<color=#d7d7d7>" + text + "</color>"));
		}

		// Token: 0x060006B0 RID: 1712 RVA: 0x00022509 File Offset: 0x00020709
		public string GetIcon(GameKey key, GameKeyIconType gameKeyIconType)
		{
			return ControllerIconLibrary.GetIconId(key, gameKeyIconType, false);
		}

		// Token: 0x060006B1 RID: 1713 RVA: 0x00022513 File Offset: 0x00020713
		public static LazyGameKeyTip Select(bool active = true, bool gamepadOnly = true, bool translate = true)
		{
			if (!LazyGameKeyTip.selectAndBackLocalesInitialized)
			{
				Debug.LogWarning("Locales for Select & Back requires initialization. Please call once InitSelectAndBackLocales");
			}
			return new LazyGameKeyTip(GameKey.Select, LazyGameKeyTip.uiSelectLocale, active, gamepadOnly, translate);
		}

		// Token: 0x060006B2 RID: 1714 RVA: 0x00022538 File Offset: 0x00020738
		public static LazyGameKeyTip Back(bool active = true, bool gamepadOnly = true, bool translate = true)
		{
			if (!LazyGameKeyTip.selectAndBackLocalesInitialized)
			{
				Debug.LogWarning("Locales for Select & Back requires initialization. Please call once InitSelectAndBackLocales");
			}
			return new LazyGameKeyTip(GameKey.Back, LazyGameKeyTip.uiBackLocale, active, gamepadOnly, translate);
		}

		// Token: 0x040003F9 RID: 1017
		private const string INACTIVE_TIP_COLOR = "#d7d7d7";

		// Token: 0x040003FA RID: 1018
		private GameKey key;

		// Token: 0x040003FB RID: 1019
		private string text;

		// Token: 0x040003FC RID: 1020
		private bool active;

		// Token: 0x040003FD RID: 1021
		private bool gamepadOnly;

		// Token: 0x040003FE RID: 1022
		private bool translate;

		// Token: 0x040003FF RID: 1023
		private static string uiSelectLocale = "Select";

		// Token: 0x04000400 RID: 1024
		private static string uiBackLocale = "Back";

		// Token: 0x04000401 RID: 1025
		private static bool selectAndBackLocalesInitialized;
	}
}
