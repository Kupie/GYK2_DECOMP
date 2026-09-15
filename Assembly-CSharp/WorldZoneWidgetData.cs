using System;
using System.Collections.Generic;
using LazyBearTechnology;

// Token: 0x020008D8 RID: 2264
public class WorldZoneWidgetData : LazyWidgetDataBase
{
	// Token: 0x170008E1 RID: 2273
	// (get) Token: 0x06003AFD RID: 15101 RVA: 0x00119EC0 File Offset: 0x001180C0
	// (set) Token: 0x06003AFE RID: 15102 RVA: 0x00119EC8 File Offset: 0x001180C8
	public WorldZoneData WorldZoneData { get; private set; }

	// Token: 0x170008E2 RID: 2274
	// (get) Token: 0x06003AFF RID: 15103 RVA: 0x00119ED1 File Offset: 0x001180D1
	// (set) Token: 0x06003B00 RID: 15104 RVA: 0x00119ED9 File Offset: 0x001180D9
	public bool InsideTown { get; private set; }

	// Token: 0x170008E3 RID: 2275
	// (get) Token: 0x06003B01 RID: 15105 RVA: 0x00119EE2 File Offset: 0x001180E2
	// (set) Token: 0x06003B02 RID: 15106 RVA: 0x00119EEA File Offset: 0x001180EA
	public TownSubZone TownSubZone { get; private set; }

	// Token: 0x06003B03 RID: 15107 RVA: 0x00119EF4 File Offset: 0x001180F4
	public WorldZoneWidgetData()
	{
		this.WorldZoneData = MainGame.PlayerData.CurrentWorldZoneData;
		this.InsideTown = MainGame.PlayerData.insideTownZones.Count > 0;
		TownSubZone townSubZone;
		if (MainGame.PlayerData.insideTownSubZones.Count <= 0)
		{
			townSubZone = null;
		}
		else
		{
			List<TownSubZone> insideTownSubZones = MainGame.PlayerData.insideTownSubZones;
			townSubZone = insideTownSubZones[insideTownSubZones.Count - 1];
		}
		this.TownSubZone = townSubZone;
	}

	// Token: 0x06003B04 RID: 15108 RVA: 0x00119F61 File Offset: 0x00118161
	public WorldZoneWidgetData(WorldZoneData worldZoneData)
		: this()
	{
		this.WorldZoneData = worldZoneData;
	}
}
