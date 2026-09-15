using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x0200043E RID: 1086
public class RiverDropSystem : ICustomUpdatable
{
	// Token: 0x170004F1 RID: 1265
	// (get) Token: 0x06001CA6 RID: 7334 RVA: 0x00085D68 File Offset: 0x00083F68
	private RiverDropSystemData Data
	{
		get
		{
			MainGame instance = MainGame.Instance;
			GameSave gameSave = ((instance != null) ? instance.GameSave : null);
			if (gameSave == null)
			{
				return null;
			}
			GameSave gameSave2 = gameSave;
			if (gameSave2.riverDropSystemData == null)
			{
				gameSave2.riverDropSystemData = new RiverDropSystemData();
			}
			RiverDropSystemData riverDropSystemData = gameSave.riverDropSystemData;
			if (riverDropSystemData.floatingDrops == null)
			{
				riverDropSystemData.floatingDrops = new List<RiverFloatingDropData>();
			}
			return gameSave.riverDropSystemData;
		}
	}

	// Token: 0x06001CA7 RID: 7335 RVA: 0x00085DC4 File Offset: 0x00083FC4
	public void ClearRuntimeState()
	{
		foreach (DropView dropView in this.views.Values)
		{
			if (dropView != null)
			{
				dropView.DespawnView();
			}
		}
		this.views.Clear();
		this.throws.Clear();
	}

	// Token: 0x06001CA8 RID: 7336 RVA: 0x00085E3C File Offset: 0x0008403C
	public bool BeginDump(Item item, RiverBodyReceiver receiver, Vector3 throwStartPos)
	{
		this.EnsureSubscribed();
		if (item == null || receiver == null || !receiver.HasFlowSpline)
		{
			Debug.LogError("[RiverDropSystem] Cannot dump body: missing item or flow spline.");
			return false;
		}
		Vector3 vector;
		float num;
		if (!receiver.GetNearestFlowPoint(throwStartPos, out vector, out num))
		{
			Debug.LogError("[RiverDropSystem] Failed to find nearest flow point.");
			return false;
		}
		Wgo wgo = receiver.Wgo;
		RiverFloatingDropData riverFloatingDropData = new RiverFloatingDropData
		{
			item = item,
			wgoUniqueId = ((wgo != null) ? wgo.Data.UniqueId : SGuid.Empty),
			worldId = ((wgo != null) ? wgo.Data.WorldId : MainGame.PlayerData.currentGameSceneId),
			splineIndex = 0,
			t = num,
			splineWorldLengths = receiver.CopySplineWorldLengths(),
			flowSpeed = receiver.FlowSpeed,
			fallSpeed = receiver.FallSpeed
		};
		this.Data.floatingDrops.Add(riverFloatingDropData);
		this.throws[riverFloatingDropData.UniqueId] = new RiverDropSystem.ThrowSession
		{
			startPos = throwStartPos,
			endPos = vector,
			elapsed = 0f,
			duration = Mathf.Max(0.01f, receiver.ThrowDuration),
			heightCurve = receiver.ThrowHeightCurve
		};
		this.SpawnView(riverFloatingDropData, throwStartPos, false);
		return true;
	}

	// Token: 0x06001CA9 RID: 7337 RVA: 0x00085F84 File Offset: 0x00084184
	public void HandleReceiverReady(RiverBodyReceiver receiver)
	{
		this.EnsureSubscribed();
		if (receiver == null || receiver.Wgo == null || this.Data == null)
		{
			return;
		}
		SGuid uniqueId = receiver.Wgo.Data.UniqueId;
		for (int i = 0; i < this.Data.floatingDrops.Count; i++)
		{
			RiverFloatingDropData riverFloatingDropData = this.Data.floatingDrops[i];
			if (!(riverFloatingDropData.wgoUniqueId != uniqueId) && (!this.views.ContainsKey(riverFloatingDropData.UniqueId) || !(this.views[riverFloatingDropData.UniqueId] != null)))
			{
				this.TrySpawnViewOnSpline(riverFloatingDropData, receiver);
			}
		}
	}

	// Token: 0x06001CAA RID: 7338 RVA: 0x00086038 File Offset: 0x00084238
	public void HandleSceneReady(GameScene gameScene)
	{
		this.EnsureSubscribed();
		if (gameScene == null || this.Data == null)
		{
			return;
		}
		for (int i = 0; i < this.Data.floatingDrops.Count; i++)
		{
			RiverFloatingDropData riverFloatingDropData = this.Data.floatingDrops[i];
			if (!(riverFloatingDropData.worldId != gameScene.Id) && (!this.views.ContainsKey(riverFloatingDropData.UniqueId) || !(this.views[riverFloatingDropData.UniqueId] != null)))
			{
				Wgo wgoViewGlobal = GameScene.GetWgoViewGlobal(riverFloatingDropData.wgoUniqueId);
				RiverBodyReceiver riverBodyReceiver = ((wgoViewGlobal != null) ? wgoViewGlobal.RiverBodyReceiver : null);
				if (!(riverBodyReceiver == null))
				{
					this.TrySpawnViewOnSpline(riverFloatingDropData, riverBodyReceiver);
				}
			}
		}
	}

	// Token: 0x06001CAB RID: 7339 RVA: 0x00086100 File Offset: 0x00084300
	public void CustomUpdate(float deltaTime)
	{
		this.EnsureSubscribed();
		RiverDropSystemData data = this.Data;
		if (((data != null) ? data.floatingDrops : null) == null || this.Data.floatingDrops.Count == 0)
		{
			return;
		}
		if (MainGame.IsGamePaused)
		{
			return;
		}
		for (int i = this.Data.floatingDrops.Count - 1; i >= 0; i--)
		{
			RiverFloatingDropData riverFloatingDropData = this.Data.floatingDrops[i];
			if (((riverFloatingDropData != null) ? riverFloatingDropData.item : null) == null)
			{
				this.RemoveAt(i);
			}
			else
			{
				SGuid uniqueId = riverFloatingDropData.UniqueId;
				RiverDropSystem.ThrowSession throwSession;
				if (this.throws.TryGetValue(uniqueId, out throwSession))
				{
					this.UpdateThrow(riverFloatingDropData, throwSession, deltaTime);
				}
				else
				{
					riverFloatingDropData.t += riverFloatingDropData.GetCurrentSpeed() * deltaTime / riverFloatingDropData.GetCurrentSplineLength();
					if (riverFloatingDropData.t >= 1f)
					{
						if (!riverFloatingDropData.HasMoreSplinesAfterCurrent())
						{
							this.RemoveAt(i);
							goto IL_00FF;
						}
						riverFloatingDropData.splineIndex++;
						riverFloatingDropData.t = 0f;
						this.SetViewBob(uniqueId, false);
					}
					this.ApplySplinePose(riverFloatingDropData);
				}
			}
			IL_00FF:;
		}
	}

	// Token: 0x06001CAC RID: 7340 RVA: 0x00086218 File Offset: 0x00084418
	private void UpdateThrow(RiverFloatingDropData dropData, RiverDropSystem.ThrowSession throwSession, float deltaTime)
	{
		throwSession.elapsed += deltaTime;
		float num = Mathf.Clamp01(throwSession.elapsed / throwSession.duration);
		Vector3 vector = Vector3.Lerp(throwSession.startPos, throwSession.endPos, num);
		float num2 = 0f;
		if (throwSession.heightCurve != null && throwSession.heightCurve.length > 0)
		{
			num2 = throwSession.heightCurve.Evaluate(num);
		}
		vector.y += num2;
		DropView dropView;
		if (this.views.TryGetValue(dropData.UniqueId, out dropView) && dropView != null)
		{
			dropView.SetRiverWorldPosition(vector);
		}
		if (num < 1f)
		{
			return;
		}
		this.throws.Remove(dropData.UniqueId);
		this.SetViewBob(dropData.UniqueId, dropData.splineIndex == 0);
		this.ApplySplinePose(dropData);
		if (this.views.TryGetValue(dropData.UniqueId, out dropView) && dropView != null)
		{
			LazyAudio.PlayAtGameObject("fishing_blop", dropView.transform, SpatialType.sound3D, true);
		}
	}

	// Token: 0x06001CAD RID: 7341 RVA: 0x0008631C File Offset: 0x0008451C
	private void ApplySplinePose(RiverFloatingDropData dropData)
	{
		DropView dropView;
		if (!this.views.TryGetValue(dropData.UniqueId, out dropView) || dropView == null)
		{
			return;
		}
		Wgo wgoViewGlobal = GameScene.GetWgoViewGlobal(dropData.wgoUniqueId);
		RiverBodyReceiver riverBodyReceiver = ((wgoViewGlobal != null) ? wgoViewGlobal.RiverBodyReceiver : null);
		Vector3 vector;
		if (riverBodyReceiver == null || !riverBodyReceiver.TryEvaluatePose(dropData.splineIndex, dropData.t, out vector))
		{
			return;
		}
		dropView.SetRiverWorldPosition(vector);
	}

	// Token: 0x06001CAE RID: 7342 RVA: 0x00086390 File Offset: 0x00084590
	private void TrySpawnViewOnSpline(RiverFloatingDropData dropData, RiverBodyReceiver receiver)
	{
		Vector3 vector = receiver.transform.position;
		Vector3 vector2;
		if (receiver.TryEvaluatePose(dropData.splineIndex, dropData.t, out vector2))
		{
			vector = vector2;
		}
		this.SpawnView(dropData, vector, dropData.splineIndex == 0);
	}

	// Token: 0x06001CAF RID: 7343 RVA: 0x000863D4 File Offset: 0x000845D4
	private void SpawnView(RiverFloatingDropData dropData, Vector3 worldPos, bool enableBob)
	{
		if (((dropData != null) ? dropData.item : null) == null)
		{
			return;
		}
		PlayerController playerController = MainGame.PlayerController;
		Transform transform;
		if (playerController == null)
		{
			transform = null;
		}
		else
		{
			GameScene currentGameScene = playerController.CurrentGameScene;
			transform = ((currentGameScene != null) ? currentGameScene.transform : null);
		}
		Transform transform2 = transform;
		if (transform2 == null)
		{
			return;
		}
		if (MainGame.PlayerController.CurrentGameScene.Id != dropData.worldId)
		{
			return;
		}
		DropView dropView = DropView.SpawnDrop(new DropData(dropData.item, worldPos, dropData.worldId), transform2, true);
		if (dropView == null)
		{
			return;
		}
		dropView.PrepareAsRiverDump();
		dropView.SetRiverWorldPosition(worldPos);
		dropView.SetWaterBobEnabled(enableBob);
		this.views[dropData.UniqueId] = dropView;
	}

	// Token: 0x06001CB0 RID: 7344 RVA: 0x00086480 File Offset: 0x00084680
	private void SetViewBob(SGuid id, bool enabled)
	{
		DropView dropView;
		if (this.views.TryGetValue(id, out dropView) && dropView != null)
		{
			dropView.SetWaterBobEnabled(enabled);
		}
	}

	// Token: 0x06001CB1 RID: 7345 RVA: 0x000864B0 File Offset: 0x000846B0
	private void RemoveAt(int index)
	{
		RiverFloatingDropData riverFloatingDropData = this.Data.floatingDrops[index];
		SGuid sguid = ((riverFloatingDropData != null) ? riverFloatingDropData.UniqueId : SGuid.Empty);
		if (sguid != SGuid.Empty)
		{
			this.throws.Remove(sguid);
			DropView dropView;
			if (this.views.TryGetValue(sguid, out dropView))
			{
				this.views.Remove(sguid);
				if (dropView != null)
				{
					dropView.DespawnView();
				}
			}
		}
		this.Data.floatingDrops.RemoveAt(index);
	}

	// Token: 0x06001CB2 RID: 7346 RVA: 0x00086538 File Offset: 0x00084738
	private void HandleWgoSpawn(Wgo wgo)
	{
		if (wgo == null)
		{
			return;
		}
		RiverBodyReceiver riverBodyReceiver = wgo.RiverBodyReceiver;
		if (riverBodyReceiver == null)
		{
			riverBodyReceiver = wgo.GetComponentInChildren<RiverBodyReceiver>(true);
		}
		if (riverBodyReceiver != null)
		{
			this.HandleReceiverReady(riverBodyReceiver);
		}
	}

	// Token: 0x06001CB3 RID: 7347 RVA: 0x00086577 File Offset: 0x00084777
	private void EnsureSubscribed()
	{
		if (this.subscribedToWgoSpawn)
		{
			return;
		}
		this.subscribedToWgoSpawn = true;
		Wgo.OnWgoSpawn += this.HandleWgoSpawn;
	}

	// Token: 0x04001ABD RID: 6845
	private readonly Dictionary<SGuid, DropView> views = new Dictionary<SGuid, DropView>();

	// Token: 0x04001ABE RID: 6846
	private readonly Dictionary<SGuid, RiverDropSystem.ThrowSession> throws = new Dictionary<SGuid, RiverDropSystem.ThrowSession>();

	// Token: 0x04001ABF RID: 6847
	private bool subscribedToWgoSpawn;

	// Token: 0x0200043F RID: 1087
	private class ThrowSession
	{
		// Token: 0x04001AC0 RID: 6848
		public Vector3 startPos;

		// Token: 0x04001AC1 RID: 6849
		public Vector3 endPos;

		// Token: 0x04001AC2 RID: 6850
		public float elapsed;

		// Token: 0x04001AC3 RID: 6851
		public float duration;

		// Token: 0x04001AC4 RID: 6852
		public AnimationCurve heightCurve;
	}
}
