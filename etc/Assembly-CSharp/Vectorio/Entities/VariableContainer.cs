using System;
using System.Collections.Generic;
using UnityEngine;

namespace Vectorio.Entities
{
	// Token: 0x0200029C RID: 668
	[Serializable]
	public class VariableContainer
	{
		// Token: 0x060012E0 RID: 4832 RVA: 0x00056D54 File Offset: 0x00054F54
		public VariableContainer(VariableObject variableObject)
		{
			this.SetComponentType(variableObject.Type);
			foreach (VariableObject.StringVariable stringVariable in variableObject.stringVariables)
			{
				this.StringVariables.Add(stringVariable.key, stringVariable.value);
			}
			foreach (VariableObject.IntVariable intVariable in variableObject.intVariables)
			{
				this.IntVariables.Add(intVariable.key, intVariable.value);
			}
			foreach (VariableObject.FloatVariable floatVariable in variableObject.floatVariables)
			{
				this.FloatVariables.Add(floatVariable.key, floatVariable.value);
			}
			foreach (VariableObject.BoolVariable boolVariable in variableObject.boolVariables)
			{
				this.BoolVariables.Add(boolVariable.key, boolVariable.value);
			}
		}

		// Token: 0x060012E1 RID: 4833 RVA: 0x00056EFC File Offset: 0x000550FC
		public VariableContainer(byte index, string value)
		{
			this.StringVariables.Add(index, value);
		}

		// Token: 0x060012E2 RID: 4834 RVA: 0x00056F50 File Offset: 0x00055150
		public VariableContainer(byte index, int value)
		{
			this.IntVariables.Add(index, value);
		}

		// Token: 0x060012E3 RID: 4835 RVA: 0x00056FA4 File Offset: 0x000551A4
		public VariableContainer(byte index, float value)
		{
			this.FloatVariables.Add(index, value);
		}

		// Token: 0x060012E4 RID: 4836 RVA: 0x00056FF8 File Offset: 0x000551F8
		public VariableContainer(byte index, bool value)
		{
			this.BoolVariables.Add(index, value);
		}

		// Token: 0x060012E5 RID: 4837 RVA: 0x0005704B File Offset: 0x0005524B
		public VariableContainer()
		{
		}

		// Token: 0x1700025A RID: 602
		// (get) Token: 0x060012E6 RID: 4838 RVA: 0x00057086 File Offset: 0x00055286
		// (set) Token: 0x060012E7 RID: 4839 RVA: 0x0005708E File Offset: 0x0005528E
		public bool UseType { get; set; } = true;

		// Token: 0x1700025B RID: 603
		// (get) Token: 0x060012E8 RID: 4840 RVA: 0x00057097 File Offset: 0x00055297
		// (set) Token: 0x060012E9 RID: 4841 RVA: 0x0005709F File Offset: 0x0005529F
		public string TypeKey { get; set; }

		// Token: 0x1700025C RID: 604
		// (get) Token: 0x060012EA RID: 4842 RVA: 0x000570A8 File Offset: 0x000552A8
		// (set) Token: 0x060012EB RID: 4843 RVA: 0x000570B0 File Offset: 0x000552B0
		public byte? C_Index { get; set; }

		// Token: 0x1700025D RID: 605
		// (get) Token: 0x060012EC RID: 4844 RVA: 0x000570B9 File Offset: 0x000552B9
		// (set) Token: 0x060012ED RID: 4845 RVA: 0x000570C1 File Offset: 0x000552C1
		public Dictionary<byte, string> StringVariables { get; set; } = new Dictionary<byte, string>();

		// Token: 0x1700025E RID: 606
		// (get) Token: 0x060012EE RID: 4846 RVA: 0x000570CA File Offset: 0x000552CA
		// (set) Token: 0x060012EF RID: 4847 RVA: 0x000570D2 File Offset: 0x000552D2
		public Dictionary<byte, int> IntVariables { get; set; } = new Dictionary<byte, int>();

		// Token: 0x1700025F RID: 607
		// (get) Token: 0x060012F0 RID: 4848 RVA: 0x000570DB File Offset: 0x000552DB
		// (set) Token: 0x060012F1 RID: 4849 RVA: 0x000570E3 File Offset: 0x000552E3
		public Dictionary<byte, float> FloatVariables { get; set; } = new Dictionary<byte, float>();

		// Token: 0x17000260 RID: 608
		// (get) Token: 0x060012F2 RID: 4850 RVA: 0x000570EC File Offset: 0x000552EC
		// (set) Token: 0x060012F3 RID: 4851 RVA: 0x000570F4 File Offset: 0x000552F4
		public Dictionary<byte, bool> BoolVariables { get; set; } = new Dictionary<byte, bool>();

		// Token: 0x060012F4 RID: 4852 RVA: 0x000570FD File Offset: 0x000552FD
		public bool TryGetString(byte index, out string value)
		{
			if (this.StringVariables.ContainsKey(index))
			{
				value = this.StringVariables[index];
				return true;
			}
			value = "";
			return false;
		}

		// Token: 0x060012F5 RID: 4853 RVA: 0x00057125 File Offset: 0x00055325
		public bool TryGetInt(byte index, out int value)
		{
			if (this.IntVariables.ContainsKey(index))
			{
				value = this.IntVariables[index];
				return true;
			}
			value = 0;
			return false;
		}

		// Token: 0x060012F6 RID: 4854 RVA: 0x00057149 File Offset: 0x00055349
		public bool TryGetFloat(byte index, out float value)
		{
			if (this.FloatVariables.ContainsKey(index))
			{
				value = this.FloatVariables[index];
				return true;
			}
			value = 0f;
			return false;
		}

		// Token: 0x060012F7 RID: 4855 RVA: 0x00057171 File Offset: 0x00055371
		public bool TryGetBool(byte index, out bool value)
		{
			if (this.BoolVariables.ContainsKey(index))
			{
				value = this.BoolVariables[index];
				return true;
			}
			value = false;
			return false;
		}

		// Token: 0x060012F8 RID: 4856 RVA: 0x00057198 File Offset: 0x00055398
		public void ApplyToEntity(Entity entity)
		{
			if (this.UseType)
			{
				Type type = Type.GetType(this.TypeKey);
				EntityComponent entityComponent;
				if (type != null && entity.TryGet_EComponent(type, out entityComponent))
				{
					entityComponent.ApplyVariableContainer(this);
					return;
				}
				Debug.Log("[VARIABLE CONTAINER] Could not find component with type " + this.TypeKey);
				return;
			}
			else
			{
				if (this.C_Index == null)
				{
					Debug.Log("[VARIABLE CONTAINER] No valid key has been provided for this container!");
					return;
				}
				EntityComponent entityComponent2;
				if (entity.TryGet_EComponent(this.C_Index.Value, out entityComponent2))
				{
					entityComponent2.ApplyVariableContainer(this);
					return;
				}
				Debug.Log("[VARIABLE CONTAINER] Could not find component with byte index " + this.C_Index.Value.ToString());
				return;
			}
		}

		// Token: 0x060012F9 RID: 4857 RVA: 0x0005724C File Offset: 0x0005544C
		private void SetComponentType(string type)
		{
			this.TypeKey = type;
			this.UseType = true;
		}

		// Token: 0x060012FA RID: 4858 RVA: 0x0005725C File Offset: 0x0005545C
		private void SetComponentIndex(byte index)
		{
			this.C_Index = new byte?(index);
			this.UseType = false;
		}

		// Token: 0x060012FB RID: 4859 RVA: 0x00057274 File Offset: 0x00055474
		public override string ToString()
		{
			string text = "";
			foreach (KeyValuePair<byte, string> keyValuePair in this.StringVariables)
			{
				text = string.Concat(new string[]
				{
					text,
					keyValuePair.Key.ToString(),
					": ",
					keyValuePair.Value,
					" (string)\n"
				});
			}
			foreach (KeyValuePair<byte, int> keyValuePair2 in this.IntVariables)
			{
				text = string.Concat(new string[]
				{
					text,
					keyValuePair2.Key.ToString(),
					": ",
					keyValuePair2.Value.ToString(),
					" (int)\n"
				});
			}
			foreach (KeyValuePair<byte, float> keyValuePair3 in this.FloatVariables)
			{
				text = string.Concat(new string[]
				{
					text,
					keyValuePair3.Key.ToString(),
					": ",
					keyValuePair3.Value.ToString(),
					" (float)\n"
				});
			}
			foreach (KeyValuePair<byte, bool> keyValuePair4 in this.BoolVariables)
			{
				text = string.Concat(new string[]
				{
					text,
					keyValuePair4.Key.ToString(),
					": ",
					keyValuePair4.Value.ToString(),
					" (bool)\n"
				});
			}
			return text;
		}
	}
}
