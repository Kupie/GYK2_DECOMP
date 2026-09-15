using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000197 RID: 407
public class DropViewAtomMesh : DropViewAtomBase
{
	// Token: 0x170001A3 RID: 419
	// (get) Token: 0x06000A54 RID: 2644 RVA: 0x000342DE File Offset: 0x000324DE
	public DropViewAtomMeshElement MeshElement
	{
		get
		{
			return this.displayedElement;
		}
	}

	// Token: 0x06000A55 RID: 2645 RVA: 0x000342E8 File Offset: 0x000324E8
	public override void Activate(string iconId)
	{
		this.zombieOverheadContainer.gameObject.SetActive(false);
		this.zombieDropContainer.gameObject.SetActive(false);
		DropViewAtomMeshElement dropViewAtomMeshElement = null;
		this.defaultElement.gameObject.SetActive(false);
		this.boxElement.gameObject.SetActive(false);
		foreach (DropViewAtomMeshElement dropViewAtomMeshElement2 in this.elements)
		{
			if (dropViewAtomMeshElement2.name == iconId)
			{
				dropViewAtomMeshElement = dropViewAtomMeshElement2;
			}
			dropViewAtomMeshElement2.gameObject.SetActive(false);
		}
		if (dropViewAtomMeshElement == null)
		{
			dropViewAtomMeshElement = this.defaultElement;
			Debug.LogError("Not found element for item icon id: " + iconId + ", using placeholder");
		}
		if (dropViewAtomMeshElement != null)
		{
			this.displayedElement = dropViewAtomMeshElement;
			this.displayedElement.gameObject.SetActive(true);
		}
		base.Activate(iconId);
	}

	// Token: 0x06000A56 RID: 2646 RVA: 0x000343E4 File Offset: 0x000325E4
	public override SpriteText GetSpriteText()
	{
		return this.displayedElement.spriteText;
	}

	// Token: 0x06000A57 RID: 2647 RVA: 0x000343F1 File Offset: 0x000325F1
	public void ActivateZombie(string iconId, Texture2D lutTexture)
	{
		if (this.isOverhead)
		{
			this.ActivateZombieOverhead(iconId, lutTexture);
			return;
		}
		this.ActivateZombieDrop(iconId, lutTexture);
	}

	// Token: 0x06000A58 RID: 2648 RVA: 0x0003440C File Offset: 0x0003260C
	private void ActivateZombieOverhead(string iconId, Texture2D lutTexture)
	{
		this.ActivateZombie(1, iconId, lutTexture, this.zombieWorkerOverheadPrefabs);
	}

	// Token: 0x06000A59 RID: 2649 RVA: 0x0003441D File Offset: 0x0003261D
	private void ActivateZombieDrop(string iconId, Texture2D lutTexture)
	{
		this.ActivateZombie(2, iconId, lutTexture, this.zombieWorkerDropPrefabs);
	}

	// Token: 0x06000A5A RID: 2650 RVA: 0x00034430 File Offset: 0x00032630
	private void ActivateZombie(int poseId, string iconId, Texture2D lutTexture, List<DropViewAtomMeshElement> sourcePrefabs)
	{
		this.zombieOverheadContainer.gameObject.SetActive(false);
		this.zombieDropContainer.gameObject.SetActive(false);
		DropViewAtomMeshElement dropViewAtomMeshElement = null;
		string text = string.Format("drops_item_zombie_pose{0}_t{1}", poseId, iconId);
		foreach (DropViewAtomMeshElement dropViewAtomMeshElement2 in sourcePrefabs)
		{
			if (dropViewAtomMeshElement2.name == text)
			{
				dropViewAtomMeshElement = dropViewAtomMeshElement2;
				break;
			}
		}
		if (dropViewAtomMeshElement != null)
		{
			foreach (DropViewAtomMeshElement dropViewAtomMeshElement3 in this.elements)
			{
				dropViewAtomMeshElement3.gameObject.SetActive(false);
			}
			this.boxElement.gameObject.SetActive(false);
			this.defaultElement.gameObject.SetActive(false);
			if (this.lastDisplayedZombieElement != null)
			{
				this.lastDisplayedZombieElement.gameObject.SetActive(false);
			}
			DropViewAtomMeshZombieContainer dropViewAtomMeshZombieContainer = ((poseId == 1) ? this.zombieOverheadContainer : this.zombieDropContainer);
			Dictionary<string, DropViewAtomMeshElement> dictionary = ((poseId == 1) ? this.cachedZombieOverheadElements : this.cachedZombieDropElements);
			dropViewAtomMeshZombieContainer.gameObject.SetActive(true);
			this.lastDisplayedZombieElement = this.GetCachedZombieElement(text, dropViewAtomMeshElement, dropViewAtomMeshZombieContainer, dictionary);
			this.lastDisplayedZombieElement.transform.localPosition = Vector3.zero;
			this.displayedElement = this.lastDisplayedZombieElement;
			this.lastDisplayedZombieElement.spriteText = dropViewAtomMeshZombieContainer.spriteText;
			this.lastDisplayedZombieElement.physicsCollider = ((poseId == 1) ? this.zombieOverheadCollider : this.zombieDropCollider);
			this.displayedElement.gameObject.SetActive(true);
			this.ApplyLutTexture(this.displayedElement, lutTexture);
		}
		base.Activate(iconId);
	}

	// Token: 0x06000A5B RID: 2651 RVA: 0x00034610 File Offset: 0x00032810
	private DropViewAtomMeshElement GetCachedZombieElement(string zombieWorkerPrefabName, DropViewAtomMeshElement prefab, DropViewAtomMeshZombieContainer container, Dictionary<string, DropViewAtomMeshElement> cache)
	{
		DropViewAtomMeshElement dropViewAtomMeshElement;
		if (!cache.TryGetValue(zombieWorkerPrefabName, out dropViewAtomMeshElement) || dropViewAtomMeshElement == null)
		{
			dropViewAtomMeshElement = global::UnityEngine.Object.Instantiate<DropViewAtomMeshElement>(prefab, container.container, true);
			dropViewAtomMeshElement.name = zombieWorkerPrefabName;
			cache[zombieWorkerPrefabName] = dropViewAtomMeshElement;
		}
		return dropViewAtomMeshElement;
	}

	// Token: 0x06000A5C RID: 2652 RVA: 0x00034654 File Offset: 0x00032854
	public override void Deactivate()
	{
		this.SetInteractionState(false);
		if (this.displayedElement != null)
		{
			this.displayedElement.gameObject.SetActive(false);
		}
		if (this.lastDisplayedZombieElement != null)
		{
			this.lastDisplayedZombieElement.gameObject.SetActive(false);
		}
		this.displayedElement = null;
		this.lastDisplayedZombieElement = null;
		base.Deactivate();
	}

	// Token: 0x06000A5D RID: 2653 RVA: 0x000346BC File Offset: 0x000328BC
	public override void SetInteractionState(bool isUnderInteraction)
	{
		if (this.displayedElement == null || this.displayedElement.object3D == null)
		{
			return;
		}
		foreach (Object3DMesh object3DMesh in this.displayedElement.object3D.Object3DMeshes)
		{
			if (isUnderInteraction)
			{
				object3DMesh.ReplaceBlueToColor();
			}
			else
			{
				object3DMesh.ReplaceBlueToTransparent();
			}
		}
	}

	// Token: 0x06000A5E RID: 2654 RVA: 0x00034748 File Offset: 0x00032948
	private void ApplyLutTexture(DropViewAtomMeshElement element, Texture2D lutTexture)
	{
		if (element == null)
		{
			return;
		}
		string text = ((lutTexture != null) ? lutTexture.name : "null");
		if (element.object3D == null)
		{
			Debug.LogWarning(string.Concat(new string[] { "DropViewAtomMesh: Can't apply LUT [", text, "] to [", element.name, "], object3D is missing" }));
			return;
		}
		if (element.object3D.Object3DMeshes == null || element.object3D.Object3DMeshes.Count == 0)
		{
			Debug.LogWarning(string.Concat(new string[] { "DropViewAtomMesh: Can't apply LUT [", text, "] to [", element.name, "], Object3DMeshes is empty" }));
			return;
		}
		foreach (Object3DMesh object3DMesh in element.object3D.Object3DMeshes)
		{
			if (object3DMesh == null)
			{
				Debug.LogWarning(string.Concat(new string[] { "DropViewAtomMesh: Can't apply LUT [", text, "] to [", element.name, "], Object3DMesh is null" }));
			}
			else
			{
				object3DMesh.CustomInit();
				object3DMesh.SetLutTexture(lutTexture);
			}
		}
	}

	// Token: 0x04000BBB RID: 3003
	[SerializeField]
	private List<DropViewAtomMeshElement> elements = new List<DropViewAtomMeshElement>();

	// Token: 0x04000BBC RID: 3004
	[SerializeField]
	private DropViewAtomMeshElement defaultElement;

	// Token: 0x04000BBD RID: 3005
	[SerializeField]
	[Space]
	private DropViewAtomMeshZombieContainer zombieOverheadContainer;

	// Token: 0x04000BBE RID: 3006
	[SerializeField]
	private DropViewAtomMeshZombieContainer zombieDropContainer;

	// Token: 0x04000BBF RID: 3007
	[SerializeField]
	[Space]
	private Collider zombieOverheadCollider;

	// Token: 0x04000BC0 RID: 3008
	[SerializeField]
	private Collider zombieDropCollider;

	// Token: 0x04000BC1 RID: 3009
	[SerializeField]
	private List<DropViewAtomMeshElement> zombieWorkerOverheadPrefabs;

	// Token: 0x04000BC2 RID: 3010
	[SerializeField]
	private List<DropViewAtomMeshElement> zombieWorkerDropPrefabs;

	// Token: 0x04000BC3 RID: 3011
	[SerializeField]
	private DropViewAtomMeshElement boxElement;

	// Token: 0x04000BC4 RID: 3012
	[SerializeField]
	private SpriteRenderer boxSpriteRenderer;

	// Token: 0x04000BC5 RID: 3013
	[SerializeField]
	public bool isOverhead;

	// Token: 0x04000BC6 RID: 3014
	private DropViewAtomMeshElement displayedElement;

	// Token: 0x04000BC7 RID: 3015
	private DropViewAtomMeshElement lastDisplayedZombieElement;

	// Token: 0x04000BC8 RID: 3016
	private readonly Dictionary<string, DropViewAtomMeshElement> cachedZombieOverheadElements = new Dictionary<string, DropViewAtomMeshElement>();

	// Token: 0x04000BC9 RID: 3017
	private readonly Dictionary<string, DropViewAtomMeshElement> cachedZombieDropElements = new Dictionary<string, DropViewAtomMeshElement>();

	// Token: 0x04000BCA RID: 3018
	private Material boxElementMat;

	// Token: 0x04000BCB RID: 3019
	private static readonly int replaceBlueColor = Shader.PropertyToID("_ReplaceBlueColor");
}
