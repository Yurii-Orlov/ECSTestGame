using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SpritePacker
{
    public static class SceneExtensions
    {
        /// <summary>
        /// Call this for get target component from all objects below root in hierarchy. Disabled objects include
        /// </summary>
        /// <typeparam name="T">Target component</typeparam>
        /// <param name="root">Root object</param>
        /// <param name="targetComponents">List of components</param>
        public static void GetRootComponents<T>(this Transform root, List<T> targetComponents, string ignoredTag = "")
        {
            if (root.transform.tag != ignoredTag)
            {

                var childsCount = root.childCount;
                var targetComponent = root.GetComponent<T>();
                if (targetComponent != null)
                    targetComponents.Add(targetComponent);
                if (childsCount > 0)
                    for (int i = 0; i < childsCount; i++)
                        root.GetChild(i).GetRootComponents(targetComponents, ignoredTag);
            }
        }

        /// <summary>
        /// Call this for get target component from all objects in target scene. Disabled objects include
        /// </summary>
        /// <typeparam name="T">Target component</typeparam>
        /// <param name="scene">Target scene</param>
        /// <param name="targetComponents">List of components</param>
        public static void GetSceneComponents<T>(this Scene scene, List<T> targetComponents, string ignoredTag = "")
        {
            var rootObjects = scene.GetRootGameObjects();
            foreach (var root in rootObjects)
                root.transform.GetRootComponents(targetComponents, ignoredTag);
        }
    }

}
