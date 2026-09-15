using System;
using FlowCanvas;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x02000B31 RID: 2865
[ExecuteAlways]
[DefaultExecutionOrder(-1)]
public class WeatherComponent : MonoBehaviour
{
	// Token: 0x17000B6A RID: 2922
	// (get) Token: 0x06004C36 RID: 19510 RVA: 0x001678DC File Offset: 0x00165ADC
	public WeatherComponent.AnimationType Animating
	{
		get
		{
			return this.animating;
		}
	}

	// Token: 0x06004C37 RID: 19511 RVA: 0x001678E4 File Offset: 0x00165AE4
	private void Awake()
	{
		this.parameters.Init();
		if (!WeatherSystem.Instance.components.ContainsKey(base.name))
		{
			WeatherSystem.Instance.components.Add(base.name, this);
		}
	}

	// Token: 0x06004C38 RID: 19512 RVA: 0x0016791E File Offset: 0x00165B1E
	private void OnDisable()
	{
		this.parameters.OnDisable();
	}

	// Token: 0x06004C39 RID: 19513 RVA: 0x0016792B File Offset: 0x00165B2B
	public void ClearState()
	{
		this.intensity = 0f;
		this.targetIntensity = 0f;
		this.animating = WeatherComponent.AnimationType.None;
		SoundHandler soundHandler = this.soundHandler;
		if (soundHandler != null)
		{
			soundHandler.Stop();
		}
		this.soundHandler = null;
		this.OnIntensityChanged();
	}

	// Token: 0x06004C3A RID: 19514 RVA: 0x0016796C File Offset: 0x00165B6C
	public void OnIntensityChanged()
	{
		this.parameters.UpdateParameters(this.intensity, this);
		if (Application.isPlaying)
		{
			if (!string.IsNullOrEmpty(this.sound))
			{
				if (this.intensity.EqualsTo(0f, 1E-05f))
				{
					SoundHandler soundHandler = this.soundHandler;
					if (soundHandler != null)
					{
						soundHandler.Pause();
					}
				}
				else
				{
					if (this.soundHandler == null)
					{
						this.soundHandler = LazyAudio.Play(this.sound);
					}
					else if (this.soundHandler.IsPaused)
					{
						this.soundHandler.UnPause();
					}
					this.soundHandler.SetVolume(this.intensity);
				}
			}
			if (this.hasOnDisableFS && this.onDisableFS != null && this.intensity == 0f)
			{
				GlobalScriptsManager.RunFlowScript(this.onDisableFS, null, FlowScriptLoadMode.DeserializeOnInit);
			}
		}
	}

	// Token: 0x06004C3B RID: 19515 RVA: 0x00167A44 File Offset: 0x00165C44
	public void FadeIn()
	{
		base.gameObject.SetActive(true);
		this.targetIntensity = 1f;
		this.animating = WeatherComponent.AnimationType.FadeIn;
		if (this.hasOnEnableFS && this.onEnableFS != null)
		{
			GlobalScriptsManager.RunFlowScript(this.onEnableFS, null, FlowScriptLoadMode.DeserializeOnInit);
		}
	}

	// Token: 0x06004C3C RID: 19516 RVA: 0x00167A92 File Offset: 0x00165C92
	public void FadeOut()
	{
		base.gameObject.SetActive(true);
		this.targetIntensity = 0f;
		this.animating = WeatherComponent.AnimationType.FadeOut;
	}

	// Token: 0x06004C3D RID: 19517 RVA: 0x00167AB2 File Offset: 0x00165CB2
	public void FadeOutIfActive()
	{
		if (this.intensity > 0f)
		{
			this.FadeOut();
		}
	}

	// Token: 0x06004C3E RID: 19518 RVA: 0x00167AC8 File Offset: 0x00165CC8
	public void FadeInAndFadeOutOthers()
	{
		foreach (WeatherComponent weatherComponent in WeatherSystem.Instance.components.Values)
		{
			weatherComponent.FadeOutIfActive();
		}
		this.FadeIn();
	}

	// Token: 0x06004C3F RID: 19519 RVA: 0x00167B28 File Offset: 0x00165D28
	public void CustomUpdate()
	{
		if (this.animating == WeatherComponent.AnimationType.None)
		{
			return;
		}
		float num = Mathf.Sign(this.targetIntensity - this.intensity);
		this.intensity += num * Time.deltaTime / WeatherSystem.Instance.fadeTime;
		if ((num > 0f && this.intensity >= this.targetIntensity) || (num < 0f && this.intensity <= this.targetIntensity))
		{
			this.intensity = this.targetIntensity;
			this.animating = WeatherComponent.AnimationType.None;
		}
		this.OnIntensityChanged();
	}

	// Token: 0x06004C40 RID: 19520 RVA: 0x00167BB8 File Offset: 0x00165DB8
	public T GetControllableParameterOfType<T>() where T : ControllableParameter
	{
		foreach (ControllableParameter controllableParameter in this.parameters.parameters)
		{
			T t = controllableParameter as T;
			if (t != null)
			{
				return t;
			}
		}
		return default(T);
	}

	// Token: 0x04003D51 RID: 15697
	[Range(0f, 1f)]
	public float intensity;

	// Token: 0x04003D52 RID: 15698
	public string sound = "";

	// Token: 0x04003D53 RID: 15699
	public bool hasOnEnableFS;

	// Token: 0x04003D54 RID: 15700
	[SerializeField]
	private FlowScript onEnableFS;

	// Token: 0x04003D55 RID: 15701
	public bool hasOnDisableFS;

	// Token: 0x04003D56 RID: 15702
	[SerializeField]
	private FlowScript onDisableFS;

	// Token: 0x04003D57 RID: 15703
	private SoundHandler soundHandler;

	// Token: 0x04003D58 RID: 15704
	[Space(10f)]
	[SerializeField]
	private ControllableParameterList parameters = new ControllableParameterList();

	// Token: 0x04003D59 RID: 15705
	private WeatherComponent.AnimationType animating;

	// Token: 0x04003D5A RID: 15706
	private float targetIntensity;

	// Token: 0x02000B32 RID: 2866
	public enum AnimationType
	{
		// Token: 0x04003D5C RID: 15708
		None,
		// Token: 0x04003D5D RID: 15709
		FadeIn,
		// Token: 0x04003D5E RID: 15710
		FadeOut
	}
}
