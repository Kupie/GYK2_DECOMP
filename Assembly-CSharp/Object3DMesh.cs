using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using LazyBearTechnology;
using LinqTools;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x02000511 RID: 1297
[ExecuteInEditMode]
public class Object3DMesh : MonoBehaviour
{
	// Token: 0x17000579 RID: 1401
	// (get) Token: 0x0600217F RID: 8575 RVA: 0x0009DD19 File Offset: 0x0009BF19
	public int SubMeshCount
	{
		get
		{
			return this.subMeshes.Count;
		}
	}

	// Token: 0x06002180 RID: 8576 RVA: 0x0009DD26 File Offset: 0x0009BF26
	public int GetSubMeshIndex(Object3DSubMesh subMesh)
	{
		return this.subMeshes.IndexOf(subMesh);
	}

	// Token: 0x1700057A RID: 1402
	// (get) Token: 0x06002181 RID: 8577 RVA: 0x0009DD34 File Offset: 0x0009BF34
	public Renderer ObjRenderer
	{
		get
		{
			if (this.objRenderer == null || this.objRenderer.gameObject == null)
			{
				this.objRenderer = base.GetComponent<Renderer>();
			}
			return this.objRenderer;
		}
	}

	// Token: 0x1700057B RID: 1403
	// (get) Token: 0x06002182 RID: 8578 RVA: 0x0009DD69 File Offset: 0x0009BF69
	public bool IsATree
	{
		get
		{
			return this.subMeshes.Any((Object3DSubMesh mesh) => mesh.Type == Object3DSubMesh.MeshType.TreeCrown);
		}
	}

	// Token: 0x1700057C RID: 1404
	// (get) Token: 0x06002183 RID: 8579 RVA: 0x0009DD95 File Offset: 0x0009BF95
	public Color SelectionTintColor
	{
		get
		{
			return this.selectionTintColor;
		}
	}

	// Token: 0x1700057D RID: 1405
	// (get) Token: 0x06002184 RID: 8580 RVA: 0x0009DD9D File Offset: 0x0009BF9D
	public float SelectionTintAmount
	{
		get
		{
			return this.selectionTintAmount;
		}
	}

	// Token: 0x06002185 RID: 8581 RVA: 0x0009DDA5 File Offset: 0x0009BFA5
	public void CustomInit()
	{
		this.InitSubMeshes();
		this.ApplyMaterials();
	}

	// Token: 0x06002186 RID: 8582 RVA: 0x0009DDB3 File Offset: 0x0009BFB3
	private void Start()
	{
		this.TryInit();
	}

	// Token: 0x06002187 RID: 8583 RVA: 0x0009DDBB File Offset: 0x0009BFBB
	private void TryInit()
	{
		if (!this.initialized)
		{
			this.InitSubMeshes();
			this.ApplyMaterials();
			this.initialized = true;
		}
	}

	// Token: 0x06002188 RID: 8584 RVA: 0x0009DDD8 File Offset: 0x0009BFD8
	private void InitSubMeshes()
	{
		for (int i = 0; i < this.subMeshes.Count<Object3DSubMesh>(); i++)
		{
			this.subMeshes[i].Init(this, this.ObjRenderer, i);
		}
	}

	// Token: 0x06002189 RID: 8585 RVA: 0x0009DE14 File Offset: 0x0009C014
	public void SetSelectionTint(Color color, float amount)
	{
		this.selectionTintColor = color;
		this.selectionTintAmount = amount;
		for (int i = 0; i < this.subMeshes.Count; i++)
		{
			Object3DSubMesh object3DSubMesh = this.subMeshes[i];
			if (object3DSubMesh != null)
			{
				object3DSubMesh.SetSelectionTint(color, amount);
			}
		}
	}

	// Token: 0x0600218A RID: 8586 RVA: 0x0009DE60 File Offset: 0x0009C060
	public void ApplyMaterials()
	{
		for (int i = 0; i < this.subMeshes.Count; i++)
		{
			this.subMeshes[i].ApplyMaterial();
		}
	}

	// Token: 0x0600218B RID: 8587 RVA: 0x0009DE94 File Offset: 0x0009C094
	private void Editor_PlayAnimationChop()
	{
		Object3D componentInParent = base.GetComponentInParent<Object3D>();
		if (componentInParent != null)
		{
			using (List<Object3DMesh>.Enumerator enumerator = componentInParent.Object3DMeshes.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Object3DMesh object3DMesh = enumerator.Current;
					object3DMesh.PlayAnimationChop(null);
				}
				return;
			}
		}
		this.PlayAnimationChop(null);
	}

	// Token: 0x0600218C RID: 8588 RVA: 0x0009DEFC File Offset: 0x0009C0FC
	public void PlayAnimationChop(Action onAction)
	{
		if (this.tweenDestruct != null)
		{
			return;
		}
		Tween tween = this.tweenChop;
		if (tween != null)
		{
			tween.Kill(false);
		}
		this.isTreeDestructing = true;
		this.treeChoppingPhase = (this.treeDestructionPhase = 0f);
		this.tweenChop = DOTween.To(() => this.treeChoppingPhase, delegate(float a)
		{
			this.treeChoppingPhase = a;
		}, 1f, LazySingletonSO<GlobalResources>.Instance.fxSettings.treeChopAnimLen).OnUpdate(new TweenCallback(this.ApplyMaterials)).OnComplete(delegate
		{
			this.treeChoppingPhase = 0f;
			if ((double)this.treeDestructionPhase < 0.01)
			{
				this.isTreeDestructing = false;
			}
			this.tweenChop = null;
			this.ApplyMaterials();
		});
		if (onAction != null)
		{
			onAction();
		}
	}

	// Token: 0x0600218D RID: 8589 RVA: 0x0009DFA4 File Offset: 0x0009C1A4
	private void Editor_PlayAnimationDestruction()
	{
		Object3D componentInParent = base.GetComponentInParent<Object3D>();
		if (componentInParent != null)
		{
			using (List<Object3DMesh>.Enumerator enumerator = componentInParent.Object3DMeshes.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Object3DMesh object3DMesh = enumerator.Current;
					object3DMesh.PlayAnimationDestruction(null, true);
				}
				return;
			}
		}
		this.PlayAnimationDestruction(null, true);
	}

	// Token: 0x0600218E RID: 8590 RVA: 0x0009E010 File Offset: 0x0009C210
	public void PlayAnimationDestruction(Action onAction, bool restoreOnComplete = false)
	{
		this.CancelDestructionActionTimer();
		Tween tween = this.tweenChop;
		if (tween != null)
		{
			tween.Kill(false);
		}
		this.tweenChop = null;
		if (this.tweenDestruct != null)
		{
			Object3DMesh.tweensDestruct.Remove(this.tweenDestruct);
			this.tweenDestruct.Kill(false);
			this.tweenDestruct = null;
		}
		this.isTreeDestructing = true;
		this.treeChoppingPhase = (this.treeDestructionPhase = 0f);
		this.tweenDestruct = DOTween.To(() => this.treeDestructionPhase, delegate(float a)
		{
			this.treeDestructionPhase = a;
		}, 1f, LazySingletonSO<GlobalResources>.Instance.fxSettings.GetTreeDestroyAnimLen(this.treeHeight)).OnUpdate(new TweenCallback(this.ApplyMaterials)).OnComplete(delegate
		{
			Object3DMesh.tweensDestruct.Remove(this.tweenDestruct);
			this.tweenDestruct = null;
			if (restoreOnComplete)
			{
				this.treeDestructionPhase = 0f;
			}
			this.ApplyMaterials();
		});
		Object3DMesh.tweensDestruct.Add(this.tweenDestruct);
		if (onAction != null)
		{
			this.destructionActionTimerId = LazyTimer.AddTimer(LazySingletonSO<GlobalResources>.Instance.fxSettings.treeDestroyActionTime, onAction, null);
		}
	}

	// Token: 0x0600218F RID: 8591 RVA: 0x0009E126 File Offset: 0x0009C326
	private void CancelDestructionActionTimer()
	{
		if (this.destructionActionTimerId == -1)
		{
			return;
		}
		LazyTimer.Stop(this.destructionActionTimerId);
		this.destructionActionTimerId = -1;
	}

	// Token: 0x06002190 RID: 8592 RVA: 0x0009E148 File Offset: 0x0009C348
	public static void SetDestructionTweenPauseState(bool isPaused)
	{
		foreach (Tween tween in Object3DMesh.tweensDestruct)
		{
			if (tween != null)
			{
				if (isPaused)
				{
					tween.Pause<Tween>();
				}
				else
				{
					tween.Play<Tween>();
				}
			}
		}
	}

	// Token: 0x06002191 RID: 8593 RVA: 0x0009E1AC File Offset: 0x0009C3AC
	public void ResetAnimValues()
	{
		this.isTreeDestructing = false;
		this.treeChoppingPhase = (this.treeDestructionPhase = 0f);
		this.ApplyMaterials();
	}

	// Token: 0x06002192 RID: 8594 RVA: 0x0009E1DA File Offset: 0x0009C3DA
	public void ReplaceBlueToColor()
	{
		this.isBlueReplacingToColor = true;
		this.ApplyMaterials();
	}

	// Token: 0x06002193 RID: 8595 RVA: 0x0009E1E9 File Offset: 0x0009C3E9
	public void ReplaceBlueToTransparent()
	{
		this.isBlueReplacingToColor = false;
		this.ApplyMaterials();
	}

	// Token: 0x06002194 RID: 8596 RVA: 0x0009E1F8 File Offset: 0x0009C3F8
	public void SetLutTexture(Texture2D texture)
	{
		foreach (Object3DSubMesh object3DSubMesh in this.subMeshes)
		{
			object3DSubMesh.SetLutTexture(texture);
		}
	}

	// Token: 0x06002195 RID: 8597 RVA: 0x0009E24C File Offset: 0x0009C44C
	public Object3DSubMesh GetSubMesh(int index)
	{
		if (index < 0 || index >= this.subMeshes.Count)
		{
			return null;
		}
		return this.subMeshes[index];
	}

	// Token: 0x06002196 RID: 8598 RVA: 0x0009E26E File Offset: 0x0009C46E
	public void SetTransparency(float transparencyValue)
	{
		this.transparencyValue = transparencyValue;
		this.ApplyMaterials();
	}

	// Token: 0x06002197 RID: 8599 RVA: 0x0009E280 File Offset: 0x0009C480
	private void OnDestroy()
	{
		GlobalResources.TryReleaseTransparentMaterialFor(this);
		this.CancelDestructionActionTimer();
		Tween tween = this.tweenChop;
		if (tween != null)
		{
			tween.Kill(false);
		}
		this.tweenChop = null;
		if (this.tweenDestruct != null)
		{
			Object3DMesh.tweensDestruct.Remove(this.tweenDestruct);
			this.tweenDestruct.Kill(false);
			this.tweenDestruct = null;
		}
	}

	// Token: 0x04001E14 RID: 7700
	public static readonly Color replaceBlueColor = new Color(1f, 1f, 0f);

	// Token: 0x04001E15 RID: 7701
	public static readonly Color replaceBlueTransparent = new Color(1f, 1f, 0f, 0f);

	// Token: 0x04001E16 RID: 7702
	public static readonly int matIdMainTexture = Shader.PropertyToID("_MainTex");

	// Token: 0x04001E17 RID: 7703
	public static readonly int matIdLutTexture = Shader.PropertyToID("_ReplaceLUT");

	// Token: 0x04001E18 RID: 7704
	public static readonly int matIdMainTexture2 = Shader.PropertyToID("_MainTex2");

	// Token: 0x04001E19 RID: 7705
	public static readonly int matIdNormalTexture = Shader.PropertyToID("_NormalMap");

	// Token: 0x04001E1A RID: 7706
	public static readonly int matIdUseNormalMap = Shader.PropertyToID("_UseNormalMap");

	// Token: 0x04001E1B RID: 7707
	public static readonly int matIdOpaqueShadowMesh = Shader.PropertyToID("_OpaqueShadowMesh");

	// Token: 0x04001E1C RID: 7708
	public static readonly int matIdIsTreeCrown = Shader.PropertyToID("_IsTreeCrown");

	// Token: 0x04001E1D RID: 7709
	public static readonly int matIdTreeChopAmplitude = Shader.PropertyToID("_TreeChopAmplitude");

	// Token: 0x04001E1E RID: 7710
	public static readonly int matIdUseCrownDeformOnly = Shader.PropertyToID("_UseCrownDeformOnly");

	// Token: 0x04001E1F RID: 7711
	public static readonly int matIdIsUnlit = Shader.PropertyToID("_Unlit");

	// Token: 0x04001E20 RID: 7712
	public static readonly int matIdReplaceBlue = Shader.PropertyToID("_ReplaceBlue");

	// Token: 0x04001E21 RID: 7713
	public static readonly int matIdReplaceBlueColor = Shader.PropertyToID("_ReplaceBlueColor");

	// Token: 0x04001E22 RID: 7714
	public static readonly int matIdAlphaCutoff = Shader.PropertyToID("_Cutoff");

	// Token: 0x04001E23 RID: 7715
	public static readonly int matIdTransparencyOcclusionValue = Shader.PropertyToID("_TransparencyOcclusionValue");

	// Token: 0x04001E24 RID: 7716
	public static readonly int matIdWorldWindEnabled = Shader.PropertyToID("_WorldWindEnabled");

	// Token: 0x04001E25 RID: 7717
	public static readonly int matIdSelectionTintColor = Shader.PropertyToID("_SelectionTintColor");

	// Token: 0x04001E26 RID: 7718
	public static readonly int matIdSelectionTintAmount = Shader.PropertyToID("_SelectionTintAmount");

	// Token: 0x04001E27 RID: 7719
	[FormerlySerializedAs("atoms")]
	[SerializeField]
	private List<Object3DSubMesh> subMeshes = new List<Object3DSubMesh>
	{
		new Object3DSubMesh()
	};

	// Token: 0x04001E28 RID: 7720
	[NonSerialized]
	public bool isTreeDestructing;

	// Token: 0x04001E29 RID: 7721
	[NonSerialized]
	public float treeDestructionPhase;

	// Token: 0x04001E2A RID: 7722
	[NonSerialized]
	public float treeChoppingPhase;

	// Token: 0x04001E2B RID: 7723
	[NonSerialized]
	public bool isBlueReplacingToColor;

	// Token: 0x04001E2C RID: 7724
	[NonSerialized]
	public float transparencyValue;

	// Token: 0x04001E2D RID: 7725
	[NonSerialized]
	private Color selectionTintColor = Color.white;

	// Token: 0x04001E2E RID: 7726
	[NonSerialized]
	private float selectionTintAmount;

	// Token: 0x04001E2F RID: 7727
	private Tween tweenChop;

	// Token: 0x04001E30 RID: 7728
	private Tween tweenDestruct;

	// Token: 0x04001E31 RID: 7729
	private static HashSet<Tween> tweensDestruct = new HashSet<Tween>();

	// Token: 0x04001E32 RID: 7730
	private int destructionActionTimerId = -1;

	// Token: 0x04001E33 RID: 7731
	[Range(0f, 10f)]
	public float treeHeight = 4f;

	// Token: 0x04001E34 RID: 7732
	private Renderer objRenderer;

	// Token: 0x04001E35 RID: 7733
	private bool initialized;
}
