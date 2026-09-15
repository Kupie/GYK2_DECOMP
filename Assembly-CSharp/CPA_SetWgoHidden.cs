using System;

// Token: 0x0200030C RID: 780
[Serializable]
public class CPA_SetWgoHidden : CapturePointAction
{
	// Token: 0x060014C5 RID: 5317 RVA: 0x00065A04 File Offset: 0x00063C04
	public override void Execute(LazyConsts.Fighting.TeamType teamType, FightingLevel level)
	{
		if (this.teamType != teamType)
		{
			return;
		}
		if (string.IsNullOrEmpty(this.wgoUniqueId))
		{
			return;
		}
		WgoData wgoData = MainGame.Instance.GameSave.worldData.GetWgoData(SGuid.Parse(this.wgoUniqueId));
		if (wgoData != null)
		{
			wgoData.IsHidden = false;
		}
	}

	// Token: 0x04001582 RID: 5506
	public bool isHidden;

	// Token: 0x04001583 RID: 5507
	public string wgoUniqueId;
}
