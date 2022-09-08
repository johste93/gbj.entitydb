using GBJ.EntityDB.CustomAssetReferencers;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GBJ.EntityDB.AssetReferenceHolders
{

    public class AssetReferenceTextAssetHolder : AssetReferenceHolder
    {
        public AssetReferenceTextAsset AssetReference;
        public override AssetReference GetAssetReference() => AssetReference;
        public override void NewAssetReference(string guid) => AssetReference = new AssetReferenceTextAsset(guid);

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
            public AssetReferenceTextAsset AssetReference;
        }
#endif
    }
}