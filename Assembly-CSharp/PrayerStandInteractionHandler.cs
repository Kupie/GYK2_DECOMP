using System;
using LazyBearTechnology;

// Token: 0x02000656 RID: 1622
public class PrayerStandInteractionHandler : WGOInteractionHandlerBase
{
	// Token: 0x06002B06 RID: 11014 RVA: 0x000CBF64 File Offset: 0x000CA164
	public override bool Interact(PlayerController interactor)
	{
		if (base.Interact(interactor))
		{
			return true;
		}
		SermonConfigDef data = GameBalance.Me.GetData<SermonConfigDef>(this.assignedWgo.Data.id);
		if (data == null)
		{
			return false;
		}
		if (MainGame.PlayerData.GetRes("sermon_ready", 0f) < 1f)
		{
			GlobalScriptsManager.FireEvent("System_Pray", "sermon_is_not_ready", null);
			return false;
		}
		WorldData worldData = MainGame.Instance.GameSave.WorldData;
		WorldZoneData worldZoneDataById = worldData.GetGameSceneDataById(interactor.PlayerData.currentGameSceneId).GetWorldZoneDataById(data.curWorldZoneId);
		WorldZoneData worldZoneDataById2 = worldData.GetGameSceneDataById(interactor.PlayerData.currentGameSceneId).GetWorldZoneDataById(data.attachedWorldZoneId);
		UIPrayWindowData uiprayWindowData = new UIPrayWindowData(interactor.PlayerData, this.assignedWgo.Data, (int)worldZoneDataById2.GetTotalQuality(), (int)worldZoneDataById.GetTotalQuality(), MainGame.PlayerData.GetResInt("happiness"), data.rewardBoxId);
		LazyUI.GetWindow<UIPrayWindow>().Open(uiprayWindowData);
		return true;
	}

	// Token: 0x06002B07 RID: 11015 RVA: 0x000CC057 File Offset: 0x000CA257
	public override bool HasInteraction(PlayerController interactor)
	{
		return base.HasInteraction(interactor) || GameBalance.Me.GetData<SermonConfigDef>(this.assignedWgo.Data.id) != null;
	}

	// Token: 0x06002B08 RID: 11016 RVA: 0x000CC084 File Offset: 0x000CA284
	protected override InteractionInfos FormInteractionInfo()
	{
		InteractionInfos interactionInfos = base.FormInteractionInfo();
		if (!interactionInfos.IsEmpty)
		{
			return interactionInfos;
		}
		string text;
		return new InteractionInfos(new InteractionInfo(base.TryGetCustomInteractionStr(out text) ? text : base.LocalizeHintWithActionIcon("hint_pray", GameKey.Interaction)));
	}
}
