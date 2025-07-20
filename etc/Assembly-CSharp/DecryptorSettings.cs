using System;

// Token: 0x020001AE RID: 430
public class DecryptorSettings : EntitySettings
{
	// Token: 0x06000DE9 RID: 3561 RVA: 0x0003DE44 File Offset: 0x0003C044
	public override void Set(EntityComponent component)
	{
		Decryptor decryptor = component as Decryptor;
		if (decryptor != null)
		{
			this._decryptor = decryptor;
		}
	}

	// Token: 0x06000DEA RID: 3562 RVA: 0x00003212 File Offset: 0x00001412
	public override void CustomUpdate()
	{
	}

	// Token: 0x06000DEB RID: 3563 RVA: 0x0003DE62 File Offset: 0x0003C062
	public void Open()
	{
		this._decryptor.OnQuickEdit();
	}

	// Token: 0x06000DEC RID: 3564 RVA: 0x00003212 File Offset: 0x00001412
	public override void Clear()
	{
	}

	// Token: 0x04000A4C RID: 2636
	private Decryptor _decryptor;
}
