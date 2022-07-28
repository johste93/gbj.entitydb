using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GBJ.EntityDB.CustomAssetReferencers
{
    [Serializable]
    public class AssetReferencePhysicMaterial : AssetReferenceT<PhysicMaterial>
    {
        public AssetReferencePhysicMaterial(string guid) : base(guid)
        {
        }
    }
}