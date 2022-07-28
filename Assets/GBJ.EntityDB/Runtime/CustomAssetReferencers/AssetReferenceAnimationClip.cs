using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GBJ.EntityDB.CustomAssetReferencers
{
    [Serializable]
    public class AssetReferenceAnimationClip : AssetReferenceT<AnimationClip>
    {
        public AssetReferenceAnimationClip(string guid) : base(guid)
        {
        }
    }
}