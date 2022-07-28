using System;
using UnityEditor;
using UnityEngine.AddressableAssets;
using UnityEngine.U2D;

/// GBJ.EntityDB.AssetReferenceAtlasedSprite replaces UnityEngine.AddressableAssets.AssetReferenceAtlasedSprite so that it will render properly in the DB View.
namespace GBJ.EntityDB.CustomAssetReferencers
{
    [Serializable]
    public class AssetReferenceAtlasedSprite : AssetReferenceT<SpriteAtlas> //Changed from Sprite -> SpriteAtlas
    {
        public AssetReferenceAtlasedSprite(string guid) : base(guid)
        {
        }

        
        public override bool ValidateAsset(UnityEngine.Object obj)
        {
	        return obj is SpriteAtlas;
        }
        
        public override bool ValidateAsset(string path)
        {
#if UNITY_EDITOR
            return AssetDatabase.GetMainAssetTypeAtPath(path) == typeof(SpriteAtlas);
#else
            return false;
#endif
        }
        
#if UNITY_EDITOR
        /// <summary>
        /// SpriteAtlas Type-specific override of parent editorAsset. Used by the editor to represent the main asset referenced.
        /// </summary>
        public new SpriteAtlas editorAsset
        {
	        get
	        {
		        if (CachedAsset != null || string.IsNullOrEmpty(AssetGUID))
			        return CachedAsset as SpriteAtlas;

		        var assetPath = AssetDatabase.GUIDToAssetPath(AssetGUID);
		        var main = AssetDatabase.LoadMainAssetAtPath(assetPath) as SpriteAtlas;
		        if (main != null)
			        CachedAsset = main;
		        return main;
	        }
        }
#endif
    }
}