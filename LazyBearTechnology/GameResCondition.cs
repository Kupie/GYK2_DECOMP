using System;
using System.Collections.Generic;
using UnityEngine;

namespace LazyBearTechnology
{
	// Token: 0x020000F3 RID: 243
	[Serializable]
	public class GameResCondition
	{
		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x0600046C RID: 1132 RVA: 0x000176CA File Offset: 0x000158CA
		public List<string> TypesList
		{
			get
			{
				return this.resType;
			}
		}

		// Token: 0x0600046E RID: 1134 RVA: 0x000176FC File Offset: 0x000158FC
		public bool CheckCondition(GameRes gameRes)
		{
			return this.CheckCondition((string id) => gameRes.Get(id, 0f));
		}

		// Token: 0x0600046F RID: 1135 RVA: 0x00017728 File Offset: 0x00015928
		public bool CheckCondition(GameResInt gameResInt)
		{
			return this.CheckCondition((string id) => (float)gameResInt.Get(id));
		}

		// Token: 0x06000470 RID: 1136 RVA: 0x00017754 File Offset: 0x00015954
		private bool CheckCondition(Func<string, float> getFunction)
		{
			for (int i = 0; i < this.resValues.Count; i++)
			{
				float num = getFunction(this.resType[i]);
				float num2 = this.resValues[i];
				bool flag;
				switch (this.resCond[i])
				{
				case GameResCondition.Condition.Equals:
					flag = Mathf.Approximately(num, num2);
					break;
				case GameResCondition.Condition.More:
					flag = num > num2;
					break;
				case GameResCondition.Condition.Less:
					flag = num < num2;
					break;
				case GameResCondition.Condition.MoreEq:
					flag = num >= num2;
					break;
				case GameResCondition.Condition.LessEq:
					flag = num <= num2;
					break;
				default:
					throw new Exception(string.Format("Condition {0} not implemented.", this.resCond[i]));
				}
				if (!flag)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000471 RID: 1137 RVA: 0x00017820 File Offset: 0x00015A20
		public static string ConditionToString(GameResCondition.Condition condition)
		{
			string text;
			switch (condition)
			{
			case GameResCondition.Condition.Equals:
				text = "==";
				break;
			case GameResCondition.Condition.More:
				text = ">";
				break;
			case GameResCondition.Condition.Less:
				text = "<";
				break;
			case GameResCondition.Condition.MoreEq:
				text = ">=";
				break;
			case GameResCondition.Condition.LessEq:
				text = "<=";
				break;
			default:
				text = "?";
				break;
			}
			return text;
		}

		// Token: 0x0400020E RID: 526
		[SerializeField]
		private List<float> resValues = new List<float>();

		// Token: 0x0400020F RID: 527
		[SerializeField]
		private List<string> resType = new List<string>();

		// Token: 0x04000210 RID: 528
		[SerializeField]
		private List<GameResCondition.Condition> resCond = new List<GameResCondition.Condition>();

		// Token: 0x020001D2 RID: 466
		[Serializable]
		public enum Condition
		{
			// Token: 0x0400062B RID: 1579
			Null,
			// Token: 0x0400062C RID: 1580
			Equals,
			// Token: 0x0400062D RID: 1581
			More,
			// Token: 0x0400062E RID: 1582
			Less,
			// Token: 0x0400062F RID: 1583
			MoreEq,
			// Token: 0x04000630 RID: 1584
			LessEq
		}
	}
}
