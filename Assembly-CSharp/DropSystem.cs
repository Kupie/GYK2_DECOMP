using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x0200042C RID: 1068
public class DropSystem : ICustomUpdatable
{
	// Token: 0x170004E7 RID: 1255
	// (get) Token: 0x06001C45 RID: 7237 RVA: 0x00084092 File Offset: 0x00082292
	public WorldData WorldData
	{
		get
		{
			return MainGame.Instance.GameSave.worldData;
		}
	}

	// Token: 0x06001C46 RID: 7238 RVA: 0x000840A4 File Offset: 0x000822A4
	public bool DropItem(Item droppableItem, string worldId, Vector3 pos, List<Item> droppedItemsList = null)
	{
		if (string.IsNullOrEmpty(worldId))
		{
			Debug.LogError("DropItem: worldId couldn't be empty");
			return false;
		}
		Debug.Log(string.Format("DropItem: {0} {1}", droppableItem.id, droppableItem.Count));
		if (droppableItem.Definition.isFuel)
		{
			return false;
		}
		if (droppableItem.Definition.isTechPoint)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			if (droppableItem.id.EndsWith("tech_red"))
			{
				num += droppableItem.Count;
			}
			if (droppableItem.id.EndsWith("tech_green"))
			{
				num2 += droppableItem.Count;
			}
			if (droppableItem.id.EndsWith("tech_blue"))
			{
				num3 += droppableItem.Count;
			}
			if (num + num2 + num3 > 0)
			{
				this.DropTechPoints(pos, num, num2, num3);
				return true;
			}
			return false;
		}
		else
		{
			if (droppableItem.Count <= 1)
			{
				if (droppedItemsList != null)
				{
					droppedItemsList.Add(droppableItem);
				}
				this.DropItemInternal(droppableItem, worldId, pos);
				return true;
			}
			if (droppableItem.Count > 5 && droppableItem.Definition.itemSize == ItemSize.Small)
			{
				int i = droppableItem.Count;
				while (i > 0)
				{
					int num4 = i;
					if (droppableItem.Definition.stackCount > 0 && num4 > droppableItem.Definition.stackCount)
					{
						num4 = droppableItem.Definition.stackCount;
					}
					i -= num4;
					Item item = new Item(droppableItem.id, num4);
					if (droppedItemsList != null)
					{
						droppedItemsList.Add(item);
					}
					this.DropItemInternal(item, worldId, pos);
				}
				return true;
			}
			for (int j = 0; j < droppableItem.Count; j++)
			{
				Item item2 = new Item(droppableItem.id, 1);
				if (droppedItemsList != null)
				{
					droppedItemsList.Add(item2);
				}
				this.DropItemInternal(item2, worldId, pos);
			}
			return true;
		}
	}

	// Token: 0x06001C47 RID: 7239 RVA: 0x0008424A File Offset: 0x0008244A
	public bool DropGameResAtom(GameResAtom gameResAtom, string worldId, Vector3 pos)
	{
		return this.DropItem(gameResAtom.ItemFromAtom(), worldId, pos, null);
	}

	// Token: 0x06001C48 RID: 7240 RVA: 0x0008425B File Offset: 0x0008245B
	public void DropItemAsDropView(Item droppableItem, string worldId, Vector3 pos)
	{
		if (droppableItem == null || string.IsNullOrEmpty(droppableItem.id) || string.IsNullOrEmpty(worldId))
		{
			return;
		}
		this.DropItemInternal(droppableItem, worldId, pos);
	}

	// Token: 0x06001C49 RID: 7241 RVA: 0x00084280 File Offset: 0x00082480
	public void CollectAllGameResDropsToPlayer(float duration)
	{
		Transform playerDropCollectorTransform = DropSystem.GetPlayerDropCollectorTransform();
		if (playerDropCollectorTransform == null)
		{
			return;
		}
		for (int i = 0; i < this.WorldData.gameSceneDataList.Count; i++)
		{
			GameSceneData gameSceneData = this.WorldData.gameSceneDataList[i];
			this.CollectGameResDropsFromList(gameSceneData.droppedItems, playerDropCollectorTransform, duration);
			this.CollectGameResDropsFromList(gameSceneData.queuedDrops, playerDropCollectorTransform, duration);
		}
		TechPointsSpawner.FlushPendingAsWorldDrops();
		TechPointDrop.CollectAllToPlayer(playerDropCollectorTransform, duration);
		this.CollectViewlessTechPointDrops();
	}

	// Token: 0x06001C4A RID: 7242 RVA: 0x000842F8 File Offset: 0x000824F8
	public void RemoveDrop(DropData drop, string worldId)
	{
		if (string.IsNullOrEmpty(worldId))
		{
			Debug.LogError("DropItem: worldId couldn't be empty");
			return;
		}
		this.WorldData.GetGameSceneDataById(worldId).RemoveDrop(drop);
	}

	// Token: 0x06001C4B RID: 7243 RVA: 0x00084320 File Offset: 0x00082520
	public void CustomUpdate(float deltaTime)
	{
		for (int i = this.WorldData.gameSceneDataList.Count - 1; i >= 0; i--)
		{
			GameSceneData gameSceneData = this.WorldData.gameSceneDataList[i];
			for (int j = gameSceneData.droppedItems.Count - 1; j >= 0; j--)
			{
				DropData dropData = gameSceneData.droppedItems[j];
				if (!dropData.CanNotBeAutoDestroyed.ResultFlag && !dropData.IsRemoving && dropData.AutoDestroyTimer > 0f)
				{
					dropData.AutoDestroyTimer -= deltaTime;
					if (dropData.AutoDestroyTimer <= 0f)
					{
						DropView dropView = null;
						foreach (GameScene gameScene in LazySingleton<GameSceneManager>.Instance.LoadedGameScenes)
						{
							dropView = gameScene.GetDropView(dropData.Item);
							if (dropView != null)
							{
								break;
							}
						}
						RaycastHit raycastHit;
						if (dropView != null && SpecialPhysicsCastUtils.TryGetTopmostGroundHit(dropData.Position, out raycastHit))
						{
							MainGame.WorldData.AddWgoData(new WgoData("body_drop_grave_object", raycastHit.point, dropData.WorldId));
						}
						MainGame.PlayerData.SubRes("cur_bodies_count", 1f);
						gameSceneData.RemoveDrop(dropData);
						if (MainGame.PlayerData.CurrentWorldZoneData != null && MainGame.PlayerData.CurrentWorldZoneData.Definition.id == "morgue")
						{
							GUIElements.Instance.WorldZoneWidget.Draw(new WorldZoneWidgetData());
						}
					}
				}
			}
		}
	}

	// Token: 0x06001C4C RID: 7244 RVA: 0x000844CC File Offset: 0x000826CC
	public void DropTechPoints(Vector3 pos, int r, int g, int b)
	{
		TechPointsSpawner.CreateSpawner(pos, r, g, b, 0);
	}

	// Token: 0x06001C4D RID: 7245 RVA: 0x000844DC File Offset: 0x000826DC
	private void CollectGameResDropsFromList(List<DropData> drops, Transform target, float duration)
	{
		if (drops == null || drops.Count == 0)
		{
			return;
		}
		for (int i = drops.Count - 1; i >= 0; i--)
		{
			DropData dropData = drops[i];
			if (dropData != null && dropData.IsResDrop && !dropData.IsRemoving)
			{
				DropView dropView = DropSystem.FindDropView(dropData);
				if (dropView != null && !dropView.IsDespawning)
				{
					dropView.MoveToCollectorTimed(target, duration);
				}
				else
				{
					MainGame.PlayerData.CollectResDrop(dropData);
				}
			}
		}
	}

	// Token: 0x06001C4E RID: 7246 RVA: 0x00084550 File Offset: 0x00082750
	private void CollectViewlessTechPointDrops()
	{
		PlayerController playerController = MainGame.PlayerController;
		Vector3 vector = ((playerController != null) ? playerController.transform.position : Vector3.zero);
		for (int i = 0; i < this.WorldData.gameSceneDataList.Count; i++)
		{
			GameSceneData gameSceneData = this.WorldData.gameSceneDataList[i];
			List<TechPointDropData> techPointDrops = gameSceneData.techPointDrops;
			if (techPointDrops != null && techPointDrops.Count != 0)
			{
				for (int j = techPointDrops.Count - 1; j >= 0; j--)
				{
					TechPointDropData techPointDropData = techPointDrops[j];
					if (techPointDropData != null && !TechPointDrop.IsTracked(techPointDropData))
					{
						string text = TechDef.FlyingReses[(int)techPointDropData.type];
						FlyingTechPoint.Drop(vector, text, null, -1);
						LazyAudio.Play("tech_point_collect");
						gameSceneData.RemoveTechPointDrop(techPointDropData);
					}
				}
			}
		}
	}

	// Token: 0x06001C4F RID: 7247 RVA: 0x00084628 File Offset: 0x00082828
	private static DropView FindDropView(DropData drop)
	{
		if (((drop != null) ? drop.Item : null) == null || LazySingleton<GameSceneManager>.Instance == null)
		{
			return null;
		}
		List<GameScene> loadedGameScenes = LazySingleton<GameSceneManager>.Instance.LoadedGameScenes;
		for (int i = 0; i < loadedGameScenes.Count; i++)
		{
			GameScene gameScene = loadedGameScenes[i];
			DropView dropView;
			if (gameScene != null && gameScene.TryGetDropView(drop.Item, out dropView))
			{
				return dropView;
			}
		}
		return null;
	}

	// Token: 0x06001C50 RID: 7248 RVA: 0x00084694 File Offset: 0x00082894
	private static Transform GetPlayerDropCollectorTransform()
	{
		PlayerController playerController = MainGame.PlayerController;
		if (playerController == null)
		{
			return null;
		}
		DropSearcher componentInChildren = playerController.GetComponentInChildren<DropSearcher>(true);
		if (componentInChildren != null && componentInChildren.CollectorObjectTransform != null)
		{
			return componentInChildren.CollectorObjectTransform;
		}
		return playerController.transform;
	}

	// Token: 0x06001C51 RID: 7249 RVA: 0x000846DE File Offset: 0x000828DE
	private void DropItemInternal(Item droppableItem, string worldId, Vector3 pos)
	{
		if (this.WorldData.LoadedScenes.Contains(worldId))
		{
			this.DropItemIntoWorld(droppableItem, worldId, pos);
			return;
		}
		this.WorldData.GetGameSceneDataById(worldId).AddDropToQueue(droppableItem, pos, false);
	}

	// Token: 0x06001C52 RID: 7250 RVA: 0x00084711 File Offset: 0x00082911
	private void DropItemIntoWorld(Item droppableItem, string worldId, Vector3 pos)
	{
		if (string.IsNullOrEmpty(droppableItem.id))
		{
			Debug.LogError("[DropSystem]: tried to drop an empty item.");
			return;
		}
		this.WorldData.GetGameSceneDataById(worldId).AddDrop(droppableItem, pos);
	}
}
