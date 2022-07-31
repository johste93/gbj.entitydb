using GBJ.EntityDB.CustomAssetReferencers;
using Newtonsoft.Json;
using UnityEngine.AddressableAssets;
using UnityEngine;

namespace GBJ.EntityDB.AssetReferenceHolders
{
	public class AssetReferenceSceneHolder : AssetReferenceHolder
	{
		public AssetReferenceScene AssetReference;
		public override AssetReference GetAssetReference() => AssetReference;
		public override void NewAssetReference(string guid) => AssetReference = new AssetReferenceScene(guid);

#if UNITY_EDITOR
		//This field is used to render the AssetReference Drawer in Editor Window.
		[JsonIgnore] public AssetReferenceScriptableObject ScriptableObject;

		public override void Serialize()
		{
			ScriptableObject = UnityEngine.ScriptableObject.CreateInstance<AssetReferenceScriptableObject>();
			SerializedObject = new UnityEditor.SerializedObject(ScriptableObject);
		}
		
		public class AssetReferenceScriptableObject : ScriptableObject
		{
			public AssetReferenceScene AssetReference;
		}
#endif
	}
}