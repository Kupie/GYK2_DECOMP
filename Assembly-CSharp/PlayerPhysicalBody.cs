using System;
using System.Collections;
using System.Collections.Generic;
using LazyBearTechnology;
using Pathfinding.RVO;
using UnityEngine;

// Token: 0x02000399 RID: 921
public class PlayerPhysicalBody : MonoBehaviour, ICombatEntity, IPhysicallyMutable
{
	// Token: 0x17000431 RID: 1073
	// (get) Token: 0x060018A2 RID: 6306 RVA: 0x00074AF7 File Offset: 0x00072CF7
	public PlayerView PlayerView
	{
		get
		{
			return this.playerView;
		}
	}

	// Token: 0x17000432 RID: 1074
	// (get) Token: 0x060018A3 RID: 6307 RVA: 0x00074AFF File Offset: 0x00072CFF
	public Rigidbody Rb
	{
		get
		{
			return this.rb;
		}
	}

	// Token: 0x17000433 RID: 1075
	// (get) Token: 0x060018A4 RID: 6308 RVA: 0x00074B07 File Offset: 0x00072D07
	public PlayerPhysicsConfig PhysicsConfig
	{
		get
		{
			return this.physicsConfig;
		}
	}

	// Token: 0x17000434 RID: 1076
	// (get) Token: 0x060018A5 RID: 6309 RVA: 0x00074B0F File Offset: 0x00072D0F
	// (set) Token: 0x060018A6 RID: 6310 RVA: 0x00074B17 File Offset: 0x00072D17
	public float SpeedMultiplier { get; set; } = 1f;

	// Token: 0x17000435 RID: 1077
	// (get) Token: 0x060018A7 RID: 6311 RVA: 0x00074B20 File Offset: 0x00072D20
	public Vector3 GravityNormalized
	{
		get
		{
			return this.gravityNormalized;
		}
	}

	// Token: 0x17000436 RID: 1078
	// (get) Token: 0x060018A8 RID: 6312 RVA: 0x00074B28 File Offset: 0x00072D28
	public MeshCollider MeshCollider
	{
		get
		{
			return this.meshCollider;
		}
	}

	// Token: 0x060018A9 RID: 6313 RVA: 0x00074B30 File Offset: 0x00072D30
	public void Init()
	{
		this.playerView.Init();
		this.prevDirection = this.playerView.GetAnimationDirection();
		this.dynamicMultiFlag.Init(new Action<bool>(this.SetDynamicActive), true);
		if (this.physicsConfig == null)
		{
			Debug.LogError("PlayerPhysicsConfig must be set");
		}
	}

	// Token: 0x060018AA RID: 6314 RVA: 0x00074B8C File Offset: 0x00072D8C
	public void PrepareForGame(PlayerData playerData)
	{
		this.playerData = playerData;
		this.SetPosition(playerData.position.Value);
		this.playerView.PrepareForGame(playerData);
		this.damageEffectComponent.Init(this);
		if (this.rvo != null)
		{
			this.rvo.priority = 0.85f;
		}
	}

	// Token: 0x060018AB RID: 6315 RVA: 0x00074BE8 File Offset: 0x00072DE8
	public void UnPrepareFromGame()
	{
		this.isOnGround = false;
		this.hasContactPoints = false;
		this.shouldStickOnGround = false;
		this.isJumping = false;
		this.isMovementLocked = false;
		this.hasAimDirection = false;
		this.aimDirection = Vector2.zero;
		this.playerView.UnPrepareFromGame();
	}

	// Token: 0x060018AC RID: 6316 RVA: 0x00074C35 File Offset: 0x00072E35
	public void SetDirectionLock(bool isEnabled)
	{
		this.isSetRotationLocked = !isEnabled;
	}

	// Token: 0x060018AD RID: 6317 RVA: 0x00074C44 File Offset: 0x00072E44
	public void InitNetworkPlayer(NetworkPlayer networkPlayer)
	{
		PlayerController component = base.GetComponent<PlayerController>();
		component.SetPlayerData(networkPlayer.playerData);
		component.Initialize();
		component.ResetControlState();
		component.gameObject.SetActive(true);
		component.WispController.gameObject.SetActive(true);
		component.enabled = false;
		this.SetDynamicActive(true);
		this.SetPosition(networkPlayer.playerData.position.Value);
		base.enabled = false;
	}

	// Token: 0x060018AE RID: 6318 RVA: 0x00074CB8 File Offset: 0x00072EB8
	public void SetPosition(Vector3 position)
	{
		bool flag = false;
		if (this.playerData.CurrentWorldZoneData != null && !this.playerData.CurrentWorldZoneData.wholeZoneRect.Contains(position.XZ2()))
		{
			this.playerData.SetCurrentWorldZoneData(null, new Action(this.RedrawWorldZoneWidget));
			this.currentContainerWorldZone = null;
			this.currentNonContainerWorldZone = null;
			flag = true;
		}
		for (int i = this.playerData.insideTownZones.Count - 1; i >= 0; i--)
		{
			TownZone townZone = this.playerData.insideTownZones[i];
			if (townZone && !townZone.Collider.bounds.Contains(position))
			{
				this.playerData.RemoveTownZone(townZone);
				flag = true;
			}
		}
		for (int j = this.playerData.insideTownSubZones.Count - 1; j >= 0; j--)
		{
			TownSubZone townSubZone = this.playerData.insideTownSubZones[j];
			if (townSubZone && !townSubZone.Collider.bounds.Contains(position))
			{
				this.playerData.RemoveTownSubZone(townSubZone);
				flag = true;
			}
		}
		if (flag)
		{
			GUIElements.Instance.WorldZoneWidget.Draw(new WorldZoneWidgetData());
		}
		this.contactCollisions.Clear();
		this.contactPoints.Clear();
		this.rb.position = position;
		base.transform.position = position;
		this.playerData.position.Value = position;
	}

	// Token: 0x060018AF RID: 6319 RVA: 0x00074E30 File Offset: 0x00073030
	public void MoveByDirection(Vector2 direction)
	{
		if (!this.physicsConfig)
		{
			return;
		}
		if (MainGame.IsGamePaused)
		{
			return;
		}
		this.UpdatePhysics();
		if (!this.HasMovementPossibility(direction))
		{
			this.SetCurrentAnimationState(global::AnimationState.Idle);
			return;
		}
		float num = this.physicsConfig.speed * this.SpeedMultiplier;
		this.SetCurrentAnimationState(global::AnimationState.Walk);
		this.playerView.UpdateAnimationDirection(this.GetFacingDirection(direction));
		Vector3 vector = Vector3.ProjectOnPlane(this.gravityNormalized, Vector3.right);
		Vector3 vector2 = Vector3.ProjectOnPlane(this.gravityNormalized, Vector3.back);
		Quaternion quaternion = Quaternion.FromToRotation(Physics.gravity.normalized, vector);
		Quaternion quaternion2 = Quaternion.FromToRotation(Physics.gravity.normalized, vector2);
		Vector3 normalized = new Vector3(direction.x, 0f, direction.y).normalized;
		Vector3 vector3 = quaternion * quaternion2 * normalized;
		vector3 = Vector3.Scale(vector3, new Vector3(1f, 0.5999999f, 0.8f));
		Vector3 vector4 = Vector3.ProjectOnPlane(Physics.gravity.normalized * vector3.magnitude, this.gravityNormalized);
		vector3 += vector4 * this.physicsConfig.slopeGravityMovementSlowdown;
		Vector3 vector5 = vector3 * (num * Time.fixedDeltaTime);
		this.rb.AddForce(vector5, ForceMode.VelocityChange);
		Debug.DrawRay(base.transform.position, vector3, Color.black, 0.2f);
		this.movedByDirectionThisFrame = true;
	}

	// Token: 0x060018B0 RID: 6320 RVA: 0x00074FB4 File Offset: 0x000731B4
	public void MoveByPosition(Vector3 position, Vector2 movementDirection, bool needDirectionChange = true)
	{
		if (!this.rb.isKinematic)
		{
			Debug.LogError("Trying to move non-static RB by position");
			return;
		}
		this.SetCurrentAnimationState(global::AnimationState.Walk);
		if (needDirectionChange)
		{
			this.playerView.UpdateAnimationDirection(movementDirection.normalized);
		}
		this.rb.position = position;
		this.UpdatePlayerData();
	}

	// Token: 0x060018B1 RID: 6321 RVA: 0x00075007 File Offset: 0x00073207
	public void StopMoving()
	{
		if (this.playerView.PlayerAnimation.AnimationState != global::AnimationState.Walk)
		{
			return;
		}
		this.SetCurrentAnimationState(global::AnimationState.Idle);
	}

	// Token: 0x060018B2 RID: 6322 RVA: 0x00075024 File Offset: 0x00073224
	public void SetFacingDirection(Vector2 direction)
	{
		if (direction.sqrMagnitude < 0.0001f)
		{
			return;
		}
		Vector2 normalized = direction.normalized;
		if (!this.isSetRotationLocked)
		{
			this.playerData.Direction = normalized;
		}
		this.playerView.UpdateAnimationDirection(normalized);
	}

	// Token: 0x060018B3 RID: 6323 RVA: 0x00075068 File Offset: 0x00073268
	public void SetAimDirection(Vector2 direction)
	{
		if (direction.sqrMagnitude < 0.0001f)
		{
			return;
		}
		Vector2 normalized = direction.normalized;
		this.hasAimDirection = true;
		this.aimDirection = normalized;
		if (!this.isSetRotationLocked)
		{
			this.playerData.Direction = normalized;
		}
		this.playerView.UpdateAnimationDirection(normalized);
	}

	// Token: 0x060018B4 RID: 6324 RVA: 0x000750BA File Offset: 0x000732BA
	public void ClearAimDirection()
	{
		this.hasAimDirection = false;
		this.aimDirection = Vector2.zero;
	}

	// Token: 0x060018B5 RID: 6325 RVA: 0x000750CE File Offset: 0x000732CE
	private Vector2 GetFacingDirection(Vector2 movementDirection)
	{
		if (this.hasAimDirection)
		{
			return this.aimDirection;
		}
		if (!this.isSetRotationLocked)
		{
			return movementDirection.normalized;
		}
		return this.playerData.Direction;
	}

	// Token: 0x060018B6 RID: 6326 RVA: 0x000750FA File Offset: 0x000732FA
	public void SetNonKinematicFlag(PlayerDynamicType type, bool isDynamic)
	{
		this.dynamicMultiFlag.UpdateFlag(type, isDynamic);
	}

	// Token: 0x060018B7 RID: 6327 RVA: 0x00075109 File Offset: 0x00073309
	private void SetDynamicActive(bool isDynamic)
	{
		this.Mute(null);
		this.rb.isKinematic = !isDynamic;
	}

	// Token: 0x060018B8 RID: 6328 RVA: 0x00075121 File Offset: 0x00073321
	public void LockMovement(bool isLock)
	{
		this.isMovementLocked = isLock;
		this.SetDynamicActive(!isLock);
	}

	// Token: 0x17000437 RID: 1079
	// (get) Token: 0x060018B9 RID: 6329 RVA: 0x00075134 File Offset: 0x00073334
	// (set) Token: 0x060018BA RID: 6330 RVA: 0x0007513C File Offset: 0x0007333C
	public bool IsMuted { get; set; }

	// Token: 0x060018BB RID: 6331 RVA: 0x00075145 File Offset: 0x00073345
	public void Mute(Action onUnmuted = null)
	{
		if (this.muteCoroutine != null)
		{
			base.StopCoroutine(this.muteCoroutine);
		}
		this.muteCoroutine = base.StartCoroutine(this.MuteForOneFrameCoroutine(onUnmuted));
	}

	// Token: 0x060018BC RID: 6332 RVA: 0x0007516E File Offset: 0x0007336E
	private IEnumerator MuteForOneFrameCoroutine(Action onUnmuted)
	{
		this.IsMuted = true;
		yield return new WaitForFixedUpdate();
		yield return new WaitForFixedUpdate();
		this.IsMuted = false;
		this.muteCoroutine = null;
		if (onUnmuted != null)
		{
			onUnmuted();
		}
		yield break;
	}

	// Token: 0x060018BD RID: 6333 RVA: 0x00075184 File Offset: 0x00073384
	private bool HasMovementPossibility(Vector2 direction)
	{
		bool flag = !direction.magnitude.EqualsTo(0f, 1E-05f);
		if (flag && !this.isMoving)
		{
			this.isMoving = true;
			GlobalEventsSystem.FireTrigger(GlobalEventsSystem.Event.Type.PlayerStartMoving, "");
		}
		else if (!flag && this.isMoving)
		{
			this.isMoving = false;
		}
		return flag && !this.isMovementLocked;
	}

	// Token: 0x17000438 RID: 1080
	// (get) Token: 0x060018BE RID: 6334 RVA: 0x000751EB File Offset: 0x000733EB
	public SGuid CombatEntityUID
	{
		get
		{
			return this.playerData.Guid;
		}
	}

	// Token: 0x17000439 RID: 1081
	// (get) Token: 0x060018BF RID: 6335 RVA: 0x00028294 File Offset: 0x00026494
	public LazyConsts.Fighting.TeamType TeamType
	{
		get
		{
			return LazyConsts.Fighting.TeamType.Player;
		}
	}

	// Token: 0x1700043A RID: 1082
	// (get) Token: 0x060018C0 RID: 6336 RVA: 0x0003C7FE File Offset: 0x0003A9FE
	public LazyConsts.Fighting.EntityType EntityType
	{
		get
		{
			return LazyConsts.Fighting.EntityType.Player;
		}
	}

	// Token: 0x1700043B RID: 1083
	// (get) Token: 0x060018C1 RID: 6337 RVA: 0x000751F8 File Offset: 0x000733F8
	public Vector3 CombatEntityPosition
	{
		get
		{
			return this.rb.position;
		}
	}

	// Token: 0x1700043C RID: 1084
	// (get) Token: 0x060018C2 RID: 6338 RVA: 0x00075205 File Offset: 0x00073405
	public int ArmorValue
	{
		get
		{
			Item itemByType = this.playerData.toolBeltInventory.GetItemByType(ItemType.BodyArmor);
			if (itemByType == null)
			{
				return 0;
			}
			return itemByType.Definition.quality;
		}
	}

	// Token: 0x060018C3 RID: 6339 RVA: 0x0007522C File Offset: 0x0007342C
	public float GetCombatEntityDistance(Vector3 from, LazyConsts.Fighting.TeamType teamType)
	{
		return (from - this.CombatEntityPosition).magnitude;
	}

	// Token: 0x1700043D RID: 1085
	// (get) Token: 0x060018C4 RID: 6340 RVA: 0x0007524D File Offset: 0x0007344D
	public HPComponent CombatEntityHpComponent
	{
		get
		{
			return this.playerData.hpComponent;
		}
	}

	// Token: 0x1700043E RID: 1086
	// (get) Token: 0x060018C5 RID: 6341 RVA: 0x0007525A File Offset: 0x0007345A
	public int CombatEntityQuality
	{
		get
		{
			return this.fightingQuality;
		}
	}

	// Token: 0x060018C6 RID: 6342 RVA: 0x00075262 File Offset: 0x00073462
	public float GetCombatEntityGameRes(string resId)
	{
		if (this.playerData == null)
		{
			return 0f;
		}
		return this.playerData.GetRes(resId, 0f);
	}

	// Token: 0x1700043F RID: 1087
	// (get) Token: 0x060018C7 RID: 6343 RVA: 0x00075283 File Offset: 0x00073483
	// (set) Token: 0x060018C8 RID: 6344 RVA: 0x0007528B File Offset: 0x0007348B
	public int AttackPriority { get; set; }

	// Token: 0x17000440 RID: 1088
	// (get) Token: 0x060018C9 RID: 6345 RVA: 0x00028294 File Offset: 0x00026494
	public bool HasAnyDockPoint
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000441 RID: 1089
	// (get) Token: 0x060018CA RID: 6346 RVA: 0x00075294 File Offset: 0x00073494
	// (set) Token: 0x060018CB RID: 6347 RVA: 0x0007529C File Offset: 0x0007349C
	public bool IsActiveCombatant { get; set; }

	// Token: 0x060018CC RID: 6348 RVA: 0x00002318 File Offset: 0x00000518
	public void OnOtherCombatTargetReachedToMe(ICombatEntity other)
	{
	}

	// Token: 0x060018CD RID: 6349 RVA: 0x00002318 File Offset: 0x00000518
	public void OnOtherCombatTargetHitMe(AttackContext ctx)
	{
	}

	// Token: 0x060018CE RID: 6350 RVA: 0x000752A8 File Offset: 0x000734A8
	private void FixedUpdate()
	{
		this.UpdateWorldZone();
		if (this.rb.isKinematic)
		{
			return;
		}
		if (this.invisibleWalls)
		{
			this.invisibleWalls.UpdateEdgeWallsState(base.transform.position);
		}
		this.hasPhysUpdatedThisFixedFrame = false;
		this.UpdatePhysics();
		this.UpdatePlayerData();
		this.movedByDirectionThisFrame = false;
	}

	// Token: 0x060018CF RID: 6351 RVA: 0x00075308 File Offset: 0x00073508
	private void UpdatePlayerData()
	{
		if ((this.prevPosition - this.rb.position).magnitude > 0.001f)
		{
			this.prevPosition = this.rb.position;
			this.playerData.position.Value = this.prevPosition;
		}
		Vector2 animationDirection = this.playerView.GetAnimationDirection();
		if ((this.prevDirection - animationDirection).magnitude > 0.001f)
		{
			this.prevDirection = animationDirection;
			this.playerData.Direction = animationDirection;
		}
	}

	// Token: 0x060018D0 RID: 6352 RVA: 0x0007539B File Offset: 0x0007359B
	private void SetCurrentAnimationState(global::AnimationState state)
	{
		this.playerData.charState.Value = state;
		this.playerView.PlayerAnimation.SetState(state);
	}

	// Token: 0x060018D1 RID: 6353 RVA: 0x000753C0 File Offset: 0x000735C0
	private void UpdatePhysics()
	{
		if (!this.physicsConfig)
		{
			return;
		}
		if (this.hasPhysUpdatedThisFixedFrame)
		{
			return;
		}
		this.hasPhysUpdatedThisFixedFrame = true;
		this.gravityNormalized = Physics.gravity.normalized;
		PlayerPhysicalBody.GroundHit groundHit = this.GetGroundHit();
		Vector3 vector;
		bool flag = this.HasCustomGravityField(out vector);
		this.isOnGround = !this.isJumping && (this.hasContactPoints || this.shouldStickOnGround);
		if (this.invisibleWalls)
		{
			if (!this.isOnGround && this.invisibleWalls.IsWallsCheckEnabled)
			{
				this.invisibleWalls.ChangeWallsVisibility(false);
			}
			this.invisibleWalls.IsWallsCheckEnabled = this.isOnGround;
		}
		if (this.isOnGround)
		{
			if (!this.hasContactPoints)
			{
				this.gravityNormalized = (flag ? (-vector) : (-groundHit.normal.normalized));
				this.rb.AddForce(this.gravityNormalized * (Physics.gravity.magnitude * this.rb.mass * this.physicsConfig.stickForceMult), ForceMode.Force);
				Debug.DrawRay(base.transform.position, this.gravityNormalized, Color.green, 1f);
				return;
			}
			this.gravityNormalized = this.CalculateGravityVectorByContacts();
			this.rb.AddForce(this.gravityNormalized * (Physics.gravity.magnitude * this.rb.mass * this.physicsConfig.contactForceMult), ForceMode.Force);
			Debug.DrawRay(base.transform.position, this.gravityNormalized, Color.cyan);
			if (!this.movedByDirectionThisFrame)
			{
				Vector3 vector2 = -this.gravityNormalized;
				Vector3 vector3 = Vector3.ProjectOnPlane(this.rb.linearVelocity, vector2);
				this.rb.linearVelocity -= vector3;
				return;
			}
		}
		else if (!this.isJumping)
		{
			this.rb.AddForce(this.gravityNormalized * (Physics.gravity.magnitude * this.rb.mass * this.physicsConfig.gravityFallScale), ForceMode.Acceleration);
		}
	}

	// Token: 0x060018D2 RID: 6354 RVA: 0x000755F0 File Offset: 0x000737F0
	private void OnCollisionEnter(Collision collisionSource)
	{
		if (!this.IsCollisionValid(collisionSource))
		{
			return;
		}
		SerializableCollision serializableCollision = SerializableCollision.CreateFrom(collisionSource);
		if (!this.contactCollisions.TryAdd(collisionSource.gameObject.GetHashCode(), serializableCollision))
		{
			return;
		}
		if (!this.shouldStickOnGround)
		{
			this.shouldStickOnGround = true;
		}
		this.UpdateContactPoints();
	}

	// Token: 0x060018D3 RID: 6355 RVA: 0x00075640 File Offset: 0x00073840
	private void OnCollisionStay(Collision collisionSource)
	{
		if (!this.IsCollisionValid(collisionSource))
		{
			return;
		}
		SerializableCollision serializableCollision;
		if (!this.contactCollisions.TryGetValue(collisionSource.gameObject.GetHashCode(), out serializableCollision))
		{
			return;
		}
		serializableCollision.UpdateContactPoints(collisionSource);
		this.UpdateContactPoints();
	}

	// Token: 0x060018D4 RID: 6356 RVA: 0x0007567F File Offset: 0x0007387F
	private void OnCollisionExit(Collision collisionSource)
	{
		if (!this.IsCollisionValid(collisionSource))
		{
			return;
		}
		if (!this.contactCollisions.Remove(collisionSource.gameObject.GetHashCode()))
		{
			return;
		}
		this.UpdateContactPoints();
	}

	// Token: 0x060018D5 RID: 6357 RVA: 0x000756AA File Offset: 0x000738AA
	private int RaycastWorldZones()
	{
		return Physics.RaycastNonAlloc(new Ray(base.transform.position + Vector3.up * 50f, Vector3.down), PlayerPhysicalBody.raycastWZResults, 100f, 131072);
	}

	// Token: 0x060018D6 RID: 6358 RVA: 0x000756E9 File Offset: 0x000738E9
	private static bool TryGetWorldZone(Collider col, out WorldZone worldZone)
	{
		return col.TryGetComponent<WorldZone>(out worldZone) || (col.transform.parent && col.transform.parent.gameObject.TryGetComponent<WorldZone>(out worldZone));
	}

	// Token: 0x060018D7 RID: 6359 RVA: 0x00075720 File Offset: 0x00073920
	private void EnterWorldZone(WorldZone worldZone)
	{
		if (this.playerData.CurrentWorldZoneData == worldZone.Data)
		{
			return;
		}
		this.playerData.SetCurrentWorldZoneData(worldZone.Data, new Action(this.RedrawWorldZoneWidget));
		GlobalEventsSystem.FireTrigger(GlobalEventsSystem.Event.Type.WalkIntoWorldZone, worldZone.Data.id);
		Debug.Log(string.Format("Player entered world zone: {0}, Quality: [{1}]", worldZone.Id, worldZone.Data.GetTotalQuality()));
		WorldZoneWidgetData worldZoneWidgetData = new WorldZoneWidgetData(worldZone.Data);
		GUIElements.Instance.WorldZoneWidget.Draw(worldZoneWidgetData);
		if (MainGame.Instance.GameSave.knowledgeSystem.visitedWorldZones != null && !MainGame.Instance.GameSave.knowledgeSystem.visitedWorldZones.Contains(worldZone.Id))
		{
			MainGame.Instance.GameSave.knowledgeSystem.visitedWorldZones.Add(worldZone.Id);
		}
		worldZone.RedrawWgosWidgets();
	}

	// Token: 0x060018D8 RID: 6360 RVA: 0x00075810 File Offset: 0x00073A10
	private void ExitWorldZone(WorldZone worldZone)
	{
		WorldZoneData worldZoneData = ((worldZone != null) ? worldZone.Data : this.playerData.CurrentWorldZoneData);
		if (worldZoneData == null)
		{
			return;
		}
		Debug.Log("Player exited world zone: " + worldZoneData.id);
		this.playerData.SetCurrentWorldZoneData(null, new Action(this.RedrawWorldZoneWidget));
		GUIElements.Instance.WorldZoneWidget.Draw(new WorldZoneWidgetData());
		if (worldZone != null)
		{
			worldZone.HideWgosWorldZoneWidgets();
		}
	}

	// Token: 0x060018D9 RID: 6361 RVA: 0x0007588E File Offset: 0x00073A8E
	private bool IsCollisionValid(Collision collision)
	{
		return PlayerPhysicalBody.IsInLayerMask(collision.gameObject, 6144) && !(collision.gameObject == base.gameObject);
	}

	// Token: 0x060018DA RID: 6362 RVA: 0x000758C0 File Offset: 0x00073AC0
	private Vector3 CalculateGravityVectorByContacts()
	{
		Vector3 vector = Vector3.zero;
		for (int i = 0; i < this.contactPoints.Count; i++)
		{
			vector += this.contactPoints[i].normal;
		}
		return -vector.normalized;
	}

	// Token: 0x060018DB RID: 6363 RVA: 0x00075910 File Offset: 0x00073B10
	private void UpdateContactPoints()
	{
		this.contactPoints.Clear();
		foreach (SerializableCollision serializableCollision in this.contactCollisions.Values)
		{
			for (int i = 0; i < serializableCollision.contactPoints.Length; i++)
			{
				SerializableContactPoint serializableContactPoint = serializableCollision.contactPoints[i];
				if (serializableCollision.layer == 12)
				{
					this.contactPoints.Add(serializableContactPoint);
				}
				else if (this.physicsConfig && this.physicsConfig.maxGroundAngle > Vector3.Angle(serializableContactPoint.normal, -Physics.gravity.normalized))
				{
					this.contactPoints.Add(serializableContactPoint);
				}
			}
		}
		this.hasContactPoints = this.contactPoints.Count > 0;
		this.UpdateShadowcasterFromContactPoints();
	}

	// Token: 0x060018DC RID: 6364 RVA: 0x00075A04 File Offset: 0x00073C04
	private void UpdateShadowcasterFromContactPoints()
	{
		if (!this.playerView || !this.playerView.cylinderShadowcaster)
		{
			return;
		}
		if (!this.isOnGround)
		{
			this.playerView.cylinderShadowcaster.gameObject.SetActive(false);
			return;
		}
		this.playerView.cylinderShadowcaster.gameObject.SetActive(true);
		Vector3 vector = -this.CalculateGravityVectorByContacts();
		this.UpdateShadowcaster(vector);
	}

	// Token: 0x060018DD RID: 6365 RVA: 0x00075A7C File Offset: 0x00073C7C
	private void UpdateShadowcaster(Vector3 groundNormal)
	{
		float num = (this.playerView.cylinderShadowcaster.parent ? this.playerView.cylinderShadowcaster.parent.localScale.x : 1f);
		Quaternion quaternion = Quaternion.FromToRotation(Vector3.up, groundNormal);
		if (num < 0f)
		{
			quaternion = Quaternion.Euler(quaternion.eulerAngles.x, quaternion.eulerAngles.y, -quaternion.eulerAngles.z);
		}
		this.playerView.cylinderShadowcaster.localRotation = quaternion;
		float num2 = Vector3.Angle(groundNormal, Vector3.up) * 0.017453292f;
		float num3 = 1f + 0.5f * Mathf.Sin(num2 * 2f);
		float num4 = Mathf.Pow(Mathf.Cos(num2), 2f);
		Vector3 cylinderShadowCasterScale = this.playerView.cylinderShadowCasterScale;
		cylinderShadowCasterScale.y *= num3;
		cylinderShadowCasterScale.z *= num4;
		this.playerView.cylinderShadowcaster.localScale = cylinderShadowCasterScale;
	}

	// Token: 0x060018DE RID: 6366 RVA: 0x00075B84 File Offset: 0x00073D84
	private PlayerPhysicalBody.GroundHit GetGroundHit()
	{
		float num = 0.01666667f;
		Vector3 vector = base.transform.position + Vector3.up * num;
		PlayerPhysicalBody.GroundHit groundHit = new PlayerPhysicalBody.GroundHit(new Vector3(base.transform.position.x, 0f, base.transform.position.z), Vector3.up);
		if (!this.physicsConfig)
		{
			return groundHit;
		}
		int num2 = Physics.RaycastNonAlloc(new Ray(vector, Vector3.down), PlayerPhysicalBody.raycastResults, this.physicsConfig.groundRaycastLength, 6144);
		if (num2 > 0)
		{
			RaycastHit raycastHit = PlayerPhysicalBody.raycastResults[0];
			float num3 = (raycastHit.point - vector).magnitude;
			for (int i = 1; i < num2; i++)
			{
				RaycastHit raycastHit2 = PlayerPhysicalBody.raycastResults[i];
				float magnitude = (raycastHit2.point - vector).magnitude;
				if (magnitude < num3)
				{
					num3 = magnitude;
					raycastHit = raycastHit2;
				}
			}
			groundHit = new PlayerPhysicalBody.GroundHit(raycastHit.point, raycastHit.normal);
		}
		return groundHit;
	}

	// Token: 0x060018DF RID: 6367 RVA: 0x00075CA0 File Offset: 0x00073EA0
	private PlayerPhysicalBody.GroundHit GetGroundHitFromVector(Vector3 position, Vector3 direction)
	{
		if (!this.physicsConfig)
		{
			return null;
		}
		PlayerPhysicalBody.GroundHit groundHit = null;
		Ray ray = new Ray(position, direction);
		RaycastHit[] array = new RaycastHit[2];
		int num = Physics.RaycastNonAlloc(ray, array, this.physicsConfig.groundRaycastLength, 2048);
		if (num > 0)
		{
			RaycastHit raycastHit = array[0];
			float num2 = (raycastHit.point - position).magnitude;
			for (int i = 1; i < num; i++)
			{
				RaycastHit raycastHit2 = array[i];
				float magnitude = (raycastHit2.point - position).magnitude;
				if (magnitude < num2)
				{
					num2 = magnitude;
					raycastHit = raycastHit2;
				}
			}
			groundHit = new PlayerPhysicalBody.GroundHit(raycastHit.point, raycastHit.normal);
		}
		return groundHit;
	}

	// Token: 0x060018E0 RID: 6368 RVA: 0x00075D5C File Offset: 0x00073F5C
	private bool HasCustomGravityField(out Vector3 fieldNormal)
	{
		fieldNormal = Vector3.up;
		float num = 0.01666667f;
		Vector3 vector = base.transform.position + Vector3.up * num;
		Ray ray = new Ray(vector, Vector3.down);
		RaycastHit[] array = new RaycastHit[1];
		if (Physics.RaycastNonAlloc(ray, array, this.physicsConfig.groundRaycastLength, 8192) > 0)
		{
			fieldNormal = array[0].normal;
			return true;
		}
		return false;
	}

	// Token: 0x060018E1 RID: 6369 RVA: 0x00075DD6 File Offset: 0x00073FD6
	private static bool IsInLayerMask(GameObject gameObject, LayerMask mask)
	{
		return mask == (mask | (1 << gameObject.layer));
	}

	// Token: 0x060018E2 RID: 6370 RVA: 0x00075DF4 File Offset: 0x00073FF4
	private void RedrawWorldZoneWidget()
	{
		if (this.playerData.CurrentWorldZoneData == null)
		{
			return;
		}
		WorldZoneWidgetData worldZoneWidgetData = new WorldZoneWidgetData(this.playerData.CurrentWorldZoneData);
		GUIElements.Instance.WorldZoneWidget.Draw(worldZoneWidgetData);
	}

	// Token: 0x060018E3 RID: 6371 RVA: 0x00075E30 File Offset: 0x00074030
	private void UpdateWorldZone()
	{
		int num = this.RaycastWorldZones();
		WorldZone worldZone = null;
		WorldZone worldZone2 = null;
		TownZone townZone = null;
		TownSubZone townSubZone = null;
		MapZone mapZone = null;
		int i = 0;
		while (i < num)
		{
			Collider collider = PlayerPhysicalBody.raycastWZResults[i].collider;
			WorldZone worldZone3;
			if (!PlayerPhysicalBody.TryGetWorldZone(collider, out worldZone3))
			{
				goto IL_00C9;
			}
			if (worldZone3.Data.Definition.displayType != WorldZoneDef.DisplayType.Hidden)
			{
				WorldZoneData.WorldZoneType worldZoneType = worldZone3.WorldZoneType;
				if (worldZoneType != WorldZoneData.WorldZoneType.Default)
				{
					if (worldZoneType != WorldZoneData.WorldZoneType.SimpleNotContainer)
					{
						goto IL_00C9;
					}
					if (worldZone2 == null)
					{
						worldZone2 = worldZone3;
						goto IL_00C9;
					}
					goto IL_00C9;
				}
				else
				{
					if (worldZone == null)
					{
						worldZone = worldZone3;
						goto IL_00C9;
					}
					if (Vector3.Distance(worldZone3.Data.Center, this.playerData.position.Value) < Vector3.Distance(worldZone.Data.Center, this.playerData.position.Value))
					{
						worldZone = worldZone3;
						goto IL_00C9;
					}
					goto IL_00C9;
				}
			}
			IL_0112:
			i++;
			continue;
			IL_00C9:
			TownZone townZone2;
			if (townZone == null && collider.TryGetComponent<TownZone>(out townZone2))
			{
				townZone = townZone2;
			}
			TownSubZone townSubZone2;
			if (townSubZone == null && collider.TryGetComponent<TownSubZone>(out townSubZone2))
			{
				townSubZone = townSubZone2;
			}
			MapZone mapZone2;
			if (mapZone == null && collider.TryGetComponent<MapZone>(out mapZone2))
			{
				mapZone = mapZone2;
				goto IL_0112;
			}
			goto IL_0112;
		}
		if (worldZone == null && this.currentContainerWorldZone != null)
		{
			this.ExitZoneByType(PlayerPhysicalBody.ZoneColliderType.Container);
		}
		if (worldZone2 == null && this.currentNonContainerWorldZone != null)
		{
			this.ExitZoneByType(PlayerPhysicalBody.ZoneColliderType.NonContainer);
		}
		if (townZone == null && this.currentTownZone != null)
		{
			this.ExitZoneByType(PlayerPhysicalBody.ZoneColliderType.Town);
		}
		if (townSubZone == null && this.currentTownSubZone != null)
		{
			this.ExitZoneByType(PlayerPhysicalBody.ZoneColliderType.TownSubzone);
		}
		if (this.currentMapZone != null && mapZone != this.currentMapZone)
		{
			this.currentMapZone = null;
		}
		if (worldZone != null)
		{
			if (worldZone != this.currentContainerWorldZone || this.playerData.CurrentWorldZoneData != worldZone.Data)
			{
				this.ExitAllZones();
				this.EnterWorldZone(worldZone);
				this.currentContainerWorldZone = worldZone;
			}
		}
		else if (worldZone2 != null)
		{
			if (worldZone2 != this.currentNonContainerWorldZone || this.playerData.CurrentWorldZoneData != worldZone2.Data)
			{
				this.ExitAllZones();
				this.EnterWorldZone(worldZone2);
				this.currentNonContainerWorldZone = worldZone2;
			}
		}
		else if (townZone != null)
		{
			if (townZone != this.currentTownZone)
			{
				this.ExitAllZonesExcept(PlayerPhysicalBody.ZoneColliderType.NonContainer);
				this.playerData.AddTownZone(townZone);
				GUIElements.Instance.WorldZoneWidget.Draw(new WorldZoneWidgetData());
				this.currentTownZone = townZone;
			}
		}
		else if (townSubZone != null && townSubZone != this.currentTownSubZone)
		{
			this.ExitAllZonesExcept(PlayerPhysicalBody.ZoneColliderType.Town);
			this.playerData.AddTownSubZone(townSubZone);
			GUIElements.Instance.WorldZoneWidget.Draw(new WorldZoneWidgetData());
			this.currentTownSubZone = townSubZone;
		}
		if (mapZone != null && mapZone != this.currentMapZone)
		{
			if (!MainGame.Instance.GameSave.knowledgeSystem.IsCharTabLocked(CharacterWindowData.CharPage.Map) && !MainGame.Instance.GameSave.knowledgeSystem.knownMapZones.Contains(mapZone.Id))
			{
				MainGame.Instance.GameSave.knowledgeSystem.knownMapZones.Add(mapZone.Id);
			}
			this.currentMapZone = mapZone;
		}
	}

	// Token: 0x060018E4 RID: 6372 RVA: 0x00076190 File Offset: 0x00074390
	private void ExitZoneByType(PlayerPhysicalBody.ZoneColliderType zoneColliderType)
	{
		switch (zoneColliderType)
		{
		case PlayerPhysicalBody.ZoneColliderType.Container:
			if (this.currentContainerWorldZone != null)
			{
				this.ExitWorldZone(this.currentContainerWorldZone);
				this.currentContainerWorldZone = null;
				return;
			}
			break;
		case PlayerPhysicalBody.ZoneColliderType.NonContainer:
			if (this.currentNonContainerWorldZone != null)
			{
				this.ExitWorldZone(this.currentNonContainerWorldZone);
				this.currentNonContainerWorldZone = null;
				return;
			}
			break;
		case PlayerPhysicalBody.ZoneColliderType.TownSubzone:
			if (this.currentTownSubZone != null)
			{
				this.playerData.RemoveTownSubZone(this.currentTownSubZone);
				GUIElements.Instance.WorldZoneWidget.Draw(new WorldZoneWidgetData());
				this.currentTownSubZone = null;
				return;
			}
			break;
		case PlayerPhysicalBody.ZoneColliderType.Town:
			if (this.currentTownZone != null)
			{
				this.playerData.RemoveTownZone(this.currentTownZone);
				GUIElements.Instance.WorldZoneWidget.Draw(new WorldZoneWidgetData());
				this.currentTownZone = null;
			}
			break;
		default:
			return;
		}
	}

	// Token: 0x060018E5 RID: 6373 RVA: 0x00076274 File Offset: 0x00074474
	private void ExitAllZonesExcept(PlayerPhysicalBody.ZoneColliderType zoneColliderType)
	{
		foreach (KeyValuePair<PlayerPhysicalBody.ZoneColliderType, Action<PlayerPhysicalBody>> keyValuePair in this.exitZoneActions)
		{
			if (keyValuePair.Key != zoneColliderType)
			{
				keyValuePair.Value(this);
			}
		}
	}

	// Token: 0x060018E6 RID: 6374 RVA: 0x000762D8 File Offset: 0x000744D8
	private void ExitAllZones()
	{
		foreach (KeyValuePair<PlayerPhysicalBody.ZoneColliderType, Action<PlayerPhysicalBody>> keyValuePair in this.exitZoneActions)
		{
			keyValuePair.Value(this);
		}
	}

	// Token: 0x060018E7 RID: 6375 RVA: 0x00076334 File Offset: 0x00074534
	public PlayerPhysicalBody()
	{
		Dictionary<PlayerPhysicalBody.ZoneColliderType, Action<PlayerPhysicalBody>> dictionary = new Dictionary<PlayerPhysicalBody.ZoneColliderType, Action<PlayerPhysicalBody>>();
		dictionary.Add(PlayerPhysicalBody.ZoneColliderType.Container, delegate(PlayerPhysicalBody obj)
		{
			obj.ExitZoneByType(PlayerPhysicalBody.ZoneColliderType.Container);
		});
		dictionary.Add(PlayerPhysicalBody.ZoneColliderType.NonContainer, delegate(PlayerPhysicalBody obj)
		{
			obj.ExitZoneByType(PlayerPhysicalBody.ZoneColliderType.NonContainer);
		});
		dictionary.Add(PlayerPhysicalBody.ZoneColliderType.TownSubzone, delegate(PlayerPhysicalBody obj)
		{
			obj.ExitZoneByType(PlayerPhysicalBody.ZoneColliderType.TownSubzone);
		});
		dictionary.Add(PlayerPhysicalBody.ZoneColliderType.Town, delegate(PlayerPhysicalBody obj)
		{
			obj.ExitZoneByType(PlayerPhysicalBody.ZoneColliderType.Town);
		});
		this.exitZoneActions = dictionary;
		this.IsActiveCombatant = true;
		base..ctor();
	}

	// Token: 0x04001827 RID: 6183
	private const float WORLD_ZONE_RAYCAST_Y_SHIFT = 50f;

	// Token: 0x04001828 RID: 6184
	[SerializeField]
	private PlayerPhysicsConfig physicsConfig;

	// Token: 0x04001829 RID: 6185
	[Space]
	[SerializeField]
	private PlayerInvisibleWalls invisibleWalls;

	// Token: 0x0400182A RID: 6186
	[Space]
	[SerializeField]
	private Rigidbody rb;

	// Token: 0x0400182B RID: 6187
	[SerializeField]
	private MeshCollider meshCollider;

	// Token: 0x0400182C RID: 6188
	[SerializeField]
	private PlayerView playerView;

	// Token: 0x0400182D RID: 6189
	private bool hasContactPoints;

	// Token: 0x0400182E RID: 6190
	private bool isOnGround;

	// Token: 0x0400182F RID: 6191
	private bool shouldStickOnGround;

	// Token: 0x04001830 RID: 6192
	private bool isJumping;

	// Token: 0x04001831 RID: 6193
	private Dictionary<int, SerializableCollision> contactCollisions = new Dictionary<int, SerializableCollision>();

	// Token: 0x04001832 RID: 6194
	private List<SerializableContactPoint> contactPoints = new List<SerializableContactPoint>();

	// Token: 0x04001833 RID: 6195
	[SerializeField]
	private RVOController rvo;

	// Token: 0x04001834 RID: 6196
	private MultiFlagAND<PlayerDynamicType> dynamicMultiFlag = new MultiFlagAND<PlayerDynamicType>();

	// Token: 0x04001835 RID: 6197
	private PlayerData playerData;

	// Token: 0x04001836 RID: 6198
	private Vector3 prevPosition;

	// Token: 0x04001837 RID: 6199
	private Vector2 prevDirection;

	// Token: 0x04001838 RID: 6200
	private Vector3 gravityNormalized = Physics.gravity.normalized;

	// Token: 0x04001839 RID: 6201
	private bool hasPhysUpdatedThisFixedFrame;

	// Token: 0x0400183A RID: 6202
	private bool movedByDirectionThisFrame;

	// Token: 0x0400183B RID: 6203
	private static RaycastHit[] raycastResults = new RaycastHit[10];

	// Token: 0x0400183C RID: 6204
	private static RaycastHit[] raycastWZResults = new RaycastHit[15];

	// Token: 0x0400183D RID: 6205
	[SerializeField]
	private DamageEffectComponent damageEffectComponent;

	// Token: 0x0400183E RID: 6206
	[SerializeField]
	private int fightingQuality = 5;

	// Token: 0x0400183F RID: 6207
	private bool isMoving;

	// Token: 0x04001840 RID: 6208
	private bool isMovementLocked;

	// Token: 0x04001841 RID: 6209
	private bool isSetRotationLocked;

	// Token: 0x04001842 RID: 6210
	private bool hasAimDirection;

	// Token: 0x04001843 RID: 6211
	private Vector2 aimDirection;

	// Token: 0x04001844 RID: 6212
	[SerializeField]
	private WorldZone currentContainerWorldZone;

	// Token: 0x04001845 RID: 6213
	[SerializeField]
	private WorldZone currentNonContainerWorldZone;

	// Token: 0x04001846 RID: 6214
	[SerializeField]
	private TownZone currentTownZone;

	// Token: 0x04001847 RID: 6215
	[SerializeField]
	private TownSubZone currentTownSubZone;

	// Token: 0x04001848 RID: 6216
	[SerializeField]
	private MapZone currentMapZone;

	// Token: 0x0400184A RID: 6218
	private readonly Dictionary<PlayerPhysicalBody.ZoneColliderType, Action<PlayerPhysicalBody>> exitZoneActions;

	// Token: 0x0400184C RID: 6220
	private Coroutine muteCoroutine;

	// Token: 0x0200039A RID: 922
	public class GroundHit
	{
		// Token: 0x060018E9 RID: 6377 RVA: 0x00076451 File Offset: 0x00074651
		public GroundHit(Vector3 position, Vector3 normal)
		{
			this.position = position;
			this.normal = normal;
		}

		// Token: 0x0400184F RID: 6223
		public Vector3 position;

		// Token: 0x04001850 RID: 6224
		public Vector3 normal;
	}

	// Token: 0x0200039B RID: 923
	private enum ZoneColliderType
	{
		// Token: 0x04001852 RID: 6226
		Container,
		// Token: 0x04001853 RID: 6227
		NonContainer,
		// Token: 0x04001854 RID: 6228
		TownSubzone,
		// Token: 0x04001855 RID: 6229
		Town
	}
}
