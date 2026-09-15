using System;
using System.Collections.Generic;
using FlowCanvas;
using ParadoxNotion.Design;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000BCF RID: 3023
	[Name("Has Prepared Resurrection Table", 0)]
	[Category("Game/Environment")]
	[Color("313c8f")]
	public class Flow_HasPreparedResurrectionTable : GKCustomFlowNode
	{
		// Token: 0x06004E73 RID: 20083 RVA: 0x00171D21 File Offset: 0x0016FF21
		protected override void RegisterPorts()
		{
			this.hasPreparedResurrectionTable = base.AddValueOutput<bool>("isSelectedDayToday", new ValueHandler<bool>(this.HasPreparedResurrectionTable), "");
		}

		// Token: 0x06004E74 RID: 20084 RVA: 0x00171D48 File Offset: 0x0016FF48
		private bool HasPreparedResurrectionTable()
		{
			List<WgoData> wgoDataList = MainGame.WorldData.GetWgoDataList("resurrection_table_1");
			if (wgoDataList.Count == 0)
			{
				return false;
			}
			using (List<WgoData>.Enumerator enumerator = wgoDataList.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.GetGameResInt("resurrection_prepared") == 1)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x04003F9B RID: 16283
		private ValueOutput<bool> hasPreparedResurrectionTable;
	}
}
