using System;
using GBJ.EntityDB;
using GBJ.EntityDB.AssetReferenceHolders;
using Newtonsoft.Json;

public class DemoEntity : Entity
{
	public string StringValue;
	public int IntValue;
	public float FloatValue;
	public bool BoolValue;
	public StringComparison EnumValue;
	
	[JsonConverter(typeof(AssetReferenceHolderConverter<GenericAssetReferenceHolder>))]
	public GenericAssetReferenceHolder GenericAssetReference = new GenericAssetReferenceHolder();
	
	[JsonConverter(typeof(AssetReferenceHolderConverter<AssetReferenceGameObjectHolder>))]
	public AssetReferenceGameObjectHolder GameObjectReference = new AssetReferenceGameObjectHolder();
	
	[JsonConverter(typeof(AssetReferenceHolderConverter<AssetReferenceSpriteHolder>))]
	public AssetReferenceSpriteHolder SpriteReference = new AssetReferenceSpriteHolder();
	
	[JsonConverter(typeof(AssetReferenceHolderConverter<AssetReferenceAtlasedSpriteHolder>))]
	public AssetReferenceAtlasedSpriteHolder AtlasedSpriteReference = new AssetReferenceAtlasedSpriteHolder();
	
	[JsonConverter(typeof(AssetReferenceHolderConverter<AssetReferenceAudioClipHolder>))]
	public AssetReferenceAudioClipHolder AudioClipReference = new AssetReferenceAudioClipHolder();
	
	[JsonConverter(typeof(AssetReferenceHolderConverter<AssetReferenceTextureHolder>))]
	public AssetReferenceTextureHolder TextureReference = new AssetReferenceTextureHolder();
	
	[JsonConverter(typeof(AssetReferenceHolderConverter<AssetReferenceTexture2DHolder>))]
	public AssetReferenceTexture2DHolder Texture2DReference = new AssetReferenceTexture2DHolder();
	
	[JsonConverter(typeof(AssetReferenceHolderConverter<AssetReferenceTexture3DHolder>))]
	public AssetReferenceTexture3DHolder Texture3DReference = new AssetReferenceTexture3DHolder();
	
	[JsonConverter(typeof(AssetReferenceHolderConverter<AssetReferenceSceneHolder>))]
	public AssetReferenceSceneHolder SceneReference = new AssetReferenceSceneHolder();
	
	[JsonConverter(typeof(AssetReferenceHolderConverter<AssetReferenceAnimationClipHolder>))]
	public AssetReferenceAnimationClipHolder AnimationClipReference = new AssetReferenceAnimationClipHolder();
	
	[JsonConverter(typeof(AssetReferenceHolderConverter<AssetReferenceAudioMixerHolder>))]
	public AssetReferenceAudioMixerHolder AudioMixerReference = new AssetReferenceAudioMixerHolder();
	
	[JsonConverter(typeof(AssetReferenceHolderConverter<AssetReferenceComputeShaderHolder>))]
	public AssetReferenceComputeShaderHolder ComputeShaderReference = new AssetReferenceComputeShaderHolder();
	
	[JsonConverter(typeof(AssetReferenceHolderConverter<AssetReferenceFontHolder>))]
	public AssetReferenceFontHolder FontReference = new AssetReferenceFontHolder();
	
	[JsonConverter(typeof(AssetReferenceHolderConverter<AssetReferenceGUISkinHolder>))]
	public AssetReferenceGUISkinHolder GUISkinReference = new AssetReferenceGUISkinHolder();
	
	[JsonConverter(typeof(AssetReferenceHolderConverter<AssetReferenceMaterialHolder>))]
	public AssetReferenceMaterialHolder MaterialReference = new AssetReferenceMaterialHolder();
	
	[JsonConverter(typeof(AssetReferenceHolderConverter<AssetReferenceMeshHolder>))]
	public AssetReferenceMeshHolder MeshReference = new AssetReferenceMeshHolder();
	
	[JsonConverter(typeof(AssetReferenceHolderConverter<AssetReferencePhysicMaterialHolder>))]
	public AssetReferencePhysicMaterialHolder PhysicMaterialReference = new AssetReferencePhysicMaterialHolder();

	[JsonConverter(typeof(AssetReferenceHolderConverter<AssetReferenceShaderHolder>))]
	public AssetReferenceShaderHolder ShaderReference = new AssetReferenceShaderHolder();
	
	[JsonConverter(typeof(AssetReferenceHolderConverter<AssetReferenceVideoClipHolder>))]
	public AssetReferenceVideoClipHolder VideoClipReference = new AssetReferenceVideoClipHolder();
	
	[JsonConverter(typeof(AssetReferenceHolderConverter<AssetReferenceModelHolder>))]
	public AssetReferenceModelHolder ModelReference = new AssetReferenceModelHolder();
	
	[JsonConverter(typeof(AssetReferenceHolderConverter<AssetReferenceTextAssetHolder>))]
	public AssetReferenceTextAssetHolder TextAssetReference = new AssetReferenceTextAssetHolder();
}
