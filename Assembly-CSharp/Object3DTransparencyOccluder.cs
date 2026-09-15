using System;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

// Token: 0x02000520 RID: 1312
public class Object3DTransparencyOccluder
{
	// Token: 0x17000588 RID: 1416
	// (get) Token: 0x060021E2 RID: 8674 RVA: 0x0009F507 File Offset: 0x0009D707
	public static Object3DTransparencyOccluder Shared { get; } = new Object3DTransparencyOccluder();

	// Token: 0x060021E3 RID: 8675 RVA: 0x0009F50E File Offset: 0x0009D70E
	public void BeginFrame()
	{
		this.frameOccluders.Clear();
	}

	// Token: 0x060021E4 RID: 8676 RVA: 0x0009F51C File Offset: 0x0009D71C
	public void AddOccludersFromCapsule(Vector3 p1, Vector3 p2, float capsuleRadius, float distance)
	{
		NativeArray<RaycastHit> nativeArray = new NativeArray<RaycastHit>(10, Allocator.TempJob, NativeArrayOptions.ClearMemory);
		NativeArray<CapsulecastCommand> nativeArray2 = new NativeArray<CapsulecastCommand>(1, Allocator.TempJob, NativeArrayOptions.ClearMemory);
		QueryParameters queryParameters = new QueryParameters(32768, true, QueryTriggerInteraction.Ignore, true);
		Vector3 vector = Quaternion.Euler(53.130104f, 0f, 0f) * Vector3.back;
		CapsulecastCommand capsulecastCommand = new CapsulecastCommand(p1, p2, capsuleRadius, vector, queryParameters, distance);
		nativeArray2[0] = capsulecastCommand;
		CapsulecastCommand.ScheduleBatch(nativeArray2, nativeArray, 10, 10, default(JobHandle)).Complete();
		for (int i = 0; i < 10; i++)
		{
			RaycastHit raycastHit = nativeArray[i];
			if (!(raycastHit.collider == null))
			{
				Object3D componentInParent = raycastHit.collider.gameObject.GetComponentInParent<Object3D>();
				if (componentInParent != null)
				{
					this.frameOccluders.Add(componentInParent);
				}
			}
		}
		nativeArray.Dispose();
		nativeArray2.Dispose();
	}

	// Token: 0x060021E5 RID: 8677 RVA: 0x0009F608 File Offset: 0x0009D808
	public void EndFrame()
	{
		using (HashSet<Object3D>.Enumerator enumerator = this.frameOccluders.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				Object3D obj = enumerator.Current;
				if (!(obj == null))
				{
					Object3DTransparencyOccluder.TweenContainer tweenContainer2;
					if (!this.playingOccluders.Contains(obj))
					{
						Object3DTransparencyOccluder.TweenContainer tweenContainer = new Object3DTransparencyOccluder.TweenContainer();
						obj.TransparencyValue = 0f;
						Tween tween = DOTween.To(() => obj.TransparencyValue, delegate(float v)
						{
							obj.TransparencyValue = v;
						}, 1f, 0.25f).SetEase(Ease.OutExpo).SetAutoKill(false)
							.OnRewind(delegate
							{
								this.playingTweens.Remove(obj);
								this.playingOccluders.Remove(obj);
							});
						tweenContainer.tween = tween;
						this.playingTweens[obj] = tweenContainer;
						this.playingOccluders.Add(obj);
					}
					else if (this.playingTweens.TryGetValue(obj, out tweenContainer2) && tweenContainer2.isPlayingBackwards)
					{
						tweenContainer2.tween.PlayForward();
						tweenContainer2.isPlayingBackwards = false;
					}
				}
			}
		}
		foreach (Object3D object3D in this.playingOccluders)
		{
			Object3DTransparencyOccluder.TweenContainer tweenContainer3;
			if (!(object3D == null) && !this.frameOccluders.Contains(object3D) && this.playingTweens.TryGetValue(object3D, out tweenContainer3))
			{
				tweenContainer3.tween.PlayBackwards();
				tweenContainer3.isPlayingBackwards = true;
			}
		}
	}

	// Token: 0x060021E6 RID: 8678 RVA: 0x0009F7E0 File Offset: 0x0009D9E0
	public void Clear()
	{
		foreach (KeyValuePair<Object3D, Object3DTransparencyOccluder.TweenContainer> keyValuePair in this.playingTweens)
		{
			Object3DTransparencyOccluder.TweenContainer value = keyValuePair.Value;
			if (((value != null) ? value.tween : null) != null && keyValuePair.Value.tween.IsActive())
			{
				keyValuePair.Value.tween.Kill(false);
			}
		}
		foreach (Object3D object3D in this.playingOccluders)
		{
			if (object3D != null)
			{
				object3D.TransparencyValue = 0f;
			}
		}
		this.playingTweens.Clear();
		this.playingOccluders.Clear();
		this.frameOccluders.Clear();
	}

	// Token: 0x04001E89 RID: 7817
	private const float TRANSPARENCY_TWEEN_DURATION = 0.25f;

	// Token: 0x04001E8A RID: 7818
	private const int MAX_OCCLUDER_HITS = 10;

	// Token: 0x04001E8B RID: 7819
	private readonly HashSet<Object3D> frameOccluders = new HashSet<Object3D>(10);

	// Token: 0x04001E8C RID: 7820
	private readonly HashSet<Object3D> playingOccluders = new HashSet<Object3D>(10);

	// Token: 0x04001E8D RID: 7821
	private readonly Dictionary<Object3D, Object3DTransparencyOccluder.TweenContainer> playingTweens = new Dictionary<Object3D, Object3DTransparencyOccluder.TweenContainer>(10);

	// Token: 0x02000521 RID: 1313
	private class TweenContainer
	{
		// Token: 0x04001E8E RID: 7822
		public Tween tween;

		// Token: 0x04001E8F RID: 7823
		public bool isPlayingBackwards;
	}
}
