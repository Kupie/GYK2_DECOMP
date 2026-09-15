using System;

namespace LazyBearTechnology
{
	// Token: 0x02000165 RID: 357
	public abstract class LazyWidget<T> : LazyWidgetBase where T : LazyWidgetDataBase
	{
		// Token: 0x060007AB RID: 1963 RVA: 0x00027170 File Offset: 0x00025370
		public virtual void Draw(T data)
		{
			this.SetData(data);
			base.Draw();
			this.Redraw();
		}

		// Token: 0x060007AC RID: 1964 RVA: 0x00027185 File Offset: 0x00025385
		public override void Draw(LazyWidgetDataBase data)
		{
			this.Draw(data as T);
		}

		// Token: 0x060007AD RID: 1965 RVA: 0x00027198 File Offset: 0x00025398
		public override void Redraw()
		{
		}

		// Token: 0x060007AE RID: 1966 RVA: 0x0002719A File Offset: 0x0002539A
		public override Type GetDataType()
		{
			return typeof(T);
		}

		// Token: 0x060007AF RID: 1967 RVA: 0x000271A6 File Offset: 0x000253A6
		public override void Hide()
		{
			base.Hide();
		}

		// Token: 0x060007B0 RID: 1968 RVA: 0x000271AE File Offset: 0x000253AE
		protected virtual void SetData(T data)
		{
			this.data = data;
		}

		// Token: 0x040004BD RID: 1213
		protected T data;
	}
}
