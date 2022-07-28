using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GBJ.EntityDB.AssetReferenceHolders
{
	public class AssetReferenceTexture2DHolder : AssetReferenceHolder
	{
		public AssetReferenceTexture2D AssetReference;
		public override AssetReference GetAssetReference() => AssetReference;

#if UNITY_EDITOR
		//This field is used to render the AssetReference Drawer in Editor Window.
		[JsonIgnore] public AssetReferenceScriptableObject ScriptableObject;

		public override void Serialize()
		{
			ScriptableObject = UnityEngine.ScriptableObject.CreateInstance<AssetReferenceScriptableObject>();
			SerializedObject = new UnityEditor.SerializedObject(ScriptableObject);
		}

		public override void NewAssetReference(string guid) => AssetReference = new AssetReferenceTexture2D(guid);

		public class AssetReferenceScriptableObject : ScriptableObject
		{
			public AssetReferenceTexture2D AssetReference;
		}
#endif
	}
}