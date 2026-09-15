using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using LazyBearTechnology;
using Pathfinding;
using UnityEngine;

// Token: 0x0200030F RID: 783
public class FightingCapturePoint : MonoBehaviour
{
	// Token: 0x14000022 RID: 34
	// (add) Token: 0x060014CE RID: 5326 RVA: 0x00065B58 File Offset: 0x00063D58
	// (remove) Token: 0x060014CF RID: 5327 RVA: 0x00065B90 File Offset: 0x00063D90
	public event Action<FightingCapturePoint> OnCapturedByTeam;

	// Token: 0x1700038E RID: 910
	// (get) Token: 0x060014D0 RID: 5328 RVA: 0x00065BC5 File Offset: 0x00063DC5
	// (set) Token: 0x060014D1 RID: 5329 RVA: 0x00065BCD File Offset: 0x00063DCD
	public bool LockedForCapture
	{
		get
		{
			return this.lockedForCapture;
		}
		set
		{
			if (this.lockedForCapture == value)
			{
				return;
			}
			this.lockedForCapture = value;
			this.SetVfxActive(FightingCapturePoint.CaptureProgress.None, true);
		}
	}

	// Token: 0x1700038F RID: 911
	// (get) Token: 0x060014D2 RID: 5330 RVA: 0x00065BE8 File Offset: 0x00063DE8
	public float CurrentProgress
	{
		get
		{
			return this.currentProgress;
		}
	}

	// Token: 0x17000390 RID: 912
	// (get) Token: 0x060014D3 RID: 5331 RVA: 0x00065BF0 File Offset: 0x00063DF0
	public LazyConsts.Fighting.TeamType OwnedByTeamDefault
	{
		get
		{
			return this.ownedByTeam;
		}
	}

	// Token: 0x17000391 RID: 913
	// (get) Token: 0x060014D4 RID: 5332 RVA: 0x00065BF8 File Offset: 0x00063DF8
	public LazyConsts.Fighting.TeamType OwnedByTeam
	{
		get
		{
			return this.ownedByTeamInGame;
		}
	}

	// Token: 0x17000392 RID: 914
	// (get) Token: 0x060014D5 RID: 5333 RVA: 0x00065C00 File Offset: 0x00063E00
	public bool IsCapturedByAllies
	{
		get
		{
			return this.isCapturedByAllies;
		}
	}

	// Token: 0x17000393 RID: 915
	// (get) Token: 0x060014D6 RID: 5334 RVA: 0x00065C08 File Offset: 0x00063E08
	public float Radius
	{
		get
		{
			return this.radius;
		}
	}

	// Token: 0x17000394 RID: 916
	// (get) Token: 0x060014D7 RID: 5335 RVA: 0x00065C10 File Offset: 0x00063E10
	public FightingSector Sector
	{
		get
		{
			return this.sector;
		}
	}

	// Token: 0x060014D8 RID: 5336 RVA: 0x00065C18 File Offset: 0x00063E18
	public bool IsOnCapturePoint(Vector3 worldPosition)
	{
		return this.OverlapsCaptureVolume(worldPosition);
	}

	// Token: 0x060014D9 RID: 5337 RVA: 0x00065C24 File Offset: 0x00063E24
	public bool MatchesAllyFlagStandPosition(Vector3 worldPosition)
	{
		if (this.isBasePoint)
		{
			return false;
		}
		Vector3 vector;
		if (this.TryGetAllyFlagStandPosition(out vector))
		{
			float num = 0.25f;
			if ((worldPosition - vector).XZ().sqrMagnitude <= num)
			{
				return true;
			}
		}
		return this.ContainsPosition(worldPosition);
	}

	// Token: 0x060014DA RID: 5338 RVA: 0x00065C6C File Offset: 0x00063E6C
	public bool IsLinkedFlagStand(FlagStandComponent flagStand)
	{
		if (!flagStand || this.isBasePoint)
		{
			return false;
		}
		if (this.allyFlagStand)
		{
			return this.allyFlagStand == flagStand;
		}
		Vector3 vector = (flagStand.StandWgo ? flagStand.StandWgo.Data.Position : flagStand.transform.position);
		return this.ContainsPosition(vector);
	}

	// Token: 0x060014DB RID: 5339 RVA: 0x00065CD7 File Offset: 0x00063ED7
	public void BindAllyFlagController(AgentsGroupFlagController controller)
	{
		if (!controller)
		{
			return;
		}
		if (this.isBasePoint)
		{
			controller.ClearCapturePointBinding();
			return;
		}
		controller.BindCapturePoint(this);
	}

	// Token: 0x060014DC RID: 5340 RVA: 0x00065CF8 File Offset: 0x00063EF8
	public void TryBindFlagStand(FlagStandComponent flagStand)
	{
		if (!this.IsLinkedFlagStand(flagStand))
		{
			return;
		}
		flagStand.TryBindFlagToCapturePoint(this);
	}

	// Token: 0x060014DD RID: 5341 RVA: 0x00065D0C File Offset: 0x00063F0C
	public bool ContainsPosition(Vector3 worldPosition)
	{
		float num = this.radius;
		if (num <= 0f && this.col)
		{
			num = Mathf.Max(this.col.bounds.extents.x, this.col.bounds.extents.z);
		}
		return num > 0f && (worldPosition - base.transform.position).XZ().magnitude < num;
	}

	// Token: 0x060014DE RID: 5342 RVA: 0x00065D98 File Offset: 0x00063F98
	public void SetActiveState(bool isActive)
	{
		base.gameObject.SetActive(isActive);
		this.SetProgress(this.hasStartValue ? this.startValue : 1f);
		this.SetOwnedByTeam(this.ownedByTeam);
		if (isActive)
		{
			base.StartCoroutine(this.CalculateRadiusNextFixedUpdate());
		}
	}

	// Token: 0x060014DF RID: 5343 RVA: 0x00065DE8 File Offset: 0x00063FE8
	private IEnumerator CalculateRadiusNextFixedUpdate()
	{
		yield return new WaitForFixedUpdate();
		this.radius = Mathf.Max(this.col.bounds.extents.x, this.col.bounds.extents.z);
		Debug.Log(string.Format("CapturePoint {0} radius: {1}", base.name, this.radius), this);
		float num = this.radius / 2f;
		using (List<FightingCapturePoint.TeamsFlag>.Enumerator enumerator = this.teamFlags.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				FightingCapturePoint.TeamsFlag teamsFlag = enumerator.Current;
				teamsFlag.ringVfxBase.transform.localScale = new Vector3(num, 1f, num);
				teamsFlag.ringVfxActive.transform.localScale = new Vector3(num, 1f, num);
			}
			yield break;
		}
		yield break;
	}

	// Token: 0x060014E0 RID: 5344 RVA: 0x00065DF7 File Offset: 0x00063FF7
	public void Init(FightingSector sector = null)
	{
		base.enabled = true;
		this.isActivated = true;
		this.sector = sector;
		this.isCapturedByAllies = false;
		this.SetVfxActive(FightingCapturePoint.CaptureProgress.None, true);
		this.TryBindLinkedFlagStand();
	}

	// Token: 0x060014E1 RID: 5345 RVA: 0x00065E23 File Offset: 0x00064023
	private void TryBindLinkedFlagStand()
	{
		this.TryBindFlagStand(this.allyFlagStand);
	}

	// Token: 0x060014E2 RID: 5346 RVA: 0x00065E34 File Offset: 0x00064034
	private bool TryGetAllyFlagStandPosition(out Vector3 position)
	{
		if (!this.allyFlagStand)
		{
			position = default(Vector3);
			return false;
		}
		Wgo standWgo = this.allyFlagStand.StandWgo;
		position = (standWgo ? standWgo.Data.Position : this.allyFlagStand.transform.position);
		return true;
	}

	// Token: 0x060014E3 RID: 5347 RVA: 0x00065E8F File Offset: 0x0006408F
	public void DeInit()
	{
		base.enabled = false;
		this.isActivated = false;
		this.allies.Clear();
		this.enemies.Clear();
		this.reservedStandardSlots.Clear();
		this.reservedWaitingSlots.Clear();
	}

	// Token: 0x060014E4 RID: 5348 RVA: 0x00065ECC File Offset: 0x000640CC
	private void Awake()
	{
		if (!this.col)
		{
			this.col = base.GetComponent<Collider>();
		}
		this.SetActiveState(false);
		base.enabled = false;
		LazySingleton<FightingGameController>.Instance.TargetsDatabase.OnTargetRemoved += this.HandleTargetRemoved;
		this.radius = Mathf.Max(this.col.bounds.extents.x, this.col.bounds.extents.z);
		this.SetProgress(this.hasStartValue ? this.startValue : 1f);
		this.SetVfxActive(FightingCapturePoint.CaptureProgress.None, true);
	}

	// Token: 0x060014E5 RID: 5349 RVA: 0x00065F7C File Offset: 0x0006417C
	private void Update()
	{
		if (MainGame.IsGamePaused)
		{
			return;
		}
		this.CustomUpdate();
		FightingCapturePoint.CaptureProgress captureProgress = this.captureProgress;
		if (captureProgress == FightingCapturePoint.CaptureProgress.InProgressDecr || captureProgress == FightingCapturePoint.CaptureProgress.InProgressIncr || this.currentRotationSpeed.More(0f, 1E-05f))
		{
			captureProgress = this.captureProgress;
			float num = ((captureProgress == FightingCapturePoint.CaptureProgress.InProgressDecr || captureProgress == FightingCapturePoint.CaptureProgress.InProgressIncr) ? 1f : (-1f));
			this.currentRotationSpeed = Mathf.Clamp01(this.currentRotationSpeed + num * Time.deltaTime);
			if (this.rotatableTransform)
			{
				this.rotatableTransform.Rotate(Vector3.up, Time.deltaTime * this.currentRotationSpeed * this.maxRotationSpeed, Space.World);
			}
			if (this.sparksOfRingParticleSystem)
			{
				ParticleSystem.VelocityOverLifetimeModule velocityOverLifetime = this.sparksOfRingParticleSystem.velocityOverLifetime;
				velocityOverLifetime.enabled = true;
				ParticleSystem.MinMaxCurve orbitalY = velocityOverLifetime.orbitalY;
				orbitalY.constant = Time.deltaTime * this.currentRotationSpeed * this.maxRotationSpeedForSparks;
				velocityOverLifetime.orbitalY = orbitalY;
			}
		}
	}

	// Token: 0x060014E6 RID: 5350 RVA: 0x00066074 File Offset: 0x00064274
	public void CustomUpdate()
	{
		if (!this.isActivated || this.LockedForCapture)
		{
			return;
		}
		this.RefreshCaptureOccupants();
		float num = 0f;
		float num2 = 0f;
		bool flag = false;
		bool flag2 = false;
		foreach (ICombatEntity combatEntity in this.allies)
		{
			num += (float)combatEntity.CombatEntityQuality;
			flag = !num.EqualsTo(0f, 1E-05f);
		}
		foreach (ICombatEntity combatEntity2 in this.enemies)
		{
			num2 += (float)combatEntity2.CombatEntityQuality;
			flag2 = !num2.EqualsTo(0f, 1E-05f);
		}
		float num3 = num / this.captureComplexity;
		float num4 = num2 / this.captureComplexity;
		float num5 = ((this.ownedByTeamInGame == LazyConsts.Fighting.TeamType.Player) ? 1f : (-1f)) * (num3 - num4);
		float num6 = this.PreCalculateCaptureProgress(num5);
		if (flag && flag2)
		{
			this.SetVfxActive(FightingCapturePoint.CaptureProgress.State, false);
			return;
		}
		float num7 = Mathf.Sign(num6 - this.currentProgress);
		this.SetVfxActive(Mathf.Abs(Mathf.Clamp01(num6) - this.currentProgress).EqualsTo(0f, 1E-05f) ? FightingCapturePoint.CaptureProgress.State : ((num7 > 0f) ? FightingCapturePoint.CaptureProgress.InProgressIncr : FightingCapturePoint.CaptureProgress.InProgressDecr), false);
		if (this.currentProgress.EqualsOrMore(0f, 1E-05f) && num6 < 0f)
		{
			LazyConsts.Fighting.TeamType teamType = ((this.ownedByTeamInGame == LazyConsts.Fighting.TeamType.Player) ? LazyConsts.Fighting.TeamType.WildZombie : LazyConsts.Fighting.TeamType.Player);
			this.isCapturedByAllies = teamType == LazyConsts.Fighting.TeamType.Player;
			this.ExecuteCaptureEvents(teamType);
			this.ExecuteLostEvents(this.ownedByTeamInGame);
			this.SetOwnedByTeam(teamType);
			this.flagFlyingUpProgress = 0f;
			this.flagFlyAnimTween = DOTween.To(() => this.flagFlyingUpProgress, delegate(float v)
			{
				this.flagFlyingUpProgress = v;
				this.SetProgress(v);
			}, 1f, 0.5f).SetEase(Ease.OutBounce);
			this.SetVfxActive(FightingCapturePoint.CaptureProgress.State, true);
			Action<FightingCapturePoint> onCapturedByTeam = this.OnCapturedByTeam;
			if (onCapturedByTeam != null)
			{
				onCapturedByTeam(this);
			}
		}
		this.CalculateCaptureProgress(num5);
	}

	// Token: 0x060014E7 RID: 5351 RVA: 0x000662B4 File Offset: 0x000644B4
	public void SetOwnedByTeam(LazyConsts.Fighting.TeamType team)
	{
		this.ownedByTeamInGame = team;
		foreach (FightingCapturePoint.TeamsFlag teamsFlag in this.teamFlags)
		{
			foreach (GameObject gameObject in teamsFlag.objs)
			{
				gameObject.gameObject.SetActive(teamsFlag.team == team);
				if (teamsFlag.team == team)
				{
					this.rotatableTransform = teamsFlag.ringVfxBase.transform.GetChild(0);
					this.sparksOfRingParticleSystem = teamsFlag.sparksOfRingParticleSystem;
				}
			}
		}
	}

	// Token: 0x060014E8 RID: 5352 RVA: 0x00066380 File Offset: 0x00064580
	private void HandleTargetRemoved(TargetInfo targetInfo)
	{
		LazyConsts.Fighting.TeamType team = targetInfo.Team;
		if (team != LazyConsts.Fighting.TeamType.Player)
		{
			if (team == LazyConsts.Fighting.TeamType.WildZombie)
			{
				this.enemies.Remove(targetInfo.entity);
			}
		}
		else
		{
			this.allies.Remove(targetInfo.entity);
		}
		this.ReleaseSlot(targetInfo.entity.CombatEntityUID);
	}

	// Token: 0x060014E9 RID: 5353 RVA: 0x000663D4 File Offset: 0x000645D4
	public bool TryReserveSlot(SGuid agentId, out Vector3 position, out bool isWaitingSlot, bool allowWaitingSlot = true)
	{
		position = Vector3.zero;
		isWaitingSlot = false;
		if (this.reservedStandardSlots.TryGetValue(agentId, out position))
		{
			return true;
		}
		if (this.reservedWaitingSlots.TryGetValue(agentId, out position))
		{
			if (allowWaitingSlot)
			{
				isWaitingSlot = true;
				return true;
			}
			this.reservedWaitingSlots.Remove(agentId);
		}
		float num = Mathf.Max(0.1f, this.radius - 0.06666668f);
		if (this.TryFindAvailablePosition(num, 0f, this.reservedStandardSlots.Values, out position))
		{
			this.reservedStandardSlots[agentId] = position;
			return true;
		}
		if (allowWaitingSlot && this.TryFindAvailablePosition(this.radius + 3f, this.radius + 0.5f, this.reservedWaitingSlots.Values, out position))
		{
			this.reservedWaitingSlots[agentId] = position;
			isWaitingSlot = true;
			return true;
		}
		return false;
	}

	// Token: 0x060014EA RID: 5354 RVA: 0x000664B4 File Offset: 0x000646B4
	public void ReleaseSlot(SGuid agentId)
	{
		this.reservedStandardSlots.Remove(agentId);
		this.reservedWaitingSlots.Remove(agentId);
	}

	// Token: 0x060014EB RID: 5355 RVA: 0x000664D0 File Offset: 0x000646D0
	private bool TryFindAvailablePosition(float maxRadius, float minRadius, IEnumerable<Vector3> existingSlots, out Vector3 position)
	{
		position = Vector3.zero;
		if (AstarPath.active == null || AstarPath.active.graphs.Length <= 12)
		{
			return false;
		}
		RecastGraph recastGraph = AstarPath.active.graphs[12] as RecastGraph;
		if (recastGraph == null)
		{
			return false;
		}
		Vector3 position2 = base.transform.position;
		NearestNodeConstraint walkable = NearestNodeConstraint.Walkable;
		walkable.graphMask = GraphMask.FromGraph(recastGraph);
		for (int i = 0; i < 24; i++)
		{
			Vector2 vector = global::UnityEngine.Random.insideUnitCircle;
			Mathf.Lerp(minRadius, maxRadius, global::UnityEngine.Random.value);
			if (minRadius > 0f)
			{
				vector = vector.normalized * Mathf.Lerp(minRadius, maxRadius, Mathf.Sqrt(global::UnityEngine.Random.value));
			}
			else
			{
				vector *= maxRadius;
			}
			Vector3 vector2 = new Vector3(position2.x + vector.x, position2.y, position2.z + vector.y);
			GraphNode graphNode = recastGraph.PointOnNavmesh(vector2, walkable);
			if (graphNode != null)
			{
				Vector3 vector3 = (Vector3)graphNode.position;
				float num = vector3.x - position2.x;
				float num2 = vector3.z - position2.z;
				float num3 = num * num + num2 * num2;
				if (num3 <= maxRadius * maxRadius && num3 >= minRadius * minRadius)
				{
					bool flag = false;
					foreach (Vector3 vector4 in existingSlots)
					{
						if ((vector3 - vector4).sqrMagnitude < 0.64000005f)
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						position = vector3;
						return true;
					}
				}
			}
		}
		return false;
	}

	// Token: 0x060014EC RID: 5356 RVA: 0x00066684 File Offset: 0x00064884
	private void OnDestroy()
	{
		if (LazySingleton<FightingGameController>.Instance && LazySingleton<FightingGameController>.Instance.TargetsDatabase != null)
		{
			LazySingleton<FightingGameController>.Instance.TargetsDatabase.OnTargetRemoved -= this.HandleTargetRemoved;
		}
		Tween tween = this.flagFlyAnimTween;
		if (tween == null)
		{
			return;
		}
		tween.Kill(false);
	}

	// Token: 0x060014ED RID: 5357 RVA: 0x000666D8 File Offset: 0x000648D8
	private void RefreshCaptureOccupants()
	{
		this.PruneInvalidCaptureOccupants();
		FightingGameController instance = LazySingleton<FightingGameController>.Instance;
		if (instance == null || instance.TargetsDatabase == null)
		{
			return;
		}
		this.TryAddOverlappingCombatants(instance.TargetsDatabase.GetTargetsByTeam(LazyConsts.Fighting.TeamType.Player), this.allies);
		this.TryAddOverlappingCombatants(instance.TargetsDatabase.GetTargetsByTeam(LazyConsts.Fighting.TeamType.WildZombie), this.enemies);
	}

	// Token: 0x060014EE RID: 5358 RVA: 0x00066734 File Offset: 0x00064934
	private void PruneInvalidCaptureOccupants()
	{
		for (int i = this.allies.Count - 1; i >= 0; i--)
		{
			ICombatEntity combatEntity = this.allies[i];
			if (!this.CountsAsOccupyingCapturePoint(combatEntity))
			{
				this.allies.RemoveAt(i);
				if (combatEntity != null)
				{
					this.ReleaseSlot(combatEntity.CombatEntityUID);
				}
			}
		}
		for (int j = this.enemies.Count - 1; j >= 0; j--)
		{
			ICombatEntity combatEntity2 = this.enemies[j];
			if (!this.CountsAsOccupyingCapturePoint(combatEntity2))
			{
				this.enemies.RemoveAt(j);
				if (combatEntity2 != null)
				{
					this.ReleaseSlot(combatEntity2.CombatEntityUID);
				}
			}
		}
	}

	// Token: 0x060014EF RID: 5359 RVA: 0x000667D4 File Offset: 0x000649D4
	private void TryAddOverlappingCombatants(IReadOnlyList<ICombatEntity> candidates, List<ICombatEntity> occupants)
	{
		for (int i = 0; i < candidates.Count; i++)
		{
			ICombatEntity combatEntity = candidates[i];
			if (this.CountsAsOccupyingCapturePoint(combatEntity) && !occupants.Contains(combatEntity))
			{
				occupants.Add(combatEntity);
			}
		}
	}

	// Token: 0x060014F0 RID: 5360 RVA: 0x00066814 File Offset: 0x00064A14
	private bool CountsAsOccupyingCapturePoint(ICombatEntity entity)
	{
		if (entity == null)
		{
			return false;
		}
		global::UnityEngine.Object @object = entity as global::UnityEngine.Object;
		if (@object != null && !@object)
		{
			return false;
		}
		if (!entity.IsActiveCombatant)
		{
			return false;
		}
		HPComponent combatEntityHpComponent = entity.CombatEntityHpComponent;
		return (combatEntityHpComponent == null || combatEntityHpComponent.Hp > 0 || !combatEntityHpComponent.WasDamagedAtLeastOnce) && this.OverlapsCaptureVolume(entity.CombatEntityPosition);
	}

	// Token: 0x060014F1 RID: 5361 RVA: 0x0006686D File Offset: 0x00064A6D
	private bool OverlapsCaptureVolume(Vector3 position)
	{
		return this.IsPointInsideCaptureCollider(position) || this.IsPointInsideCaptureCollider(position + Vector3.up * 0.5f);
	}

	// Token: 0x060014F2 RID: 5362 RVA: 0x00066898 File Offset: 0x00064A98
	private bool IsPointInsideCaptureCollider(Vector3 position)
	{
		if (!this.col)
		{
			return this.ContainsPosition(position);
		}
		return (this.col.ClosestPoint(position) - position).sqrMagnitude < 0.0001f;
	}

	// Token: 0x060014F3 RID: 5363 RVA: 0x000668DB File Offset: 0x00064ADB
	private void CalculateCaptureProgress(float delta)
	{
		this.currentProgress = this.PreCalculateCaptureProgress(delta);
		this.SetProgress(this.currentProgress);
	}

	// Token: 0x060014F4 RID: 5364 RVA: 0x000668F6 File Offset: 0x00064AF6
	private float PreCalculateCaptureProgress(float delta)
	{
		return this.currentProgress + delta * Time.deltaTime / this.captureDuration;
	}

	// Token: 0x060014F5 RID: 5365 RVA: 0x0006690D File Offset: 0x00064B0D
	private void SetProgress(float progress)
	{
		this.currentProgress = Mathf.Clamp01(progress);
		this.flagTr.position = Vector3.Lerp(this.downPosTr.position, this.upPosTr.position, this.currentProgress);
	}

	// Token: 0x060014F6 RID: 5366 RVA: 0x00066948 File Offset: 0x00064B48
	private void ExecuteCaptureEvents(LazyConsts.Fighting.TeamType whoCaptured)
	{
		foreach (CapturePointAction capturePointAction in this.OnCaptureActions)
		{
			capturePointAction.Execute(whoCaptured, this);
			capturePointAction.Execute(whoCaptured, LazySingleton<FightingGameController>.Instance.CurrentLevel);
		}
	}

	// Token: 0x060014F7 RID: 5367 RVA: 0x000669AC File Offset: 0x00064BAC
	private void ExecuteLostEvents(LazyConsts.Fighting.TeamType whoLost)
	{
		foreach (CapturePointAction capturePointAction in this.OnLostActions)
		{
			capturePointAction.Execute(whoLost, this);
			capturePointAction.Execute(whoLost, LazySingleton<FightingGameController>.Instance.CurrentLevel);
		}
	}

	// Token: 0x060014F8 RID: 5368 RVA: 0x00066A10 File Offset: 0x00064C10
	private void SetVfxActive(FightingCapturePoint.CaptureProgress captureProgress, bool ignoreCheck = false)
	{
		if (!ignoreCheck && this.captureProgress == captureProgress)
		{
			return;
		}
		foreach (FightingCapturePoint.TeamsFlag teamsFlag in this.teamFlags)
		{
			teamsFlag.ringVfxActive.SetActive(captureProgress == FightingCapturePoint.CaptureProgress.InProgressDecr && teamsFlag.team == LazyConsts.Fighting.TeamType.Player);
			teamsFlag.ringVfxBase.SetActive(true);
		}
		this.captureProgress = captureProgress;
	}

	// Token: 0x0400158E RID: 5518
	private const float CAPTURE_DURATION = 10f;

	// Token: 0x0400158F RID: 5519
	private const float CAPTURE_COMPLEXITY = 20f;

	// Token: 0x04001590 RID: 5520
	private const float VFX_BASE_SCALE = 2f;

	// Token: 0x04001591 RID: 5521
	[SerializeField]
	private Collider col;

	// Token: 0x04001592 RID: 5522
	public Transform flagTr;

	// Token: 0x04001593 RID: 5523
	public Transform upPosTr;

	// Token: 0x04001594 RID: 5524
	public Transform downPosTr;

	// Token: 0x04001595 RID: 5525
	[Header("Ally flag stand")]
	[SerializeField]
	[Tooltip("Flag stand WGO in this sector.")]
	private FlagStandComponent allyFlagStand;

	// Token: 0x04001596 RID: 5526
	public List<FightingCapturePoint.TeamsFlag> teamFlags = new List<FightingCapturePoint.TeamsFlag>();

	// Token: 0x04001597 RID: 5527
	public bool isBasePoint;

	// Token: 0x04001598 RID: 5528
	[SerializeField]
	private bool hasStartValue;

	// Token: 0x04001599 RID: 5529
	[SerializeField]
	[Range(0f, 1f)]
	private float startValue;

	// Token: 0x0400159A RID: 5530
	[Header("Capture duration")]
	public float captureDuration = 10f;

	// Token: 0x0400159B RID: 5531
	public float captureComplexity = 20f;

	// Token: 0x0400159C RID: 5532
	[SerializeField]
	private LazyConsts.Fighting.TeamType ownedByTeam = LazyConsts.Fighting.TeamType.WildZombie;

	// Token: 0x0400159D RID: 5533
	[SerializeField]
	private LazyConsts.Fighting.TeamType ownedByTeamInGame;

	// Token: 0x0400159E RID: 5534
	private bool isActivated;

	// Token: 0x0400159F RID: 5535
	private bool isCapturedByAllies;

	// Token: 0x040015A0 RID: 5536
	private FightingSector sector;

	// Token: 0x040015A1 RID: 5537
	public readonly List<ICombatEntity> allies = new List<ICombatEntity>();

	// Token: 0x040015A2 RID: 5538
	public readonly List<ICombatEntity> enemies = new List<ICombatEntity>();

	// Token: 0x040015A3 RID: 5539
	private float currentProgress;

	// Token: 0x040015A4 RID: 5540
	private float radius;

	// Token: 0x040015A5 RID: 5541
	private float flagFlyingUpProgress;

	// Token: 0x040015A6 RID: 5542
	private Tween flagFlyAnimTween;

	// Token: 0x040015A7 RID: 5543
	private bool lockedForCapture;

	// Token: 0x040015A8 RID: 5544
	private FightingCapturePoint.CaptureProgress captureProgress;

	// Token: 0x040015A9 RID: 5545
	[SerializeReference]
	public List<CapturePointAction> OnCaptureActions = new List<CapturePointAction>();

	// Token: 0x040015AA RID: 5546
	[SerializeReference]
	public List<CapturePointAction> OnLostActions = new List<CapturePointAction>();

	// Token: 0x040015AB RID: 5547
	private Transform rotatableTransform;

	// Token: 0x040015AC RID: 5548
	private ParticleSystem sparksOfRingParticleSystem;

	// Token: 0x040015AD RID: 5549
	[SerializeField]
	private float maxRotationSpeedForSparks = 10f;

	// Token: 0x040015AE RID: 5550
	[SerializeField]
	private float maxRotationSpeed = 100f;

	// Token: 0x040015AF RID: 5551
	[SerializeField]
	private float timeToRotateToMaxSpeed = 1f;

	// Token: 0x040015B0 RID: 5552
	[SerializeField]
	private float currentRotationSpeed;

	// Token: 0x040015B1 RID: 5553
	private Dictionary<SGuid, Vector3> reservedStandardSlots = new Dictionary<SGuid, Vector3>();

	// Token: 0x040015B2 RID: 5554
	private Dictionary<SGuid, Vector3> reservedWaitingSlots = new Dictionary<SGuid, Vector3>();

	// Token: 0x040015B3 RID: 5555
	private const float MIN_SLOT_DISTANCE = 0.8f;

	// Token: 0x040015B4 RID: 5556
	private const float FLAG_STAND_POSITION_THRESHOLD = 0.5f;

	// Token: 0x02000310 RID: 784
	[Serializable]
	public class TeamsFlag
	{
		// Token: 0x040015B5 RID: 5557
		public LazyConsts.Fighting.TeamType team;

		// Token: 0x040015B6 RID: 5558
		public List<GameObject> objs = new List<GameObject>();

		// Token: 0x040015B7 RID: 5559
		public GameObject ringVfxBase;

		// Token: 0x040015B8 RID: 5560
		public GameObject ringVfxActive;

		// Token: 0x040015B9 RID: 5561
		public ParticleSystem sparksOfRingParticleSystem;
	}

	// Token: 0x02000311 RID: 785
	private enum CaptureProgress
	{
		// Token: 0x040015BB RID: 5563
		None,
		// Token: 0x040015BC RID: 5564
		State,
		// Token: 0x040015BD RID: 5565
		InProgressIncr,
		// Token: 0x040015BE RID: 5566
		InProgressDecr
	}
}
