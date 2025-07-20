using System;
using System.Collections.Generic;
using UnityEngine;

namespace XNode
{
	// Token: 0x0200024F RID: 591
	[Serializable]
	public abstract class NodeGraph : BaseData
	{
		// Token: 0x060010F2 RID: 4338 RVA: 0x0004E689 File Offset: 0x0004C889
		public T AddNode<T>() where T : Node
		{
			return this.AddNode(typeof(T)) as T;
		}

		// Token: 0x060010F3 RID: 4339 RVA: 0x0004E6A8 File Offset: 0x0004C8A8
		public virtual Node AddNode(Type type)
		{
			Node.graphHotfix = this;
			Node node = ScriptableObject.CreateInstance(type) as Node;
			node.graph = this;
			this.nodes.Add(node);
			return node;
		}

		// Token: 0x060010F4 RID: 4340 RVA: 0x0004E6DC File Offset: 0x0004C8DC
		public virtual Node CopyNode(Node original)
		{
			Node.graphHotfix = this;
			Node node = Object.Instantiate<Node>(original);
			node.graph = this;
			node.ClearConnections();
			this.nodes.Add(node);
			return node;
		}

		// Token: 0x060010F5 RID: 4341 RVA: 0x0004E710 File Offset: 0x0004C910
		public virtual void RemoveNode(Node node)
		{
			node.ClearConnections();
			this.nodes.Remove(node);
			if (Application.isPlaying)
			{
				Object.Destroy(node);
			}
		}

		// Token: 0x060010F6 RID: 4342 RVA: 0x0004E734 File Offset: 0x0004C934
		public virtual void Clear()
		{
			if (Application.isPlaying)
			{
				for (int i = 0; i < this.nodes.Count; i++)
				{
					Object.Destroy(this.nodes[i]);
				}
			}
			this.nodes.Clear();
		}

		// Token: 0x060010F7 RID: 4343 RVA: 0x0004E77C File Offset: 0x0004C97C
		public virtual NodeGraph Copy()
		{
			NodeGraph nodeGraph = Object.Instantiate<NodeGraph>(this);
			for (int i = 0; i < this.nodes.Count; i++)
			{
				if (!(this.nodes[i] == null))
				{
					Node.graphHotfix = nodeGraph;
					Node node = Object.Instantiate<Node>(this.nodes[i]);
					node.graph = nodeGraph;
					nodeGraph.nodes[i] = node;
				}
			}
			for (int j = 0; j < nodeGraph.nodes.Count; j++)
			{
				if (!(nodeGraph.nodes[j] == null))
				{
					foreach (NodePort nodePort in nodeGraph.nodes[j].Ports)
					{
						nodePort.Redirect(this.nodes, nodeGraph.nodes);
					}
				}
			}
			return nodeGraph;
		}

		// Token: 0x060010F8 RID: 4344 RVA: 0x0004E86C File Offset: 0x0004CA6C
		protected virtual void OnDestroy()
		{
			this.Clear();
		}

		// Token: 0x04000EC8 RID: 3784
		[SerializeField]
		public List<Node> nodes = new List<Node>();

		// Token: 0x02000250 RID: 592
		[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
		public class RequireNodeAttribute : Attribute
		{
			// Token: 0x060010FA RID: 4346 RVA: 0x0004E887 File Offset: 0x0004CA87
			public RequireNodeAttribute(Type type)
			{
				this.type0 = type;
				this.type1 = null;
				this.type2 = null;
			}

			// Token: 0x060010FB RID: 4347 RVA: 0x0004E8A4 File Offset: 0x0004CAA4
			public RequireNodeAttribute(Type type, Type type2)
			{
				this.type0 = type;
				this.type1 = type2;
				this.type2 = null;
			}

			// Token: 0x060010FC RID: 4348 RVA: 0x0004E8C1 File Offset: 0x0004CAC1
			public RequireNodeAttribute(Type type, Type type2, Type type3)
			{
				this.type0 = type;
				this.type1 = type2;
				this.type2 = type3;
			}

			// Token: 0x060010FD RID: 4349 RVA: 0x0004E8DE File Offset: 0x0004CADE
			public bool Requires(Type type)
			{
				return !(type == null) && (type == this.type0 || type == this.type1 || type == this.type2);
			}

			// Token: 0x04000EC9 RID: 3785
			public Type type0;

			// Token: 0x04000ECA RID: 3786
			public Type type1;

			// Token: 0x04000ECB RID: 3787
			public Type type2;
		}
	}
}
