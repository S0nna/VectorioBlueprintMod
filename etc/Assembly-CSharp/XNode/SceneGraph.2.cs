using System;

namespace XNode
{
	// Token: 0x02000255 RID: 597
	public class SceneGraph<T> : SceneGraph where T : NodeGraph
	{
		// Token: 0x17000204 RID: 516
		// (get) Token: 0x0600112D RID: 4397 RVA: 0x0004F4ED File Offset: 0x0004D6ED
		// (set) Token: 0x0600112E RID: 4398 RVA: 0x0004F4FF File Offset: 0x0004D6FF
		public new T graph
		{
			get
			{
				return this.graph as T;
			}
			set
			{
				this.graph = value;
			}
		}
	}
}
