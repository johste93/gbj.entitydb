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

		protected override int maxColumnCount => 30;
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
			DrawColumn("Text Asset", x => x.Value.TextAssetReference);
			DrawColumn("Strings", x => x.Value.Strings);
			DrawColumn("Ints", x => x.Value.Ints);
			DrawColumn("Floats", x => x.Value.Floats);
			DrawColumn("Bools", x => x.Value.Bools);
			DrawColumn("Enums", x => x.Value.Enums);
		}

		protected override void DrawRow(DemoEntity entry, DemoEntity unmodified, bool changeColorIfChanged = false)
		{
			this.DrawCell(() => entry.StringValue, x => entry.StringValue = x, changeColorIfChanged, () => unmodified?.StringValue);
			this.DrawCell(() => entry.IntValue, x => entry.IntValue = x, changeColorIfChanged, () => unmodified?.IntValue);
			this.DrawCell(() => entry.FloatValue, x => entry.FloatValue = x, changeColorIfChanged, () => unmodified?.FloatValue);
			this.DrawCell(() => entry.BoolValue, x => entry.BoolValue = x, changeColorIfChanged, () => unmodified?.BoolValue);
			this.DrawCell(() => (int) entry.EnumValue, x => entry.EnumValue = (StringComparison) x, typeof(StringComparison), changeColorIfChanged, () => (int?) unmodified?.EnumValue);
			this.DrawCell(() => entry.GenericAssetReference, x => entry.GenericAssetReference = x, changeColorIfChanged, () => unmodified?.GenericAssetReference);
			this.DrawCell(() => entry.GameObjectReference, x => entry.GameObjectReference = x, changeColorIfChanged, () => unmodified?.GameObjectReference);
			this.DrawCell(() => entry.SpriteReference, x => entry.SpriteReference = x, changeColorIfChanged, () => unmodified?.SpriteReference);
			this.DrawCell(() => entry.AtlasedSpriteReference, x => entry.AtlasedSpriteReference = x, changeColorIfChanged, () => unmodified?.AtlasedSpriteReference);
			this.DrawCell(() => entry.AudioClipReference, x => entry.AudioClipReference = x, changeColorIfChanged, () => unmodified?.AudioClipReference);
			this.DrawCell(() => entry.TextureReference, x => entry.TextureReference = x, changeColorIfChanged, () => unmodified?.TextureReference);
			this.DrawCell(() => entry.Texture2DReference, x => entry.Texture2DReference = x, changeColorIfChanged, () => unmodified?.Texture2DReference);
			this.DrawCell(() => entry.Texture3DReference, x => entry.Texture3DReference = x, changeColorIfChanged, () => unmodified?.Texture3DReference);
			this.DrawCell(() => entry.SceneReference, x => entry.SceneReference = x, changeColorIfChanged, () => unmodified?.SceneReference);
			this.DrawCell(() => entry.AnimationClipReference, x => entry.AnimationClipReference = x, changeColorIfChanged, () => unmodified?.AnimationClipReference);
			this.DrawCell(() => entry.AudioMixerReference, x => entry.AudioMixerReference = x, changeColorIfChanged, () => unmodified?.AudioMixerReference);
			this.DrawCell(() => entry.ComputeShaderReference, x => entry.ComputeShaderReference = x, changeColorIfChanged, () => unmodified?.ComputeShaderReference);
			this.DrawCell(() => entry.FontReference, x => entry.FontReference = x, changeColorIfChanged, () => unmodified?.FontReference);
			this.DrawCell(() => entry.GUISkinReference, x => entry.GUISkinReference = x, changeColorIfChanged, () => unmodified?.GUISkinReference);
			this.DrawCell(() => entry.MaterialReference, x => entry.MaterialReference = x, changeColorIfChanged, () => unmodified?.MaterialReference);
			this.DrawCell(() => entry.MeshReference, x => entry.MeshReference = x, changeColorIfChanged, () => unmodified?.MeshReference);
			this.DrawCell(() => entry.PhysicMaterialReference, x => entry.PhysicMaterialReference = x, changeColorIfChanged, () => unmodified?.PhysicMaterialReference);
			this.DrawCell(() => entry.ShaderReference, x => entry.ShaderReference = x, changeColorIfChanged, () => unmodified?.ShaderReference);
			this.DrawCell(() => entry.VideoClipReference, x => entry.VideoClipReference = x, changeColorIfChanged, () => unmodified?.VideoClipReference);
			this.DrawCell(() => entry.ModelReference, x => entry.ModelReference = x, changeColorIfChanged, () => unmodified?.ModelReference);
			this.DrawCell(() => entry.TextAssetReference, x => entry.TextAssetReference = x, changeColorIfChanged, () => unmodified?.TextAssetReference);
			this.DrawCellList(() => entry.Strings, x => entry.Strings = x, changeColorIfChanged, () => unmodified?.Strings);
			this.DrawCellList(() => entry.Ints, x => entry.Ints = x, changeColorIfChanged, () => unmodified?.Ints);
			this.DrawCellList(() => entry.Floats, x => entry.Floats = x, changeColorIfChanged, () => unmodified?.Floats);
			this.DrawCellList(() => entry.Bools, x => entry.Bools = x, changeColorIfChanged, () => unmodified?.Bools);
			this.DrawCellList(() => entry.Enums, x => entry.Enums = x, typeof(StringComparison), changeColorIfChanged, () => unmodified?.Enums);
		}
	}
}