using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GBJ.EntityDB.CustomAssetReferencers
{
    [Serializable]
    public class AssetReferenceAudioClip : AssetReferenceT<AudioClip>
    {
        public AssetReferenceAudioClip(string guid) : base(guid)
        {
        }
    }
}