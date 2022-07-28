using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GBJ.EntityDB.CustomAssetReferencers
{
    [Serializable]
    public class AssetReferenceComputeShader : AssetReferenceT<ComputeShader>
    {
        public AssetReferenceComputeShader(string guid) : base(guid)
        {
        }
    }
}