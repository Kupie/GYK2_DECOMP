using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000159 RID: 345
public class ConveyorBuildPointer : WgoBuildPointer
{
	// Token: 0x06000844 RID: 2116 RVA: 0x000284EC File Offset: 0x000266EC
	public override bool TryDoBuildAction()
	{
		if (!this.shownAsActive)
		{
			return false;
		}
		Action takeResourcesAction = this.takeResourcesAction;
		if (takeResourcesAction != null)
		{
			takeResourcesAction();
		}
		WGODef wgodef;
		if (GameBalance.Me.conveyorWgosCache.TryGetValue(this.buildData.WgoId, out wgodef))
		{
			WgoData wgoData = new ConveyorWgoData(wgodef.conveyorType, this.buildData.WgoId, this.target.Data.Position, this.gameScene.Id);
			if (this.target.CanBeRotated() && this.target.MainWgoPart.WgoPartData.rotationIndex != -1)
			{
				wgoData.MainWgoPartData.variationId = this.target.MainWgoPart.WgoPartData.variationId;
				wgoData.MainWgoPartData.rotationIndex = this.target.MainWgoPart.WgoPartData.rotationIndex;
			}
			Wgo wgo = this.gameScene.AddWgoData(wgoData, false);
			if (this.buildData.Definition != null)
			{
				foreach (LazyExpression lazyExpression in this.buildData.Definition.expressionAfterBuilding)
				{
					lazyExpression.EvaluateBool(wgo.Data);
				}
			}
			this.MakeConnections(wgo);
			this.TryMakeAutoBuilds(wgo);
			return true;
		}
		Debug.LogError("ConveyorBuildPointer could not find wgo def [{buildData.WgoId}]");
		return false;
	}

	// Token: 0x06000845 RID: 2117 RVA: 0x0002865C File Offset: 0x0002685C
	public void MakeConnections(Wgo builtWgo)
	{
		Physics.SyncTransforms();
		ConveyorWgoData conveyorWgoData = builtWgo.Data as ConveyorWgoData;
		if (conveyorWgoData == null)
		{
			return;
		}
		ConnectivityRestriction connectivityRestriction = null;
		Collider[] array = new Collider[10];
		if (Physics.OverlapBoxNonAlloc(builtWgo.transform.position, builtWgo.MainWgoPart.WgoPartData.BakedData.ChunkBounds.withoutShadows.extents / 4f, array, Quaternion.identity) > 0)
		{
			foreach (Collider collider in array)
			{
				if (!(collider == null))
				{
					Wgo componentInParent = collider.GetComponentInParent<Wgo>();
					ConnectivityRestriction connectivityRestriction2;
					if ((!(componentInParent != null) || (!componentInParent.Data.isTempObject && !(componentInParent == builtWgo))) && collider.TryGetComponent<ConnectivityRestriction>(out connectivityRestriction2))
					{
						connectivityRestriction = connectivityRestriction2;
						break;
					}
				}
			}
		}
		BuildConnector[] componentsInChildren = builtWgo.GetComponentsInChildren<BuildConnector>();
		for (int j = 0; j < componentsInChildren.Length; j++)
		{
			if (componentsInChildren[j].gameObject.activeInHierarchy)
			{
				Collider[] array3 = new Collider[10];
				if (Physics.OverlapBoxNonAlloc(componentsInChildren[j].BoxCollider.bounds.center, componentsInChildren[j].BoxCollider.bounds.extents / 2f, array3, Quaternion.identity, 524288) > 0)
				{
					foreach (Collider collider2 in array3)
					{
						if (!(collider2 == null))
						{
							Wgo componentInParent2 = collider2.GetComponentInParent<Wgo>();
							if ((!(componentInParent2 != null) || (!componentInParent2.Data.isTempObject && !(componentInParent2 == builtWgo))) && componentInParent2 != null && componentInParent2.Data is ConveyorWgoData)
							{
								if (componentInParent2.Data.Definition.conveyorType != ConveyorElementType.UndergroundCell)
								{
									componentsInChildren[j].TryConnect(componentInParent2);
									break;
								}
								ConveyorCellSequenceIdentifier conveyorCellSequenceIdentifier;
								if ((builtWgo.Data.Definition.conveyorType == ConveyorElementType.UndergroundCell || !(connectivityRestriction != null) || !connectivityRestriction.disabledTypes.Contains(componentInParent2.Data.Definition.conveyorType)) && collider2.TryGetComponent<ConveyorCellSequenceIdentifier>(out conveyorCellSequenceIdentifier) && conveyorCellSequenceIdentifier.isStartElement)
								{
									componentsInChildren[j].TryConnect(componentInParent2);
									break;
								}
							}
						}
					}
				}
			}
		}
		BoxCollider[] componentsInChildren2 = builtWgo.GetComponentsInChildren<BoxCollider>();
		List<BoxCollider> list = new List<BoxCollider>();
		for (int k = 0; k < componentsInChildren2.Length; k++)
		{
			if (componentsInChildren2[k].gameObject.layer == 19)
			{
				list.Add(componentsInChildren2[k]);
			}
		}
		if (list.Count == 0)
		{
			return;
		}
		bool flag = builtWgo.Data.Definition.conveyorType == ConveyorElementType.UndergroundCell;
		foreach (BoxCollider boxCollider in list)
		{
			ConveyorCellSequenceIdentifier conveyorCellSequenceIdentifier2;
			if (!flag || (boxCollider.TryGetComponent<ConveyorCellSequenceIdentifier>(out conveyorCellSequenceIdentifier2) && conveyorCellSequenceIdentifier2.isStartElement))
			{
				Collider[] array4 = new Collider[20];
				Vector3 vector = boxCollider.bounds.extents / 2f;
				vector.y = 0.2f;
				if (Physics.OverlapBoxNonAlloc(boxCollider.bounds.center, vector, array4, Quaternion.identity, 134217728) > 0)
				{
					for (int l = 0; l < array4.Length; l++)
					{
						if (array4[l] != null)
						{
							Wgo componentInParent3 = array4[l].GetComponentInParent<Wgo>();
							if (!(componentInParent3 != null) || (!componentInParent3.Data.isTempObject && !(componentInParent3 == builtWgo)))
							{
								BuildConnector component = array4[l].GetComponent<BuildConnector>();
								if (!(component == null))
								{
									component.TryConnect(builtWgo);
								}
							}
						}
					}
				}
			}
		}
		conveyorWgoData.ConveyorComponent.UpdateWgoPartState();
		MainGame.Instance.conveyorSystem.AddConveyorObject(conveyorWgoData.ConveyorComponent);
		ConveyorAnimator componentInChildren = builtWgo.GetComponentInChildren<ConveyorAnimator>();
		if (componentInChildren == null)
		{
			return;
		}
		componentInChildren.TryRegister();
	}

	// Token: 0x06000846 RID: 2118 RVA: 0x00028A6C File Offset: 0x00026C6C
	public static void TryMakeConnectionsOutsideBuildSystem(Wgo builtWgo)
	{
		Physics.SyncTransforms();
		ConveyorWgoData conveyorWgoData = builtWgo.Data as ConveyorWgoData;
		if (conveyorWgoData == null)
		{
			return;
		}
		ConnectivityRestriction connectivityRestriction = null;
		Collider[] array = new Collider[10];
		if (Physics.OverlapBoxNonAlloc(builtWgo.transform.position, builtWgo.MainWgoPart.WgoPartData.BakedData.ChunkBounds.withoutShadows.extents / 4f, array, Quaternion.identity) > 0)
		{
			foreach (Collider collider in array)
			{
				if (!(collider == null))
				{
					Wgo componentInParent = collider.GetComponentInParent<Wgo>();
					ConnectivityRestriction connectivityRestriction2;
					if ((!(componentInParent != null) || (!componentInParent.Data.isTempObject && !(componentInParent == builtWgo))) && collider.TryGetComponent<ConnectivityRestriction>(out connectivityRestriction2))
					{
						connectivityRestriction = connectivityRestriction2;
						break;
					}
				}
			}
		}
		BuildConnector[] componentsInChildren = builtWgo.GetComponentsInChildren<BuildConnector>();
		for (int j = 0; j < componentsInChildren.Length; j++)
		{
			if (componentsInChildren[j].gameObject.activeInHierarchy)
			{
				Collider[] array3 = new Collider[10];
				if (Physics.OverlapBoxNonAlloc(componentsInChildren[j].BoxCollider.bounds.center, componentsInChildren[j].BoxCollider.bounds.extents / 2f, array3, Quaternion.identity, 524288) > 0)
				{
					foreach (Collider collider2 in array3)
					{
						if (!(collider2 == null))
						{
							Wgo componentInParent2 = collider2.GetComponentInParent<Wgo>();
							if ((!(componentInParent2 != null) || (!componentInParent2.Data.isTempObject && !(componentInParent2 == builtWgo))) && componentInParent2 != null && componentInParent2.Data is ConveyorWgoData)
							{
								if (componentInParent2.Data.Definition.conveyorType != ConveyorElementType.UndergroundCell)
								{
									componentsInChildren[j].TryConnect(componentInParent2);
									break;
								}
								ConveyorCellSequenceIdentifier conveyorCellSequenceIdentifier;
								if ((builtWgo.Data.Definition.conveyorType == ConveyorElementType.UndergroundCell || !(connectivityRestriction != null) || !connectivityRestriction.disabledTypes.Contains(componentInParent2.Data.Definition.conveyorType)) && collider2.TryGetComponent<ConveyorCellSequenceIdentifier>(out conveyorCellSequenceIdentifier) && conveyorCellSequenceIdentifier.isStartElement)
								{
									componentsInChildren[j].TryConnect(componentInParent2);
									break;
								}
							}
						}
					}
				}
			}
		}
		BoxCollider[] componentsInChildren2 = builtWgo.GetComponentsInChildren<BoxCollider>();
		List<BoxCollider> list = new List<BoxCollider>();
		for (int k = 0; k < componentsInChildren2.Length; k++)
		{
			if (componentsInChildren2[k].gameObject.layer == 19)
			{
				list.Add(componentsInChildren2[k]);
			}
		}
		if (list.Count == 0)
		{
			return;
		}
		bool flag = builtWgo.Data.Definition.conveyorType == ConveyorElementType.UndergroundCell;
		foreach (BoxCollider boxCollider in list)
		{
			ConveyorCellSequenceIdentifier conveyorCellSequenceIdentifier2;
			if (!flag || (boxCollider.TryGetComponent<ConveyorCellSequenceIdentifier>(out conveyorCellSequenceIdentifier2) && conveyorCellSequenceIdentifier2.isStartElement))
			{
				Collider[] array4 = new Collider[20];
				Vector3 vector = boxCollider.bounds.extents / 2f;
				vector.y = 0.2f;
				if (Physics.OverlapBoxNonAlloc(boxCollider.bounds.center, vector, array4, Quaternion.identity, 134217728) > 0)
				{
					for (int l = 0; l < array4.Length; l++)
					{
						if (array4[l] != null)
						{
							Wgo componentInParent3 = array4[l].GetComponentInParent<Wgo>();
							if (!(componentInParent3 != null) || (!componentInParent3.Data.isTempObject && !(componentInParent3 == builtWgo)))
							{
								BuildConnector component = array4[l].GetComponent<BuildConnector>();
								if (!(component == null))
								{
									component.TryConnect(builtWgo);
								}
							}
						}
					}
				}
			}
		}
		conveyorWgoData.ConveyorComponent.UpdateWgoPartState();
		MainGame.Instance.conveyorSystem.AddConveyorObject(conveyorWgoData.ConveyorComponent);
		ConveyorAnimator componentInChildren = builtWgo.GetComponentInChildren<ConveyorAnimator>();
		if (componentInChildren != null)
		{
			componentInChildren.TryRegister();
		}
		ConveyorBuildPointer.TryMakeAutoBuildsOutsideBuildSystem(builtWgo);
	}

	// Token: 0x06000847 RID: 2119 RVA: 0x00028E84 File Offset: 0x00027084
	private static void TryMakeAutoBuildsOutsideBuildSystem(Wgo builtWgo)
	{
		ConveyorCellAutoBuilder componentInChildren = builtWgo.GetComponentInChildren<ConveyorCellAutoBuilder>();
		if (componentInChildren != null)
		{
			Wgo wgo = componentInChildren.AutoBuildCell();
			if (wgo != null)
			{
				ConveyorBuildPointer.TryMakeConnectionsOutsideBuildSystem(wgo);
				ConveyorWgoData conveyorWgoData = builtWgo.Data as ConveyorWgoData;
				if (conveyorWgoData != null)
				{
					wgo.Data.SetGameRes("conveyor_build_is_not_removable", 1);
					conveyorWgoData.HardConnectedWGOs.Add(new SGuid(wgo.Data.UniqueId.Guid));
				}
			}
		}
	}

	// Token: 0x06000848 RID: 2120 RVA: 0x00028EF8 File Offset: 0x000270F8
	private void TryMakeAutoBuilds(Wgo builtWgo)
	{
		ConveyorCellAutoBuilder componentInChildren = builtWgo.GetComponentInChildren<ConveyorCellAutoBuilder>();
		if (componentInChildren != null)
		{
			Wgo wgo = componentInChildren.AutoBuildCell();
			if (wgo != null)
			{
				this.MakeConnections(wgo);
				ConveyorWgoData conveyorWgoData = builtWgo.Data as ConveyorWgoData;
				if (conveyorWgoData != null)
				{
					wgo.Data.SetGameRes("conveyor_build_is_not_removable", 1);
					conveyorWgoData.HardConnectedWGOs.Add(new SGuid(wgo.Data.UniqueId.Guid));
				}
			}
		}
	}
}
