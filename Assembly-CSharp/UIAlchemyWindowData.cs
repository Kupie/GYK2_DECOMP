using System;
using LazyBearTechnology;

// Token: 0x020008E1 RID: 2273
public class UIAlchemyWindowData : LazyWidgetDataBase
{
	// Token: 0x170008EB RID: 2283
	// (get) Token: 0x06003B56 RID: 15190 RVA: 0x0011C0C6 File Offset: 0x0011A2C6
	// (set) Token: 0x06003B57 RID: 15191 RVA: 0x0011C0CE File Offset: 0x0011A2CE
	public Wgo Wgo { get; private set; }

	// Token: 0x06003B58 RID: 15192 RVA: 0x0011C0D7 File Offset: 0x0011A2D7
	public UIAlchemyWindowData(Wgo wgo)
	{
		this.Wgo = wgo;
		this.Wgo.Data.TrySetWorker(MainGame.PlayerController, null);
	}
}
