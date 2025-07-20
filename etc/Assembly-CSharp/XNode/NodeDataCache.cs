using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace XNode
{
	// Token: 0x0200024A RID: 586
	public static class NodeDataCache
	{
		// Token: 0x170001F5 RID: 501
		// (get) Token: 0x060010DF RID: 4319 RVA: 0x0004DE9B File Offset: 0x0004C09B
		private static bool Initialized
		{
			get
			{
				return NodeDataCache.portDataCache != null;
			}
		}

		// Token: 0x060010E0 RID: 4320 RVA: 0x0004DEA8 File Offset: 0x0004C0A8
		public static void UpdatePorts(Node node, Dictionary<string, NodePort> ports)
		{
			if (!NodeDataCache.Initialized)
			{
				NodeDataCache.BuildCache();
			}
			Dictionary<string, NodePort> dictionary = new Dictionary<string, NodePort>();
			Dictionary<string, List<NodePort>> dictionary2 = new Dictionary<string, List<NodePort>>();
			Type type = node.GetType();
			List<NodePort> list = new List<NodePort>();
			List<NodePort> list2;
			if (NodeDataCache.portDataCache.TryGetValue(type, out list2))
			{
				for (int i = 0; i < list2.Count; i++)
				{
					dictionary.Add(list2[i].fieldName, NodeDataCache.portDataCache[type][i]);
				}
			}
			foreach (NodePort nodePort in ports.Values.ToList<NodePort>())
			{
				NodePort nodePort2;
				if (dictionary.TryGetValue(nodePort.fieldName, out nodePort2))
				{
					if (nodePort.IsDynamic || nodePort.direction != nodePort2.direction || nodePort.connectionType != nodePort2.connectionType || nodePort.typeConstraint != nodePort2.typeConstraint)
					{
						if (!nodePort.IsDynamic && nodePort.direction == nodePort2.direction)
						{
							dictionary2.Add(nodePort.fieldName, nodePort.GetConnections());
						}
						nodePort.ClearConnections();
						ports.Remove(nodePort.fieldName);
					}
					else
					{
						nodePort.ValueType = nodePort2.ValueType;
					}
				}
				else if (nodePort.IsStatic)
				{
					nodePort.ClearConnections();
					ports.Remove(nodePort.fieldName);
				}
				else if (NodeDataCache.IsDynamicListPort(nodePort))
				{
					list.Add(nodePort);
				}
			}
			foreach (NodePort nodePort3 in dictionary.Values)
			{
				if (!ports.ContainsKey(nodePort3.fieldName))
				{
					NodePort nodePort4 = new NodePort(nodePort3, node);
					List<NodePort> list3;
					if (dictionary2.TryGetValue(nodePort3.fieldName, out list3))
					{
						for (int j = 0; j < list3.Count; j++)
						{
							NodePort nodePort5 = list3[j];
							if (nodePort5 != null && nodePort4.CanConnectTo(nodePort5))
							{
								nodePort4.Connect(nodePort5);
							}
						}
					}
					ports.Add(nodePort3.fieldName, nodePort4);
				}
			}
			foreach (NodePort nodePort6 in list)
			{
				string key = nodePort6.fieldName.Split(' ', StringSplitOptions.None)[0];
				NodePort nodePort7 = dictionary[key];
				nodePort6.ValueType = NodeDataCache.GetBackingValueType(nodePort7.ValueType);
				nodePort6.direction = nodePort7.direction;
				nodePort6.connectionType = nodePort7.connectionType;
				nodePort6.typeConstraint = nodePort7.typeConstraint;
			}
		}

		// Token: 0x060010E1 RID: 4321 RVA: 0x0004E180 File Offset: 0x0004C380
		private static Type GetBackingValueType(Type portValType)
		{
			if (portValType.HasElementType)
			{
				return portValType.GetElementType();
			}
			if (portValType.IsGenericType && portValType.GetGenericTypeDefinition() == typeof(List<>))
			{
				return portValType.GetGenericArguments()[0];
			}
			return portValType;
		}

		// Token: 0x060010E2 RID: 4322 RVA: 0x0004E1BC File Offset: 0x0004C3BC
		private static bool IsDynamicListPort(NodePort port)
		{
			string[] array = port.fieldName.Split(' ', StringSplitOptions.None);
			if (array.Length != 2)
			{
				return false;
			}
			FieldInfo field = port.node.GetType().GetField(array[0]);
			if (field == null)
			{
				return false;
			}
			return field.GetCustomAttributes(true).Any(delegate(object x)
			{
				Node.InputAttribute inputAttribute = x as Node.InputAttribute;
				Node.OutputAttribute outputAttribute = x as Node.OutputAttribute;
				return (inputAttribute != null && inputAttribute.dynamicPortList) || (outputAttribute != null && outputAttribute.dynamicPortList);
			});
		}

		// Token: 0x060010E3 RID: 4323 RVA: 0x0004E22C File Offset: 0x0004C42C
		private static void BuildCache()
		{
			NodeDataCache.portDataCache = new NodeDataCache.PortDataCache();
			Type baseType = typeof(Node);
			List<Type> list = new List<Type>();
			Func<Type, bool> <>9__0;
			foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				string text = assembly.GetName().Name;
				int num = text.IndexOf('.');
				if (num != -1)
				{
					text = text.Substring(0, num);
				}
				if (!(text == "UnityEditor") && !(text == "UnityEngine") && !(text == "System") && !(text == "mscorlib") && !(text == "Microsoft"))
				{
					List<Type> list2 = list;
					IEnumerable<Type> types = assembly.GetTypes();
					Func<Type, bool> predicate;
					if ((predicate = <>9__0) == null)
					{
						predicate = (<>9__0 = ((Type t) => !t.IsAbstract && baseType.IsAssignableFrom(t)));
					}
					list2.AddRange(types.Where(predicate).ToArray<Type>());
				}
			}
			for (int j = 0; j < list.Count; j++)
			{
				NodeDataCache.CachePorts(list[j]);
			}
		}

		// Token: 0x060010E4 RID: 4324 RVA: 0x0004E350 File Offset: 0x0004C550
		public static List<FieldInfo> GetNodeFields(Type nodeType)
		{
			List<FieldInfo> list = new List<FieldInfo>(nodeType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic));
			Type type = nodeType;
			while ((type = type.BaseType) != typeof(Node))
			{
				FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
				for (int i = 0; i < fields.Length; i++)
				{
					FieldInfo parentField = fields[i];
					if (list.TrueForAll((FieldInfo x) => x.Name != parentField.Name))
					{
						list.Add(parentField);
					}
				}
			}
			return list;
		}

		// Token: 0x060010E5 RID: 4325 RVA: 0x0004E3D4 File Offset: 0x0004C5D4
		private static void CachePorts(Type nodeType)
		{
			List<FieldInfo> nodeFields = NodeDataCache.GetNodeFields(nodeType);
			for (int i = 0; i < nodeFields.Count; i++)
			{
				object[] customAttributes = nodeFields[i].GetCustomAttributes(true);
				Node.InputAttribute inputAttribute = customAttributes.FirstOrDefault((object x) => x is Node.InputAttribute) as Node.InputAttribute;
				Node.OutputAttribute outputAttribute = customAttributes.FirstOrDefault((object x) => x is Node.OutputAttribute) as Node.OutputAttribute;
				if (inputAttribute != null || outputAttribute != null)
				{
					if (inputAttribute != null && outputAttribute != null)
					{
						Debug.LogError(string.Concat(new string[]
						{
							"Field ",
							nodeFields[i].Name,
							" of type ",
							nodeType.FullName,
							" cannot be both input and output."
						}));
					}
					else
					{
						if (!NodeDataCache.portDataCache.ContainsKey(nodeType))
						{
							NodeDataCache.portDataCache.Add(nodeType, new List<NodePort>());
						}
						NodeDataCache.portDataCache[nodeType].Add(new NodePort(nodeFields[i]));
					}
				}
			}
		}

		// Token: 0x04000EBE RID: 3774
		private static NodeDataCache.PortDataCache portDataCache;

		// Token: 0x0200024B RID: 587
		[Serializable]
		private class PortDataCache : Dictionary<Type, List<NodePort>>, ISerializationCallbackReceiver
		{
			// Token: 0x060010E6 RID: 4326 RVA: 0x0004E4EC File Offset: 0x0004C6EC
			public void OnBeforeSerialize()
			{
				this.keys.Clear();
				this.values.Clear();
				foreach (KeyValuePair<Type, List<NodePort>> keyValuePair in this)
				{
					this.keys.Add(keyValuePair.Key);
					this.values.Add(keyValuePair.Value);
				}
			}

			// Token: 0x060010E7 RID: 4327 RVA: 0x0004E570 File Offset: 0x0004C770
			public void OnAfterDeserialize()
			{
				base.Clear();
				if (this.keys.Count != this.values.Count)
				{
					throw new Exception(string.Format("there are {0} keys and {1} values after deserialization. Make sure that both key and value types are serializable.", Array.Empty<object>()));
				}
				for (int i = 0; i < this.keys.Count; i++)
				{
					base.Add(this.keys[i], this.values[i]);
				}
			}

			// Token: 0x04000EBF RID: 3775
			[SerializeField]
			private List<Type> keys = new List<Type>();

			// Token: 0x04000EC0 RID: 3776
			[SerializeField]
			private List<List<NodePort>> values = new List<List<NodePort>>();
		}
	}
}
