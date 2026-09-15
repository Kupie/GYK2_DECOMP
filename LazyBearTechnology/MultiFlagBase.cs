using System;
using System.Collections.Generic;
using System.Linq;

namespace LazyBearTechnology
{
	// Token: 0x02000189 RID: 393
	public abstract class MultiFlagBase<T> where T : Enum
	{
		// Token: 0x17000131 RID: 305
		// (get) Token: 0x060008B2 RID: 2226 RVA: 0x0002A5F0 File Offset: 0x000287F0
		public bool ResultFlag
		{
			get
			{
				return this.currentResultFlag;
			}
		}

		// Token: 0x060008B3 RID: 2227 RVA: 0x0002A5F8 File Offset: 0x000287F8
		public void Init(Action<bool> onChangedAction, bool initialFlag = false)
		{
			this.onChangedAction = onChangedAction;
			this.currentResultFlag = initialFlag;
		}

		// Token: 0x060008B4 RID: 2228 RVA: 0x0002A608 File Offset: 0x00028808
		public void UpdateFlag(T t, bool newValue)
		{
			if (!this.flags.ContainsKey(t))
			{
				this.AddFlag(t, newValue);
				return;
			}
			this.flags[t] = newValue;
			this.CalcResult();
		}

		// Token: 0x060008B5 RID: 2229 RVA: 0x0002A634 File Offset: 0x00028834
		public bool HasFlag(T t)
		{
			return this.flags.ContainsKey(t);
		}

		// Token: 0x060008B6 RID: 2230 RVA: 0x0002A644 File Offset: 0x00028844
		public bool GetFlag(T state)
		{
			bool flag;
			if (!this.flags.TryGetValue(state, out flag))
			{
				return this.DefaultValueForMissingFlag;
			}
			return flag;
		}

		// Token: 0x060008B7 RID: 2231 RVA: 0x0002A669 File Offset: 0x00028869
		public void Reset(bool initialFlag = false)
		{
			this.flags.Clear();
			this.currentResultFlag = initialFlag;
			this.CalcResult();
		}

		// Token: 0x060008B8 RID: 2232 RVA: 0x0002A684 File Offset: 0x00028884
		public Dictionary<T, bool> GetAllFlags()
		{
			Dictionary<T, bool> dictionary = new Dictionary<T, bool>();
			foreach (KeyValuePair<T, bool> keyValuePair in this.flags)
			{
				dictionary.Add(keyValuePair.Key, keyValuePair.Value);
			}
			return dictionary;
		}

		// Token: 0x060008B9 RID: 2233 RVA: 0x0002A6EC File Offset: 0x000288EC
		public bool GetResultFlagExceptFlagTypes(params T[] ignoredFlagTypes)
		{
			foreach (KeyValuePair<T, bool> keyValuePair in this.flags)
			{
				if (!ignoredFlagTypes.Contains(keyValuePair.Key) && this.IsExceptMatch(keyValuePair.Value))
				{
					return this.ExceptResultOnMatch;
				}
			}
			return this.ExceptResultDefault;
		}

		// Token: 0x060008BA RID: 2234 RVA: 0x0002A768 File Offset: 0x00028968
		public bool GetResultFlagExceptFlagTypesNonAlloc(params T[] ignoreStates)
		{
			foreach (KeyValuePair<T, bool> keyValuePair in this.flags)
			{
				if (!ignoreStates.Contains(keyValuePair.Key) && this.IsExceptMatch(keyValuePair.Value))
				{
					return this.ExceptResultOnMatch;
				}
			}
			return this.ExceptResultDefault;
		}

		// Token: 0x060008BB RID: 2235 RVA: 0x0002A7C0 File Offset: 0x000289C0
		public bool AreFlagsSet(params T[] flagTypesToCompare)
		{
			foreach (KeyValuePair<T, bool> keyValuePair in this.flags)
			{
				if (flagTypesToCompare.Contains(keyValuePair.Key) && this.IsComparedFlagsMatch(keyValuePair.Value))
				{
					return this.ComparedFlagsResultOnMatch;
				}
			}
			return this.ComparedFlagsResultDefault;
		}

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x060008BC RID: 2236
		protected abstract bool DefaultValueForMissingFlag { get; }

		// Token: 0x060008BD RID: 2237
		protected abstract bool IsExceptMatch(bool flagValue);

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x060008BE RID: 2238
		protected abstract bool ExceptResultOnMatch { get; }

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x060008BF RID: 2239
		protected abstract bool ExceptResultDefault { get; }

		// Token: 0x060008C0 RID: 2240
		protected abstract bool IsComparedFlagsMatch(bool flagValue);

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x060008C1 RID: 2241
		protected abstract bool ComparedFlagsResultOnMatch { get; }

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x060008C2 RID: 2242
		protected abstract bool ComparedFlagsResultDefault { get; }

		// Token: 0x060008C3 RID: 2243
		protected abstract void CalcResult();

		// Token: 0x060008C4 RID: 2244 RVA: 0x0002A83C File Offset: 0x00028A3C
		private void AddFlag(T t, bool flagOnStart)
		{
			this.flags.Add(t, flagOnStart);
			this.CalcResult();
		}

		// Token: 0x060008C5 RID: 2245 RVA: 0x0002A851 File Offset: 0x00028A51
		protected void SetResultFlagAndNotify(bool resultFlag)
		{
			if (this.currentResultFlag == resultFlag)
			{
				return;
			}
			this.currentResultFlag = resultFlag;
			Action<bool> action = this.onChangedAction;
			if (action == null)
			{
				return;
			}
			action(this.currentResultFlag);
		}

		// Token: 0x04000548 RID: 1352
		protected readonly Dictionary<T, bool> flags = new Dictionary<T, bool>();

		// Token: 0x04000549 RID: 1353
		private bool currentResultFlag;

		// Token: 0x0400054A RID: 1354
		private Action<bool> onChangedAction;
	}
}
