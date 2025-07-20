using System;
using System.Collections.Generic;
using UnityEngine;

namespace XNode
{
	// Token: 0x02000239 RID: 569
	[Serializable]
	public abstract class Node : BaseData
	{
		// Token: 0x170001DE RID: 478
		// (get) Token: 0x06001074 RID: 4212 RVA: 0x0004CF8A File Offset: 0x0004B18A
		[Obsolete("Use DynamicPorts instead")]
		public IEnumerable<NodePort> InstancePorts
		{
			get
			{
				return this.DynamicPorts;
			}
		}

		// Token: 0x170001DF RID: 479
		// (get) Token: 0x06001075 RID: 4213 RVA: 0x0004CF92 File Offset: 0x0004B192
		[Obsolete("Use DynamicOutputs instead")]
		public IEnumerable<NodePort> InstanceOutputs
		{
			get
			{
				return this.DynamicOutputs;
			}
		}

		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x06001076 RID: 4214 RVA: 0x0004CF9A File Offset: 0x0004B19A
		[Obsolete("Use DynamicInputs instead")]
		public IEnumerable<NodePort> InstanceInputs
		{
			get
			{
				return this.DynamicInputs;
			}
		}

		// Token: 0x06001077 RID: 4215 RVA: 0x0004CFA2 File Offset: 0x0004B1A2
		[Obsolete("Use AddDynamicInput instead")]
		public NodePort AddInstanceInput(Type type, Node.ConnectionType connectionType = Node.ConnectionType.Multiple, Node.TypeConstraint typeConstraint = Node.TypeConstraint.None, string fieldName = null)
		{
			return this.AddDynamicInput(type, connectionType, typeConstraint, fieldName);
		}

		// Token: 0x06001078 RID: 4216 RVA: 0x0004CFAF File Offset: 0x0004B1AF
		[Obsolete("Use AddDynamicOutput instead")]
		public NodePort AddInstanceOutput(Type type, Node.ConnectionType connectionType = Node.ConnectionType.Multiple, Node.TypeConstraint typeConstraint = Node.TypeConstraint.None, string fieldName = null)
		{
			return this.AddDynamicOutput(type, connectionType, typeConstraint, fieldName);
		}

		// Token: 0x06001079 RID: 4217 RVA: 0x0004CFBC File Offset: 0x0004B1BC
		[Obsolete("Use AddDynamicPort instead")]
		private NodePort AddInstancePort(Type type, NodePort.IO direction, Node.ConnectionType connectionType = Node.ConnectionType.Multiple, Node.TypeConstraint typeConstraint = Node.TypeConstraint.None, string fieldName = null)
		{
			return this.AddDynamicPort(type, direction, connectionType, typeConstraint, fieldName);
		}

		// Token: 0x0600107A RID: 4218 RVA: 0x0004CFCB File Offset: 0x0004B1CB
		[Obsolete("Use RemoveDynamicPort instead")]
		public void RemoveInstancePort(string fieldName)
		{
			this.RemoveDynamicPort(fieldName);
		}

		// Token: 0x0600107B RID: 4219 RVA: 0x0004CFD4 File Offset: 0x0004B1D4
		[Obsolete("Use RemoveDynamicPort instead")]
		public void RemoveInstancePort(NodePort port)
		{
			this.RemoveDynamicPort(port);
		}

		// Token: 0x0600107C RID: 4220 RVA: 0x0004CFDD File Offset: 0x0004B1DD
		[Obsolete("Use ClearDynamicPorts instead")]
		public void ClearInstancePorts()
		{
			this.ClearDynamicPorts();
		}

		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x0600107D RID: 4221 RVA: 0x0004CFE5 File Offset: 0x0004B1E5
		public IEnumerable<NodePort> Ports
		{
			get
			{
				foreach (NodePort nodePort in this.ports.Values)
				{
					yield return nodePort;
				}
				Dictionary<string, NodePort>.ValueCollection.Enumerator enumerator = default(Dictionary<string, NodePort>.ValueCollection.Enumerator);
				yield break;
				yield break;
			}
		}

		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x0600107E RID: 4222 RVA: 0x0004CFF5 File Offset: 0x0004B1F5
		public IEnumerable<NodePort> Outputs
		{
			get
			{
				foreach (NodePort nodePort in this.Ports)
				{
					if (nodePort.IsOutput)
					{
						yield return nodePort;
					}
				}
				IEnumerator<NodePort> enumerator = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x0600107F RID: 4223 RVA: 0x0004D005 File Offset: 0x0004B205
		public IEnumerable<NodePort> Inputs
		{
			get
			{
				foreach (NodePort nodePort in this.Ports)
				{
					if (nodePort.IsInput)
					{
						yield return nodePort;
					}
				}
				IEnumerator<NodePort> enumerator = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x06001080 RID: 4224 RVA: 0x0004D015 File Offset: 0x0004B215
		public IEnumerable<NodePort> DynamicPorts
		{
			get
			{
				foreach (NodePort nodePort in this.Ports)
				{
					if (nodePort.IsDynamic)
					{
						yield return nodePort;
					}
				}
				IEnumerator<NodePort> enumerator = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x06001081 RID: 4225 RVA: 0x0004D025 File Offset: 0x0004B225
		public IEnumerable<NodePort> DynamicOutputs
		{
			get
			{
				foreach (NodePort nodePort in this.Ports)
				{
					if (nodePort.IsDynamic && nodePort.IsOutput)
					{
						yield return nodePort;
					}
				}
				IEnumerator<NodePort> enumerator = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x06001082 RID: 4226 RVA: 0x0004D035 File Offset: 0x0004B235
		public IEnumerable<NodePort> DynamicInputs
		{
			get
			{
				foreach (NodePort nodePort in this.Ports)
				{
					if (nodePort.IsDynamic && nodePort.IsInput)
					{
						yield return nodePort;
					}
				}
				IEnumerator<NodePort> enumerator = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x06001083 RID: 4227 RVA: 0x0004D045 File Offset: 0x0004B245
		protected void OnEnable()
		{
			if (Node.graphHotfix != null)
			{
				this.graph = Node.graphHotfix;
			}
			Node.graphHotfix = null;
			this.UpdatePorts();
			this.Init();
		}

		// Token: 0x06001084 RID: 4228 RVA: 0x0004D071 File Offset: 0x0004B271
		public void UpdatePorts()
		{
			NodeDataCache.UpdatePorts(this, this.ports);
		}

		// Token: 0x06001085 RID: 4229 RVA: 0x00003212 File Offset: 0x00001412
		protected virtual void Init()
		{
		}

		// Token: 0x06001086 RID: 4230 RVA: 0x0004D080 File Offset: 0x0004B280
		public void VerifyConnections()
		{
			foreach (NodePort nodePort in this.Ports)
			{
				nodePort.VerifyConnections();
			}
		}

		// Token: 0x06001087 RID: 4231 RVA: 0x0004D0CC File Offset: 0x0004B2CC
		public NodePort AddDynamicInput(Type type, Node.ConnectionType connectionType = Node.ConnectionType.Multiple, Node.TypeConstraint typeConstraint = Node.TypeConstraint.None, string fieldName = null)
		{
			return this.AddDynamicPort(type, NodePort.IO.Input, connectionType, typeConstraint, fieldName);
		}

		// Token: 0x06001088 RID: 4232 RVA: 0x0004D0DA File Offset: 0x0004B2DA
		public NodePort AddDynamicOutput(Type type, Node.ConnectionType connectionType = Node.ConnectionType.Multiple, Node.TypeConstraint typeConstraint = Node.TypeConstraint.None, string fieldName = null)
		{
			return this.AddDynamicPort(type, NodePort.IO.Output, connectionType, typeConstraint, fieldName);
		}

		// Token: 0x06001089 RID: 4233 RVA: 0x0004D0E8 File Offset: 0x0004B2E8
		private NodePort AddDynamicPort(Type type, NodePort.IO direction, Node.ConnectionType connectionType = Node.ConnectionType.Multiple, Node.TypeConstraint typeConstraint = Node.TypeConstraint.None, string fieldName = null)
		{
			if (fieldName == null)
			{
				fieldName = "dynamicInput_0";
				int num = 0;
				while (this.HasPort(fieldName))
				{
					string str = "dynamicInput_";
					int num2;
					num = (num2 = num + 1);
					fieldName = str + num2.ToString();
				}
			}
			else if (this.HasPort(fieldName))
			{
				Debug.LogWarning("Port '" + fieldName + "' already exists in " + base.name, this);
				return this.ports[fieldName];
			}
			NodePort nodePort = new NodePort(fieldName, type, direction, connectionType, typeConstraint, this);
			this.ports.Add(fieldName, nodePort);
			return nodePort;
		}

		// Token: 0x0600108A RID: 4234 RVA: 0x0004D17B File Offset: 0x0004B37B
		public void RemoveDynamicPort(string fieldName)
		{
			if (this.GetPort(fieldName) == null)
			{
				throw new ArgumentException("port " + fieldName + " doesn't exist");
			}
			this.RemoveDynamicPort(this.GetPort(fieldName));
		}

		// Token: 0x0600108B RID: 4235 RVA: 0x0004D1A9 File Offset: 0x0004B3A9
		public void RemoveDynamicPort(NodePort port)
		{
			if (port == null)
			{
				throw new ArgumentNullException("port");
			}
			if (port.IsStatic)
			{
				throw new ArgumentException("cannot remove static port");
			}
			port.ClearConnections();
			this.ports.Remove(port.fieldName);
		}

		// Token: 0x0600108C RID: 4236 RVA: 0x0004D1E4 File Offset: 0x0004B3E4
		[ContextMenu("Clear Dynamic Ports")]
		public void ClearDynamicPorts()
		{
			foreach (NodePort port in new List<NodePort>(this.DynamicPorts))
			{
				this.RemoveDynamicPort(port);
			}
		}

		// Token: 0x0600108D RID: 4237 RVA: 0x0004D23C File Offset: 0x0004B43C
		public NodePort GetOutputPort(string fieldName)
		{
			NodePort port = this.GetPort(fieldName);
			if (port == null || port.direction != NodePort.IO.Output)
			{
				return null;
			}
			return port;
		}

		// Token: 0x0600108E RID: 4238 RVA: 0x0004D260 File Offset: 0x0004B460
		public NodePort GetInputPort(string fieldName)
		{
			NodePort port = this.GetPort(fieldName);
			if (port == null || port.direction != NodePort.IO.Input)
			{
				return null;
			}
			return port;
		}

		// Token: 0x0600108F RID: 4239 RVA: 0x0004D284 File Offset: 0x0004B484
		public NodePort GetPort(string fieldName)
		{
			NodePort result;
			if (this.ports.TryGetValue(fieldName, out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x06001090 RID: 4240 RVA: 0x0004D2A4 File Offset: 0x0004B4A4
		public bool HasPort(string fieldName)
		{
			return this.ports.ContainsKey(fieldName);
		}

		// Token: 0x06001091 RID: 4241 RVA: 0x0004D2B4 File Offset: 0x0004B4B4
		public T GetInputValue<T>(string fieldName, T fallback = default(T))
		{
			NodePort port = this.GetPort(fieldName);
			if (port != null && port.IsConnected)
			{
				return port.GetInputValue<T>();
			}
			return fallback;
		}

		// Token: 0x06001092 RID: 4242 RVA: 0x0004D2DC File Offset: 0x0004B4DC
		public T[] GetInputValues<T>(string fieldName, params T[] fallback)
		{
			NodePort port = this.GetPort(fieldName);
			if (port != null && port.IsConnected)
			{
				return port.GetInputValues<T>();
			}
			return fallback;
		}

		// Token: 0x06001093 RID: 4243 RVA: 0x0004D304 File Offset: 0x0004B504
		public virtual object GetValue(NodePort port)
		{
			string str = "No GetValue(NodePort port) override defined for ";
			Type type = base.GetType();
			Debug.LogWarning(str + ((type != null) ? type.ToString() : null));
			return null;
		}

		// Token: 0x06001094 RID: 4244 RVA: 0x00003212 File Offset: 0x00001412
		public virtual void OnCreateConnection(NodePort from, NodePort to)
		{
		}

		// Token: 0x06001095 RID: 4245 RVA: 0x00003212 File Offset: 0x00001412
		public virtual void OnRemoveConnection(NodePort port)
		{
		}

		// Token: 0x06001096 RID: 4246 RVA: 0x0004D328 File Offset: 0x0004B528
		public void ClearConnections()
		{
			foreach (NodePort nodePort in this.Ports)
			{
				nodePort.ClearConnections();
			}
		}

		// Token: 0x04000E81 RID: 3713
		[SerializeField]
		public NodeGraph graph;

		// Token: 0x04000E82 RID: 3714
		[SerializeField]
		public Vector2 position;

		// Token: 0x04000E83 RID: 3715
		[SerializeField]
		private Node.NodePortDictionary ports = new Node.NodePortDictionary();

		// Token: 0x04000E84 RID: 3716
		public static NodeGraph graphHotfix;

		// Token: 0x0200023A RID: 570
		public enum ShowBackingValue
		{
			// Token: 0x04000E86 RID: 3718
			Never,
			// Token: 0x04000E87 RID: 3719
			Unconnected,
			// Token: 0x04000E88 RID: 3720
			Always
		}

		// Token: 0x0200023B RID: 571
		public enum ConnectionType
		{
			// Token: 0x04000E8A RID: 3722
			Multiple,
			// Token: 0x04000E8B RID: 3723
			Override
		}

		// Token: 0x0200023C RID: 572
		public enum TypeConstraint
		{
			// Token: 0x04000E8D RID: 3725
			None,
			// Token: 0x04000E8E RID: 3726
			Inherited,
			// Token: 0x04000E8F RID: 3727
			Strict,
			// Token: 0x04000E90 RID: 3728
			InheritedInverse
		}

		// Token: 0x0200023D RID: 573
		[AttributeUsage(AttributeTargets.Field)]
		public class InputAttribute : Attribute
		{
			// Token: 0x170001E7 RID: 487
			// (get) Token: 0x06001098 RID: 4248 RVA: 0x0004D387 File Offset: 0x0004B587
			// (set) Token: 0x06001099 RID: 4249 RVA: 0x0004D38F File Offset: 0x0004B58F
			[Obsolete("Use dynamicPortList instead")]
			public bool instancePortList
			{
				get
				{
					return this.dynamicPortList;
				}
				set
				{
					this.dynamicPortList = value;
				}
			}

			// Token: 0x0600109A RID: 4250 RVA: 0x0004D398 File Offset: 0x0004B598
			public InputAttribute(Node.ShowBackingValue backingValue = Node.ShowBackingValue.Unconnected, Node.ConnectionType connectionType = Node.ConnectionType.Multiple, Node.TypeConstraint typeConstraint = Node.TypeConstraint.None, bool dynamicPortList = false)
			{
				this.backingValue = backingValue;
				this.connectionType = connectionType;
				this.dynamicPortList = dynamicPortList;
				this.typeConstraint = typeConstraint;
			}

			// Token: 0x04000E91 RID: 3729
			public Node.ShowBackingValue backingValue;

			// Token: 0x04000E92 RID: 3730
			public Node.ConnectionType connectionType;

			// Token: 0x04000E93 RID: 3731
			public bool dynamicPortList;

			// Token: 0x04000E94 RID: 3732
			public Node.TypeConstraint typeConstraint;
		}

		// Token: 0x0200023E RID: 574
		[AttributeUsage(AttributeTargets.Field)]
		public class OutputAttribute : Attribute
		{
			// Token: 0x170001E8 RID: 488
			// (get) Token: 0x0600109B RID: 4251 RVA: 0x0004D3BD File Offset: 0x0004B5BD
			// (set) Token: 0x0600109C RID: 4252 RVA: 0x0004D3C5 File Offset: 0x0004B5C5
			[Obsolete("Use dynamicPortList instead")]
			public bool instancePortList
			{
				get
				{
					return this.dynamicPortList;
				}
				set
				{
					this.dynamicPortList = value;
				}
			}

			// Token: 0x0600109D RID: 4253 RVA: 0x0004D3CE File Offset: 0x0004B5CE
			public OutputAttribute(Node.ShowBackingValue backingValue = Node.ShowBackingValue.Never, Node.ConnectionType connectionType = Node.ConnectionType.Multiple, Node.TypeConstraint typeConstraint = Node.TypeConstraint.None, bool dynamicPortList = false)
			{
				this.backingValue = backingValue;
				this.connectionType = connectionType;
				this.dynamicPortList = dynamicPortList;
				this.typeConstraint = typeConstraint;
			}

			// Token: 0x0600109E RID: 4254 RVA: 0x0004D3F3 File Offset: 0x0004B5F3
			[Obsolete("Use constructor with TypeConstraint")]
			public OutputAttribute(Node.ShowBackingValue backingValue, Node.ConnectionType connectionType, bool dynamicPortList) : this(backingValue, connectionType, Node.TypeConstraint.None, dynamicPortList)
			{
			}

			// Token: 0x04000E95 RID: 3733
			public Node.ShowBackingValue backingValue;

			// Token: 0x04000E96 RID: 3734
			public Node.ConnectionType connectionType;

			// Token: 0x04000E97 RID: 3735
			public bool dynamicPortList;

			// Token: 0x04000E98 RID: 3736
			public Node.TypeConstraint typeConstraint;
		}

		// Token: 0x0200023F RID: 575
		[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
		public class CreateNodeMenuAttribute : Attribute
		{
			// Token: 0x0600109F RID: 4255 RVA: 0x0004D3FF File Offset: 0x0004B5FF
			public CreateNodeMenuAttribute(string menuName)
			{
				this.menuName = menuName;
				this.order = 0;
			}

			// Token: 0x060010A0 RID: 4256 RVA: 0x0004D415 File Offset: 0x0004B615
			public CreateNodeMenuAttribute(string menuName, int order)
			{
				this.menuName = menuName;
				this.order = order;
			}

			// Token: 0x04000E99 RID: 3737
			public string menuName;

			// Token: 0x04000E9A RID: 3738
			public int order;
		}

		// Token: 0x02000240 RID: 576
		[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
		public class DisallowMultipleNodesAttribute : Attribute
		{
			// Token: 0x060010A1 RID: 4257 RVA: 0x0004D42B File Offset: 0x0004B62B
			public DisallowMultipleNodesAttribute(int max = 1)
			{
				this.max = max;
			}

			// Token: 0x04000E9B RID: 3739
			public int max;
		}

		// Token: 0x02000241 RID: 577
		[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
		public class NodeTintAttribute : Attribute
		{
			// Token: 0x060010A2 RID: 4258 RVA: 0x0004D43A File Offset: 0x0004B63A
			public NodeTintAttribute(float r, float g, float b)
			{
				this.color = new Color(r, g, b);
			}

			// Token: 0x060010A3 RID: 4259 RVA: 0x0004D450 File Offset: 0x0004B650
			public NodeTintAttribute(string hex)
			{
				ColorUtility.TryParseHtmlString(hex, out this.color);
			}

			// Token: 0x060010A4 RID: 4260 RVA: 0x0004D465 File Offset: 0x0004B665
			public NodeTintAttribute(byte r, byte g, byte b)
			{
				this.color = new Color32(r, g, b, byte.MaxValue);
			}

			// Token: 0x04000E9C RID: 3740
			public Color color;
		}

		// Token: 0x02000242 RID: 578
		[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
		public class NodeWidthAttribute : Attribute
		{
			// Token: 0x060010A5 RID: 4261 RVA: 0x0004D485 File Offset: 0x0004B685
			public NodeWidthAttribute(int width)
			{
				this.width = width;
			}

			// Token: 0x04000E9D RID: 3741
			public int width;
		}

		// Token: 0x02000243 RID: 579
		[Serializable]
		private class NodePortDictionary : Dictionary<string, NodePort>, ISerializationCallbackReceiver
		{
			// Token: 0x060010A6 RID: 4262 RVA: 0x0004D494 File Offset: 0x0004B694
			public void OnBeforeSerialize()
			{
				this.keys.Clear();
				this.values.Clear();
				foreach (KeyValuePair<string, NodePort> keyValuePair in this)
				{
					this.keys.Add(keyValuePair.Key);
					this.values.Add(keyValuePair.Value);
				}
			}

			// Token: 0x060010A7 RID: 4263 RVA: 0x0004D518 File Offset: 0x0004B718
			public void OnAfterDeserialize()
			{
				base.Clear();
				if (this.keys.Count != this.values.Count)
				{
					throw new Exception(string.Concat(new string[]
					{
						"there are ",
						this.keys.Count.ToString(),
						" keys and ",
						this.values.Count.ToString(),
						" values after deserialization. Make sure that both key and value types are serializable."
					}));
				}
				for (int i = 0; i < this.keys.Count; i++)
				{
					base.Add(this.keys[i], this.values[i]);
				}
			}

			// Token: 0x04000E9E RID: 3742
			[SerializeField]
			private List<string> keys = new List<string>();

			// Token: 0x04000E9F RID: 3743
			[SerializeField]
			private List<NodePort> values = new List<NodePort>();
		}
	}
}
