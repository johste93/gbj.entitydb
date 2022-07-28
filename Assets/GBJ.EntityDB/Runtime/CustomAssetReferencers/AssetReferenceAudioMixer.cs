using System;
using UnityEngine.AddressableAssets;
using UnityEngine.Audio;

namespace GBJ.EntityDB.CustomAssetReferencers
{
    [Serializable]
    public class AssetReferenceAudioMixer : AssetReferenceT<AudioMixer>
    {
        public AssetReferenceAudioMixer(string guid) : base(guid)
        {
        }
    }
}