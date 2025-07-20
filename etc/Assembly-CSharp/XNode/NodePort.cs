using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace XNode
{
	// Token: 0x02000251 RID: 593
	[Serializable]
	public class NodePort
	{
		// Token: 0x170001F6 RID: 502
		// (get) Token: 0x060010FE RID: 4350 RVA: 0x0004E91C File Offset: 0x0004CB1C
		public int ConnectionCount
		{
			get
			{
				return this.connections.Count;
			}
		}

		// Token: 0x170001F7 RID: 503
		// (get) Token: 0x060010FF RID: 4351 RVA: 0x0004E92C File Offset: 0x0004CB2C
		public NodePort Connection
		{
			get
			{
				for (int i = 0; i < this.connections.Count; i++)
				{
					if (this.connections[i] != null)
					{
						return this.connections[i].Port;
					}
				}
				return null;
			}
		}

		// Token: 0x170001F8 RID: 504
		// (get) Token: 0x06001100 RID: 4352 RVA: 0x0004E970 File Offset: 0x0004CB70
		// (set) Token: 0x06001101 RID: 4353 RVA: 0x0004E978 File Offset: 0x0004CB78
		public NodePort.IO direction
		{
			get
			{
				return this._direction;
			}
			internal set
			{
				this._direction = value;
			}
		}

		// Token: 0x170001F9 RID: 505
		// (get) Token: 0x06001102 RID: 4354 RVA: 0x0004E981 File Offset: 0x0004CB81
		// (set) Token: 0x06001103 RID: 4355 RVA: 0x0004E989 File Offset: 0x0004CB89
		public Node.ConnectionType connectionType
		{
			get
			{
				return this._connectionType;
			}
			internal set
			{
				this._connectionType = value;
			}
		}

		// Token: 0x170001FA RID: 506
		// (get) Token: 0x06001104 RID: 4356 RVA: 0x0004E992 File Offset: 0x0004CB92
		// (set) Token: 0x06001105 RID: 4357 RVA: 0x0004E99A File Offset: 0x0004CB9A
		public Node.TypeConstraint typeConstraint
		{
			get
			{
				return this._typeConstraint;
			}
			internal set
			{
				this._typeConstraint = value;
			}
		}

		// Token: 0x170001FB RID: 507
		// (get) Token: 0x06001106 RID: 4358 RVA: 0x0004E9A3 File Offset: 0x0004CBA3
		public bool IsConnected
		{
			get
			{
				return this.connections.Count != 0;
			}
		}

		// Token: 0x170001FC RID: 508
		// (get) Token: 0x06001107 RID: 4359 RVA: 0x0004E9B3 File Offset: 0x0004CBB3
		public bool IsInput
		{
			get
			{
				return this.direction == NodePort.IO.Input;
			}
		}

		// Token: 0x170001FD RID: 509
		// (get) Token: 0x06001108 RID: 4360 RVA: 0x0004E9BE File Offset: 0x0004CBBE
		public bool IsOutput
		{
			get
			{
				return this.direction == NodePort.IO.Output;
			}
		}

		// Token: 0x170001FE RID: 510
		// (get) Token: 0x06001109 RID: 4361 RVA: 0x0004E9C9 File Offset: 0x0004CBC9
		public string fieldName
		{
			get
			{
				return this._fieldName;
			}
		}

		// Token: 0x170001FF RID: 511
		// (get) Token: 0x0600110A RID: 4362 RVA: 0x0004E9D1 File Offset: 0x0004CBD1
		public Node node
		{
			get
			{
				return this._node;
			}
		}

		// Token: 0x17000200 RID: 512
		// (get) Token: 0x0600110B RID: 4363 RVA: 0x0004E9D9 File Offset: 0x0004CBD9
		public bool IsDynamic
		{
			get
			{
				return this._dynamic;
			}
		}

		// Token: 0x17000201 RID: 513
		// (get) Token: 0x0600110C RID: 4364 RVA: 0x0004E9E1 File Offset: 0x0004CBE1
		public bool IsStatic
		{
			get
			{
				return !this._dynamic;
			}
		}

		// Token: 0x17000202 RID: 514
		// (get) Token: 0x0600110D RID: 4365 RVA: 0x0004E9EC File Offset: 0x0004CBEC
		// (set) Token: 0x0600110E RID: 4366 RVA: 0x0004EA21 File Offset: 0x0004CC21
		public Type ValueType
		{
			get
			{
				if (this.valueType == null && !string.IsNullOrEmpty(this._typeQualifiedName))
				{
					this.valueType = Type.GetType(this._typeQualifiedName, false);
				}
				return this.valueType;
			}
			set
			{
				this.valueType = value;
				if (value != null)
				{
					this._typeQualifiedName = value.AssemblyQualifiedName;
				}
			}
		}

		// Token: 0x0600110F RID: 4367 RVA: 0x0004EA40 File Offset: 0x0004CC40
		public NodePort(FieldInfo fieldInfo)
		{
			this._fieldName = fieldInfo.Name;
			this.ValueType = fieldInfo.FieldType;
			this._dynamic = false;
			object[] customAttributes = fieldInfo.GetCustomAttributes(false);
			for (int i = 0; i < customAttributes.Length; i++)
			{
				if (customAttributes[i] is Node.InputAttribute)
				{
					this._direction = NodePort.IO.Input;
					this._connectionType = (customAttributes[i] as Node.InputAttribute).connectionType;
					this._typeConstraint = (customAttributes[i] as Node.InputAttribute).typeConstraint;
				}
				else if (customAttributes[i] is Node.OutputAttribute)
				{
					this._direction = NodePort.IO.Output;
					this._connectionType = (customAttributes[i] as Node.OutputAttribute).connectionType;
					this._typeConstraint = (customAttributes[i] as Node.OutputAttribute).typeConstraint;
				}
			}
		}

		// Token: 0x06001110 RID: 4368 RVA: 0x0004EB04 File Offset: 0x0004CD04
		public NodePort(NodePort nodePort, Node node)
		{
			this._fieldName = nodePort._fieldName;
			this.ValueType = nodePort.valueType;
			this._direction = nodePort.direction;
			this._dynamic = nodePort._dynamic;
			this._connectionType = nodePort._connectionType;
			this._typeConstraint = nodePort._typeConstraint;
			this._node = node;
		}

		// Token: 0x06001111 RID: 4369 RVA: 0x0004EB74 File Offset: 0x0004CD74
		public NodePort(string fieldName, Type type, NodePort.IO direction, Node.ConnectionType connectionType, Node.TypeConstraint typeConstraint, Node node)
		{
			this._fieldName = fieldName;
			this.ValueType = type;
			this._direction = direction;
			this._node = node;
			this._dynamic = true;
			this._connectionType = connectionType;
			this._typeConstraint = typeConstraint;
		}

		// Token: 0x06001112 RID: 4370 RVA: 0x0004EBC8 File Offset: 0x0004CDC8
		public void VerifyConnections()
		{
			for (int i = this.connections.Count - 1; i >= 0; i--)
			{
				if (!(this.connections[i].node != null) || string.IsNullOrEmpty(this.connections[i].fieldName) || this.connections[i].node.GetPort(this.connections[i].fieldName) == null)
				{
					this.connections.RemoveAt(i);
				}
			}
		}

		// Token: 0x06001113 RID: 4371 RVA: 0x0004EC53 File Offset: 0x0004CE53
		public object GetOutputValue()
		{
			if (this.direction == NodePort.IO.Input)
			{
				return null;
			}
			return this.node.GetValue(this);
		}

		// Token: 0x06001114 RID: 4372 RVA: 0x0004EC6C File Offset: 0x0004CE6C
		public object GetInputValue()
		{
			NodePort connection = this.Connection;
			if (connection == null)
			{
				return null;
			}
			return connection.GetOutputValue();
		}

		// Token: 0x06001115 RID: 4373 RVA: 0x0004EC8C File Offset: 0x0004CE8C
		public object[] GetInputValues()
		{
			object[] array = new object[this.ConnectionCount];
			for (int i = 0; i < this.ConnectionCount; i++)
			{
				NodePort port = this.connections[i].Port;
				if (port == null)
				{
					this.connections.RemoveAt(i);
					i--;
				}
				else
				{
					array[i] = port.GetOutputValue();
				}
			}
			return array;
		}

		// Token: 0x06001116 RID: 4374 RVA: 0x0004ECE8 File Offset: 0x0004CEE8
		public T GetInputValue<T>()
		{
			object inputValue = this.GetInputValue();
			if (!(inputValue is T))
			{
				return default(T);
			}
			return (T)((object)inputValue);
		}

		// Token: 0x06001117 RID: 4375 RVA: 0x0004ED14 File Offset: 0x0004CF14
		public T[] GetInputValues<T>()
		{
			object[] inputValues = this.GetInputValues();
			T[] array = new T[inputValues.Length];
			for (int i = 0; i < inputValues.Length; i++)
			{
				if (inputValues[i] is T)
				{
					array[i] = (T)((object)inputValues[i]);
				}
			}
			return array;
		}

		// Token: 0x06001118 RID: 4376 RVA: 0x0004ED5C File Offset: 0x0004CF5C
		public bool TryGetInputValue<T>(out T value)
		{
			object inputValue = this.GetInputValue();
			if (inputValue is T)
			{
				value = (T)((object)inputValue);
				return true;
			}
			value = default(T);
			return false;
		}

		// Token: 0x06001119 RID: 4377 RVA: 0x0004ED90 File Offset: 0x0004CF90
		public float GetInputSum(float fallback)
		{
			object[] inputValues = this.GetInputValues();
			if (inputValues.Length == 0)
			{
				return fallback;
			}
			float num = 0f;
			for (int i = 0; i < inputValues.Length; i++)
			{
				if (inputValues[i] is float)
				{
					num += (float)inputValues[i];
				}
			}
			return num;
		}

		// Token: 0x0600111A RID: 4378 RVA: 0x0004EDD4 File Offset: 0x0004CFD4
		public int GetInputSum(int fallback)
		{
			object[] inputValues = this.GetInputValues();
			if (inputValues.Length == 0)
			{
				return fallback;
			}
			int num = 0;
			for (int i = 0; i < inputValues.Length; i++)
			{
				if (inputValues[i] is int)
				{
					num += (int)inputValues[i];
				}
			}
			return num;
		}

		// Token: 0x0600111B RID: 4379 RVA: 0x0004EE14 File Offset: 0x0004D014
		public void Connect(NodePort port)
		{
			if (this.connections == null)
			{
				this.connections = new List<NodePort.PortConnection>();
			}
			if (port == null)
			{
				Debug.LogWarning("Cannot connect to null port");
				return;
			}
			if (port == this)
			{
				Debug.LogWarning("Cannot connect port to self.");
				return;
			}
			if (this.IsConnectedTo(port))
			{
				Debug.LogWarning("Port already connected. ");
				return;
			}
			if (this.direction == port.direction)
			{
				Debug.LogWarning("Cannot connect two " + ((this.direction == NodePort.IO.Input) ? "input" : "output") + " connections");
				return;
			}
			if (port.connectionType == Node.ConnectionType.Override && port.ConnectionCount != 0)
			{
				port.ClearConnections();
			}
			if (this.connectionType == Node.ConnectionType.Override && this.ConnectionCount != 0)
			{
				this.ClearConnections();
			}
			this.connections.Add(new NodePort.PortConnection(port));
			if (port.connections == null)
			{
				port.connections = new List<NodePort.PortConnection>();
			}
			if (!port.IsConnectedTo(this))
			{
				port.connections.Add(new NodePort.PortConnection(this));
			}
			this.node.OnCreateConnection(this, port);
			port.node.OnCreateConnection(this, port);
		}

		// Token: 0x0600111C RID: 4380 RVA: 0x0004EF24 File Offset: 0x0004D124
		public List<NodePort> GetConnections()
		{
			List<NodePort> list = new List<NodePort>();
			for (int i = 0; i < this.connections.Count; i++)
			{
				NodePort connection = this.GetConnection(i);
				if (connection != null)
				{
					list.Add(connection);
				}
			}
			return list;
		}

		// Token: 0x0600111D RID: 4381 RVA: 0x0004EF60 File Offset: 0x0004D160
		public NodePort GetConnection(int i)
		{
			if (this.connections[i].node == null || string.IsNullOrEmpty(this.connections[i].fieldName))
			{
				this.connections.RemoveAt(i);
				return null;
			}
			NodePort port = this.connections[i].node.GetPort(this.connections[i].fieldName);
			if (port == null)
			{
				this.connections.RemoveAt(i);
				return null;
			}
			return port;
		}

		// Token: 0x0600111E RID: 4382 RVA: 0x0004EFE8 File Offset: 0x0004D1E8
		public int GetConnectionIndex(NodePort port)
		{
			for (int i = 0; i < this.ConnectionCount; i++)
			{
				if (this.connections[i].Port == port)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x0600111F RID: 4383 RVA: 0x0004F020 File Offset: 0x0004D220
		public bool IsConnectedTo(NodePort port)
		{
			for (int i = 0; i < this.connections.Count; i++)
			{
				if (this.connections[i].Port == port)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001120 RID: 4384 RVA: 0x0004F05C File Offset: 0x0004D25C
		public bool CanConnectTo(NodePort port)
		{
			NodePort nodePort = null;
			NodePort nodePort2 = null;
			if (this.IsInput)
			{
				nodePort = this;
			}
			else
			{
				nodePort2 = this;
			}
			if (port.IsInput)
			{
				nodePort = port;
			}
			else
			{
				nodePort2 = port;
			}
			return nodePort != null && nodePort2 != null && (nodePort.typeConstraint != Node.TypeConstraint.Inherited || nodePort.ValueType.IsAssignableFrom(nodePort2.ValueType)) && (nodePort.typeConstraint != Node.TypeConstraint.Strict || !(nodePort.ValueType != nodePort2.ValueType)) && (nodePort.typeConstraint != Node.TypeConstraint.InheritedInverse || nodePort2.ValueType.IsAssignableFrom(nodePort.ValueType)) && (nodePort2.typeConstraint != Node.TypeConstraint.Inherited || nodePort.ValueType.IsAssignableFrom(nodePort2.ValueType)) && (nodePort2.typeConstraint != Node.TypeConstraint.Strict || !(nodePort.ValueType != nodePort2.ValueType)) && (nodePort2.typeConstraint != Node.TypeConstraint.InheritedInverse || nodePort2.ValueType.IsAssignableFrom(nodePort.ValueType));
		}

		// Token: 0x06001121 RID: 4385 RVA: 0x0004F148 File Offset: 0x0004D348
		public void Disconnect(NodePort port)
		{
			for (int i = this.connections.Count - 1; i >= 0; i--)
			{
				if (this.connections[i].Port == port)
				{
					this.connections.RemoveAt(i);
				}
			}
			if (port != null)
			{
				for (int j = 0; j < port.connections.Count; j++)
				{
					if (port.connections[j].Port == this)
					{
						port.connections.RemoveAt(j);
					}
				}
			}
			this.node.OnRemoveConnection(this);
			if (port != null)
			{
				port.node.OnRemoveConnection(port);
			}
		}

		// Token: 0x06001122 RID: 4386 RVA: 0x0004F1E4 File Offset: 0x0004D3E4
		public void Disconnect(int i)
		{
			NodePort port = this.connections[i].Port;
			if (port != null)
			{
				for (int j = 0; j < port.connections.Count; j++)
				{
					if (port.connections[j].Port == this)
					{
						port.connections.RemoveAt(i);
					}
				}
			}
			this.connections.RemoveAt(i);
			this.node.OnRemoveConnection(this);
			if (port != null)
			{
				port.node.OnRemoveConnection(port);
			}
		}

		// Token: 0x06001123 RID: 4387 RVA: 0x0004F263 File Offset: 0x0004D463
		public void ClearConnections()
		{
			while (this.connections.Count > 0)
			{
				this.Disconnect(this.connections[0].Port);
			}
		}

		// Token: 0x06001124 RID: 4388 RVA: 0x0004F28C File Offset: 0x0004D48C
		public List<Vector2> GetReroutePoints(int index)
		{
			return this.connections[index].reroutePoints;
		}

		// Token: 0x06001125 RID: 4389 RVA: 0x0004F2A0 File Offset: 0x0004D4A0
		public void SwapConnections(NodePort targetPort)
		{
			int count = this.connections.Count;
			int count2 = targetPort.connections.Count;
			List<NodePort> list = new List<NodePort>();
			List<NodePort> list2 = new List<NodePort>();
			for (int i = 0; i < count; i++)
			{
				list.Add(this.connections[i].Port);
			}
			for (int j = 0; j < count2; j++)
			{
				list2.Add(targetPort.connections[j].Port);
			}
			this.ClearConnections();
			targetPort.ClearConnections();
			for (int k = 0; k < list.Count; k++)
			{
				targetPort.Connect(list[k]);
			}
			for (int l = 0; l < list2.Count; l++)
			{
				this.Connect(list2[l]);
			}
		}

		// Token: 0x06001126 RID: 4390 RVA: 0x0004F374 File Offset: 0x0004D574
		public void AddConnections(NodePort targetPort)
		{
			int connectionCount = targetPort.ConnectionCount;
			for (int i = 0; i < connectionCount; i++)
			{
				NodePort port = targetPort.connections[i].Port;
				this.Connect(port);
			}
		}

		// Token: 0x06001127 RID: 4391 RVA: 0x0004F3B0 File Offset: 0x0004D5B0
		public void MoveConnections(NodePort targetPort)
		{
			int count = this.connections.Count;
			for (int i = 0; i < count; i++)
			{
				NodePort port = targetPort.connections[i].Port;
				this.Connect(port);
			}
			this.ClearConnections();
		}

		// Token: 0x06001128 RID: 4392 RVA: 0x0004F3F4 File Offset: 0x0004D5F4
		public void Redirect(List<Node> oldNodes, List<Node> newNodes)
		{
			foreach (NodePort.PortConnection portConnection in this.connections)
			{
				int num = oldNodes.IndexOf(portConnection.node);
				if (num >= 0)
				{
					portConnection.node = newNodes[num];
				}
			}
		}

		// Token: 0x04000ECC RID: 3788
		private Type valueType;

		// Token: 0x04000ECD RID: 3789
		[SerializeField]
		private string _fieldName;

		// Token: 0x04000ECE RID: 3790
		[SerializeField]
		private Node _node;

		// Token: 0x04000ECF RID: 3791
		[SerializeField]
		private string _typeQualifiedName;

		// Token: 0x04000ED0 RID: 3792
		[SerializeField]
		private List<NodePort.PortConnection> connections = new List<NodePort.PortConnection>();

		// Token: 0x04000ED1 RID: 3793
		[SerializeField]
		private NodePort.IO _direction;

		// Token: 0x04000ED2 RID: 3794
		[SerializeField]
		private Node.ConnectionType _connectionType;

		// Token: 0x04000ED3 RID: 3795
		[SerializeField]
		private Node.TypeConstraint _typeConstraint;

		// Token: 0x04000ED4 RID: 3796
		[SerializeField]
		private bool _dynamic;

		// Token: 0x02000252 RID: 594
		public enum IO
		{
			// Token: 0x04000ED6 RID: 3798
			Input,
			// Token: 0x04000ED7 RID: 3799
			Output
		}

		// Token: 0x02000253 RID: 595
		[Serializable]
		private class PortConnection
		{
			// Token: 0x17000203 RID: 515
			// (get) Token: 0x06001129 RID: 4393 RVA: 0x0004F460 File Offset: 0x0004D660
			public NodePort Port
			{
				get
				{
					if (this.port == null)
					{
						return this.port = this.GetPort();
					}
					return this.port;
				}
			}

			// Token: 0x0600112A RID: 4394 RVA: 0x0004F48B File Offset: 0x0004D68B
			public PortConnection(NodePort port)
			{
				this.port = port;
				this.node = port.node;
				this.fieldName = port.fieldName;
			}

			// Token: 0x0600112B RID: 4395 RVA: 0x0004F4BD File Offset: 0x0004D6BD
			private NodePort GetPort()
			{
				if (this.node == null || string.IsNullOrEmpty(this.fieldName))
				{
					return null;
				}
				return this.node.GetPort(this.fieldName);
			}

			// Token: 0x04000ED8 RID: 3800
			[SerializeField]
			public string fieldName;

			// Token: 0x04000ED9 RID: 3801
			[SerializeField]
			public Node node;

			// Token: 0x04000EDA RID: 3802
			[NonSerialized]
			private NodePort port;

			// Token: 0x04000EDB RID: 3803
			[SerializeField]
			public List<Vector2> reroutePoints = new List<Vector2>();
		}
	}
}
