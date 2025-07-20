using System;

// Token: 0x020001A8 RID: 424
public class ColorPalette : Singleton<ColorPalette>
{
	// Token: 0x170001A9 RID: 425
	// (get) Token: 0x06000DD7 RID: 3543 RVA: 0x0003D6F5 File Offset: 0x0003B8F5
	public Accent Color
	{
		get
		{
			return this._color;
		}
	}

	// Token: 0x04000A1B RID: 2587
	private Accent _color;
}
