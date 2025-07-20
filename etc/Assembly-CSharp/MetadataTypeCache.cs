using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

// Token: 0x0200006B RID: 107
public static class MetadataTypeCache
{
	// Token: 0x06000504 RID: 1284 RVA: 0x0001A728 File Offset: 0x00018928
	public static void Initialize()
	{
		if (MetadataTypeCache._isInitialized)
		{
			return;
		}
		Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
		for (int i = 0; i < assemblies.Length; i++)
		{
			foreach (Type type in from t in assemblies[i].GetTypes()
			where t.IsSubclassOf(typeof(ComponentMetadataWrapper)) && !t.IsAbstract
			select t)
			{
				MetadataForAttribute customAttribute = type.GetCustomAttribute<MetadataForAttribute>();
				if (customAttribute != null)
				{
					Type componentType = customAttribute.ComponentType;
					if (componentType != null)
					{
						if (!MetadataTypeCache.MetadataTypeMappings.ContainsKey(componentType))
						{
							Debug.Log(string.Format("[METADATA] Mapped {0} to {1}", type, componentType));
							MetadataTypeCache.MetadataTypeMappings.Add(componentType, type);
						}
						else
						{
							Debug.LogWarning(string.Format("[METADATA] Duplicate mapping found for {0}. Existing: {1}, New: {2}", componentType, MetadataTypeCache.MetadataTypeMappings[componentType], type));
						}
					}
				}
			}
		}
		MetadataTypeCache._isInitialized = true;
		Debug.Log(string.Format("[METADATA] Initialization complete with {0} mappings.", MetadataTypeCache.MetadataTypeMappings.Count));
	}

	// Token: 0x06000505 RID: 1285 RVA: 0x0001A84C File Offset: 0x00018A4C
	public static Type GetMetadataType(Type componentType)
	{
		while (componentType != null)
		{
			Type result;
			if (MetadataTypeCache.MetadataTypeMappings.TryGetValue(componentType, out result))
			{
				return result;
			}
			componentType = componentType.BaseType;
		}
		return null;
	}

	// Token: 0x17000048 RID: 72
	// (get) Token: 0x06000506 RID: 1286 RVA: 0x0001A87E File Offset: 0x00018A7E
	public static int CachedMetadataCount
	{
		get
		{
			return MetadataTypeCache.MetadataTypeMappings.Count;
		}
	}

	// Token: 0x040002EA RID: 746
	private static readonly Dictionary<Type, Type> MetadataTypeMappings = new Dictionary<Type, Type>();

	// Token: 0x040002EB RID: 747
	private static bool _isInitialized = false;
}
