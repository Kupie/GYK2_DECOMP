using System;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.Audio;

// Token: 0x02000123 RID: 291
public static class InWorldSfxFilterController
{
	// Token: 0x06000709 RID: 1801 RVA: 0x00021BDC File Offset: 0x0001FDDC
	public static void Init()
	{
		if (InWorldSfxFilterController.isInitialized)
		{
			return;
		}
		InWorldSfxFilterController.isInitialized = true;
		GameObject gameObject = new GameObject("InWorldSfxFilterController");
		if (MainGame.Instance != null)
		{
			gameObject.transform.SetParent(MainGame.Instance.transform);
		}
		gameObject.AddComponent<InWorldSfxFilterController.Driver>();
		LazyWindowsStackController.OnWindowOpened += InWorldSfxFilterController.OnWindowStackChanged;
		LazyWindowsStackController.OnWindowClosed += InWorldSfxFilterController.OnWindowStackChanged;
		MainGame.OnGoToMainMenu = (Action)Delegate.Combine(MainGame.OnGoToMainMenu, new Action(InWorldSfxFilterController.OnGoToMainMenu));
		MainGame.OnGameStarted = (Action)Delegate.Combine(MainGame.OnGameStarted, new Action(InWorldSfxFilterController.OnGameStarted));
		InWorldSfxFilterController.SetEnabledImmediate(false);
	}

	// Token: 0x0600070A RID: 1802 RVA: 0x00021C94 File Offset: 0x0001FE94
	public static void Shutdown()
	{
		if (!InWorldSfxFilterController.isInitialized)
		{
			return;
		}
		LazyWindowsStackController.OnWindowOpened -= InWorldSfxFilterController.OnWindowStackChanged;
		LazyWindowsStackController.OnWindowClosed -= InWorldSfxFilterController.OnWindowStackChanged;
		MainGame.OnGoToMainMenu = (Action)Delegate.Remove(MainGame.OnGoToMainMenu, new Action(InWorldSfxFilterController.OnGoToMainMenu));
		MainGame.OnGameStarted = (Action)Delegate.Remove(MainGame.OnGameStarted, new Action(InWorldSfxFilterController.OnGameStarted));
		InWorldSfxFilterController.isInitialized = false;
		InWorldSfxFilterController.cachedMixer = null;
	}

	// Token: 0x0600070B RID: 1803 RVA: 0x00021D17 File Offset: 0x0001FF17
	public static void SetEnabled(bool enabled)
	{
		InWorldSfxFilterController.targetValue = (enabled ? 0f : (-80f));
	}

	// Token: 0x0600070C RID: 1804 RVA: 0x00021D2D File Offset: 0x0001FF2D
	public static void SetEnabledImmediate(bool enabled)
	{
		InWorldSfxFilterController.targetValue = (enabled ? 0f : (-80f));
		InWorldSfxFilterController.currentValue = InWorldSfxFilterController.targetValue;
		InWorldSfxFilterController.Apply(InWorldSfxFilterController.currentValue);
	}

	// Token: 0x0600070D RID: 1805 RVA: 0x00021D57 File Offset: 0x0001FF57
	public static void Reapply()
	{
		if (!InWorldSfxFilterController.isInitialized)
		{
			return;
		}
		InWorldSfxFilterController.Apply(InWorldSfxFilterController.currentValue);
	}

	// Token: 0x0600070E RID: 1806 RVA: 0x00021D6B File Offset: 0x0001FF6B
	public static void SyncToGameplayUiState()
	{
		InWorldSfxFilterController.SetEnabled(InWorldSfxFilterController.ShouldEnable());
	}

	// Token: 0x0600070F RID: 1807 RVA: 0x00021D77 File Offset: 0x0001FF77
	private static bool ShouldEnable()
	{
		return MainGame.Instance != null && MainGame.Instance.gameState == MainGame.GameState.InGame && LazyWindowsStackController.HasAnyModalWindowOpened && !InWorldSfxFilterController.IsFishingMinigameSuppressingDuck();
	}

	// Token: 0x06000710 RID: 1808 RVA: 0x00021DA4 File Offset: 0x0001FFA4
	private static bool IsFishingMinigameSuppressingDuck()
	{
		UIFishingWindow uifishingWindow = LazyWindowsStackController.ActiveWindow as UIFishingWindow;
		return uifishingWindow != null && uifishingWindow.IsShown && !uifishingWindow.IsBaitSelectionVisible;
	}

	// Token: 0x06000711 RID: 1809 RVA: 0x00021DD2 File Offset: 0x0001FFD2
	private static void OnWindowStackChanged(LazyWidgetBase _)
	{
		InWorldSfxFilterController.SyncToGameplayUiState();
	}

	// Token: 0x06000712 RID: 1810 RVA: 0x00021DD9 File Offset: 0x0001FFD9
	private static void OnGoToMainMenu()
	{
		InWorldSfxFilterController.SetEnabledImmediate(false);
	}

	// Token: 0x06000713 RID: 1811 RVA: 0x00021DD2 File Offset: 0x0001FFD2
	private static void OnGameStarted()
	{
		InWorldSfxFilterController.SyncToGameplayUiState();
	}

	// Token: 0x06000714 RID: 1812 RVA: 0x00021DE4 File Offset: 0x0001FFE4
	private static void Apply(float value)
	{
		AudioMixer audioMixer;
		if (!InWorldSfxFilterController.TryGetMixer(out audioMixer))
		{
			return;
		}
		if (audioMixer.SetFloat("inworld_sfx_filter", value))
		{
			return;
		}
		if (InWorldSfxFilterController.loggedMissingParameter)
		{
			return;
		}
		InWorldSfxFilterController.loggedMissingParameter = true;
		Debug.LogError("InWorldSfxFilterController: mixer parameter [inworld_sfx_filter] is not exposed");
	}

	// Token: 0x06000715 RID: 1813 RVA: 0x00021E22 File Offset: 0x00020022
	private static bool TryGetMixer(out AudioMixer mixer)
	{
		if (InWorldSfxFilterController.cachedMixer == null)
		{
			LazyAudio.TryGetAudioMixer(out InWorldSfxFilterController.cachedMixer);
		}
		mixer = InWorldSfxFilterController.cachedMixer;
		return mixer != null;
	}

	// Token: 0x040008FB RID: 2299
	public const string MixerParameter = "inworld_sfx_filter";

	// Token: 0x040008FC RID: 2300
	private const float DisabledValue = -80f;

	// Token: 0x040008FD RID: 2301
	private const float EnabledValue = 0f;

	// Token: 0x040008FE RID: 2302
	private const float TransitionDuration = 0.12f;

	// Token: 0x040008FF RID: 2303
	private static float currentValue = -80f;

	// Token: 0x04000900 RID: 2304
	private static float targetValue = -80f;

	// Token: 0x04000901 RID: 2305
	private static bool isInitialized;

	// Token: 0x04000902 RID: 2306
	private static bool loggedMissingParameter;

	// Token: 0x04000903 RID: 2307
	private static AudioMixer cachedMixer;

	// Token: 0x02000124 RID: 292
	private sealed class Driver : MonoBehaviour
	{
		// Token: 0x06000717 RID: 1815 RVA: 0x00021E64 File Offset: 0x00020064
		private void LateUpdate()
		{
			InWorldSfxFilterController.SyncToGameplayUiState();
			if (!Mathf.Approximately(InWorldSfxFilterController.currentValue, InWorldSfxFilterController.targetValue))
			{
				float num = Mathf.Abs(80f);
				float num2 = ((num > 0f) ? (num / 0.12f) : 0f);
				InWorldSfxFilterController.currentValue = Mathf.MoveTowards(InWorldSfxFilterController.currentValue, InWorldSfxFilterController.targetValue, num2 * Time.unscaledDeltaTime);
			}
			InWorldSfxFilterController.Apply(InWorldSfxFilterController.currentValue);
		}
	}
}
