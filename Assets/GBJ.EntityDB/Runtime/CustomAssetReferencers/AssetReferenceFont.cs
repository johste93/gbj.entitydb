using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GBJ.EntityDB.CustomAssetReferencers
{
    [Serializable]
    public class AssetReferenceFont : AssetReferenceT<Font>
    {
        public AssetReferenceFont(string guid) : base(guid)
        {
        }
    }
}