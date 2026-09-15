using System;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.Playables;

// Token: 0x02000705 RID: 1797
public class CinematicsScene : MonoBehaviour
{
	// Token: 0x17000757 RID: 1879
	// (get) Token: 0x06002F63 RID: 12131 RVA: 0x000E357D File Offset: 0x000E177D
	public Transform CameraTarget
	{
		get
		{
			return this.cameraTarget;
		}
	}

	// Token: 0x17000758 RID: 1880
	// (get) Token: 0x06002F64 RID: 12132 RVA: 0x000E3585 File Offset: 0x000E1785
	public Animator Animator
	{
		get
		{
			return this.animator;
		}
	}

	// Token: 0x17000759 RID: 1881
	// (get) Token: 0x06002F65 RID: 12133 RVA: 0x000E358D File Offset: 0x000E178D
	public PlayableDirector Director
	{
		get
		{
			return this.director;
		}
	}

	// Token: 0x06002F66 RID: 12134 RVA: 0x000E3598 File Offset: 0x000E1798
	private void Awake()
	{
		this.animator.enabled = false;
		this.director.playOnAwake = false;
		this.director.initialTime = 0.0;
		this.commonObject = base.GetComponentInChildren<CinematicsCommonObject>(true);
		this.ApplyCinematicViewScaleByCurrentResolution();
	}

	// Token: 0x06002F67 RID: 12135 RVA: 0x000E35E4 File Offset: 0x000E17E4
	public void Play()
	{
		this.animator.runtimeAnimatorController = null;
		this.animator.enabled = true;
		TextStyleComponent componentInChildren = base.GetComponentInChildren<TextStyleComponent>(true);
		if (componentInChildren)
		{
			componentInChildren.ApplyStyle();
		}
		this.animator.Rebind();
		this.animator.Update(0f);
		this.director.Stop();
		this.director.time = 0.0;
		this.director.RebuildGraph();
		this.director.Play();
	}

	// Token: 0x06002F68 RID: 12136 RVA: 0x000E3670 File Offset: 0x000E1870
	private void ApplyCinematicViewScaleByCurrentResolution()
	{
		bool flag = PlatformFeatureConfig.Get(GamePlatformResolver.Current).platform == GamePlatform.Switch;
		bool flag2 = (flag && ResolutionConfig.currentResolution.AppliedHeight <= 720) || (ResolutionConfig.currentResolution != null && ResolutionConfig.currentResolution.UseMainMenuScaleX2);
		base.transform.localScale = (flag2 ? this.cinematicViewScaleX2 : this.cinematicViewScaleX1);
		if (this.commonObject == null)
		{
			return;
		}
		this.commonObject.SetSwitchVariant(flag);
		Transform center = this.commonObject.GetCenter(flag);
		if (center != null)
		{
			this.cameraTarget = center;
		}
	}

	// Token: 0x04002649 RID: 9801
	[SerializeField]
	private Animator animator;

	// Token: 0x0400264A RID: 9802
	[SerializeField]
	private PlayableDirector director;

	// Token: 0x0400264B RID: 9803
	[SerializeField]
	private Transform cameraTarget;

	// Token: 0x0400264C RID: 9804
	[SerializeField]
	private Vector3 cinematicViewScaleX1 = Vector3.one;

	// Token: 0x0400264D RID: 9805
	[SerializeField]
	private Vector3 cinematicViewScaleX2 = Vector3.one * 2f;

	// Token: 0x0400264E RID: 9806
	private CinematicsCommonObject commonObject;
}
