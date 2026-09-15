using System;
using System.Collections;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Audio;
using UnityEngine.ResourceManagement.AsyncOperations;

// Token: 0x02000866 RID: 2150
public static class VoiceOverModePreview
{
	// Token: 0x060036FD RID: 14077 RVA: 0x0010A043 File Offset: 0x00108243
	public static void Warmup()
	{
		VoiceOverModePreview.EnsureClipLoadStarted();
	}

	// Token: 0x060036FE RID: 14078 RVA: 0x0010A04A File Offset: 0x0010824A
	public static void Play(VoiceOverMode mode, MonoBehaviour coroutineHost)
	{
		VoiceOverModePreview.Stop(coroutineHost);
		if (mode == VoiceOverMode.VoiceOver)
		{
			VoiceOverModePreview.PlayVoiceOverPreview();
			return;
		}
		if (coroutineHost == null || LazySpeechEngine.Instance == null)
		{
			return;
		}
		VoiceOverModePreview.previewRoutine = coroutineHost.StartCoroutine(VoiceOverModePreview.PlayMumblingPreview());
	}

	// Token: 0x060036FF RID: 14079 RVA: 0x0010A084 File Offset: 0x00108284
	public static void Stop(MonoBehaviour coroutineHost)
	{
		if (VoiceOverModePreview.previewRoutine != null && coroutineHost != null)
		{
			coroutineHost.StopCoroutine(VoiceOverModePreview.previewRoutine);
			VoiceOverModePreview.previewRoutine = null;
		}
		if (VoiceOverModePreview.previewSource != null)
		{
			VoiceOverModePreview.previewSource.Stop();
		}
		LazySpeechEngine instance = LazySpeechEngine.Instance;
		if (instance == null)
		{
			return;
		}
		instance.Stop(VoiceOverModePreview.VoiceOverPreviewVoiceId);
	}

	// Token: 0x06003700 RID: 14080 RVA: 0x0010A0E0 File Offset: 0x001082E0
	private static void PlayVoiceOverPreview()
	{
		AudioClip clip = VoiceOverModePreview.GetClip();
		if (clip == null)
		{
			return;
		}
		AudioSource audioSource = VoiceOverModePreview.GetPreviewSource();
		audioSource.clip = clip;
		audioSource.loop = false;
		audioSource.outputAudioMixerGroup = VoiceOverModePreview.GetSpeechMixerGroup();
		audioSource.Play();
	}

	// Token: 0x06003701 RID: 14081 RVA: 0x0010A120 File Offset: 0x00108320
	private static void EnsureClipLoadStarted()
	{
		if (VoiceOverModePreview.cachedClip != null || VoiceOverModePreview.clipHandle.IsValid())
		{
			return;
		}
		VoiceOverModePreview.clipHandle = Addressables.LoadAssetAsync<AudioClip>("153_crossroad_dark_come_1.wav");
	}

	// Token: 0x06003702 RID: 14082 RVA: 0x0010A14B File Offset: 0x0010834B
	private static AudioClip GetClip()
	{
		if (VoiceOverModePreview.cachedClip != null)
		{
			return VoiceOverModePreview.cachedClip;
		}
		VoiceOverModePreview.EnsureClipLoadStarted();
		if (!VoiceOverModePreview.clipHandle.IsValid())
		{
			return null;
		}
		VoiceOverModePreview.cachedClip = VoiceOverModePreview.clipHandle.WaitForCompletion();
		return VoiceOverModePreview.cachedClip;
	}

	// Token: 0x06003703 RID: 14083 RVA: 0x0010A187 File Offset: 0x00108387
	private static IEnumerator PlayMumblingPreview()
	{
		float remaining = 1.5f;
		LazySpeechEngine engine = LazySpeechEngine.Instance;
		while (remaining > 0f && engine != null)
		{
			engine.Play(VoiceOverModePreview.VoiceOverPreviewVoiceId, remaining);
			remaining -= Time.unscaledDeltaTime;
			yield return null;
		}
		LazySpeechEngine lazySpeechEngine = engine;
		if (lazySpeechEngine != null)
		{
			lazySpeechEngine.Stop(VoiceOverModePreview.VoiceOverPreviewVoiceId);
		}
		VoiceOverModePreview.previewRoutine = null;
		yield break;
	}

	// Token: 0x06003704 RID: 14084 RVA: 0x0010A190 File Offset: 0x00108390
	private static AudioSource GetPreviewSource()
	{
		if (VoiceOverModePreview.previewSource != null)
		{
			return VoiceOverModePreview.previewSource;
		}
		GameObject gameObject = new GameObject("VoiceOverModePreview");
		global::UnityEngine.Object.DontDestroyOnLoad(gameObject);
		VoiceOverModePreview.previewSource = gameObject.AddComponent<AudioSource>();
		VoiceOverModePreview.previewSource.playOnAwake = false;
		VoiceOverModePreview.previewSource.spatialBlend = 0f;
		return VoiceOverModePreview.previewSource;
	}

	// Token: 0x06003705 RID: 14085 RVA: 0x0010A1E9 File Offset: 0x001083E9
	private static AudioMixerGroup GetSpeechMixerGroup()
	{
		if (!(LazySingletonSO<AudioConfig>.Instance != null))
		{
			return null;
		}
		return LazySingletonSO<AudioConfig>.Instance.voiceOverGroup;
	}

	// Token: 0x04002BE3 RID: 11235
	private const string VoiceOverPreviewId = "153_crossroad_dark_come_1";

	// Token: 0x04002BE4 RID: 11236
	private static readonly VoiceID VoiceOverPreviewVoiceId = VoiceID.Larry;

	// Token: 0x04002BE5 RID: 11237
	private const float MumblingPreviewDuration = 1.5f;

	// Token: 0x04002BE6 RID: 11238
	private static Coroutine previewRoutine;

	// Token: 0x04002BE7 RID: 11239
	private static AudioSource previewSource;

	// Token: 0x04002BE8 RID: 11240
	private static AudioClip cachedClip;

	// Token: 0x04002BE9 RID: 11241
	private static AsyncOperationHandle<AudioClip> clipHandle;
}
