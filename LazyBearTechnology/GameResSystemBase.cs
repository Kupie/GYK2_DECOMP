using System;

namespace LazyBearTechnology
{
	// Token: 0x020000F5 RID: 245
	public abstract class GameResSystemBase
	{
		// Token: 0x06000477 RID: 1143 RVA: 0x00017911 File Offset: 0x00015B11
		protected GameResSystemBase(string gameResAtomName, GameRes gameRes)
		{
			this.gameResAtomName = gameResAtomName;
			this.resForChanges = gameRes;
		}

		// Token: 0x06000478 RID: 1144 RVA: 0x00017927 File Offset: 0x00015B27
		public virtual void Set(float value, bool silent = false)
		{
			this.resForChanges.SetWithoutSystemsCheck(this.gameResAtomName, value);
			if (!silent)
			{
				Action<float> action = this.onValueChanged;
				if (action == null)
				{
					return;
				}
				action(this.resForChanges.Get(this.gameResAtomName, 0f));
			}
		}

		// Token: 0x06000479 RID: 1145 RVA: 0x00017964 File Offset: 0x00015B64
		public virtual void Add(float value, bool silent = false)
		{
			this.resForChanges.AddWithoutSystemsCheck(this.gameResAtomName, value);
			if (!silent)
			{
				Action<float> action = this.onValueChanged;
				if (action == null)
				{
					return;
				}
				action(this.resForChanges.Get(this.gameResAtomName, 0f));
			}
		}

		// Token: 0x0600047A RID: 1146 RVA: 0x000179A1 File Offset: 0x00015BA1
		public virtual float Get()
		{
			return this.resForChanges.GetWithoutSystemsCheck(this.gameResAtomName, 0f);
		}

		// Token: 0x04000214 RID: 532
		public Action<float> onValueChanged;

		// Token: 0x04000215 RID: 533
		protected readonly GameRes resForChanges;

		// Token: 0x04000216 RID: 534
		protected readonly string gameResAtomName;
	}
}
