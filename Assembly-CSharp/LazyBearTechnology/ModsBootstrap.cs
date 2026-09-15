using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace LazyBearTechnology
{
	// Token: 0x02000C51 RID: 3153
	public static class ModsBootstrap
	{
		// Token: 0x06005068 RID: 20584 RVA: 0x0017C86C File Offset: 0x0017AA6C
		public static void EnsureOnStartup()
		{
			ModsBootstrap.workshopScanAttempted = false;
			try
			{
				Directory.CreateDirectory(ModsPaths.Root);
				File.WriteAllText(Path.Combine(ModsPaths.Root, "README.txt"), ModsDocumentation.Load("Mods/README"));
				VoiceOverModLoader.SearchRootsProvider = new Func<IReadOnlyList<string>>(ModsPaths.GetModSearchRoots);
				SteamWorkshopCreatorConfig.EnsureAndReload();
				LanguageModLoader.Scan();
			}
			catch (Exception ex)
			{
				Debug.LogWarning("[Mods] Failed to initialize mods folder: " + ex.Message);
			}
		}

		// Token: 0x06005069 RID: 20585 RVA: 0x0017C8F0 File Offset: 0x0017AAF0
		public static void Tick()
		{
			SteamWorkshopInstalledItems.Tick();
			if (!ModsBootstrap.workshopScanAttempted && SteamWorkshopInstalledItems.IsSteamReady())
			{
				ModsBootstrap.workshopScanAttempted = true;
				ModsBootstrap.ReloadMods(false);
			}
			else if (SteamWorkshopInstalledItems.PendingRescan)
			{
				SteamWorkshopInstalledItems.PendingRescan = false;
				ModsBootstrap.ReloadMods(false);
			}
			if (!Input.GetKeyDown(KeyCode.F10))
			{
				return;
			}
			if (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift))
			{
				return;
			}
			ModsBootstrap.ReloadAndScaffold();
		}

		// Token: 0x0600506A RID: 20586 RVA: 0x0017C95C File Offset: 0x0017AB5C
		public static void ReloadAndScaffold()
		{
			ModsBootstrap.ReloadMods(true);
		}

		// Token: 0x0600506B RID: 20587 RVA: 0x0017C964 File Offset: 0x0017AB64
		private static void ReloadMods(bool scaffold)
		{
			try
			{
				Directory.CreateDirectory(ModsPaths.Root);
				SteamWorkshopCreatorConfig.EnsureAndReload();
				if (scaffold)
				{
					File.WriteAllText(Path.Combine(ModsPaths.Root, "README.txt"), ModsDocumentation.Load("Mods/README"));
					Directory.CreateDirectory(ModsPaths.LanguagesRoot);
					File.WriteAllText(Path.Combine(ModsPaths.LanguagesRoot, "README.txt"), ModsDocumentation.Load("Mods/Languages/README"));
					LanguageModLoader.EnsureExamplePack();
				}
				LanguageModLoader.Scan();
				if (GameSettings.Instance != null)
				{
					GameSettings.Instance.ApplyLanguageSettings(true);
				}
				if (GUIElements.Instance != null)
				{
					GUIElements.Instance.UpdateLocalizedLabels();
					TextStyleComponent.RefreshAll();
					LazyButton.RefreshAll();
				}
				UIGameSettingsWindow.RefreshLanguageSwitcherIfOpen();
				Debug.Log("[Mods] Reloaded. Folders: " + string.Join(" | ", ModsPaths.GetModSearchRoots()));
			}
			catch (Exception ex)
			{
				Debug.LogWarning("[Mods] Reload failed: " + ex.Message);
			}
		}

		// Token: 0x040041EC RID: 16876
		private static bool workshopScanAttempted;
	}
}
