﻿#if UNITY_EDITOR
using UnityEngine;

namespace JSLCore.AssetBundle
{
    public class AssetBundleLoadAssetRequestSimulate<T> : AssetBundleLoadAssetRequest<T> where T : Object
    {
        private string m_assetBundleName;
        private string m_assetName;

        public AssetBundleLoadAssetRequestSimulate(string assetBundleName, string assetName)
        {
            m_assetBundleName = assetBundleName;
            m_assetName = assetName;
        }

        public override T GetAsset()
        {
            string[] assetPaths = UnityEditor.AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName(m_assetBundleName, m_assetName);

            if (assetPaths == null || assetPaths.Length == 0)
            {
                return null;
            }

            T asset = null;
            foreach (string path in assetPaths)
            {
                asset = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(path);
                if (asset != null)
                {
                    break;
                }
            }

            return asset;
        }

        public override bool Update()
        {
            return false;
        }

        public override bool IsDone()
        {
            return true;
        }

        public override float GetProgress()
        {
            return 1f;
        }
    }
}
#endif