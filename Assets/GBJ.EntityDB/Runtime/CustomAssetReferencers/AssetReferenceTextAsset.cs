using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GBJ.EntityDB.CustomAssetReferencers
{
    [Serializable]
    public class AssetReferenceTextAsset : AssetReferenceT<TextAsset>
    {
        public AssetReferenceTextAsset(string guid) : base(guid)
        {
        }
    }
}