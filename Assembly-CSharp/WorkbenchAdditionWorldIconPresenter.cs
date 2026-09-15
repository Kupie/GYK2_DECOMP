using System;
using System.Collections.Generic;

// Token: 0x02000164 RID: 356
public static class WorkbenchAdditionWorldIconPresenter
{
	// Token: 0x060008B1 RID: 2225 RVA: 0x0002CD05 File Offset: 0x0002AF05
	public static bool TryGet(SGuid wgoUniqueId, out WorkbenchAdditionWorldIconPresenter.State state)
	{
		return WorkbenchAdditionWorldIconPresenter.states.TryGetValue(wgoUniqueId, out state);
	}

	// Token: 0x060008B2 RID: 2226 RVA: 0x0002CD14 File Offset: 0x0002AF14
	public static void Replace(Dictionary<Wgo, WorkbenchAdditionWorldIconPresenter.State> newStates)
	{
		WorkbenchAdditionWorldIconPresenter.toRedraw.Clear();
		foreach (KeyValuePair<Wgo, WorkbenchAdditionWorldIconPresenter.State> keyValuePair in newStates)
		{
			Wgo key = keyValuePair.Key;
			if (key && key.Data != null)
			{
				SGuid uniqueId = key.Data.UniqueId;
				WorkbenchAdditionWorldIconPresenter.State state;
				if (!WorkbenchAdditionWorldIconPresenter.states.TryGetValue(uniqueId, out state) || !state.Equals(keyValuePair.Value))
				{
					WorkbenchAdditionWorldIconPresenter.toRedraw.Add(key);
				}
			}
		}
		foreach (KeyValuePair<SGuid, Wgo> keyValuePair2 in WorkbenchAdditionWorldIconPresenter.displayedWgos)
		{
			Wgo value = keyValuePair2.Value;
			if (value && value.Data != null && !newStates.ContainsKey(value))
			{
				WorkbenchAdditionWorldIconPresenter.toRedraw.Add(value);
			}
		}
		WorkbenchAdditionWorldIconPresenter.states.Clear();
		WorkbenchAdditionWorldIconPresenter.displayedWgos.Clear();
		foreach (KeyValuePair<Wgo, WorkbenchAdditionWorldIconPresenter.State> keyValuePair3 in newStates)
		{
			Wgo key2 = keyValuePair3.Key;
			if (key2 && key2.Data != null)
			{
				WorkbenchAdditionWorldIconPresenter.states[key2.Data.UniqueId] = keyValuePair3.Value;
				WorkbenchAdditionWorldIconPresenter.displayedWgos[key2.Data.UniqueId] = key2;
			}
		}
		for (int i = 0; i < WorkbenchAdditionWorldIconPresenter.toRedraw.Count; i++)
		{
			Wgo wgo = WorkbenchAdditionWorldIconPresenter.toRedraw[i];
			if (wgo && wgo.Data != null)
			{
				wgo.DrawWidgets();
			}
		}
		WorkbenchAdditionWorldIconPresenter.toRedraw.Clear();
	}

	// Token: 0x060008B3 RID: 2227 RVA: 0x0002CF08 File Offset: 0x0002B108
	public static void Clear()
	{
		if (WorkbenchAdditionWorldIconPresenter.states.Count == 0)
		{
			return;
		}
		WorkbenchAdditionWorldIconPresenter.toRedraw.Clear();
		foreach (Wgo wgo in WorkbenchAdditionWorldIconPresenter.displayedWgos.Values)
		{
			if (wgo && wgo.Data != null)
			{
				WorkbenchAdditionWorldIconPresenter.toRedraw.Add(wgo);
			}
		}
		WorkbenchAdditionWorldIconPresenter.states.Clear();
		WorkbenchAdditionWorldIconPresenter.displayedWgos.Clear();
		for (int i = 0; i < WorkbenchAdditionWorldIconPresenter.toRedraw.Count; i++)
		{
			Wgo wgo2 = WorkbenchAdditionWorldIconPresenter.toRedraw[i];
			if (wgo2 && wgo2.Data != null)
			{
				wgo2.DrawWidgets();
			}
		}
		WorkbenchAdditionWorldIconPresenter.toRedraw.Clear();
	}

	// Token: 0x04000A6E RID: 2670
	private static readonly Dictionary<SGuid, WorkbenchAdditionWorldIconPresenter.State> states = new Dictionary<SGuid, WorkbenchAdditionWorldIconPresenter.State>();

	// Token: 0x04000A6F RID: 2671
	private static readonly Dictionary<SGuid, Wgo> displayedWgos = new Dictionary<SGuid, Wgo>();

	// Token: 0x04000A70 RID: 2672
	private static readonly List<Wgo> toRedraw = new List<Wgo>();

	// Token: 0x02000165 RID: 357
	public readonly struct State
	{
		// Token: 0x060008B5 RID: 2229 RVA: 0x0002D000 File Offset: 0x0002B200
		public State(string iconId, bool isInRange, bool usePlotWorldY = false, float plotWorldYOffset = 0f)
		{
			this.IconId = iconId;
			this.IsInRange = isInRange;
			this.UsePlotWorldY = usePlotWorldY;
			this.PlotWorldYOffset = plotWorldYOffset;
		}

		// Token: 0x060008B6 RID: 2230 RVA: 0x0002D020 File Offset: 0x0002B220
		public bool Equals(WorkbenchAdditionWorldIconPresenter.State other)
		{
			return this.IsInRange == other.IsInRange && this.UsePlotWorldY == other.UsePlotWorldY && this.PlotWorldYOffset.Equals(other.PlotWorldYOffset) && this.IconId == other.IconId;
		}

		// Token: 0x04000A71 RID: 2673
		public readonly string IconId;

		// Token: 0x04000A72 RID: 2674
		public readonly bool IsInRange;

		// Token: 0x04000A73 RID: 2675
		public readonly bool UsePlotWorldY;

		// Token: 0x04000A74 RID: 2676
		public readonly float PlotWorldYOffset;
	}
}
