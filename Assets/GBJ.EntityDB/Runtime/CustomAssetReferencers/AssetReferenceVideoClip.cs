using System;
using UnityEngine.Video;
using UnityEngine.AddressableAssets;

namespace GBJ.EntityDB.CustomAssetReferencers
{
    [Serializable]
    public class AssetReferenceVideoClip : AssetReferenceT<VideoClip>
    {
        public AssetReferenceVideoClip(string guid) : base(guid)
        {
        }
    }
}