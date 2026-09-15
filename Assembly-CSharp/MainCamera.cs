using System;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;

// Token: 0x02000174 RID: 372
[ExecuteAlways]
public class MainCamera : MonoBehaviour, ISoundZoneRecognizable
{
	// Token: 0x14000002 RID: 2
	// (add) Token: 0x06000932 RID: 2354 RVA: 0x0002F2A4 File Offset: 0x0002D4A4
	// (remove) Token: 0x06000933 RID: 2355 RVA: 0x0002F2D8 File Offset: 0x0002D4D8
	public static event Action<MainCamera.RenderMode> OnRenderModeChanged;

	// Token: 0x1700016B RID: 363
	// (get) Token: 0x06000934 RID: 2356 RVA: 0x0002F30B File Offset: 0x0002D50B
	public RenderTexture RenderTexture
	{
		get
		{
			return this.renderTexture;
		}
	}

	// Token: 0x1700016C RID: 364
	// (get) Token: 0x06000935 RID: 2357 RVA: 0x0002F313 File Offset: 0x0002D513
	public Camera Camera
	{
		get
		{
			return this.cameraComponent;
		}
	}

	// Token: 0x1700016D RID: 365
	// (get) Token: 0x06000936 RID: 2358 RVA: 0x0002F31B File Offset: 0x0002D51B
	public PostProcessVolume PostProcessVolume
	{
		get
		{
			return this.postProcessVolume;
		}
	}

	// Token: 0x1700016E RID: 366
	// (get) Token: 0x06000937 RID: 2359 RVA: 0x0002F323 File Offset: 0x0002D523
	public BlitCameraRT BlitCameraRT
	{
		get
		{
			return this.blitCameraRT;
		}
	}

	// Token: 0x1700016F RID: 367
	// (get) Token: 0x06000938 RID: 2360 RVA: 0x0002F32B File Offset: 0x0002D52B
	private Animator Animator
	{
		get
		{
			if (this.animator == null)
			{
				this.animator = base.GetComponent<Animator>();
			}
			return this.animator;
		}
	}

	// Token: 0x06000939 RID: 2361 RVA: 0x0002F350 File Offset: 0x0002D550
	public Vector3 GetGroundPointPos()
	{
		Vector3 zero = Vector3.zero;
		if (Physics.RaycastNonAlloc(this.cameraComponent.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)), this.results, float.PositiveInfinity, 15) > 0)
		{
			return this.results[0].point;
		}
		return zero;
	}

	// Token: 0x0600093A RID: 2362 RVA: 0x0002F3AC File Offset: 0x0002D5AC
	public void OnResolutionChanged(int width, int height)
	{
		if (this.renderMode == MainCamera.RenderMode.Lightweight)
		{
			this.prevPixelSize = ResolutionConfig.PixelSize;
			this.ReCreateRenderTexture();
		}
		if (this.deformTextureCamera != null && this.cameraComponent != null)
		{
			int width2 = ResolutionConfig.Width;
			int height2 = ResolutionConfig.Height;
			this.deformTextureCamera.ChangeRenderTargetSize(width2, height2, this.cameraComponent.orthographicSize);
		}
	}

	// Token: 0x0600093B RID: 2363 RVA: 0x0002F414 File Offset: 0x0002D614
	public void SetRenderType(MainCamera.RenderMode renderType)
	{
		this.CleanupDepthCopy();
		this.ReleaseRenderTexture();
		this.blitCameraRT.gameObject.SetActive(false);
		this.cameraComponent.targetTexture = null;
		if (renderType != MainCamera.RenderMode.Native && renderType == MainCamera.RenderMode.Lightweight)
		{
			this.blitCameraRT.gameObject.SetActive(true);
			this.ReCreateRenderTexture();
		}
		this.renderMode = renderType;
		Action<MainCamera.RenderMode> onRenderModeChanged = MainCamera.OnRenderModeChanged;
		if (onRenderModeChanged == null)
		{
			return;
		}
		onRenderModeChanged(renderType);
	}

	// Token: 0x0600093C RID: 2364 RVA: 0x0002F47F File Offset: 0x0002D67F
	public MainCamera.RenderMode GetRenderType()
	{
		return this.renderMode;
	}

	// Token: 0x0600093D RID: 2365 RVA: 0x0002F488 File Offset: 0x0002D688
	private void Awake()
	{
		base.transform.eulerAngles = new Vector3(53.130104f, 0f, 0f);
		if (!base.TryGetComponent<PostProcessVolume>(out this.postProcessVolume))
		{
			Debug.LogError("PostProcessVolume wasn't found");
		}
		this.prevPixelSize = ResolutionConfig.PixelSize;
		this.defaultPostProcessProfile = ((this.postProcessVolume != null) ? this.postProcessVolume.sharedProfile : null);
		this.CacheBloomFromActiveProfile();
	}

	// Token: 0x0600093E RID: 2366 RVA: 0x0002F500 File Offset: 0x0002D700
	public void SetPostProcessProfile(string profileName)
	{
		if (string.IsNullOrEmpty(profileName))
		{
			this.ResetPostProcessProfile();
			return;
		}
		PostProcessProfile postProcessProfile = this.FindPostProcessProfile(profileName);
		if (postProcessProfile == null)
		{
			Debug.LogError("Post process profile [" + profileName + "] wasn't found");
			return;
		}
		this.ApplyPostProcessProfile(postProcessProfile);
	}

	// Token: 0x0600093F RID: 2367 RVA: 0x0002F54A File Offset: 0x0002D74A
	public void ResetPostProcessProfile()
	{
		this.ApplyPostProcessProfile(this.defaultPostProcessProfile);
	}

	// Token: 0x06000940 RID: 2368 RVA: 0x0002F558 File Offset: 0x0002D758
	private PostProcessProfile FindPostProcessProfile(string profileName)
	{
		if (this.postProcessProfiles == null)
		{
			return null;
		}
		for (int i = 0; i < this.postProcessProfiles.Count; i++)
		{
			PostProcessProfile postProcessProfile = this.postProcessProfiles[i];
			if (postProcessProfile != null && postProcessProfile.name == profileName)
			{
				return postProcessProfile;
			}
		}
		return null;
	}

	// Token: 0x06000941 RID: 2369 RVA: 0x0002F5AC File Offset: 0x0002D7AC
	private void ApplyPostProcessProfile(PostProcessProfile asset)
	{
		if (asset == null || this.postProcessVolume == null)
		{
			return;
		}
		if (this.postProcessVolume.HasInstantiatedProfile())
		{
			PostProcessProfile profile = this.postProcessVolume.profile;
			this.postProcessVolume.profile = null;
			if (profile != null)
			{
				if (Application.isPlaying)
				{
					global::UnityEngine.Object.Destroy(profile);
				}
				else
				{
					global::UnityEngine.Object.DestroyImmediate(profile);
				}
			}
		}
		this.postProcessVolume.sharedProfile = asset;
		this.CacheBloomFromActiveProfile();
		if (EnvironmentEngine.Instance != null)
		{
			EnvironmentEngine.Instance.RefreshAppliedLightPreset();
		}
		Debug.Log("Set post process profile to: " + asset.name);
	}

	// Token: 0x06000942 RID: 2370 RVA: 0x0002F652 File Offset: 0x0002D852
	private void CacheBloomFromActiveProfile()
	{
		this.bloom = ((this.PostProcessVolume != null) ? this.PostProcessVolume.profile.GetSetting<Bloom>() : null);
		this.UpdateBloomThreshold();
	}

	// Token: 0x06000943 RID: 2371 RVA: 0x0002F681 File Offset: 0x0002D881
	private void Start()
	{
		if (this.renderMode == MainCamera.RenderMode.Lightweight)
		{
			this.ReCreateRenderTexture();
		}
	}

	// Token: 0x06000944 RID: 2372 RVA: 0x0002F692 File Offset: 0x0002D892
	private void OnPreRender()
	{
		Shader.EnableKeyword("WORLD_RENDER");
		if (this.depthBindCmd != null)
		{
			Graphics.ExecuteCommandBuffer(this.depthBindCmd);
		}
	}

	// Token: 0x06000945 RID: 2373 RVA: 0x0002F6B1 File Offset: 0x0002D8B1
	private void OnPostRender()
	{
		Shader.DisableKeyword("WORLD_RENDER");
	}

	// Token: 0x06000946 RID: 2374 RVA: 0x0002F6C0 File Offset: 0x0002D8C0
	private void Update()
	{
		if (this.isFreeCamera)
		{
			this.cameraComponent.orthographicSize = this.storedOrthographicSize * this.freeCameraSizeK;
			if (this.isPlayingAnimation && !string.IsNullOrEmpty(this.currentAnimationName))
			{
				string text = ((this.Animator.GetCurrentAnimatorClipInfo(0).Length != 0) ? this.Animator.GetCurrentAnimatorClipInfo(0)[0].clip.name : string.Empty);
				if (this.waitingForEnterAnimation && text == this.currentAnimationName)
				{
					this.waitingForEnterAnimation = false;
				}
				else if (text != this.currentAnimationName && !this.waitingForEnterAnimation)
				{
					Debug.Log("Camera.Update: Animation finished: " + text + " != " + this.currentAnimationName);
					this.OnAnimationFinished();
				}
			}
		}
		if (this.renderMode == MainCamera.RenderMode.Lightweight && this.prevPixelSize != ResolutionConfig.PixelSize)
		{
			this.prevPixelSize = ResolutionConfig.PixelSize;
			this.ReCreateRenderTexture();
		}
	}

	// Token: 0x06000947 RID: 2375 RVA: 0x0002F7BC File Offset: 0x0002D9BC
	private void SetFreeCameraMode(bool isFreeCamera)
	{
		Debug.Log(string.Format("Camera.SetFreeCameraMode: {0}", isFreeCamera));
		this.isFreeCamera = isFreeCamera;
		base.GetComponent<CinemachineBrain>().enabled = !isFreeCamera;
		if (isFreeCamera)
		{
			this.storedOrthographicSize = this.cameraComponent.orthographicSize;
			this.Update();
			return;
		}
		this.cameraComponent.orthographicSize = this.storedOrthographicSize;
		this.freeCameraSizeK = 1f;
	}

	// Token: 0x06000948 RID: 2376 RVA: 0x0002F82C File Offset: 0x0002DA2C
	public void PlayAnimation(string animationName, Action onFinished)
	{
		Debug.Log("Camera.PlayAnimation: " + animationName);
		this.currentAnimationName = animationName;
		this.waitingForEnterAnimation = true;
		this.isPlayingAnimation = true;
		this.onAnimationFinished = onFinished;
		this.SetFreeCameraMode(true);
		this.Animator.enabled = true;
		this.Animator.Play(animationName);
	}

	// Token: 0x06000949 RID: 2377 RVA: 0x0002F884 File Offset: 0x0002DA84
	public void UpdateBloomThreshold()
	{
		if (this.bloom == null)
		{
			return;
		}
		float num = 0f;
		foreach (Func<float> func in this.additionalThresholdGetters)
		{
			if (func != null)
			{
				num += func();
			}
		}
		this.bloom.threshold.value = this.bloomBaseThreshold + num;
	}

	// Token: 0x0600094A RID: 2378 RVA: 0x0002F90C File Offset: 0x0002DB0C
	public void AddAdditionalBloomThresholdGetter(Func<float> getter)
	{
		if (getter == null)
		{
			return;
		}
		this.additionalThresholdGetters.Add(getter);
	}

	// Token: 0x0600094B RID: 2379 RVA: 0x0002F91E File Offset: 0x0002DB1E
	private void OnAnimationFinished()
	{
		Debug.Log("Camera.OnAnimationFinished");
		this.isPlayingAnimation = false;
		this.Animator.enabled = false;
		this.SetFreeCameraMode(false);
		Action action = this.onAnimationFinished;
		this.onAnimationFinished = null;
		if (action == null)
		{
			return;
		}
		action();
	}

	// Token: 0x0600094C RID: 2380 RVA: 0x0002F95C File Offset: 0x0002DB5C
	private void ReCreateRenderTexture()
	{
		this.CleanupDepthCopy();
		this.ReleaseRenderTexture();
		int width = ResolutionConfig.Width;
		int height = ResolutionConfig.Height;
		this.renderTexture = new RenderTexture(width, height, GraphicsFormat.R8G8B8A8_UNorm, GraphicsFormat.D32_SFloat_S8_UInt);
		this.renderTexture.filterMode = FilterMode.Point;
		this.renderTexture.Create();
		this.cameraComponent.targetTexture = this.renderTexture;
		this.SetupDepthCopy();
		LightRTManager.NotifyWorldRenderTargetChanged();
	}

	// Token: 0x17000170 RID: 368
	// (get) Token: 0x0600094D RID: 2381 RVA: 0x0002F9C5 File Offset: 0x0002DBC5
	public bool IsDepthCopyActive
	{
		get
		{
			return this.depthBindCmd != null;
		}
	}

	// Token: 0x0600094E RID: 2382 RVA: 0x0002F9D0 File Offset: 0x0002DBD0
	public void SetDepthCopyEnabled(bool enabled)
	{
		if (enabled)
		{
			this.SetupDepthCopy();
			return;
		}
		this.CleanupDepthCopy();
	}

	// Token: 0x0600094F RID: 2383 RVA: 0x0002F9E4 File Offset: 0x0002DBE4
	private void SetupDepthCopy()
	{
		this.CleanupDepthCopy();
		if (this.renderTexture == null)
		{
			return;
		}
		this.depthBindCmd = new CommandBuffer
		{
			name = "Bind RT Depth"
		};
		this.depthBindCmd.SetGlobalTexture("_CameraDepthTexture", this.renderTexture, RenderTextureSubElement.Depth);
		Graphics.ExecuteCommandBuffer(this.depthBindCmd);
	}

	// Token: 0x06000950 RID: 2384 RVA: 0x0002FA43 File Offset: 0x0002DC43
	private void CleanupDepthCopy()
	{
		if (this.depthBindCmd != null)
		{
			this.depthBindCmd.Release();
			this.depthBindCmd = null;
			Shader.SetGlobalTexture("_CameraDepthTexture", Texture2D.whiteTexture);
		}
	}

	// Token: 0x06000951 RID: 2385 RVA: 0x0002FA70 File Offset: 0x0002DC70
	private void ReleaseRenderTexture()
	{
		if (this.renderTexture == null)
		{
			return;
		}
		if (this.cameraComponent != null && this.cameraComponent.targetTexture == this.renderTexture)
		{
			this.cameraComponent.targetTexture = null;
		}
		this.renderTexture.Release();
		if (Application.isPlaying)
		{
			global::UnityEngine.Object.Destroy(this.renderTexture);
		}
		else
		{
			global::UnityEngine.Object.DestroyImmediate(this.renderTexture);
		}
		this.renderTexture = null;
	}

	// Token: 0x06000952 RID: 2386 RVA: 0x0002FAEF File Offset: 0x0002DCEF
	private void OnDestroy()
	{
		this.CleanupDepthCopy();
		this.ReleaseRenderTexture();
	}

	// Token: 0x04000ADF RID: 2783
	[SerializeField]
	private PostProcessVolume postProcessVolume;

	// Token: 0x04000AE0 RID: 2784
	[SerializeField]
	private Camera cameraComponent;

	// Token: 0x04000AE1 RID: 2785
	[SerializeField]
	private DeformTextureCamera deformTextureCamera;

	// Token: 0x04000AE2 RID: 2786
	[Space]
	[SerializeField]
	private MainCamera.RenderMode renderMode;

	// Token: 0x04000AE3 RID: 2787
	[SerializeField]
	private BlitCameraRT blitCameraRT;

	// Token: 0x04000AE4 RID: 2788
	[SerializeField]
	[Space]
	private float bloomBaseThreshold = 1.08f;

	// Token: 0x04000AE5 RID: 2789
	[SerializeField]
	[Space]
	private List<PostProcessProfile> postProcessProfiles = new List<PostProcessProfile>();

	// Token: 0x04000AE6 RID: 2790
	private List<Func<float>> additionalThresholdGetters = new List<Func<float>>();

	// Token: 0x04000AE7 RID: 2791
	private Bloom bloom;

	// Token: 0x04000AE8 RID: 2792
	private PostProcessProfile defaultPostProcessProfile;

	// Token: 0x04000AE9 RID: 2793
	private RenderTexture renderTexture;

	// Token: 0x04000AEA RID: 2794
	private int prevPixelSize;

	// Token: 0x04000AEB RID: 2795
	private CommandBuffer depthBindCmd;

	// Token: 0x04000AEC RID: 2796
	private RaycastHit[] results = new RaycastHit[1];

	// Token: 0x04000AED RID: 2797
	public float freeCameraSizeK = 1f;

	// Token: 0x04000AEE RID: 2798
	private float lastFreeCameraSizeK = 1f;

	// Token: 0x04000AEF RID: 2799
	private bool isFreeCamera;

	// Token: 0x04000AF0 RID: 2800
	private float storedOrthographicSize;

	// Token: 0x04000AF1 RID: 2801
	private string currentAnimationName = "";

	// Token: 0x04000AF2 RID: 2802
	private bool waitingForEnterAnimation;

	// Token: 0x04000AF3 RID: 2803
	private bool isPlayingAnimation;

	// Token: 0x04000AF4 RID: 2804
	private Action onAnimationFinished;

	// Token: 0x04000AF5 RID: 2805
	private Animator animator;

	// Token: 0x02000175 RID: 373
	public enum RenderMode
	{
		// Token: 0x04000AF7 RID: 2807
		Native,
		// Token: 0x04000AF8 RID: 2808
		Lightweight
	}
}
