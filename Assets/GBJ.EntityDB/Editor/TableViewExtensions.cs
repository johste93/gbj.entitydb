#nullable enable
using System;
using System.Linq;
using GBJ.EntityDB.AssetReferenceHolders;
using GBJ.EntityDB.CustomAssetReferencers;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using AssetReferenceAtlasedSprite = UnityEngine.AddressableAssets.AssetReferenceAtlasedSprite;

namespace GBJ.EntityDB.Editor
{
    public static class TableViewExtensions
    {
        public static void DrawEmpty<T>(this TableView<T> view) where T : Entity
        {
            GUIStyle style = new GUIStyle("box");
            style.margin.left = style.margin.right = style.margin.top = style.margin.bottom = 0;
            GUILayout.Box(string.Empty, style, GUILayout.Width(view.columnWidth));
        }
            
        public static void DrawCell<T>(this TableView<T> view, Getter<string> getter, Setter<string> setter, bool changeColorIfChanged = false, Getter<string?> oldValue = null) where T : Entity
        {
            if (changeColorIfChanged && oldValue != null)
                GUI.color = oldValue() == getter() ? Color.white : Color.cyan;

            setter(EditorGUILayout.TextField(getter(), GUILayout.Width(view.columnWidth)));
        }
        
        public static void DrawTextAreaCell<T>(this TableView<T> view, Getter<string> getter, Setter<string> setter, bool changeColorIfChanged = false, Getter<string?> oldValue = null) where T : Entity
        {
            if (changeColorIfChanged && oldValue != null)
                GUI.color = oldValue() == getter() ? Color.white : Color.cyan;

            setter(EditorGUILayout.TextArea(getter(), GUILayout.Width(view.columnWidth)));
        }

        public static void DrawCell<T>(this TableView<T> view, Getter<bool> getter, Setter<bool> setter, bool changeColorIfChanged = false, Getter<bool?> oldValue = null) where T : Entity
        {
            if (changeColorIfChanged && oldValue != null)
                GUI.color = oldValue() == getter() ? Color.white : Color.cyan;

            setter(EditorGUILayout.Toggle(getter(), GUILayout.Width(view.columnWidth)));
        }

        public static void DrawCell<T>(this TableView<T> view, Getter<int> getter, Setter<int> setter, bool changeColorIfChanged = false, Getter<int?> oldValue = null) where T : Entity
        {
            if (changeColorIfChanged && oldValue != null)
                GUI.color = oldValue() == getter() ? Color.white : Color.cyan;

            setter(EditorGUILayout.IntField(getter(), GUILayout.Width(view.columnWidth)));
        }

        public static void DrawCell<T>(this TableView<T> view, Getter<float> getter, Setter<float> setter, bool changeColorIfChanged = false, Getter<float?> oldValue = null) where T : Entity
        {
            if (changeColorIfChanged && oldValue != null)
                GUI.color = oldValue() == null || Mathf.Approximately(oldValue().Value, getter()) ? Color.white : Color.cyan;

            setter(EditorGUILayout.FloatField(getter(), GUILayout.Width(view.columnWidth)));
        }

        public static void DrawCell<T>(this TableView<T> view, Getter<int> getter, Setter<int> setter, Type enumType, bool changeColorIfChanged = false, Getter<int?> oldValue = null) where T : Entity
        {
            if (!enumType.IsEnum)
            {
                throw new ArgumentException("TEnum must be an enumerated type");
            }

            if (changeColorIfChanged && oldValue != null)
                GUI.color = oldValue() == getter() ? Color.white : Color.cyan;

            setter(EditorGUILayout.Popup(getter(), Enum.GetNames(enumType).Select(k => k.Replace("_", "/")).ToArray(), GUILayout.Width(view.columnWidth)));
        }

        public static void DrawCell<T>(this TableView<T> view, Getter<AssetReferenceGameObjectHolder> getter, Setter<AssetReferenceGameObjectHolder> setter, bool changeColorIfChanged = false, Getter<AssetReferenceGameObjectHolder?> oldValue = null) where T : Entity
        {
            if (changeColorIfChanged && oldValue != null)
                GUI.color = (getter() == null && oldValue() == null) || getter().Equals(oldValue()) ? Color.white : Color.cyan;

            if (getter() == null)
                setter(new AssetReferenceGameObjectHolder());

            if (getter().ScriptableObject == null)
                getter().Serialize();

            if (!string.IsNullOrEmpty(getter().AssetReference?.AssetGUID) && getter().AssetReference?.editorAsset == null)
                GUI.color = Color.red; //Busted reference

            getter().ScriptableObject.AssetReference = getter().AssetReference;
            getter().SerializedObject.Update();
            var property = getter().SerializedObject.FindProperty(nameof(AssetReferenceGameObjectHolder.AssetReferenceScriptableObject.AssetReference));
            EditorGUILayout.PropertyField(property, GUIContent.none, true, GUILayout.Width(view.columnWidth));

            if (getter().ScriptableObject.AssetReference == null || !getter().ScriptableObject.AssetReference.RuntimeKeyIsValid())
            {
                getter().AssetReference = null;
            }
            else
            {
                if (getter().AssetReference == null || !getter().AssetReference.AssetGUID.Equals(getter().ScriptableObject.AssetReference.AssetGUID))
                {
                    getter().AssetReference = new AssetReferenceGameObject(getter().ScriptableObject.AssetReference.AssetGUID);
                }
                else
                {
                    getter().AssetReference.SubObjectName = getter().ScriptableObject.AssetReference.SubObjectName;
                }
            }
        }

        public static void DrawCell<T>(this TableView<T> view, Getter<AssetReferenceSpriteHolder> getter, Setter<AssetReferenceSpriteHolder> setter, bool changeColorIfChanged = false, Getter<AssetReferenceSpriteHolder?> oldValue = null) where T : Entity
        {
            if (changeColorIfChanged && oldValue != null)
                GUI.color = (getter() == null && oldValue() == null) || getter().Equals(oldValue()) ? Color.white : Color.cyan;

            if (getter() == null)
                setter(new AssetReferenceSpriteHolder());

            if (getter().ScriptableObject == null)
                getter().Serialize();

            if (!string.IsNullOrEmpty(getter().AssetReference?.AssetGUID) && getter().AssetReference?.editorAsset == null)
                GUI.color = Color.red; //Busted reference
            
            getter().ScriptableObject.AssetReference = getter().AssetReference;
            getter().SerializedObject.Update();
            var spriteProperty = getter().SerializedObject.FindProperty(nameof(AssetReferenceSpriteHolder.AssetReferenceScriptableObject.AssetReference));
            EditorGUILayout.PropertyField(spriteProperty, GUIContent.none, true, GUILayout.Width(view.columnWidth));

            if (getter().ScriptableObject.AssetReference == null || !getter().ScriptableObject.AssetReference.RuntimeKeyIsValid())
            {
                getter().AssetReference = null;
            }
            else
            {
                if (getter().AssetReference == null || !getter().AssetReference.AssetGUID.Equals(getter().ScriptableObject.AssetReference.AssetGUID))
                {
                    getter().AssetReference = new AssetReferenceSprite(getter().ScriptableObject.AssetReference.AssetGUID);
                }
                else
                {
                    getter().AssetReference.SubObjectName = getter().ScriptableObject.AssetReference.SubObjectName;
                }
            }
        }
        
        public static void DrawCell<T>(this TableView<T> view, Getter<AssetReferenceAtlasedSpriteHolder> getter, Setter<AssetReferenceAtlasedSpriteHolder> setter, bool changeColorIfChanged = false, Getter<AssetReferenceAtlasedSpriteHolder?> oldValue = null) where T : Entity
        {
            if (changeColorIfChanged && oldValue != null)
                GUI.color = (getter() == null && oldValue() == null) || getter().Equals(oldValue()) ? Color.white : Color.cyan;

            if (getter() == null)
                setter(new AssetReferenceAtlasedSpriteHolder());

            if (getter().ScriptableObject == null)
                getter().Serialize();

            if (!string.IsNullOrEmpty(getter().AssetReference?.AssetGUID) && getter().AssetReference?.editorAsset == null)
                GUI.color = Color.red; //Busted reference
            
            getter().ScriptableObject.AssetReference = getter().AssetReference;
            getter().SerializedObject.Update();
            var property = getter().SerializedObject.FindProperty(nameof(AssetReferenceAtlasedSpriteHolder.AssetReferenceScriptableObject.AssetReference));
            EditorGUILayout.PropertyField(property, GUIContent.none, true, GUILayout.Width(view.columnWidth));

            if (getter().ScriptableObject.AssetReference == null || !getter().ScriptableObject.AssetReference.RuntimeKeyIsValid())
            {
                getter().AssetReference = null;
            }
            else
            {
                if (getter().AssetReference == null || !getter().AssetReference.AssetGUID.Equals(getter().ScriptableObject.AssetReference.AssetGUID))
                {
                    getter().AssetReference = new AssetReferenceAtlasedSprite(getter().ScriptableObject.AssetReference.AssetGUID);
                }
                else
                {
                    getter().AssetReference.SubObjectName = getter().ScriptableObject.AssetReference.SubObjectName;
                }
            }
        }

        public static void DrawCell<T>(this TableView<T> view, Getter<AssetReferenceAudioClipHolder> getter, Setter<AssetReferenceAudioClipHolder> setter, bool changeColorIfChanged = false, Getter<AssetReferenceAudioClipHolder?> oldValue = null) where T : Entity
        {
            if (changeColorIfChanged && oldValue != null)
                GUI.color = (getter() == null && oldValue() == null) || getter().Equals(oldValue()) ? Color.white : Color.cyan;

            if (getter() == null)
                setter(new AssetReferenceAudioClipHolder());

            if (getter().ScriptableObject == null)
                getter().Serialize();

            if (!string.IsNullOrEmpty(getter().AssetReference?.AssetGUID) && getter().AssetReference?.editorAsset == null)
                GUI.color = Color.red; //Busted reference
            
            getter().ScriptableObject.AssetReference = getter().AssetReference;
            getter().SerializedObject.Update();
            var property = getter().SerializedObject.FindProperty(nameof(AssetReferenceAudioClipHolder.AssetReferenceScriptableObject.AssetReference));
            EditorGUILayout.PropertyField(property, GUIContent.none, true, GUILayout.Width(view.columnWidth));
            
            if (getter().ScriptableObject.AssetReference == null || !getter().ScriptableObject.AssetReference.RuntimeKeyIsValid())
            {
                getter().AssetReference = null;
            }
            else
            {
                if (getter().AssetReference == null || !getter().AssetReference.AssetGUID.Equals(getter().ScriptableObject.AssetReference.AssetGUID))
                {
                    getter().AssetReference = new AssetReferenceAudioClip(getter().ScriptableObject.AssetReference.AssetGUID);
                }
                else
                {
                    getter().AssetReference.SubObjectName = getter().ScriptableObject.AssetReference.SubObjectName;
                }
            }
        }

        public static void DrawCell<T>(this TableView<T> view, Getter<GenericAssetReferenceHolder> getter, Setter<GenericAssetReferenceHolder> setter, bool changeColorIfChanged = false, Getter<GenericAssetReferenceHolder?> oldValue = null) where T : Entity
        {
            if (changeColorIfChanged && oldValue != null)
                GUI.color = (getter() == null && oldValue() == null) || getter().Equals(oldValue()) ? Color.white : Color.cyan;

            if (getter() == null)
                setter(new GenericAssetReferenceHolder());

            if (getter().ScriptableObject == null)
                getter().Serialize();

            if (!string.IsNullOrEmpty(getter().AssetReference?.AssetGUID) && getter().AssetReference?.editorAsset == null)
                GUI.color = Color.red; //Busted reference
            
            getter().ScriptableObject.AssetReference = getter().AssetReference;
            getter().SerializedObject.Update();
            var property = getter().SerializedObject.FindProperty(nameof(GenericAssetReferenceHolder.AssetReferenceScriptableObject.AssetReference));
            EditorGUILayout.PropertyField(property, GUIContent.none, true, GUILayout.Width(view.columnWidth));

            if (getter().ScriptableObject.AssetReference == null || !getter().ScriptableObject.AssetReference.RuntimeKeyIsValid())
            {
                getter().AssetReference = null;
            }
            else
            {
                if (getter().AssetReference == null || !getter().AssetReference.AssetGUID.Equals(getter().ScriptableObject.AssetReference.AssetGUID))
                {
                    getter().AssetReference = new AssetReference(getter().ScriptableObject.AssetReference.AssetGUID);
                }
                else
                {
                    getter().AssetReference.SubObjectName = getter().ScriptableObject.AssetReference.SubObjectName;
                }
            }
        }

        public static void DrawCell<T>(this TableView<T> view, [CanBeNull] Getter<AssetReferenceTextureHolder> getter, Setter<AssetReferenceTextureHolder> setter, bool changeColorIfChanged = false, Getter<AssetReferenceTextureHolder?> oldValue = null) where T : Entity
        {
            if (changeColorIfChanged && oldValue != null)
                GUI.color = (getter() == null && oldValue() == null) || getter().Equals(oldValue()) ? Color.white : Color.cyan;

            if (getter() == null)
                setter(new AssetReferenceTextureHolder());

            if (getter().ScriptableObject == null)
                getter().Serialize();

            if (!string.IsNullOrEmpty(getter().AssetReference?.AssetGUID) && getter().AssetReference?.editorAsset == null)
                GUI.color = Color.red; //Busted reference
            
            getter().ScriptableObject.AssetReference = getter().AssetReference;
            getter().SerializedObject.Update();
            var property = getter().SerializedObject.FindProperty(nameof(AssetReferenceTextureHolder.AssetReferenceScriptableObject.AssetReference));
            EditorGUILayout.PropertyField(property, GUIContent.none, true, GUILayout.Width(view.columnWidth));

            if (getter().ScriptableObject.AssetReference == null || !getter().ScriptableObject.AssetReference.RuntimeKeyIsValid())
            {
                getter().AssetReference = null;
            }
            else
            {
                if (getter().AssetReference == null || !getter().AssetReference.AssetGUID.Equals(getter().ScriptableObject.AssetReference.AssetGUID))
                {
                    getter().AssetReference = new AssetReferenceTexture(getter().ScriptableObject.AssetReference.AssetGUID);
                }
                else
                {
                    getter().AssetReference.SubObjectName = getter().ScriptableObject.AssetReference.SubObjectName;
                }
            }
        }

        public static void DrawCell<T>(this TableView<T> view, [CanBeNull] Getter<AssetReferenceTexture2DHolder> getter, Setter<AssetReferenceTexture2DHolder> setter, bool changeColorIfChanged = false, Getter<AssetReferenceTexture2DHolder?> oldValue = null) where T : Entity
        {
            if (changeColorIfChanged && oldValue != null)
                GUI.color = (getter() == null && oldValue() == null) || getter().Equals(oldValue()) ? Color.white : Color.cyan;

            if (getter() == null)
                setter(new AssetReferenceTexture2DHolder());

            if (getter().ScriptableObject == null)
                getter().Serialize();

            if (!string.IsNullOrEmpty(getter().AssetReference?.AssetGUID) && getter().AssetReference?.editorAsset == null)
                GUI.color = Color.red; //Busted reference
            
            getter().ScriptableObject.AssetReference = getter().AssetReference;
            getter().SerializedObject.Update();
            var property = getter().SerializedObject.FindProperty(nameof(AssetReferenceTexture2DHolder.AssetReferenceScriptableObject.AssetReference));
            EditorGUILayout.PropertyField(property, GUIContent.none, true, GUILayout.Width(view.columnWidth));

            if (getter().ScriptableObject.AssetReference == null || !getter().ScriptableObject.AssetReference.RuntimeKeyIsValid())
            {
                getter().AssetReference = null;
            }
            else
            {
                if (getter().AssetReference == null || !getter().AssetReference.AssetGUID.Equals(getter().ScriptableObject.AssetReference.AssetGUID))
                {
                    getter().AssetReference = new AssetReferenceTexture2D(getter().ScriptableObject.AssetReference.AssetGUID);
                }
                else
                {
                    getter().AssetReference.SubObjectName = getter().ScriptableObject.AssetReference.SubObjectName;
                }
            }
        }

        public static void DrawCell<T>(this TableView<T> view, [CanBeNull] Getter<AssetReferenceTexture3DHolder> getter, Setter<AssetReferenceTexture3DHolder> setter, bool changeColorIfChanged = false, Getter<AssetReferenceTexture3DHolder?> oldValue = null) where T : Entity
        {
            if (changeColorIfChanged && oldValue != null)
                GUI.color = (getter() == null && oldValue() == null) || getter().Equals(oldValue()) ? Color.white : Color.cyan;

            if (getter() == null)
                setter(new AssetReferenceTexture3DHolder());

            if (getter().ScriptableObject == null)
                getter().Serialize();

            if (!string.IsNullOrEmpty(getter().AssetReference?.AssetGUID) && getter().AssetReference?.editorAsset == null)
                GUI.color = Color.red; //Busted reference
            
            getter().ScriptableObject.AssetReference = getter().AssetReference;
            getter().SerializedObject.Update();
            var property = getter().SerializedObject.FindProperty(nameof(AssetReferenceTexture3DHolder.AssetReferenceScriptableObject.AssetReference));
            EditorGUILayout.PropertyField(property, GUIContent.none, true, GUILayout.Width(view.columnWidth));

            if (getter().ScriptableObject.AssetReference == null || !getter().ScriptableObject.AssetReference.RuntimeKeyIsValid())
            {
                getter().AssetReference = null;
            }
            else
            {
                if (getter().AssetReference == null || !getter().AssetReference.AssetGUID.Equals(getter().ScriptableObject.AssetReference.AssetGUID))
                {
                    getter().AssetReference = new AssetReferenceTexture3D(getter().ScriptableObject.AssetReference.AssetGUID);
                }
                else
                {
                    getter().AssetReference.SubObjectName = getter().ScriptableObject.AssetReference.SubObjectName;
                }
            }
        }
        
        public static void DrawCell<T>(this TableView<T> view, [CanBeNull] Getter<AssetReferenceSceneHolder> getter, Setter<AssetReferenceSceneHolder> setter, bool changeColorIfChanged = false, Getter<AssetReferenceSceneHolder?> oldValue = null) where T : Entity
        {
            if (changeColorIfChanged && oldValue != null)
                GUI.color = (getter() == null && oldValue() == null) || getter().Equals(oldValue()) ? Color.white : Color.cyan;

            if (getter() == null)
                setter(new AssetReferenceSceneHolder());

            if (getter().ScriptableObject == null)
                getter().Serialize();

            if (!string.IsNullOrEmpty(getter().AssetReference?.AssetGUID) && getter().AssetReference?.editorAsset == null)
                GUI.color = Color.red; //Busted reference
            
            getter().ScriptableObject.AssetReference = getter().AssetReference;
            getter().SerializedObject.Update();
            var property = getter().SerializedObject.FindProperty(nameof(AssetReferenceSceneHolder.AssetReferenceScriptableObject.AssetReference));
            EditorGUILayout.PropertyField(property, GUIContent.none, true, GUILayout.Width(view.columnWidth));

            if (getter().ScriptableObject.AssetReference == null || !getter().ScriptableObject.AssetReference.RuntimeKeyIsValid())
            {
                getter().AssetReference = null;
            }
            else
            {
                if (getter().AssetReference == null || !getter().AssetReference.AssetGUID.Equals(getter().ScriptableObject.AssetReference.AssetGUID))
                {
                    getter().AssetReference = new AssetReferenceScene(getter().ScriptableObject.AssetReference.AssetGUID);
                }
                else
                {
                    getter().AssetReference.SubObjectName = getter().ScriptableObject.AssetReference.SubObjectName;
                }
            }
        }
        
        public static void DrawCell<T>(this TableView<T> view, [CanBeNull] Getter<AssetReferenceAnimationClipHolder> getter, Setter<AssetReferenceAnimationClipHolder> setter, bool changeColorIfChanged = false, Getter<AssetReferenceAnimationClipHolder?> oldValue = null) where T : Entity
        {
            if (changeColorIfChanged && oldValue != null)
                GUI.color = (getter() == null && oldValue() == null) || getter().Equals(oldValue()) ? Color.white : Color.cyan;

            if (getter() == null)
                setter(new AssetReferenceAnimationClipHolder());

            if (getter().ScriptableObject == null)
                getter().Serialize();

            if (!string.IsNullOrEmpty(getter().AssetReference?.AssetGUID) && getter().AssetReference?.editorAsset == null)
                GUI.color = Color.red; //Busted reference
            
            getter().ScriptableObject.AssetReference = getter().AssetReference;
            getter().SerializedObject.Update();
            var property = getter().SerializedObject.FindProperty(nameof(AssetReferenceAnimationClipHolder.AssetReferenceScriptableObject.AssetReference));
            EditorGUILayout.PropertyField(property, GUIContent.none, true, GUILayout.Width(view.columnWidth));

            if (getter().ScriptableObject.AssetReference == null || !getter().ScriptableObject.AssetReference.RuntimeKeyIsValid())
            {
                getter().AssetReference = null;
            }
            else
            {
                if (getter().AssetReference == null || !getter().AssetReference.AssetGUID.Equals(getter().ScriptableObject.AssetReference.AssetGUID))
                {
                    getter().AssetReference = new AssetReferenceAnimationClip(getter().ScriptableObject.AssetReference.AssetGUID);
                }
                else
                {
                    getter().AssetReference.SubObjectName = getter().ScriptableObject.AssetReference.SubObjectName;
                }
            }
        }
        
        public static void DrawCell<T>(this TableView<T> view, [CanBeNull] Getter<AssetReferenceAudioMixerHolder> getter, Setter<AssetReferenceAudioMixerHolder> setter, bool changeColorIfChanged = false, Getter<AssetReferenceAudioMixerHolder?> oldValue = null) where T : Entity
        {
            if (changeColorIfChanged && oldValue != null)
                GUI.color = (getter() == null && oldValue() == null) || getter().Equals(oldValue()) ? Color.white : Color.cyan;

            if (getter() == null)
                setter(new AssetReferenceAudioMixerHolder());

            if (getter().ScriptableObject == null)
                getter().Serialize();

            if (!string.IsNullOrEmpty(getter().AssetReference?.AssetGUID) && getter().AssetReference?.editorAsset == null)
                GUI.color = Color.red; //Busted reference
            
            getter().ScriptableObject.AssetReference = getter().AssetReference;
            getter().SerializedObject.Update();
            var property = getter().SerializedObject.FindProperty(nameof(AssetReferenceAudioMixerHolder.AssetReferenceScriptableObject.AssetReference));
            EditorGUILayout.PropertyField(property, GUIContent.none, true, GUILayout.Width(view.columnWidth));

            if (getter().ScriptableObject.AssetReference == null || !getter().ScriptableObject.AssetReference.RuntimeKeyIsValid())
            {
                getter().AssetReference = null;
            }
            else
            {
                if (getter().AssetReference == null || !getter().AssetReference.AssetGUID.Equals(getter().ScriptableObject.AssetReference.AssetGUID))
                {
                    getter().AssetReference = new AssetReferenceAudioMixer(getter().ScriptableObject.AssetReference.AssetGUID);
                }
                else
                {
                    getter().AssetReference.SubObjectName = getter().ScriptableObject.AssetReference.SubObjectName;
                }
            }
        }
        
        public static void DrawCell<T>(this TableView<T> view, [CanBeNull] Getter<AssetReferenceComputeShaderHolder> getter, Setter<AssetReferenceComputeShaderHolder> setter, bool changeColorIfChanged = false, Getter<AssetReferenceComputeShaderHolder?> oldValue = null) where T : Entity
        {
            if (changeColorIfChanged && oldValue != null)
                GUI.color = (getter() == null && oldValue() == null) || getter().Equals(oldValue()) ? Color.white : Color.cyan;

            if (getter() == null)
                setter(new AssetReferenceComputeShaderHolder());

            if (getter().ScriptableObject == null)
                getter().Serialize();

            if (!string.IsNullOrEmpty(getter().AssetReference?.AssetGUID) && getter().AssetReference?.editorAsset == null)
                GUI.color = Color.red; //Busted reference
            
            getter().ScriptableObject.AssetReference = getter().AssetReference;
            getter().SerializedObject.Update();
            var property = getter().SerializedObject.FindProperty(nameof(AssetReferenceComputeShaderHolder.AssetReferenceScriptableObject.AssetReference));
            EditorGUILayout.PropertyField(property, GUIContent.none, true, GUILayout.Width(view.columnWidth));

            if (getter().ScriptableObject.AssetReference == null || !getter().ScriptableObject.AssetReference.RuntimeKeyIsValid())
            {
                getter().AssetReference = null;
            }
            else
            {
                if (getter().AssetReference == null || !getter().AssetReference.AssetGUID.Equals(getter().ScriptableObject.AssetReference.AssetGUID))
                {
                    getter().AssetReference = new AssetReferenceComputeShader(getter().ScriptableObject.AssetReference.AssetGUID);
                }
                else
                {
                    getter().AssetReference.SubObjectName = getter().ScriptableObject.AssetReference.SubObjectName;
                }
            }
        }
        
        public static void DrawCell<T>(this TableView<T> view, [CanBeNull] Getter<AssetReferenceFontHolder> getter, Setter<AssetReferenceFontHolder> setter, bool changeColorIfChanged = false, Getter<AssetReferenceFontHolder?> oldValue = null) where T : Entity
        {
            if (changeColorIfChanged && oldValue != null)
                GUI.color = (getter() == null && oldValue() == null) || getter().Equals(oldValue()) ? Color.white : Color.cyan;

            if (getter() == null)
                setter(new AssetReferenceFontHolder());

            if (getter().ScriptableObject == null)
                getter().Serialize();

            if (!string.IsNullOrEmpty(getter().AssetReference?.AssetGUID) && getter().AssetReference?.editorAsset == null)
                GUI.color = Color.red; //Busted reference
            
            getter().ScriptableObject.AssetReference = getter().AssetReference;
            getter().SerializedObject.Update();
            var property = getter().SerializedObject.FindProperty(nameof(AssetReferenceFontHolder.AssetReferenceScriptableObject.AssetReference));
            EditorGUILayout.PropertyField(property, GUIContent.none, true, GUILayout.Width(view.columnWidth));

            if (getter().ScriptableObject.AssetReference == null || !getter().ScriptableObject.AssetReference.RuntimeKeyIsValid())
            {
                getter().AssetReference = null;
            }
            else
            {
                if (getter().AssetReference == null || !getter().AssetReference.AssetGUID.Equals(getter().ScriptableObject.AssetReference.AssetGUID))
                {
                    getter().AssetReference = new AssetReferenceFont(getter().ScriptableObject.AssetReference.AssetGUID);
                }
                else
                {
                    getter().AssetReference.SubObjectName = getter().ScriptableObject.AssetReference.SubObjectName;
                }
            }
        }
        
        public static void DrawCell<T>(this TableView<T> view, [CanBeNull] Getter<AssetReferenceGUISkinHolder> getter, Setter<AssetReferenceGUISkinHolder> setter, bool changeColorIfChanged = false, Getter<AssetReferenceGUISkinHolder?> oldValue = null) where T : Entity
        {
            if (changeColorIfChanged && oldValue != null)
                GUI.color = (getter() == null && oldValue() == null) || getter().Equals(oldValue()) ? Color.white : Color.cyan;

            if (getter() == null)
                setter(new AssetReferenceGUISkinHolder());

            if (getter().ScriptableObject == null)
                getter().Serialize();

            if (!string.IsNullOrEmpty(getter().AssetReference?.AssetGUID) && getter().AssetReference?.editorAsset == null)
                GUI.color = Color.red; //Busted reference
            
            getter().ScriptableObject.AssetReference = getter().AssetReference;
            getter().SerializedObject.Update();
            var property = getter().SerializedObject.FindProperty(nameof(AssetReferenceGUISkinHolder.AssetReferenceScriptableObject.AssetReference));
            EditorGUILayout.PropertyField(property, GUIContent.none, true, GUILayout.Width(view.columnWidth));

            if (getter().ScriptableObject.AssetReference == null || !getter().ScriptableObject.AssetReference.RuntimeKeyIsValid())
            {
                getter().AssetReference = null;
            }
            else
            {
                if (getter().AssetReference == null || !getter().AssetReference.AssetGUID.Equals(getter().ScriptableObject.AssetReference.AssetGUID))
                {
                    getter().AssetReference = new AssetReferenceGUISkin(getter().ScriptableObject.AssetReference.AssetGUID);
                }
                else
                {
                    getter().AssetReference.SubObjectName = getter().ScriptableObject.AssetReference.SubObjectName;
                }
            }
        }
        
        public static void DrawCell<T>(this TableView<T> view, [CanBeNull] Getter<AssetReferenceMaterialHolder> getter, Setter<AssetReferenceMaterialHolder> setter, bool changeColorIfChanged = false, Getter<AssetReferenceMaterialHolder?> oldValue = null) where T : Entity
        {
            if (changeColorIfChanged && oldValue != null)
                GUI.color = (getter() == null && oldValue() == null) || getter().Equals(oldValue()) ? Color.white : Color.cyan;

            if (getter() == null)
                setter(new AssetReferenceMaterialHolder());

            if (getter().ScriptableObject == null)
                getter().Serialize();

            if (!string.IsNullOrEmpty(getter().AssetReference?.AssetGUID) && getter().AssetReference?.editorAsset == null)
                GUI.color = Color.red; //Busted reference
            
            getter().ScriptableObject.AssetReference = getter().AssetReference;
            getter().SerializedObject.Update();
            var property = getter().SerializedObject.FindProperty(nameof(AssetReferenceMaterialHolder.AssetReferenceScriptableObject.AssetReference));
            EditorGUILayout.PropertyField(property, GUIContent.none, true, GUILayout.Width(view.columnWidth));

            if (getter().ScriptableObject.AssetReference == null || !getter().ScriptableObject.AssetReference.RuntimeKeyIsValid())
            {
                getter().AssetReference = null;
            }
            else
            {
                if (getter().AssetReference == null || !getter().AssetReference.AssetGUID.Equals(getter().ScriptableObject.AssetReference.AssetGUID))
                {
                    getter().AssetReference = new AssetReferenceMaterial(getter().ScriptableObject.AssetReference.AssetGUID);
                }
                else
                {
                    getter().AssetReference.SubObjectName = getter().ScriptableObject.AssetReference.SubObjectName;
                }
            }
        }
        
        public static void DrawCell<T>(this TableView<T> view, [CanBeNull] Getter<AssetReferenceMeshHolder> getter, Setter<AssetReferenceMeshHolder> setter, bool changeColorIfChanged = false, Getter<AssetReferenceMeshHolder?> oldValue = null) where T : Entity
        {
            if (changeColorIfChanged && oldValue != null)
                GUI.color = (getter() == null && oldValue() == null) || getter().Equals(oldValue()) ? Color.white : Color.cyan;

            if (getter() == null)
                setter(new AssetReferenceMeshHolder());

            if (getter().ScriptableObject == null)
                getter().Serialize();

            if (!string.IsNullOrEmpty(getter().AssetReference?.AssetGUID) && getter().AssetReference?.editorAsset == null)
                GUI.color = Color.red; //Busted reference
            
            getter().ScriptableObject.AssetReference = getter().AssetReference;
            getter().SerializedObject.Update();
            var property = getter().SerializedObject.FindProperty(nameof(AssetReferenceMeshHolder.AssetReferenceScriptableObject.AssetReference));
            EditorGUILayout.PropertyField(property, GUIContent.none, true, GUILayout.Width(view.columnWidth));

            if (getter().ScriptableObject.AssetReference == null || !getter().ScriptableObject.AssetReference.RuntimeKeyIsValid())
            {
                getter().AssetReference = null;
            }
            else
            {
                if (getter().AssetReference == null || !getter().AssetReference.AssetGUID.Equals(getter().ScriptableObject.AssetReference.AssetGUID))
                {
                    getter().AssetReference = new AssetReferenceMesh(getter().ScriptableObject.AssetReference.AssetGUID);
                }
                else
                {
                    getter().AssetReference.SubObjectName = getter().ScriptableObject.AssetReference.SubObjectName;
                }
            }
        }
        
        public static void DrawCell<T>(this TableView<T> view, [CanBeNull] Getter<AssetReferencePhysicMaterialHolder> getter, Setter<AssetReferencePhysicMaterialHolder> setter, bool changeColorIfChanged = false, Getter<AssetReferencePhysicMaterialHolder?> oldValue = null) where T : Entity
        {
            if (changeColorIfChanged && oldValue != null)
                GUI.color = (getter() == null && oldValue() == null) || getter().Equals(oldValue()) ? Color.white : Color.cyan;

            if (getter() == null)
                setter(new AssetReferencePhysicMaterialHolder());

            if (getter().ScriptableObject == null)
                getter().Serialize();

            if (!string.IsNullOrEmpty(getter().AssetReference?.AssetGUID) && getter().AssetReference?.editorAsset == null)
                GUI.color = Color.red; //Busted reference
            
            getter().ScriptableObject.AssetReference = getter().AssetReference;
            getter().SerializedObject.Update();
            var property = getter().SerializedObject.FindProperty(nameof(AssetReferencePhysicMaterialHolder.AssetReferenceScriptableObject.AssetReference));
            EditorGUILayout.PropertyField(property, GUIContent.none, true, GUILayout.Width(view.columnWidth));

            if (getter().ScriptableObject.AssetReference == null || !getter().ScriptableObject.AssetReference.RuntimeKeyIsValid())
            {
                getter().AssetReference = null;
            }
            else
            {
                if (getter().AssetReference == null || !getter().AssetReference.AssetGUID.Equals(getter().ScriptableObject.AssetReference.AssetGUID))
                {
                    getter().AssetReference = new AssetReferencePhysicMaterial(getter().ScriptableObject.AssetReference.AssetGUID);
                }
                else
                {
                    getter().AssetReference.SubObjectName = getter().ScriptableObject.AssetReference.SubObjectName;
                }
            }
        }

        public static void DrawCell<T>(this TableView<T> view, [CanBeNull] Getter<AssetReferenceShaderHolder> getter, Setter<AssetReferenceShaderHolder> setter, bool changeColorIfChanged = false, Getter<AssetReferenceShaderHolder?> oldValue = null) where T : Entity
        {
            if (changeColorIfChanged && oldValue != null)
                GUI.color = (getter() == null && oldValue() == null) || getter().Equals(oldValue()) ? Color.white : Color.cyan;

            if (getter() == null)
                setter(new AssetReferenceShaderHolder());

            if (getter().ScriptableObject == null)
                getter().Serialize();

            if (!string.IsNullOrEmpty(getter().AssetReference?.AssetGUID) && getter().AssetReference?.editorAsset == null)
                GUI.color = Color.red; //Busted reference
            
            getter().ScriptableObject.AssetReference = getter().AssetReference;
            getter().SerializedObject.Update();
            var property = getter().SerializedObject.FindProperty(nameof(AssetReferenceShaderHolder.AssetReferenceScriptableObject.AssetReference));
            EditorGUILayout.PropertyField(property, GUIContent.none, true, GUILayout.Width(view.columnWidth));

            if (getter().ScriptableObject.AssetReference == null || !getter().ScriptableObject.AssetReference.RuntimeKeyIsValid())
            {
                getter().AssetReference = null;
            }
            else
            {
                if (getter().AssetReference == null || !getter().AssetReference.AssetGUID.Equals(getter().ScriptableObject.AssetReference.AssetGUID))
                {
                    getter().AssetReference = new AssetReferenceShader(getter().ScriptableObject.AssetReference.AssetGUID);
                }
                else
                {
                    getter().AssetReference.SubObjectName = getter().ScriptableObject.AssetReference.SubObjectName;
                }
            }
        }
        
        public static void DrawCell<T>(this TableView<T> view, [CanBeNull] Getter<AssetReferenceVideoClipHolder> getter, Setter<AssetReferenceVideoClipHolder> setter, bool changeColorIfChanged = false, Getter<AssetReferenceVideoClipHolder?> oldValue = null) where T : Entity
        {
            if (changeColorIfChanged && oldValue != null)
                GUI.color = (getter() == null && oldValue() == null) || getter().Equals(oldValue()) ? Color.white : Color.cyan;

            if (getter() == null)
                setter(new AssetReferenceVideoClipHolder());

            if (getter().ScriptableObject == null)
                getter().Serialize();

            if (!string.IsNullOrEmpty(getter().AssetReference?.AssetGUID) && getter().AssetReference?.editorAsset == null)
                GUI.color = Color.red; //Busted reference
            
            getter().ScriptableObject.AssetReference = getter().AssetReference;
            getter().SerializedObject.Update();
            var property = getter().SerializedObject.FindProperty(nameof(AssetReferenceVideoClipHolder.AssetReferenceScriptableObject.AssetReference));
            EditorGUILayout.PropertyField(property, GUIContent.none, true, GUILayout.Width(view.columnWidth));

            if (getter().ScriptableObject.AssetReference == null || !getter().ScriptableObject.AssetReference.RuntimeKeyIsValid())
            {
                getter().AssetReference = null;
            }
            else
            {
                if (getter().AssetReference == null || !getter().AssetReference.AssetGUID.Equals(getter().ScriptableObject.AssetReference.AssetGUID))
                {
                    getter().AssetReference = new AssetReferenceVideoClip(getter().ScriptableObject.AssetReference.AssetGUID);
                }
                else
                {
                    getter().AssetReference.SubObjectName = getter().ScriptableObject.AssetReference.SubObjectName;
                }
            }
        }
        
        public static void DrawCell<T>(this TableView<T> view, [CanBeNull] Getter<AssetReferenceModelHolder> getter, Setter<AssetReferenceModelHolder> setter, bool changeColorIfChanged = false, Getter<AssetReferenceModelHolder?> oldValue = null) where T : Entity
        {
            if (changeColorIfChanged && oldValue != null)
                GUI.color = (getter() == null && oldValue() == null) || getter().Equals(oldValue()) ? Color.white : Color.cyan;

            if (getter() == null)
                setter(new AssetReferenceModelHolder());

            if (getter().ScriptableObject == null)
                getter().Serialize();

            if (!string.IsNullOrEmpty(getter().AssetReference?.AssetGUID) && getter().AssetReference?.editorAsset == null)
                GUI.color = Color.red; //Busted reference
            
            getter().ScriptableObject.AssetReference = getter().AssetReference;
            getter().SerializedObject.Update();
            var property = getter().SerializedObject.FindProperty(nameof(AssetReferenceModelHolder.AssetReferenceScriptableObject.AssetReference));
            EditorGUILayout.PropertyField(property, GUIContent.none, true, GUILayout.Width(view.columnWidth));

            if (getter().ScriptableObject.AssetReference == null || !getter().ScriptableObject.AssetReference.RuntimeKeyIsValid())
            {
                getter().AssetReference = null;
            }
            else
            {
                if (getter().AssetReference == null || !getter().AssetReference.AssetGUID.Equals(getter().ScriptableObject.AssetReference.AssetGUID))
                {
                    getter().AssetReference = new AssetReferenceModel(getter().ScriptableObject.AssetReference.AssetGUID);
                }
                else
                {
                    getter().AssetReference.SubObjectName = getter().ScriptableObject.AssetReference.SubObjectName;
                }
            }
        }
        
        public static void DrawCell<T>(this TableView<T> view, [CanBeNull] Getter<AssetReferenceTextAssetHolder> getter, Setter<AssetReferenceTextAssetHolder> setter, bool changeColorIfChanged = false, Getter<AssetReferenceTextAssetHolder?> oldValue = null) where T : Entity
        {
            if (changeColorIfChanged && oldValue != null)
                GUI.color = (getter() == null && oldValue() == null) || getter().Equals(oldValue()) ? Color.white : Color.cyan;

            if (getter() == null)
                setter(new AssetReferenceTextAssetHolder());

            if (getter().ScriptableObject == null)
                getter().Serialize();

            if (!string.IsNullOrEmpty(getter().AssetReference?.AssetGUID) && getter().AssetReference?.editorAsset == null)
                GUI.color = Color.red; //Busted reference
            
            getter().ScriptableObject.AssetReference = getter().AssetReference;
            getter().SerializedObject.Update();
            var property = getter().SerializedObject.FindProperty(nameof(AssetReferenceTextAssetHolder.AssetReferenceScriptableObject.AssetReference));
            EditorGUILayout.PropertyField(property, GUIContent.none, true, GUILayout.Width(view.columnWidth));

            if (getter().ScriptableObject.AssetReference == null || !getter().ScriptableObject.AssetReference.RuntimeKeyIsValid())
            {
                getter().AssetReference = null;
            }
            else
            {
                if (getter().AssetReference == null || !getter().AssetReference.AssetGUID.Equals(getter().ScriptableObject.AssetReference.AssetGUID))
                {
                    getter().AssetReference = new AssetReferenceTextAsset(getter().ScriptableObject.AssetReference.AssetGUID);
                }
                else
                {
                    getter().AssetReference.SubObjectName = getter().ScriptableObject.AssetReference.SubObjectName;
                }
            }
        }
    }
}