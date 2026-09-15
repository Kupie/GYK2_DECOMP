using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace LazyBearTechnology
{
	// Token: 0x020000F1 RID: 241
	[Serializable]
	public class GameRes
	{
		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x06000436 RID: 1078 RVA: 0x00016CDC File Offset: 0x00014EDC
		public Dictionary<string, GameResSystemBase> GameResSystems
		{
			get
			{
				return this.gameResSystems ?? new Dictionary<string, GameResSystemBase>();
			}
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x06000437 RID: 1079 RVA: 0x00016CED File Offset: 0x00014EED
		public List<GameResAtom> List
		{
			get
			{
				return this.resValues;
			}
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x06000438 RID: 1080 RVA: 0x00016CF5 File Offset: 0x00014EF5
		public List<string> TypesList
		{
			get
			{
				return this.resType;
			}
		}

		// Token: 0x06000439 RID: 1081 RVA: 0x00016CFD File Offset: 0x00014EFD
		public GameRes()
		{
		}

		// Token: 0x0600043A RID: 1082 RVA: 0x00016D1C File Offset: 0x00014F1C
		public GameRes(GameRes gameRes)
		{
			int count = gameRes.List.Count;
			for (int i = 0; i < count; i++)
			{
				this.Add(gameRes.List[i]);
			}
		}

		// Token: 0x0600043B RID: 1083 RVA: 0x00016D6F File Offset: 0x00014F6F
		public GameRes(string stype, float value)
		{
			this.Set(stype, value);
		}

		// Token: 0x0600043C RID: 1084 RVA: 0x00016D98 File Offset: 0x00014F98
		public GameRes(List<GameResAtom> atoms)
		{
			if (atoms != null)
			{
				for (int i = 0; i < atoms.Count; i++)
				{
					this.Add(atoms[i]);
				}
			}
		}

		// Token: 0x0600043D RID: 1085 RVA: 0x00016DE2 File Offset: 0x00014FE2
		public void SetSystems(Dictionary<string, GameResSystemBase> gameResAtomSystems)
		{
			this.gameResSystems = gameResAtomSystems;
		}

		// Token: 0x0600043E RID: 1086 RVA: 0x00016DEB File Offset: 0x00014FEB
		public GameResSystemBase GetSystem(string stype)
		{
			return this.GameResSystems[stype];
		}

		// Token: 0x0600043F RID: 1087 RVA: 0x00016DFC File Offset: 0x00014FFC
		public float Get(string stype, float defaultValue = 0f)
		{
			GameResSystemBase gameResSystemBase;
			if (this.GameResSystems.TryGetValue(stype, out gameResSystemBase))
			{
				return gameResSystemBase.Get();
			}
			return this.GetWithoutSystemsCheck(stype, defaultValue);
		}

		// Token: 0x06000440 RID: 1088 RVA: 0x00016E28 File Offset: 0x00015028
		public float GetWithoutSystemsCheck(string stype, float defaultValue = 0f)
		{
			int num = this.resType.IndexOf(stype);
			if (num != -1)
			{
				return this.resValues[num].value;
			}
			return defaultValue;
		}

		// Token: 0x06000441 RID: 1089 RVA: 0x00016E59 File Offset: 0x00015059
		public int GetInt(string stype)
		{
			return (int)this.Get(stype, 0f);
		}

		// Token: 0x06000442 RID: 1090 RVA: 0x00016E68 File Offset: 0x00015068
		public bool Has(string stype)
		{
			return this.resType.Contains(stype);
		}

		// Token: 0x06000443 RID: 1091 RVA: 0x00016E76 File Offset: 0x00015076
		public void Clear()
		{
			this.resType.Clear();
			this.resValues.Clear();
		}

		// Token: 0x06000444 RID: 1092 RVA: 0x00016E90 File Offset: 0x00015090
		public void Set(string stype, float value)
		{
			GameResSystemBase gameResSystemBase;
			if (this.GameResSystems.TryGetValue(stype, out gameResSystemBase))
			{
				gameResSystemBase.Set(value, false);
				return;
			}
			this.SetWithoutSystemsCheck(stype, value);
		}

		// Token: 0x06000445 RID: 1093 RVA: 0x00016EC0 File Offset: 0x000150C0
		public void SetWithoutSystemsCheck(string stype, float value)
		{
			int num = this.resType.IndexOf(stype);
			if (num != -1)
			{
				this.resValues[num].value = value;
				return;
			}
			this.resType.Add(stype);
			this.resValues.Add(new GameResAtom(stype, value));
		}

		// Token: 0x06000446 RID: 1094 RVA: 0x00016F10 File Offset: 0x00015110
		public void Set(GameRes gameRes)
		{
			int count = gameRes.List.Count;
			for (int i = 0; i < count; i++)
			{
				this.Set(gameRes.List[i].type, gameRes.List[i].value);
			}
		}

		// Token: 0x06000447 RID: 1095 RVA: 0x00016F60 File Offset: 0x00015160
		public void Add(GameRes gameRes)
		{
			int count = gameRes.List.Count;
			for (int i = 0; i < count; i++)
			{
				this.Add(gameRes.List[i]);
			}
		}

		// Token: 0x06000448 RID: 1096 RVA: 0x00016F97 File Offset: 0x00015197
		public void Add(GameResAtom gameResAtom)
		{
			if (gameResAtom.IsEmpty())
			{
				return;
			}
			this.Add(gameResAtom.type, gameResAtom.value);
		}

		// Token: 0x06000449 RID: 1097 RVA: 0x00016FB4 File Offset: 0x000151B4
		public void Add(string stype, float value)
		{
			GameResSystemBase gameResSystemBase;
			if (this.GameResSystems.TryGetValue(stype, out gameResSystemBase))
			{
				gameResSystemBase.Add(value, false);
				return;
			}
			this.AddWithoutSystemsCheck(stype, value);
		}

		// Token: 0x0600044A RID: 1098 RVA: 0x00016FE4 File Offset: 0x000151E4
		public void AddWithoutSystemsCheck(string stype, float value)
		{
			int num = this.resType.IndexOf(stype);
			if (num != -1)
			{
				this.resValues[num].value += value;
				return;
			}
			this.resType.Add(stype);
			this.resValues.Add(new GameResAtom(stype, value));
		}

		// Token: 0x0600044B RID: 1099 RVA: 0x0001703C File Offset: 0x0001523C
		public void Sub(GameRes gameRes)
		{
			int count = gameRes.List.Count;
			for (int i = 0; i < count; i++)
			{
				this.Sub(gameRes.List[i]);
			}
		}

		// Token: 0x0600044C RID: 1100 RVA: 0x00017073 File Offset: 0x00015273
		public void Sub(GameResAtom gameResAtom)
		{
			if (gameResAtom.IsEmpty())
			{
				return;
			}
			this.Add(gameResAtom.type, -gameResAtom.value);
		}

		// Token: 0x0600044D RID: 1101 RVA: 0x00017091 File Offset: 0x00015291
		public void Sub(string stype, float value)
		{
			this.Add(stype, -value);
		}

		// Token: 0x0600044E RID: 1102 RVA: 0x0001709C File Offset: 0x0001529C
		public void Multiply(string stype, float value)
		{
			float num = this.Get(stype, 0f);
			if ((double)Mathf.Abs(num) > 0.0001)
			{
				this.Set(stype, num * value);
			}
		}

		// Token: 0x0600044F RID: 1103 RVA: 0x000170D4 File Offset: 0x000152D4
		public GameRes Clone()
		{
			GameRes gameRes = new GameRes();
			for (int i = this.resType.Count - 1; i >= 0; i--)
			{
				gameRes.Set(this.resType[i], this.resValues[i].value);
			}
			return gameRes;
		}

		// Token: 0x06000450 RID: 1104 RVA: 0x00017124 File Offset: 0x00015324
		public void RemoveZeroValues()
		{
			List<int> list = new List<int>();
			for (int i = this.resType.Count - 1; i >= 0; i--)
			{
				if ((double)Mathf.Abs(this.resValues[i].value) < 0.0001)
				{
					list.Add(i);
				}
			}
			for (int j = 0; j < list.Count; j++)
			{
				this.resType.RemoveAt(list[j]);
				this.resValues.RemoveAt(list[j]);
			}
		}

		// Token: 0x06000451 RID: 1105 RVA: 0x000171B0 File Offset: 0x000153B0
		public void Sort(Comparison<GameResAtom> comparison)
		{
			this.resValues.Sort(comparison);
			List<string> list = new List<string>(this.resType);
			foreach (string text in this.resType)
			{
				int num = 0;
				for (int i = 0; i < this.resValues.Count; i++)
				{
					if (this.resValues[i].type == text)
					{
						num = i;
						break;
					}
				}
				list[num] = text;
			}
			this.resType = new List<string>(list);
		}

		// Token: 0x06000452 RID: 1106 RVA: 0x00017264 File Offset: 0x00015464
		public static GameRes operator +(GameRes r1, GameRes r2)
		{
			GameRes gameRes = r1.Clone();
			int count = r2.List.Count;
			for (int i = 0; i < count; i++)
			{
				gameRes.Add(r2.List[i]);
			}
			gameRes.RemoveZeroValues();
			return gameRes;
		}

		// Token: 0x06000453 RID: 1107 RVA: 0x000172AC File Offset: 0x000154AC
		public static GameRes operator -(GameRes r1, GameRes r2)
		{
			GameRes gameRes = r1.Clone();
			int count = r2.List.Count;
			for (int i = 0; i < count; i++)
			{
				gameRes.Sub(r2.List[i]);
			}
			gameRes.RemoveZeroValues();
			return gameRes;
		}

		// Token: 0x06000454 RID: 1108 RVA: 0x000172F4 File Offset: 0x000154F4
		public bool IsEnough(GameRes sub)
		{
			bool flag = true;
			List<GameResAtom> list = sub.List;
			for (int i = 0; i < list.Count; i++)
			{
				GameResAtom gameResAtom = list[i];
				if (!this.IsEnough(gameResAtom.type, gameResAtom.value))
				{
					flag = false;
					break;
				}
			}
			return flag;
		}

		// Token: 0x06000455 RID: 1109 RVA: 0x0001733C File Offset: 0x0001553C
		public bool IsEnough(GameResAtom r)
		{
			return this.IsEnough(r.type, r.value);
		}

		// Token: 0x06000456 RID: 1110 RVA: 0x00017350 File Offset: 0x00015550
		public bool IsEnough(string type, float value)
		{
			bool flag = true;
			if (this.Get(type, 0f) < value)
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x06000457 RID: 1111 RVA: 0x00017374 File Offset: 0x00015574
		public static GameRes operator *(GameRes r1, float k)
		{
			GameRes gameRes = new GameRes();
			foreach (GameResAtom gameResAtom in r1.List)
			{
				gameRes.Set(gameResAtom.type, r1.Get(gameResAtom.type, 0f) * k);
			}
			gameRes.RemoveZeroValues();
			return gameRes;
		}

		// Token: 0x06000458 RID: 1112 RVA: 0x000173EC File Offset: 0x000155EC
		public static GameRes operator /(GameRes r1, float k)
		{
			return r1 * (1f / k);
		}

		// Token: 0x06000459 RID: 1113 RVA: 0x000173FB File Offset: 0x000155FB
		public static bool operator <(GameRes r1, GameRes r2)
		{
			return r2.IsEnough(r1);
		}

		// Token: 0x0600045A RID: 1114 RVA: 0x00017404 File Offset: 0x00015604
		public static bool operator <=(GameRes r1, GameRes r2)
		{
			return (r2 - r1).IsEmpty() || r1 < r2;
		}

		// Token: 0x0600045B RID: 1115 RVA: 0x0001741D File Offset: 0x0001561D
		public static bool operator >(GameRes r1, GameRes r2)
		{
			return r1.IsEnough(r2);
		}

		// Token: 0x0600045C RID: 1116 RVA: 0x00017426 File Offset: 0x00015626
		public static bool operator >=(GameRes r1, GameRes r2)
		{
			return (r1 - r2).IsEmpty() || r1 > r2;
		}

		// Token: 0x0600045D RID: 1117 RVA: 0x0001743F File Offset: 0x0001563F
		public static bool operator ==(GameRes r1, GameRes r2)
		{
			return r1 == r2 || (r1 != null && r2 != null && (r1 - r2).IsEmpty());
		}

		// Token: 0x0600045E RID: 1118 RVA: 0x0001745B File Offset: 0x0001565B
		public static bool operator !=(GameRes r1, GameRes r2)
		{
			return !(r1 == r2);
		}

		// Token: 0x0600045F RID: 1119 RVA: 0x00017467 File Offset: 0x00015667
		public bool IsEmpty()
		{
			this.RemoveZeroValues();
			return this.resType.Count == 0;
		}

		// Token: 0x06000460 RID: 1120 RVA: 0x00017480 File Offset: 0x00015680
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder("[GameRes: ");
			for (int i = 0; i < this.resType.Count; i++)
			{
				if (i > 0)
				{
					stringBuilder.Append(", ");
				}
				string text = this.resType[i];
				stringBuilder.Append(text);
				stringBuilder.Append("=");
				stringBuilder.Append(this.Get(text, 0f));
			}
			stringBuilder.Append("]");
			return stringBuilder.ToString();
		}

		// Token: 0x06000461 RID: 1121 RVA: 0x00017504 File Offset: 0x00015704
		public void RemoveAllBut(List<string> exceptions)
		{
			for (int i = 0; i < this.resType.Count; i++)
			{
				if (!exceptions.Contains(this.resType[i]))
				{
					this.resValues[i].value = 0f;
				}
			}
			this.RemoveZeroValues();
		}

		// Token: 0x06000462 RID: 1122 RVA: 0x00017558 File Offset: 0x00015758
		public override bool Equals(object obj)
		{
			GameRes gameRes = obj as GameRes;
			return gameRes != null && EqualityComparer<List<GameResAtom>>.Default.Equals(this.resValues, gameRes.resValues) && EqualityComparer<List<string>>.Default.Equals(this.resType, gameRes.resType) && EqualityComparer<List<GameResAtom>>.Default.Equals(this.List, gameRes.List);
		}

		// Token: 0x06000463 RID: 1123 RVA: 0x000175C0 File Offset: 0x000157C0
		public override int GetHashCode()
		{
			return ((-1419608871 * -1521134295 + EqualityComparer<List<GameResAtom>>.Default.GetHashCode(this.resValues)) * -1521134295 + EqualityComparer<List<string>>.Default.GetHashCode(this.resType)) * -1521134295 + EqualityComparer<List<GameResAtom>>.Default.GetHashCode(this.List);
		}

		// Token: 0x04000209 RID: 521
		[SerializeField]
		private List<GameResAtom> resValues = new List<GameResAtom>();

		// Token: 0x0400020A RID: 522
		[SerializeField]
		private List<string> resType = new List<string>();

		// Token: 0x0400020B RID: 523
		private Dictionary<string, GameResSystemBase> gameResSystems;
	}
}
