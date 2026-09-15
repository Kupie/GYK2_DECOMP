using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x02000332 RID: 818
public class FightingLevelPresetProcessor : MonoBehaviour
{
	// Token: 0x14000024 RID: 36
	// (add) Token: 0x060015C5 RID: 5573 RVA: 0x00069CC8 File Offset: 0x00067EC8
	// (remove) Token: 0x060015C6 RID: 5574 RVA: 0x00069D00 File Offset: 0x00067F00
	public event FightingLevelPresetProcessor.EnemiesSpawnDelegate OnEnemiesSpawn;

	// Token: 0x14000025 RID: 37
	// (add) Token: 0x060015C7 RID: 5575 RVA: 0x00069D38 File Offset: 0x00067F38
	// (remove) Token: 0x060015C8 RID: 5576 RVA: 0x00069D70 File Offset: 0x00067F70
	public event Action OnPresetStarted;

	// Token: 0x14000026 RID: 38
	// (add) Token: 0x060015C9 RID: 5577 RVA: 0x00069DA8 File Offset: 0x00067FA8
	// (remove) Token: 0x060015CA RID: 5578 RVA: 0x00069DE0 File Offset: 0x00067FE0
	public event Action OnPresetFinished;

	// Token: 0x14000027 RID: 39
	// (add) Token: 0x060015CB RID: 5579 RVA: 0x00069E18 File Offset: 0x00068018
	// (remove) Token: 0x060015CC RID: 5580 RVA: 0x00069E50 File Offset: 0x00068050
	public event Action<float> OnProgressChanged;

	// Token: 0x170003BF RID: 959
	// (get) Token: 0x060015CD RID: 5581 RVA: 0x00069E85 File Offset: 0x00068085
	// (set) Token: 0x060015CE RID: 5582 RVA: 0x00069E8D File Offset: 0x0006808D
	public bool IsPlaying { get; private set; }

	// Token: 0x170003C0 RID: 960
	// (get) Token: 0x060015CF RID: 5583 RVA: 0x00069E96 File Offset: 0x00068096
	// (set) Token: 0x060015D0 RID: 5584 RVA: 0x00069E9E File Offset: 0x0006809E
	public float CurrentProgress { get; private set; }

	// Token: 0x170003C1 RID: 961
	// (get) Token: 0x060015D1 RID: 5585 RVA: 0x00069EA7 File Offset: 0x000680A7
	// (set) Token: 0x060015D2 RID: 5586 RVA: 0x00069EAF File Offset: 0x000680AF
	public float TotalDuration { get; private set; }

	// Token: 0x170003C2 RID: 962
	// (get) Token: 0x060015D3 RID: 5587 RVA: 0x00069EB8 File Offset: 0x000680B8
	public float ProgressNormalized
	{
		get
		{
			if (this.TotalDuration <= 0f)
			{
				return 0f;
			}
			return this.CurrentProgress / this.TotalDuration;
		}
	}

	// Token: 0x170003C3 RID: 963
	// (get) Token: 0x060015D4 RID: 5588 RVA: 0x00069EDA File Offset: 0x000680DA
	// (set) Token: 0x060015D5 RID: 5589 RVA: 0x00069EE2 File Offset: 0x000680E2
	public Dictionary<int, int> SpawnedEnemiesCountByLine { get; private set; } = new Dictionary<int, int>();

	// Token: 0x060015D6 RID: 5590 RVA: 0x00069EEB File Offset: 0x000680EB
	public void StartPreset(FightingLevelPreset preset)
	{
		this.SpawnedEnemiesCountByLine.Clear();
		if (this.IsPlaying)
		{
			this.StopPreset();
		}
		this.currentPreset = preset;
		this.IsPlaying = true;
		this.mainProcessorCoroutine = base.StartCoroutine(this.ProcessPreset());
	}

	// Token: 0x060015D7 RID: 5591 RVA: 0x00069F26 File Offset: 0x00068126
	public void StopPreset()
	{
		if (!this.IsPlaying)
		{
			return;
		}
		if (this.mainProcessorCoroutine != null)
		{
			base.StopCoroutine(this.mainProcessorCoroutine);
			this.mainProcessorCoroutine = null;
		}
		this.IsPlaying = false;
		this.CurrentProgress = 0f;
		this.currentPreset = null;
	}

	// Token: 0x060015D8 RID: 5592 RVA: 0x00069F65 File Offset: 0x00068165
	private IEnumerator ProcessPreset()
	{
		this.CalculateTotalDuration();
		this.CurrentProgress = 0f;
		Action onPresetStarted = this.OnPresetStarted;
		if (onPresetStarted != null)
		{
			onPresetStarted();
		}
		int remainingLines = this.currentPreset.lines.Count;
		Action <>9__0;
		for (int i = 0; i < this.currentPreset.lines.Count; i++)
		{
			FightingLevelPreset.FightingLineData fightingLineData = this.currentPreset.lines[i];
			FightingLevelPreset.FightingLineData fightingLineData2 = fightingLineData;
			int num = i;
			Action action;
			if ((action = <>9__0) == null)
			{
				action = (<>9__0 = delegate
				{
					int remainingLines2 = remainingLines;
					remainingLines = remainingLines2 - 1;
				});
			}
			base.StartCoroutine(this.RunLineAndSignal(fightingLineData2, num, action));
		}
		while (remainingLines > 0)
		{
			while ((LazySingleton<FightingGameController>.Instance != null && LazySingleton<FightingGameController>.Instance.IsPaused) || MainGame.IsGamePaused)
			{
				yield return null;
			}
			if (!this.IsPlaying)
			{
				yield break;
			}
			this.CurrentProgress += Time.deltaTime;
			Action<float> onProgressChanged = this.OnProgressChanged;
			if (onProgressChanged != null)
			{
				onProgressChanged(this.ProgressNormalized);
			}
			yield return null;
		}
		this.CurrentProgress = this.TotalDuration;
		Action<float> onProgressChanged2 = this.OnProgressChanged;
		if (onProgressChanged2 != null)
		{
			onProgressChanged2(this.ProgressNormalized);
		}
		Action onPresetFinished = this.OnPresetFinished;
		if (onPresetFinished != null)
		{
			onPresetFinished();
		}
		this.IsPlaying = false;
		yield break;
	}

	// Token: 0x060015D9 RID: 5593 RVA: 0x00069F74 File Offset: 0x00068174
	private IEnumerator ProcessLineCoroutine(FightingLevelPreset.FightingLineData line, int lineIndex)
	{
		foreach (FightingPhaseData fightingPhaseData in line.phases)
		{
			if (!this.IsPlaying)
			{
				yield break;
			}
			FightingPhasePauseData pausePhase = fightingPhaseData as FightingPhasePauseData;
			if (pausePhase != null)
			{
				float waited = 0f;
				while (waited < (float)pausePhase.duration)
				{
					if (!this.IsPlaying)
					{
						yield break;
					}
					while ((LazySingleton<FightingGameController>.Instance != null && LazySingleton<FightingGameController>.Instance.IsPaused) || MainGame.IsGamePaused)
					{
						yield return null;
					}
					waited += Time.deltaTime;
					yield return null;
				}
			}
			else
			{
				FightingPhaseSpawnEnemiesData fightingPhaseSpawnEnemiesData = fightingPhaseData as FightingPhaseSpawnEnemiesData;
				if (fightingPhaseSpawnEnemiesData != null)
				{
					yield return base.StartCoroutine(this.SpawnEnemiesForPhase(fightingPhaseSpawnEnemiesData, line, lineIndex));
				}
			}
			pausePhase = null;
		}
		List<FightingPhaseData>.Enumerator enumerator = default(List<FightingPhaseData>.Enumerator);
		yield break;
		yield break;
	}

	// Token: 0x060015DA RID: 5594 RVA: 0x00069F91 File Offset: 0x00068191
	private IEnumerator RunLineAndSignal(FightingLevelPreset.FightingLineData line, int lineIndex, Action onComplete)
	{
		yield return this.ProcessLineCoroutine(line, lineIndex);
		if (onComplete != null)
		{
			onComplete();
		}
		yield break;
	}

	// Token: 0x060015DB RID: 5595 RVA: 0x00069FB5 File Offset: 0x000681B5
	public void SpawnEnemies(string id, int count, FightingLevelPreset.FightingLineData line)
	{
		if (this.currentPreset != null && !line.isEnabled)
		{
			return;
		}
		FightingLevelPresetProcessor.EnemiesSpawnDelegate onEnemiesSpawn = this.OnEnemiesSpawn;
		if (onEnemiesSpawn == null)
		{
			return;
		}
		onEnemiesSpawn(id, count, line);
	}

	// Token: 0x060015DC RID: 5596 RVA: 0x00069FE4 File Offset: 0x000681E4
	private void CalculateTotalDuration()
	{
		this.TotalDuration = 0f;
		if (this.currentPreset == null)
		{
			return;
		}
		foreach (FightingLevelPreset.FightingLineData fightingLineData in this.currentPreset.lines)
		{
			List<FightingPhaseData> phases = fightingLineData.phases;
			float num;
			if (phases == null)
			{
				num = 0f;
			}
			else
			{
				num = phases.Sum((FightingPhaseData phase) => (float)phase.duration);
			}
			float num2 = num;
			if (num2 > this.TotalDuration)
			{
				this.TotalDuration = num2;
			}
		}
	}

	// Token: 0x060015DD RID: 5597 RVA: 0x0006A094 File Offset: 0x00068294
	public IEnumerator SpawnEnemiesForPhase(FightingPhaseSpawnEnemiesData phaseSpawnData, FightingLevelPreset.FightingLineData line, int lineIndex)
	{
		float progress = 0f;
		phaseSpawnData.Reset();
		int num = 0;
		while (progress < (float)phaseSpawnData.duration)
		{
			if (!this.IsPlaying)
			{
				yield break;
			}
			while ((LazySingleton<FightingGameController>.Instance != null && LazySingleton<FightingGameController>.Instance.IsPaused) || MainGame.IsGamePaused)
			{
				yield return null;
			}
			progress += Time.deltaTime;
			phaseSpawnData.UpdatePhase(progress, this, line, out num);
			int num2;
			if (this.SpawnedEnemiesCountByLine.TryGetValue(lineIndex, out num2))
			{
				this.SpawnedEnemiesCountByLine[lineIndex] = num2 + num;
			}
			else
			{
				this.SpawnedEnemiesCountByLine[lineIndex] = num;
			}
			yield return null;
		}
		phaseSpawnData.FlushRemaining(this, line, out num);
		int num3;
		if (this.SpawnedEnemiesCountByLine.TryGetValue(lineIndex, out num3))
		{
			this.SpawnedEnemiesCountByLine[lineIndex] = num3 + num;
		}
		else
		{
			this.SpawnedEnemiesCountByLine[lineIndex] = num;
		}
		yield break;
	}

	// Token: 0x04001640 RID: 5696
	private FightingLevelPreset currentPreset;

	// Token: 0x04001641 RID: 5697
	private Coroutine mainProcessorCoroutine;

	// Token: 0x02000333 RID: 819
	// (Invoke) Token: 0x060015E0 RID: 5600
	public delegate void EnemiesSpawnDelegate(string id, int count, FightingLevelPreset.FightingLineData line);
}
