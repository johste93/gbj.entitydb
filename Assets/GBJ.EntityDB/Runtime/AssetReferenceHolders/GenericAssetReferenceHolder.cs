using Newtonsoft.Json;
using UnityEngine.AddressableAssets;
using UnityEngine;

namespace GBJ.EntityDB.AssetReferenceHolders
{
	public class GenericAssetReferenceHolder : AssetReferenceHolder
	{
		public AssetReference AssetReference;
		public override AssetReference GetAssetReference() => AssetReference;


#if UNITY_EDITOR
		//This field is used to render the AssetReference Drawer in Editor Window.
		[JsonIgnore] public AssetReferenceScriptableObject ScriptableObject;

		public override void Serialize()
		{
			ScriptableObject = UnityEngine.ScriptableObject.CreateInstance<AssetReferenceScriptableObject>();
			SerializedObject = new UnityEditor.SerializedObject(ScriptableObject);
		}

		public override void NewAssetReference(string guid) => AssetReference = new AssetReference(guid);

		public class AssetReferenceScriptableObject : ScriptableObject
		{
			public AssetReference AssetReference;
		}
#endif
	}
}