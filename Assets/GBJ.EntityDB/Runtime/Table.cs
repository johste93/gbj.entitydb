using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using System.IO;
using System.Linq;
using UnityEditor;

namespace GBJ.EntityDB
{
	public class Table<T> where T : Entity
	{
		protected Dictionary<string, T> table;

		public T GetById(string id)
		{
			if (!table.ContainsKey(id))
				return default(T);

			Entity entity = table[id];
			entity.Id = id;
			return (T) entity;
		}
		public bool Contains(string id) => table.ContainsKey(id);
		
		public Dictionary<string, T>.ValueCollection All() => table.Values;

		public virtual void Insert(T value)
		{
			value.Id = Guid.NewGuid().ToString();
			value.Index = table.Count;
			table.Add(value.Id, value);
		}

		public virtual void Remove(T value)
		{
			table.Remove(value.Id);

			int index = 0;
			foreach (var kVP in table)
			{
				kVP.Value.Index = index;
				index++;
			}
		}

		public virtual bool Exists() => Resources.Load(AssetName()) != null;

		public virtual void Load()
		{
			Debug.Log($"Loading: {AssetName()}");
			if (Exists())
			{
				int index = 0;
				string json = Resources.Load<TextAsset>(AssetName()).text;
				table = (JsonConvert.DeserializeObject<Dictionary<string, T>>(json) ?? new Dictionary<string, T>()).ToDictionary(x => x.Key,
					x =>
					{
						Entity entity = x.Value;
						entity.Id = x.Key;
						entity.Index = index;
						index++;
						return (T) entity;
					});
			}
		}
		public virtual void Create() => table = new Dictionary<string, T>();

		public virtual string AssetName() => $"{typeof(T).Name}.db";
		public virtual string DirectoryPath() => $"{Application.dataPath}/Resources/";

	#if UNITY_EDITOR
		
		public virtual void Save()
		{
			string json = JsonConvert.SerializeObject(table, Formatting.Indented);
			Directory.CreateDirectory(DirectoryPath());
			File.WriteAllText(Path.Combine(DirectoryPath(), $"{AssetName()}.json"), json);
		}
		
		public void RevealInFinder()
		{
			EditorUtility.RevealInFinder($"{Application.dataPath}/Resources/{AssetName()}.json");
		}

		
	#endif
	}
}