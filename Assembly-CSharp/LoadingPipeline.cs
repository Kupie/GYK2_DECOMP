using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

// Token: 0x020006CC RID: 1740
public class LoadingPipeline
{
	// Token: 0x1700072C RID: 1836
	// (get) Token: 0x06002E18 RID: 11800 RVA: 0x000DC4BA File Offset: 0x000DA6BA
	public static LoadingPipeline Instance
	{
		get
		{
			LoadingPipeline loadingPipeline;
			if ((loadingPipeline = LoadingPipeline.instance) == null)
			{
				loadingPipeline = (LoadingPipeline.instance = new LoadingPipeline());
			}
			return loadingPipeline;
		}
	}

	// Token: 0x06002E19 RID: 11801 RVA: 0x000DC4D0 File Offset: 0x000DA6D0
	public void RegisterTask(LoadingStage stage, UniTask task, float weight = 1f)
	{
		this.RegisterTask(stage, task, null, weight);
	}

	// Token: 0x06002E1A RID: 11802 RVA: 0x000DC4DC File Offset: 0x000DA6DC
	public void RegisterTask(LoadingStage stage, UniTask task, Func<float> progressGetter, float weight = 1f)
	{
		List<LoadingPipeline.TaskEntry> list;
		if (!this.stages.TryGetValue(stage, out list))
		{
			list = new List<LoadingPipeline.TaskEntry>();
			this.stages[stage] = list;
		}
		LoadingPipeline.TaskEntry taskEntry = new LoadingPipeline.TaskEntry(progressGetter, weight);
		list.Add(taskEntry);
		LoadingPipeline.TrackCompletion(task, taskEntry).Forget();
	}

	// Token: 0x06002E1B RID: 11803 RVA: 0x000DC52C File Offset: 0x000DA72C
	public float GetStageProgress(LoadingStage stage)
	{
		if (GameShutdown.IsRequested)
		{
			return 1f;
		}
		List<LoadingPipeline.TaskEntry> list;
		if (!this.stages.TryGetValue(stage, out list) || list.Count == 0)
		{
			return 1f;
		}
		float num = 0f;
		float num2 = 0f;
		foreach (LoadingPipeline.TaskEntry taskEntry in list)
		{
			num += taskEntry.Weight;
			float num3;
			if (!taskEntry.IsComplete)
			{
				Func<float> progressGetter = taskEntry.ProgressGetter;
				num3 = ((progressGetter != null) ? progressGetter() : 0f);
			}
			else
			{
				num3 = 1f;
			}
			float num4 = num3;
			num2 += num4 * taskEntry.Weight;
		}
		if (num <= 0f)
		{
			return 1f;
		}
		return num2 / num;
	}

	// Token: 0x06002E1C RID: 11804 RVA: 0x000DC5FC File Offset: 0x000DA7FC
	public bool HasStage(LoadingStage stage)
	{
		List<LoadingPipeline.TaskEntry> list;
		return this.stages.TryGetValue(stage, out list) && list.Count > 0;
	}

	// Token: 0x06002E1D RID: 11805 RVA: 0x000DC624 File Offset: 0x000DA824
	public void ClearStage(LoadingStage stage)
	{
		this.stages.Remove(stage);
	}

	// Token: 0x06002E1E RID: 11806 RVA: 0x000DC634 File Offset: 0x000DA834
	public async UniTask AwaitStage(LoadingStage stage)
	{
		GameShutdown.ThrowIfRequested();
		List<LoadingPipeline.TaskEntry> list;
		if (this.stages.TryGetValue(stage, out list))
		{
			List<UniTask> list2 = new List<UniTask>(list.Count);
			foreach (LoadingPipeline.TaskEntry taskEntry in list)
			{
				list2.Add(taskEntry.Completion.Task);
			}
			await UniTask.WhenAll(list2).AttachExternalCancellation(GameShutdown.Token);
		}
	}

	// Token: 0x06002E1F RID: 11807 RVA: 0x000DC680 File Offset: 0x000DA880
	public async UniTask AwaitFromStage(LoadingStage fromStage)
	{
		foreach (object obj in Enum.GetValues(typeof(LoadingStage)))
		{
			LoadingStage loadingStage = (LoadingStage)obj;
			if (loadingStage > fromStage)
			{
				break;
			}
			await this.AwaitStage(loadingStage);
		}
		IEnumerator enumerator = null;
	}

	// Token: 0x06002E20 RID: 11808 RVA: 0x000DC6CC File Offset: 0x000DA8CC
	public bool IsStageComplete(LoadingStage stage)
	{
		List<LoadingPipeline.TaskEntry> list;
		if (!this.stages.TryGetValue(stage, out list))
		{
			return true;
		}
		using (List<LoadingPipeline.TaskEntry>.Enumerator enumerator = list.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (!enumerator.Current.IsComplete)
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x06002E21 RID: 11809 RVA: 0x000DC734 File Offset: 0x000DA934
	private static async UniTaskVoid TrackCompletion(UniTask task, LoadingPipeline.TaskEntry entry)
	{
		bool canceled = false;
		try
		{
			await UniTask.Yield(PlayerLoopTiming.Update, GameShutdown.Token, false);
			await task;
		}
		catch (OperationCanceledException)
		{
			canceled = true;
		}
		catch (Exception ex)
		{
			if (!GameShutdown.IsRequested)
			{
				Debug.LogException(ex);
			}
		}
		finally
		{
			entry.IsComplete = true;
			if (canceled || GameShutdown.IsRequested)
			{
				entry.Completion.TrySetCanceled(default(CancellationToken));
			}
			else
			{
				entry.Completion.TrySetResult();
			}
		}
	}

	// Token: 0x0400251D RID: 9501
	private static LoadingPipeline instance;

	// Token: 0x0400251E RID: 9502
	private readonly Dictionary<LoadingStage, List<LoadingPipeline.TaskEntry>> stages = new Dictionary<LoadingStage, List<LoadingPipeline.TaskEntry>>();

	// Token: 0x020006CD RID: 1741
	private class TaskEntry
	{
		// Token: 0x06002E23 RID: 11811 RVA: 0x000DC792 File Offset: 0x000DA992
		public TaskEntry(Func<float> progressGetter, float weight)
		{
			this.ProgressGetter = progressGetter;
			this.Weight = weight;
		}

		// Token: 0x0400251F RID: 9503
		public readonly UniTaskCompletionSource Completion = new UniTaskCompletionSource();

		// Token: 0x04002520 RID: 9504
		public readonly Func<float> ProgressGetter;

		// Token: 0x04002521 RID: 9505
		public readonly float Weight;

		// Token: 0x04002522 RID: 9506
		public bool IsComplete;
	}
}
