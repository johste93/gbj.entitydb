using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;using UnityEditor.VersionControl;
using UnityEngine.AddressableAssets;

namespace GBJ.EntityDB
{
	public abstract class AssetReferenceHolder : IComparable
	{
		public abstract AssetReference GetAssetReference();

		public int CompareTo(object obj)
		{
			if (obj == null)
			{
				if (GetAssetReference() != null && !string.IsNullOrEmpty(GetAssetReference().AssetGUID))
					return 1;

				return -1;
			}

			AssetReferenceHolder other = (AssetReferenceHolder) obj;

			if (GetAssetReference() == null)
			{
				if (other.GetAssetReference() == null)
					return 0;

				return -1;
			}

			if (other.GetAssetReference() == null)
				return 1;

			//None of them are null
			if (string.IsNullOrEmpty(GetAssetReference().AssetGUID))
			{
				if (string.IsNullOrEmpty(other.GetAssetReference().AssetGUID))
					return 0;

				return -1;
			}

			if (string.IsNullOrEmpty(other.GetAssetReference().AssetGUID))
				return 1;

			return 0;
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				if (GetAssetReference() == null)
					return true;

				if (string.IsNullOrEmpty(GetAssetReference().AssetGUID))
					return true;
			}

			if (obj == null || GetType() != obj.GetType())
				return false;

			AssetReferenceHolder other = (AssetReferenceHolder) obj;

			if (GetAssetReference() == null && other.GetAssetReference() == null)
				return true;

			if (GetAssetReference() == null || other.GetAssetReference() == null)
				return false;

			if (string.IsNullOrEmpty(GetAssetReference().AssetGUID) && GetAssetReference().AssetGUID == null)
				return true;

			return string.Equals(GetAssetReference().AssetGUID, other.GetAssetReference().AssetGUID) &&
			       string.Equals(GetAssetReference().SubObjectName, other.GetAssetReference().SubObjectName);
		}

		public override int GetHashCode()
		{
			return (GetAssetReference() != null ? GetAssetReference().GetHashCode() : 0);
		}

#if UNITY_EDITOR
		//This field is used to render the AssetReference Drawer in Editor Window.
		[JsonIgnore] public UnityEditor.SerializedObject SerializedObject;

		public abstract void Serialize();

		public abstract void NewAssetReference(string guid);
#endif
	}
}