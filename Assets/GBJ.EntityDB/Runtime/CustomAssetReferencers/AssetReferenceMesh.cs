using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GBJ.EntityDB.CustomAssetReferencers
{
    [Serializable]
    public class AssetReferenceMesh : AssetReferenceT<Mesh>
    {
        public AssetReferenceMesh(string guid) : base(guid)
        {
        }
    }
}