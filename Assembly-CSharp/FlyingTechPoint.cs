using System;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using LazyBearTechnology;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;

// Token: 0x0200019B RID: 411
public class FlyingTechPoint : MonoBehaviour
{
	// Token: 0x170001A4 RID: 420
	// (get) Token: 0x06000A70 RID: 2672 RVA: 0x00034A99 File Offset: 0x00032C99
	public static Dictionary<string, int> FlyingTechPointsCountByName
	{
		get
		{
			return FlyingTechPoint.flyingTechPointsCountByName;
		}
	}

	// Token: 0x06000A71 RID: 2673 RVA: 0x00034AA0 File Offset: 0x00032CA0
	public static FlyingTechPoint DropFromUI(Vector3 uiWorldPos, string techPointName, Action onReachedDestination = null, int overrodeSorting = -1)
	{
		return FlyingTechPoint.Drop(uiWorldPos, techPointName, onReachedDestination, overrodeSorting, true);
	}

	// Token: 0x06000A72 RID: 2674 RVA: 0x00034AAC File Offset: 0x00032CAC
	public static FlyingTechPoint Drop(Vector3 pos, string techPointName, Action onReachedDestination = null, int overrodeSorting = -1)
	{
		return FlyingTechPoint.Drop(pos, techPointName, onReachedDestination, overrodeSorting, false);
	}

	// Token: 0x06000A73 RID: 2675 RVA: 0x00034AB8 File Offset: 0x00032CB8
	private static FlyingTechPoint Drop(Vector3 pos, string techPointName, Action onReachedDestination, int overrodeSorting, bool posIsUiWorldPosition)
	{
		FlyingTechPoint.<>c__DisplayClass16_0 CS$<>8__locals1 = new FlyingTechPoint.<>c__DisplayClass16_0();
		CS$<>8__locals1.techPointName = techPointName;
		CS$<>8__locals1.posIsUiWorldPosition = posIsUiWorldPosition;
		CS$<>8__locals1.pos = pos;
		CS$<>8__locals1.overrodeSorting = overrodeSorting;
		if (FlyingTechPoint.pool == null)
		{
			FlyingTechPoint.pool = LazyPooler.CreatePool<FlyingTechPoint>(Addressables.LoadAssetAsync<GameObject>("Assets/AddressableAssets/Drops/FlyingTechPoint.prefab").WaitForCompletion().GetComponent<FlyingTechPoint>(), 0, Pool.PoolType.ImmediateActivation, true, false, null);
		}
		CS$<>8__locals1.hud = LazyUI.Get<HUD>();
		CS$<>8__locals1.drop = FlyingTechPoint.pool.GetOrCreateObject<FlyingTechPoint>();
		CS$<>8__locals1.drop.ResetFlightState();
		CS$<>8__locals1.drop.onReachedDestination = onReachedDestination;
		CS$<>8__locals1.drop.techPointName = CS$<>8__locals1.techPointName;
		CS$<>8__locals1.drop.useUnscaledTime = CS$<>8__locals1.posIsUiWorldPosition;
		CS$<>8__locals1.drop.inFlight = true;
		if (!string.IsNullOrEmpty(CS$<>8__locals1.techPointName) && !FlyingTechPoint.flyingTechPointsCountByName.TryAdd(CS$<>8__locals1.techPointName, 1))
		{
			Dictionary<string, int> dictionary = FlyingTechPoint.flyingTechPointsCountByName;
			string text = CS$<>8__locals1.techPointName;
			int num = dictionary[text];
			dictionary[text] = num + 1;
		}
		if (CS$<>8__locals1.techPointName == "happiness" || CS$<>8__locals1.hud.TryTurnOnTechPointsPanel())
		{
			CS$<>8__locals1.<Drop>g__Show|0();
		}
		else
		{
			CS$<>8__locals1.drop.pendingShow = new Action(CS$<>8__locals1.<Drop>g__Show|0);
			CS$<>8__locals1.hud.onTechPointsPanelShown += CS$<>8__locals1.<Drop>g__Show|0;
		}
		return CS$<>8__locals1.drop;
	}

	// Token: 0x06000A74 RID: 2676 RVA: 0x00034C10 File Offset: 0x00032E10
	public void CompleteImmediately()
	{
		if (this.rewardApplied)
		{
			return;
		}
		this.CancelPendingShow();
		this.ApplyRewardAndFinish(true);
	}

	// Token: 0x06000A75 RID: 2677 RVA: 0x00034C28 File Offset: 0x00032E28
	public static void Clear()
	{
		FlyingTechPoint.flyingTechPointsCountByName.Clear();
	}

	// Token: 0x06000A76 RID: 2678 RVA: 0x00034C34 File Offset: 0x00032E34
	private void StartFly(Vector2 to)
	{
		this.EnsureTweenStopped();
		this.moveTween = base.transform.DOMove(to, 1f + this.delay, false).OnComplete(new TweenCallback(this.OnReachedDestination)).SetEase(Ease.OutCubic)
			.SetUpdate(this.useUnscaledTime);
	}

	// Token: 0x06000A77 RID: 2679 RVA: 0x00034C8E File Offset: 0x00032E8E
	private void OnReachedDestination()
	{
		this.ApplyRewardAndFinish(true);
	}

	// Token: 0x06000A78 RID: 2680 RVA: 0x00034C97 File Offset: 0x00032E97
	private void OnDisable()
	{
		if (!this.inFlight || this.rewardApplied)
		{
			return;
		}
		this.ApplyRewardAndFinish(false);
		this.ReleaseToPoolNextFrame();
	}

	// Token: 0x06000A79 RID: 2681 RVA: 0x00034CB8 File Offset: 0x00032EB8
	private void ApplyRewardAndFinish(bool releaseToPool)
	{
		if (this.rewardApplied)
		{
			return;
		}
		this.rewardApplied = true;
		this.inFlight = false;
		this.EnsureTweenStopped();
		if (!string.IsNullOrEmpty(this.techPointName))
		{
			if (FlyingTechPoint.flyingTechPointsCountByName.ContainsKey(this.techPointName))
			{
				Dictionary<string, int> dictionary = FlyingTechPoint.flyingTechPointsCountByName;
				string text = this.techPointName;
				int num = dictionary[text];
				dictionary[text] = num - 1;
			}
			GK2GameResSystem.GetSystem(this.techPointName).Add(1f, true);
			LazyUI.Get<HUD>().UpdateTechPointsInstant();
		}
		Action action = this.onReachedDestination;
		this.onReachedDestination = null;
		if (action != null)
		{
			action();
		}
		if (releaseToPool)
		{
			this.ReleaseToPoolIfNeeded();
		}
	}

	// Token: 0x06000A7A RID: 2682 RVA: 0x00034D5F File Offset: 0x00032F5F
	private void ReleaseToPoolIfNeeded()
	{
		if (this.releasedToPool || FlyingTechPoint.pool == null)
		{
			return;
		}
		this.releasedToPool = true;
		FlyingTechPoint.pool.ReleaseObject<FlyingTechPoint>(this);
	}

	// Token: 0x06000A7B RID: 2683 RVA: 0x00034D84 File Offset: 0x00032F84
	private async void ReleaseToPoolNextFrame()
	{
		await Awaitable.NextFrameAsync(base.destroyCancellationToken);
		if (!(this == null))
		{
			this.ReleaseToPoolIfNeeded();
		}
	}

	// Token: 0x06000A7C RID: 2684 RVA: 0x00034DBB File Offset: 0x00032FBB
	private void ResetFlightState()
	{
		this.CancelPendingShow();
		this.EnsureTweenStopped();
		this.inFlight = false;
		this.rewardApplied = false;
		this.releasedToPool = false;
		this.useUnscaledTime = false;
		this.onReachedDestination = null;
		this.techPointName = null;
		this.pendingShow = null;
	}

	// Token: 0x06000A7D RID: 2685 RVA: 0x00034DFC File Offset: 0x00032FFC
	private void CancelPendingShow()
	{
		if (this.pendingShow == null)
		{
			return;
		}
		HUD hud = LazyUI.Get<HUD>();
		if (hud != null)
		{
			hud.onTechPointsPanelShown -= this.pendingShow;
		}
		this.pendingShow = null;
	}

	// Token: 0x06000A7E RID: 2686 RVA: 0x00034E34 File Offset: 0x00033034
	private void EnsureTweenStopped()
	{
		Tweener tweener = this.moveTween;
		this.moveTween = null;
		if (tweener != null && tweener.IsActive())
		{
			tweener.Kill(false);
		}
	}

	// Token: 0x04000BD9 RID: 3033
	private static Pool pool;

	// Token: 0x04000BDA RID: 3034
	private static Dictionary<string, int> flyingTechPointsCountByName = new Dictionary<string, int>();

	// Token: 0x04000BDB RID: 3035
	[SerializeField]
	private TextMeshProUGUI label;

	// Token: 0x04000BDC RID: 3036
	private string techPointName;

	// Token: 0x04000BDD RID: 3037
	private Tweener moveTween;

	// Token: 0x04000BDE RID: 3038
	private float delay;

	// Token: 0x04000BDF RID: 3039
	private Action onReachedDestination;

	// Token: 0x04000BE0 RID: 3040
	private bool useUnscaledTime;

	// Token: 0x04000BE1 RID: 3041
	private bool inFlight;

	// Token: 0x04000BE2 RID: 3042
	private bool rewardApplied;

	// Token: 0x04000BE3 RID: 3043
	private bool releasedToPool;

	// Token: 0x04000BE4 RID: 3044
	private Action pendingShow;
}
