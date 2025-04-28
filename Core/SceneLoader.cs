using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace QuickJam.Core
{
    public class SceneLoader : MonoSingleton<SceneLoader>
    {
        public void LoadSceneAsync(string sceneName)
        {
            StartCoroutine(LoadSceneCoroutine(sceneName));
        }

        private IEnumerator LoadSceneCoroutine(string sceneName)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

            if (asyncLoad == null)
            {
                Debug.LogError($"[SceneLoader] Failed to load scene: {sceneName}");
                yield break;
            }

            asyncLoad.allowSceneActivation = false;

            while (!asyncLoad.isDone)
            {
                // You could add a loading progress bar here using asyncLoad.progress

                if (asyncLoad.progress >= 0.9f)
                {
                    // Scene is ready to be activated
                    asyncLoad.allowSceneActivation = true;
                }

                yield return null;
            }
        }
    }
}