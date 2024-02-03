using System;
using System.Linq;
using Logic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(UniqieId))]
    public class UniqueIdEdtior : UnityEditor.Editor
    {
        private void OnEnable()
        {
            var uniqueId = (UniqieId) target;
            
            if(IsPrefab(uniqueId))
                return;

            if (string.IsNullOrEmpty(uniqueId.Id))
                Generate(uniqueId);
            else
            {
                UniqieId[] uniqieIds = FindObjectsOfType<UniqieId>();
                
                if(uniqieIds.Any(other => other != uniqueId && other.Id == uniqueId.Id))
                    Generate(uniqueId);
            }
        }

        private bool IsPrefab(UniqieId uniqueId) => uniqueId.gameObject.scene.rootCount == 0;

        private void Generate(UniqieId uniqueId)
        {
            uniqueId.Id = $"{uniqueId.gameObject.scene.name}{Guid.NewGuid()}";

            if (!Application.isPlaying)
            {
                EditorUtility.SetDirty(uniqueId);
                EditorSceneManager.MarkSceneDirty(uniqueId.gameObject.scene);
            }
        }
    }
}