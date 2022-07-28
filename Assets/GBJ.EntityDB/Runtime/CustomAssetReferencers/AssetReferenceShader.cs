using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GBJ.EntityDB.CustomAssetReferencers
{
    [Serializable]
    public class AssetReferenceShader : AssetReferenceT<Shader>
    {
        public AssetReferenceShader(string guid) : base(guid)
        {
        }
    }
}