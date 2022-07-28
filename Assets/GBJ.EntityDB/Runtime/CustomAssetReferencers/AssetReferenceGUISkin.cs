using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GBJ.EntityDB.CustomAssetReferencers
{
    [Serializable]
    public class AssetReferenceGUISkin : AssetReferenceT<GUISkin>
    {
        public AssetReferenceGUISkin(string guid) : base(guid)
        {
        }
    }
}