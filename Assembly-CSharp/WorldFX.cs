using System;
using UnityEngine;

// Token: 0x02000532 RID: 1330
public class WorldFX : MonoBehaviour
{
	// Token: 0x1700058F RID: 1423
	// (get) Token: 0x06002237 RID: 8759 RVA: 0x000A0BE8 File Offset: 0x0009EDE8
	private float Duration
	{
		get
		{
			ParticleSystem particleSystem = this.MainParticleSystem;
			if (particleSystem == null)
			{
				return this.customDuration;
			}
			return particleSystem.main.duration;
		}
	}

	// Token: 0x17000590 RID: 1424
	// (get) Token: 0x06002238 RID: 8760 RVA: 0x000A0C13 File Offset: 0x0009EE13
	public ParticleSystem MainParticleSystem
	{
		get
		{
			if (!this.mainParticleSystem)
			{
				base.TryGetComponent<ParticleSystem>(out this.mainParticleSystem);
			}
			return this.mainParticleSystem;
		}
	}

	// Token: 0x06002239 RID: 8761 RVA: 0x000A0C38 File Offset: 0x0009EE38
	public static WorldFX Spawn(Vector3 worldPos, string name, Action onAction = null, Vector3 size = default(Vector3))
	{
		if (size == default(Vector3))
		{
			size = Vector3.one;
		}
		WorldFX worldFX = WorldFXPool.Get(name);
		if (worldFX == null)
		{
			Debug.LogError("Error spawning WorldFX \"" + name + "\": Effect on found.");
			return null;
		}
		worldFX.name = name;
		worldFX.transform.position = worldPos;
		worldFX.transform.rotation = Quaternion.identity;
		worldFX.transform.localScale = size;
		worldFX.gameObject.SetActive(true);
		worldFX.Play(onAction, null);
		worldFX.returnToPoolAfterEnd = true;
		return worldFX;
	}

	// Token: 0x0600223A RID: 8762 RVA: 0x000A0CCF File Offset: 0x0009EECF
	public static WorldFX Spawn(Transform atTransform, string id, Action onAction = null, Vector3 size = default(Vector3))
	{
		if (atTransform == null)
		{
			Debug.LogError("Error spawning WorldFX: target Transform is null.");
			return null;
		}
		return WorldFX.Spawn(atTransform.position, id, onAction, size);
	}

	// Token: 0x0600223B RID: 8763 RVA: 0x000A0CF4 File Offset: 0x0009EEF4
	public void Play(Action onAction, Action onFinished = null)
	{
		if (!base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(true);
		}
		if (this.MainParticleSystem)
		{
			foreach (ParticleSystem particleSystem in base.GetComponentsInChildren<ParticleSystem>())
			{
				particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
				particleSystem.Simulate(0f, true, true);
				particleSystem.Play();
			}
		}
		if (this.useAnimator && this.animator)
		{
			this.animator.Rebind();
			this.animator.Update(0f);
			this.animator.Play(0, -1, 0f);
		}
		this.onFinished = onFinished;
		this.onActionTicked = onAction;
		this.followTarget = null;
		this.playingTime = 0f;
		this.isPlaying = true;
		this.actionWasExecuted = false;
	}

	// Token: 0x0600223C RID: 8764 RVA: 0x000A0DC9 File Offset: 0x0009EFC9
	public void Follow(Transform target)
	{
		this.followTarget = target;
		this.SyncFollowTransform();
	}

	// Token: 0x0600223D RID: 8765 RVA: 0x000A0DD8 File Offset: 0x0009EFD8
	private void Stop()
	{
		if (this.MainParticleSystem)
		{
			ParticleSystem[] componentsInChildren = base.GetComponentsInChildren<ParticleSystem>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
			}
		}
		this.isPlaying = false;
	}

	// Token: 0x0600223E RID: 8766 RVA: 0x000A0E18 File Offset: 0x0009F018
	private void OnDisable()
	{
		bool flag = this.isPlaying;
		this.isPlaying = false;
		this.followTarget = null;
		this.onActionTicked = null;
		this.onFinished = null;
		if (flag)
		{
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x0600223F RID: 8767 RVA: 0x000A0E4A File Offset: 0x0009F04A
	private void LateUpdate()
	{
		this.SyncFollowTransform();
	}

	// Token: 0x06002240 RID: 8768 RVA: 0x000A0E52 File Offset: 0x0009F052
	private void SyncFollowTransform()
	{
		if (this.followTarget == null)
		{
			return;
		}
		base.transform.SetPositionAndRotation(this.followTarget.position, this.followTarget.rotation);
	}

	// Token: 0x06002241 RID: 8769 RVA: 0x000A0E84 File Offset: 0x0009F084
	private void Update()
	{
		if (this.isPlaying)
		{
			this.playingTime += Time.deltaTime;
			if (!this.actionWasExecuted && this.playingTime > this.actionTime)
			{
				Action action = this.onActionTicked;
				this.onActionTicked = null;
				this.actionWasExecuted = true;
				if (action != null)
				{
					action();
				}
			}
			if (this.playingTime > this.Duration)
			{
				this.Stop();
				Action action2 = this.onFinished;
				this.onFinished = null;
				if (this.returnToPoolAfterEnd)
				{
					WorldFXPool.Release(base.name, this);
				}
				if (action2 == null)
				{
					return;
				}
				action2();
			}
		}
	}

	// Token: 0x04001EC7 RID: 7879
	private Action onActionTicked;

	// Token: 0x04001EC8 RID: 7880
	private Action onFinished;

	// Token: 0x04001EC9 RID: 7881
	private ParticleSystem mainParticleSystem;

	// Token: 0x04001ECA RID: 7882
	private Transform followTarget;

	// Token: 0x04001ECB RID: 7883
	private bool isPlaying;

	// Token: 0x04001ECC RID: 7884
	private float playingTime;

	// Token: 0x04001ECD RID: 7885
	private bool actionWasExecuted;

	// Token: 0x04001ECE RID: 7886
	private Animator animator;

	// Token: 0x04001ECF RID: 7887
	[NonSerialized]
	public bool returnToPoolAfterEnd;

	// Token: 0x04001ED0 RID: 7888
	[SerializeField]
	private float actionTime = 1f;

	// Token: 0x04001ED1 RID: 7889
	[Space]
	[SerializeField]
	private bool useAnimator;

	// Token: 0x04001ED2 RID: 7890
	[SerializeField]
	[HideInInspector]
	private float customDuration;
}
