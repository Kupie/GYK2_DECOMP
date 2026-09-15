using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x0200033B RID: 827
[Serializable]
public class FightingPhaseSpawnEnemiesData : FightingPhaseData
{
	// Token: 0x170003CC RID: 972
	// (get) Token: 0x06001603 RID: 5635 RVA: 0x0006A720 File Offset: 0x00068920
	public int TotalEnemiesToSpawn
	{
		get
		{
			return this.enemies.Sum((EnemyData enemy) => enemy.count);
		}
	}

	// Token: 0x06001604 RID: 5636 RVA: 0x0006A74C File Offset: 0x0006894C
	public override void UpdatePhase(float progress, FightingLevelPresetProcessor processor, FightingLevelPreset.FightingLineData line, out int sentToSpawnThisTime)
	{
		sentToSpawnThisTime = 0;
		int num = 0;
		foreach (EnemyData enemyData in this.enemies)
		{
			num += enemyData.count;
		}
		if (num == 0)
		{
			return;
		}
		if (this.spitSpawnSize == -1)
		{
			if (this.totalSpawnedThisPhase == 0)
			{
				if (this.mixEnemiesForSpawn)
				{
					List<string> list = new List<string>();
					foreach (EnemyData enemyData2 in this.enemies)
					{
						int num3;
						int num2 = enemyData2.count - (this.spawnedEnemies.TryGetValue(enemyData2.id, out num3) ? num3 : 0);
						for (int i = 0; i < num2; i++)
						{
							list.Add(enemyData2.id);
						}
					}
					for (int j = list.Count - 1; j > 0; j--)
					{
						int num4 = global::UnityEngine.Random.Range(0, j + 1);
						List<string> list2 = list;
						int num5 = j;
						List<string> list3 = list;
						int num6 = num4;
						string text = list[num4];
						string text2 = list[j];
						list2[num5] = text;
						list3[num6] = text2;
					}
					Dictionary<string, int> dictionary = new Dictionary<string, int>();
					foreach (string text3 in list)
					{
						if (!dictionary.ContainsKey(text3))
						{
							dictionary[text3] = 0;
						}
						Dictionary<string, int> dictionary2 = dictionary;
						string text2 = text3;
						int num6 = dictionary2[text2];
						dictionary2[text2] = num6 + 1;
					}
					using (Dictionary<string, int>.Enumerator enumerator3 = dictionary.GetEnumerator())
					{
						while (enumerator3.MoveNext())
						{
							KeyValuePair<string, int> keyValuePair = enumerator3.Current;
							if (keyValuePair.Value > 0)
							{
								processor.SpawnEnemies(keyValuePair.Key, keyValuePair.Value, line);
								sentToSpawnThisTime += keyValuePair.Value;
								int num7;
								this.spawnedEnemies[keyValuePair.Key] = (this.spawnedEnemies.TryGetValue(keyValuePair.Key, out num7) ? num7 : 0) + keyValuePair.Value;
								this.totalSpawnedThisPhase += keyValuePair.Value;
							}
						}
						return;
					}
				}
				foreach (EnemyData enemyData3 in this.enemies)
				{
					int num9;
					int num8 = enemyData3.count - (this.spawnedEnemies.TryGetValue(enemyData3.id, out num9) ? num9 : 0);
					if (num8 > 0)
					{
						processor.SpawnEnemies(enemyData3.id, num8, line);
						sentToSpawnThisTime += num8;
						int num10;
						this.spawnedEnemies[enemyData3.id] = (this.spawnedEnemies.TryGetValue(enemyData3.id, out num10) ? num10 : 0) + num8;
						this.totalSpawnedThisPhase += num8;
					}
				}
			}
			return;
		}
		int num11 = Mathf.Max(1, this.spitSpawnSize);
		int num12 = Mathf.CeilToInt((float)num / (float)num11);
		int num13 = (Mathf.FloorToInt(Mathf.Clamp01(progress / Mathf.Max(1f, (float)this.duration)) * (float)((num12 > 1) ? (num12 - 1) : 0)) + 1) * num11;
		num13 = Mathf.Min(num13, num);
		int num14 = Mathf.Max(0, num13 - this.totalSpawnedThisPhase);
		if (num14 <= 0)
		{
			return;
		}
		int k = num14;
		if (this.mixEnemiesForSpawn)
		{
			while (k > 0)
			{
				List<string> list4 = new List<string>();
				foreach (EnemyData enemyData4 in this.enemies)
				{
					int num15;
					if ((this.spawnedEnemies.TryGetValue(enemyData4.id, out num15) ? num15 : 0) < enemyData4.count)
					{
						list4.Add(enemyData4.id);
					}
				}
				if (list4.Count == 0)
				{
					break;
				}
				string text4 = list4[global::UnityEngine.Random.Range(0, list4.Count)];
				processor.SpawnEnemies(text4, 1, line);
				sentToSpawnThisTime++;
				int num16;
				this.spawnedEnemies[text4] = (this.spawnedEnemies.TryGetValue(text4, out num16) ? num16 : 0) + 1;
				this.totalSpawnedThisPhase++;
				k--;
			}
		}
		else
		{
			foreach (EnemyData enemyData5 in this.enemies)
			{
				if (k <= 0)
				{
					break;
				}
				int num18;
				int num17 = (this.spawnedEnemies.TryGetValue(enemyData5.id, out num18) ? num18 : 0);
				int num19 = enemyData5.count - num17;
				if (num19 > 0)
				{
					int num20 = Mathf.Min(num19, k);
					processor.SpawnEnemies(enemyData5.id, num20, line);
					sentToSpawnThisTime += num20;
					this.spawnedEnemies[enemyData5.id] = num17 + num20;
					this.totalSpawnedThisPhase += num20;
					k -= num20;
				}
			}
		}
		this.totalBurstSpawnsTriggered = this.totalSpawnedThisPhase / num11;
	}

	// Token: 0x06001605 RID: 5637 RVA: 0x0006ACEC File Offset: 0x00068EEC
	public void FlushRemaining(FightingLevelPresetProcessor processor, FightingLevelPreset.FightingLineData line, out int sentToSpawnThisTime)
	{
		sentToSpawnThisTime = 0;
		foreach (EnemyData enemyData in this.enemies)
		{
			int num2;
			int num = (this.spawnedEnemies.TryGetValue(enemyData.id, out num2) ? num2 : 0);
			int num3 = enemyData.count - num;
			if (num3 > 0)
			{
				processor.SpawnEnemies(enemyData.id, num3, line);
				sentToSpawnThisTime += num3;
				this.spawnedEnemies[enemyData.id] = num + num3;
				this.totalSpawnedThisPhase += num3;
			}
		}
	}

	// Token: 0x06001606 RID: 5638 RVA: 0x0006ADA0 File Offset: 0x00068FA0
	public void Reset()
	{
		this.spawnedEnemies.Clear();
		this.totalSpawnedThisPhase = 0;
		this.totalBurstSpawnsTriggered = 0;
	}

	// Token: 0x04001661 RID: 5729
	public int spitSpawnSize = -1;

	// Token: 0x04001662 RID: 5730
	public bool mixEnemiesForSpawn;

	// Token: 0x04001663 RID: 5731
	public List<EnemyData> enemies = new List<EnemyData>();

	// Token: 0x04001664 RID: 5732
	[NonSerialized]
	private Dictionary<string, int> spawnedEnemies = new Dictionary<string, int>();

	// Token: 0x04001665 RID: 5733
	[NonSerialized]
	private int totalSpawnedThisPhase;

	// Token: 0x04001666 RID: 5734
	[NonSerialized]
	private int totalBurstSpawnsTriggered;
}
