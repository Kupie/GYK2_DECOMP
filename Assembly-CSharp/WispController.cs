using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using LinqTools;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

// Token: 0x020003C7 RID: 967
public class WispController : SerializedMonoBehaviour
{
	// Token: 0x17000478 RID: 1144
	// (get) Token: 0x060019E3 RID: 6627 RVA: 0x00079A6C File Offset: 0x00077C6C
	public Transform BubblePoint
	{
		get
		{
			return this.bubblePoint;
		}
	}

	// Token: 0x17000479 RID: 1145
	// (get) Token: 0x060019E4 RID: 6628 RVA: 0x00079A74 File Offset: 0x00077C74
	private List<ParticleSystem> ParticleSystems
	{
		get
		{
			if (this.particleSystems == null || this.particleSystems.Count == 0)
			{
				this.particleSystems = base.GetComponentsInChildren<ParticleSystem>(true).ToList<ParticleSystem>();
			}
			return this.particleSystems;
		}
	}

	// Token: 0x1700047A RID: 1146
	// (get) Token: 0x060019E5 RID: 6629 RVA: 0x00079AA3 File Offset: 0x00077CA3
	// (set) Token: 0x060019E6 RID: 6630 RVA: 0x00079AAB File Offset: 0x00077CAB
	public string Id
	{
		get
		{
			return this.id;
		}
		set
		{
			this.id = value;
		}
	}

	// Token: 0x1700047B RID: 1147
	// (get) Token: 0x060019E7 RID: 6631 RVA: 0x00079AB4 File Offset: 0x00077CB4
	public Transform TargetTransform
	{
		get
		{
			return this.targetTransform;
		}
	}

	// Token: 0x060019E8 RID: 6632 RVA: 0x00079ABC File Offset: 0x00077CBC
	public void ChangeTargetType(WispTargetType wispTargetType)
	{
		this.currentTargetType = wispTargetType;
	}

	// Token: 0x060019E9 RID: 6633 RVA: 0x00079AC5 File Offset: 0x00077CC5
	public void ChangeWispType(WispType wispType)
	{
		if (wispType != WispType.Normal)
		{
			if (wispType == WispType.Fairy)
			{
				this.wispAnimator.SetTrigger(WispController.FAIRY_APPEAR_TRIGEGR);
			}
		}
		else
		{
			this.wispAnimator.SetTrigger(WispController.FAIRY_DISAPPEAR_TRIGEGR);
		}
		this.wispType = wispType;
	}

	// Token: 0x060019EA RID: 6634 RVA: 0x00079AF7 File Offset: 0x00077CF7
	public void ChangeActiveState(bool enabled)
	{
		Debug.Log(string.Format("Wisp ChangeActiveState enabled:[{0}]", enabled));
		base.gameObject.SetActive(enabled);
	}

	// Token: 0x060019EB RID: 6635 RVA: 0x00023119 File Offset: 0x00021319
	public bool GetActiveState()
	{
		return base.gameObject.activeSelf;
	}

	// Token: 0x060019EC RID: 6636 RVA: 0x00079B1A File Offset: 0x00077D1A
	public void SetTargetTransform(Transform target)
	{
		this.targetTransform = target;
	}

	// Token: 0x060019ED RID: 6637 RVA: 0x00079B23 File Offset: 0x00077D23
	public void SetTargetVector(Vector3 target)
	{
		this.targetVector = target;
	}

	// Token: 0x060019EE RID: 6638 RVA: 0x00079B2C File Offset: 0x00077D2C
	public void SetTargetGDPoint(GDPointData gdPoint)
	{
		this.targetGDPoint = gdPoint;
	}

	// Token: 0x060019EF RID: 6639 RVA: 0x00079B35 File Offset: 0x00077D35
	public void SetTargetWgoData(WgoData wgoData)
	{
		this.targetWgoData = wgoData;
	}

	// Token: 0x060019F0 RID: 6640 RVA: 0x00079B40 File Offset: 0x00077D40
	public void TeleportToTarget(bool updateWgoData = true)
	{
		bool flag;
		Vector3 vector;
		this.DefineTarget(out flag, out vector);
		if (flag)
		{
			this.Teleport(vector, updateWgoData);
		}
	}

	// Token: 0x060019F1 RID: 6641 RVA: 0x00079B62 File Offset: 0x00077D62
	public void SetDirection(Direction direction)
	{
		this.GetWispWgoData().direction.Value = direction.ConvertToVector3();
	}

	// Token: 0x060019F2 RID: 6642 RVA: 0x00079B7F File Offset: 0x00077D7F
	[CanBeNull]
	public WgoData GetWispWgoData()
	{
		WgoData wgoDataForWisp = MainGame.WorldData.GetWgoDataForWisp(this);
		wgoDataForWisp.SetBubblePointOffset(this.bubblePoint.localPosition);
		return wgoDataForWisp;
	}

	// Token: 0x060019F3 RID: 6643 RVA: 0x00079BA0 File Offset: 0x00077DA0
	private void Teleport(Vector3 targetPos, bool updateWgoData = true)
	{
		foreach (ParticleSystem particleSystem in this.ParticleSystems)
		{
			particleSystem.Simulate(0f, true, true);
		}
		this.SetPosition(targetPos, updateWgoData);
		foreach (ParticleSystem particleSystem2 in this.ParticleSystems)
		{
			particleSystem2.Play();
		}
	}

	// Token: 0x060019F4 RID: 6644 RVA: 0x00079C40 File Offset: 0x00077E40
	private void FixedUpdate()
	{
		if (!MainGame.WorldData.HasCache)
		{
			return;
		}
		if (MainGame.IsGamePaused)
		{
			return;
		}
		bool flag;
		Vector3 vector;
		this.DefineTarget(out flag, out vector);
		if (flag)
		{
			float num = Vector3.Distance(base.transform.position, vector);
			if (num > this.teleportDistance)
			{
				this.bigDistanceTime += Time.deltaTime;
				if (this.bigDistanceTime >= this.bigDistanceTimeLimit)
				{
					this.Teleport(vector, true);
					return;
				}
			}
			else
			{
				this.bigDistanceTime = 0f;
			}
			if (num > this.safeDistanceToTarget)
			{
				Vector3 normalized = (vector - base.transform.position).normalized;
				float num2;
				if (num > this.defaultDistanceToTarget)
				{
					num2 = Mathf.Lerp(this.followSpeedMinInBigDistance, this.followSpeedMaxInBigDistance, num / this.bigDistanceToTarget);
				}
				else
				{
					num2 = Mathf.Lerp(this.followSpeedMinInDefaultDistance, this.followSpeedMaxInDefaultDistance, num / this.defaultDistanceToTarget);
				}
				Vector3 vector2 = normalized * num2 - this.rb.linearVelocity;
				vector2 = Vector3.ClampMagnitude(vector2, this.maxForce);
				this.rb.AddForce(vector2, ForceMode.VelocityChange);
			}
		}
		else
		{
			this.SetPosition(vector, true);
		}
		WispType wispType = this.wispType;
		if (wispType == WispType.Normal)
		{
			base.transform.localScale = Vector3.one;
			return;
		}
		if (wispType != WispType.Fairy)
		{
			throw new ArgumentOutOfRangeException();
		}
		if (Vector3.Distance(base.transform.position, vector) > 0.1f)
		{
			bool flag2 = base.transform.position.x <= vector.x;
			base.transform.localScale = (flag2 ? new Vector3(-1f, 1f, 1f) : Vector3.one);
			return;
		}
		WgoData wispWgoData = this.GetWispWgoData();
		if (wispWgoData != null)
		{
			bool flag3 = wispWgoData.Direction == Direction.Left;
			base.transform.localScale = (flag3 ? Vector3.one : new Vector3(-1f, 1f, 1f));
			return;
		}
	}

	// Token: 0x060019F5 RID: 6645 RVA: 0x00079E38 File Offset: 0x00078038
	private void SetPosition(Vector3 targetPos, bool updateWgoData = true)
	{
		base.transform.position = targetPos;
		this.rb.position = targetPos;
		if (updateWgoData && this.currentTargetType != WispTargetType.WgoData && MainGame.Instance.gameState == MainGame.GameState.InGame)
		{
			WgoData wispWgoData = this.GetWispWgoData();
			if (wispWgoData != null)
			{
				wispWgoData.Position = targetPos;
			}
		}
	}

	// Token: 0x060019F6 RID: 6646 RVA: 0x00079E88 File Offset: 0x00078088
	private void DefineTarget(out bool hasTarget, out Vector3 targetPos)
	{
		hasTarget = false;
		targetPos = this.animationObjectForPositionChange.localPosition;
		switch (this.currentTargetType)
		{
		case WispTargetType.Transform:
			if (this.targetTransform != null)
			{
				hasTarget = true;
				targetPos += this.targetTransform.position;
				return;
			}
			break;
		case WispTargetType.GDPoint:
			if (this.targetGDPoint != null)
			{
				hasTarget = true;
				targetPos += this.targetGDPoint.Position;
				return;
			}
			break;
		case WispTargetType.Vector:
			if (this.targetVector != default(Vector3))
			{
				hasTarget = true;
				targetPos += this.targetVector;
				return;
			}
			break;
		case WispTargetType.WgoData:
			if (this.targetWgoData != null)
			{
				hasTarget = true;
				targetPos += this.targetWgoData.Position;
				targetPos.y += this.yOffsetForWgoDataFollow;
			}
			break;
		default:
			return;
		}
	}

	// Token: 0x04001923 RID: 6435
	private static int FAIRY_APPEAR_TRIGEGR = Animator.StringToHash("fairy_appear");

	// Token: 0x04001924 RID: 6436
	private static int FAIRY_DISAPPEAR_TRIGEGR = Animator.StringToHash("fairy_disappear");

	// Token: 0x04001925 RID: 6437
	[SerializeField]
	private string id;

	// Token: 0x04001926 RID: 6438
	[SerializeField]
	private Animator wispAnimator;

	// Token: 0x04001927 RID: 6439
	[SerializeField]
	private Transform animationObjectForPositionChange;

	// Token: 0x04001928 RID: 6440
	[SerializeField]
	private Transform targetTransform;

	// Token: 0x04001929 RID: 6441
	[SerializeField]
	private Vector3 targetVector;

	// Token: 0x0400192A RID: 6442
	[SerializeField]
	private GDPointData targetGDPoint;

	// Token: 0x0400192B RID: 6443
	[SerializeField]
	private WgoData targetWgoData;

	// Token: 0x0400192C RID: 6444
	[SerializeField]
	private Rigidbody rb;

	// Token: 0x0400192D RID: 6445
	[SerializeField]
	private float safeDistanceToTarget;

	// Token: 0x0400192E RID: 6446
	[SerializeField]
	private float defaultDistanceToTarget;

	// Token: 0x0400192F RID: 6447
	[SerializeField]
	private float bigDistanceToTarget;

	// Token: 0x04001930 RID: 6448
	[SerializeField]
	private float teleportDistance = 5f;

	// Token: 0x04001931 RID: 6449
	[SerializeField]
	private float followSpeedMinInDefaultDistance;

	// Token: 0x04001932 RID: 6450
	[SerializeField]
	private float followSpeedMaxInDefaultDistance;

	// Token: 0x04001933 RID: 6451
	[SerializeField]
	private float followSpeedMinInBigDistance;

	// Token: 0x04001934 RID: 6452
	[SerializeField]
	private float followSpeedMaxInBigDistance;

	// Token: 0x04001935 RID: 6453
	[SerializeField]
	private float maxForce;

	// Token: 0x04001936 RID: 6454
	[SerializeField]
	private float bigDistanceTimeLimit;

	// Token: 0x04001937 RID: 6455
	[SerializeField]
	private float yOffsetForWgoDataFollow = 1f;

	// Token: 0x04001938 RID: 6456
	[SerializeField]
	private Transform bubblePoint;

	// Token: 0x04001939 RID: 6457
	private List<ParticleSystem> particleSystems;

	// Token: 0x0400193A RID: 6458
	[OdinSerialize]
	private Dictionary<WispType, WispTypedData> wispTypedDatas = new Dictionary<WispType, WispTypedData>();

	// Token: 0x0400193B RID: 6459
	private WispTargetType currentTargetType;

	// Token: 0x0400193C RID: 6460
	private WispType wispType;

	// Token: 0x0400193D RID: 6461
	private float bigDistanceTime;
}
