using System;
using UnityEngine.AddressableAssets;

namespace GBJ.EntityDB.CustomAssetReferencers
{
	[Serializable]
	public class AssetReferenceScene : AssetReference
	{
		public AssetReferenceScene(string guid) : base(guid)
		{
		}

		public override bool ValidateAsset(UnityEngine.Object obj)
		{
#if UNITY_EDITOR
			var type = obj.GetType();
			return typeof(UnityEditor.SceneAsset).IsAssignableFrom(type);
#else
        return false;
#endif
		}
		
		public override bool ValidateAsset(string path)
		{
#if UNITY_EDITOR
			var type = UnityEditor.AssetDatabase.GetMainAssetTypeAtPath(path);
			return typeof(UnityEditor.SceneAsset).IsAssignableFrom(type);
#else
        return false;
#endif
		}
		
#if UNITY_EDITOR
		/// <summary>
		/// Type-specific override of parent editorAsset.  Used by the editor to represent the asset referenced.
		/// </summary>
		public new UnityEditor.SceneAsset editorAsset => (UnityEditor.SceneAsset)base.editorAsset;
#endif
	}
}