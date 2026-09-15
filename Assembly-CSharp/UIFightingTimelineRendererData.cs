using System;
using LazyBearTechnology;

// Token: 0x020009C9 RID: 2505
public class UIFightingTimelineRendererData : LazyWidgetDataBase
{
	// Token: 0x140000C3 RID: 195
	// (add) Token: 0x060042A6 RID: 17062 RVA: 0x0013C768 File Offset: 0x0013A968
	// (remove) Token: 0x060042A7 RID: 17063 RVA: 0x0013C7A0 File Offset: 0x0013A9A0
	public event Action<float> OnProgressChanged;

	// Token: 0x17000A16 RID: 2582
	// (get) Token: 0x060042A8 RID: 17064 RVA: 0x0013C7D5 File Offset: 0x0013A9D5
	// (set) Token: 0x060042A9 RID: 17065 RVA: 0x0013C7DD File Offset: 0x0013A9DD
	public FightingLevelPreset Preset { get; private set; }

	// Token: 0x17000A17 RID: 2583
	// (get) Token: 0x060042AA RID: 17066 RVA: 0x0013C7E6 File Offset: 0x0013A9E6
	// (set) Token: 0x060042AB RID: 17067 RVA: 0x0013C7EE File Offset: 0x0013A9EE
	public FightingLevelPresetProcessor Processor { get; private set; }

	// Token: 0x17000A18 RID: 2584
	// (get) Token: 0x060042AC RID: 17068 RVA: 0x0013C7F7 File Offset: 0x0013A9F7
	// (set) Token: 0x060042AD RID: 17069 RVA: 0x0013C7FF File Offset: 0x0013A9FF
	public FightingLevel CurrentLevel { get; private set; }

	// Token: 0x17000A19 RID: 2585
	// (get) Token: 0x060042AE RID: 17070 RVA: 0x0013C808 File Offset: 0x0013AA08
	// (set) Token: 0x060042AF RID: 17071 RVA: 0x0013C810 File Offset: 0x0013AA10
	public float TotalTime { get; private set; }

	// Token: 0x060042B0 RID: 17072 RVA: 0x0013C81C File Offset: 0x0013AA1C
	public UIFightingTimelineRendererData(FightingLevelPreset preset, FightingLevelPresetProcessor processor, FightingLevel currentLevel)
	{
		this.Preset = preset;
		this.Processor = processor;
		this.CurrentLevel = currentLevel;
		this.TotalTime = 0f;
		float num = 0f;
		foreach (FightingLevelPreset.FightingLineData fightingLineData in this.Preset.lines)
		{
			foreach (FightingPhaseData fightingPhaseData in fightingLineData.phases)
			{
				this.TotalTime += (float)fightingPhaseData.duration;
			}
			if (this.TotalTime > num)
			{
				num = this.TotalTime;
			}
			this.TotalTime = 0f;
		}
		this.TotalTime = num;
		this.Processor.OnProgressChanged += this.HandleProgressChanged;
	}

	// Token: 0x060042B1 RID: 17073 RVA: 0x0013C920 File Offset: 0x0013AB20
	public void UnsubscribeFromProcessor()
	{
		this.Processor.OnProgressChanged -= this.HandleProgressChanged;
	}

	// Token: 0x060042B2 RID: 17074 RVA: 0x0013C939 File Offset: 0x0013AB39
	public void HandleProgressChanged(float progress)
	{
		Action<float> onProgressChanged = this.OnProgressChanged;
		if (onProgressChanged == null)
		{
			return;
		}
		onProgressChanged(progress);
	}
}
