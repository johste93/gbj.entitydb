using System;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GBJ.EntityDB.AssetReferenceHolders
{
    public class AssetReferenceGameObjectHolder : AssetReferenceHolder
    {
        public AssetReferenceGameObject AssetReference;
        public override AssetReference GetAssetReference() => AssetReference;

#if UNITY_EDITOR
        //This field is used to render the AssetReference Drawer in Editor Window.
        [JsonIgnore] public AssetReferenceScriptableObject ScriptableObject;

        public override void Serialize()
        {
            ScriptableObject = UnityEngine.ScriptableObject.CreateInstance<AssetReferenceScriptableObject>();
            SerializedObject = new UnityEditor.SerializedObject(ScriptableObject);
        }

        public override void NewAssetReference(string guid) => AssetReference = new AssetReferenceGameObject(guid);

        public class AssetReferenceScriptableObject : ScriptableObject
        {
            public AssetReferenceGameObject AssetReference;
        }
#endif
    }
}