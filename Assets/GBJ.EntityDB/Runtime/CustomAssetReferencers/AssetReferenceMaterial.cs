using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GBJ.EntityDB.CustomAssetReferencers
{
    [Serializable]
    public class AssetReferenceMaterial : AssetReferenceT<Material>
    {
        public AssetReferenceMaterial(string guid) : base(guid)
        {
        }
    }
}