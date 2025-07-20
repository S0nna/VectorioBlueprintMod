using System;

// Token: 0x0200009C RID: 156
public struct ContainerCreationData
{
	// Token: 0x1700007E RID: 126
	// (get) Token: 0x06000618 RID: 1560 RVA: 0x0001F491 File Offset: 0x0001D691
	// (set) Token: 0x06000619 RID: 1561 RVA: 0x0001F499 File Offset: 0x0001D699
	public ContainerType Type { readonly get; set; }

	// Token: 0x1700007F RID: 127
	// (get) Token: 0x0600061A RID: 1562 RVA: 0x0001F4A2 File Offset: 0x0001D6A2
	// (set) Token: 0x0600061B RID: 1563 RVA: 0x0001F4AA File Offset: 0x0001D6AA
	public StorageMode StorageMode { readonly get; set; }

	// Token: 0x17000080 RID: 128
	// (get) Token: 0x0600061C RID: 1564 RVA: 0x0001F4B3 File Offset: 0x0001D6B3
	// (set) Token: 0x0600061D RID: 1565 RVA: 0x0001F4BB File Offset: 0x0001D6BB
	public int Storage { readonly get; set; }

	// Token: 0x17000081 RID: 129
	// (get) Token: 0x0600061E RID: 1566 RVA: 0x0001F4C4 File Offset: 0x0001D6C4
	// (set) Token: 0x0600061F RID: 1567 RVA: 0x0001F4CC File Offset: 0x0001D6CC
	public int StatCode { readonly get; set; }

	// Token: 0x17000082 RID: 130
	// (get) Token: 0x06000620 RID: 1568 RVA: 0x0001F4D5 File Offset: 0x0001D6D5
	// (set) Token: 0x06000621 RID: 1569 RVA: 0x0001F4DD File Offset: 0x0001D6DD
	public ResourceData Filter { readonly get; set; }

	// Token: 0x17000083 RID: 131
	// (get) Token: 0x06000622 RID: 1570 RVA: 0x0001F4E6 File Offset: 0x0001D6E6
	// (set) Token: 0x06000623 RID: 1571 RVA: 0x0001F4EE File Offset: 0x0001D6EE
	public ContainerFlags Flags { readonly get; set; }

	// Token: 0x17000084 RID: 132
	// (get) Token: 0x06000624 RID: 1572 RVA: 0x0001F4F7 File Offset: 0x0001D6F7
	// (set) Token: 0x06000625 RID: 1573 RVA: 0x0001F504 File Offset: 0x0001D704
	public bool Xray
	{
		get
		{
			return (this.Flags & ContainerFlags.Xray) > ContainerFlags.None;
		}
		set
		{
			if (value)
			{
				this.Flags |= ContainerFlags.Xray;
				return;
			}
			this.Flags &= ~ContainerFlags.Xray;
		}
	}

	// Token: 0x17000085 RID: 133
	// (get) Token: 0x06000626 RID: 1574 RVA: 0x0001F527 File Offset: 0x0001D727
	// (set) Token: 0x06000627 RID: 1575 RVA: 0x0001F534 File Offset: 0x0001D734
	public bool GlobalStorage
	{
		get
		{
			return (this.Flags & ContainerFlags.GlobalStorage) > ContainerFlags.None;
		}
		set
		{
			if (value)
			{
				this.Flags |= ContainerFlags.GlobalStorage;
				return;
			}
			this.Flags &= ~ContainerFlags.GlobalStorage;
		}
	}

	// Token: 0x17000086 RID: 134
	// (get) Token: 0x06000628 RID: 1576 RVA: 0x0001F557 File Offset: 0x0001D757
	// (set) Token: 0x06000629 RID: 1577 RVA: 0x0001F564 File Offset: 0x0001D764
	public bool GeneratesResources
	{
		get
		{
			return (this.Flags & ContainerFlags.GeneratesResources) > ContainerFlags.None;
		}
		set
		{
			if (value)
			{
				this.Flags |= ContainerFlags.GeneratesResources;
				return;
			}
			this.Flags &= ~ContainerFlags.GeneratesResources;
		}
	}

	// Token: 0x17000087 RID: 135
	// (get) Token: 0x0600062A RID: 1578 RVA: 0x0001F587 File Offset: 0x0001D787
	// (set) Token: 0x0600062B RID: 1579 RVA: 0x0001F594 File Offset: 0x0001D794
	public bool UseFilter
	{
		get
		{
			return (this.Flags & ContainerFlags.UseFilter) > ContainerFlags.None;
		}
		set
		{
			if (value)
			{
				this.Flags |= ContainerFlags.UseFilter;
				return;
			}
			this.Flags &= ~ContainerFlags.UseFilter;
		}
	}

	// Token: 0x17000088 RID: 136
	// (get) Token: 0x0600062C RID: 1580 RVA: 0x0001F5B7 File Offset: 0x0001D7B7
	// (set) Token: 0x0600062D RID: 1581 RVA: 0x0001F5C5 File Offset: 0x0001D7C5
	public bool RouteInputToOutput
	{
		get
		{
			return (this.Flags & ContainerFlags.RouteInputToOutput) > ContainerFlags.None;
		}
		set
		{
			if (value)
			{
				this.Flags |= ContainerFlags.RouteInputToOutput;
				return;
			}
			this.Flags &= ~ContainerFlags.RouteInputToOutput;
		}
	}
}
