using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GBJ.EntityDB.CustomAssetReferencers
{
    [Serializable]
    public class AssetReferenceModel : AssetReferenceT<GameObject>
    {
        public AssetReferenceModel(string guid) : base(guid)
        {
        }

        public override bool ValidateAsset(string path)
        {
            return path.EndsWith(".fbx") || path.EndsWith(".obj") || path.EndsWith(".max") || path.EndsWith(".blend");
        }
    }
}