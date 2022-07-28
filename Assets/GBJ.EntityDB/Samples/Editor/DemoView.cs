using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using GBJ.EntityDB.Editor;

namespace GBJ.EntityDB
{
	public class DemoView : TableView<DemoEntity>
	{
		[MenuItem("Tables/Demo")]
		static void Init() => Init<DemoView>();

		protected override int maxColumnCount => 25;
		protected override void CreateNewEntry() => @new = new DemoEntity();

		protected override void DrawColumnNames()
		{
			DrawColumn("String", x => x.Value.StringValue);
			DrawColumn("Int", x => x.Value.IntValue);
			DrawColumn("Float", x => x.Value.FloatValue);
			DrawColumn("Bool", x => x.Value.BoolValue);
			DrawColumn("Enum", x => x.Value.EnumValue);
			DrawColumn("Generic Asset", x => x.Value.GenericAssetReference);
			DrawColumn("GameObject", x => x.Value.GameObjectReference);
			DrawColumn("Sprite", x => x.Value.SpriteReference);
			DrawColumn("Atlased Sprite", x => x.Value.AtlasedSpriteReference);
			DrawColumn("AudioClip", x => x.Value.AudioClipReference);
			DrawColumn("Texture", x => x.Value.TextureReference);
			DrawColumn("Texture2D", x => x.Value.Texture2DReference);
			DrawColumn("Texture3D", x => x.Value.Texture3DReference);
			DrawColumn("Scene", x => x.Value.SceneReference);
			DrawColumn("AnimationClip", x => x.Value.AnimationClipReference);
			DrawColumn("AudioMixer", x => x.Value.AudioMixerReference);
			DrawColumn("ComputeShader", x => x.Value.ComputeShaderReference);
			DrawColumn("Font", x => x.Value.FontReference);
			DrawColumn("GUISkin", x => x.Value.GUISkinReference);
			DrawColumn("Material", x => x.Value.MaterialReference);
			DrawColumn("Mesh", x => x.Value.MeshReference);
			DrawColumn("PhysicMaterial", x => x.Value.PhysicMaterialReference);
			DrawColumn("Shader", x => x.Value.ShaderReference);
			DrawColumn("VideoClip", x => x.Value.VideoClipReference);
			DrawColumn("Model", x => x.Value.ModelReference);
		}

		protected override void DrawRow(DemoEntity entry, DemoEntity unmodified, bool changeColorIfChanged = false)
		{
			this.DrawEntry(() => entry.StringValue, x => entry.StringValue = x, changeColorIfChanged, () => unmodified?.StringValue);
			this.DrawEntry(() => entry.IntValue, x => entry.IntValue = x, changeColorIfChanged, () => unmodified?.IntValue);
			this.DrawEntry(() => entry.FloatValue, x => entry.FloatValue = x, changeColorIfChanged, () => unmodified?.FloatValue);
			this.DrawEntry(() => entry.BoolValue, x => entry.BoolValue = x, changeColorIfChanged, () => unmodified?.BoolValue);
			this.DrawEntry(() => (int) entry.EnumValue, x => entry.EnumValue = (StringComparison) x, typeof(StringComparison), changeColorIfChanged, () => (int?) unmodified?.EnumValue);
			this.DrawEntry(() => entry.GenericAssetReference, x => entry.GenericAssetReference = x, changeColorIfChanged, () => unmodified?.GenericAssetReference);
			this.DrawEntry(() => entry.GameObjectReference, x => entry.GameObjectReference = x, changeColorIfChanged, () => unmodified?.GameObjectReference);
			this.DrawEntry(() => entry.SpriteReference, x => entry.SpriteReference = x, changeColorIfChanged, () => unmodified?.SpriteReference);
			this.DrawEntry(() => entry.AtlasedSpriteReference, x => entry.AtlasedSpriteReference = x, changeColorIfChanged, () => unmodified?.AtlasedSpriteReference);
			this.DrawEntry(() => entry.AudioClipReference, x => entry.AudioClipReference = x, changeColorIfChanged, () => unmodified?.AudioClipReference);
			this.DrawEntry(() => entry.TextureReference, x => entry.TextureReference = x, changeColorIfChanged, () => unmodified?.TextureReference);
			this.DrawEntry(() => entry.Texture2DReference, x => entry.Texture2DReference = x, changeColorIfChanged, () => unmodified?.Texture2DReference);
			this.DrawEntry(() => entry.Texture3DReference, x => entry.Texture3DReference = x, changeColorIfChanged, () => unmodified?.Texture3DReference);
			this.DrawEntry(() => entry.SceneReference, x => entry.SceneReference = x, changeColorIfChanged, () => unmodified?.SceneReference);
			this.DrawEntry(() => entry.AnimationClipReference, x => entry.AnimationClipReference = x, changeColorIfChanged, () => unmodified?.AnimationClipReference);
			this.DrawEntry(() => entry.AudioMixerReference, x => entry.AudioMixerReference = x, changeColorIfChanged, () => unmodified?.AudioMixerReference);
			this.DrawEntry(() => entry.ComputeShaderReference, x => entry.ComputeShaderReference = x, changeColorIfChanged, () => unmodified?.ComputeShaderReference);
			this.DrawEntry(() => entry.FontReference, x => entry.FontReference = x, changeColorIfChanged, () => unmodified?.FontReference);
			this.DrawEntry(() => entry.GUISkinReference, x => entry.GUISkinReference = x, changeColorIfChanged, () => unmodified?.GUISkinReference);
			this.DrawEntry(() => entry.MaterialReference, x => entry.MaterialReference = x, changeColorIfChanged, () => unmodified?.MaterialReference);
			this.DrawEntry(() => entry.MeshReference, x => entry.MeshReference = x, changeColorIfChanged, () => unmodified?.MeshReference);
			this.DrawEntry(() => entry.PhysicMaterialReference, x => entry.PhysicMaterialReference = x, changeColorIfChanged, () => unmodified?.PhysicMaterialReference);
			this.DrawEntry(() => entry.ShaderReference, x => entry.ShaderReference = x, changeColorIfChanged, () => unmodified?.ShaderReference);
			this.DrawEntry(() => entry.VideoClipReference, x => entry.VideoClipReference = x, changeColorIfChanged, () => unmodified?.VideoClipReference);
			this.DrawEntry(() => entry.ModelReference, x => entry.ModelReference = x, changeColorIfChanged, () => unmodified?.ModelReference);
		}
	}
}