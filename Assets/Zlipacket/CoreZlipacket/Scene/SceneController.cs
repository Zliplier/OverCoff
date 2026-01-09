using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zlipacket.CoreZlipacket.Scene.Transition;
using Zlipacket.CoreZlipacket.Tools;

namespace Zlipacket.CoreZlipacket.Scene
{
    public class SceneController : Singleton<SceneController>
    {
        [SerializeField] private string loadingSceneName;
        [SerializeField] private Transform sceneCanvas;
        public SO_SceneTransition transition;
        public float fakeLoadingTime = 0.5f;
        
        public void LoadScene(string sceneName = "")
        {
            if (string.IsNullOrEmpty(sceneName))
            {
                SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
            }
            else
            {
                StartCoroutine(TransitionToScene(sceneName));
            }
            
            Debug.Log("Scene " + sceneName + " loaded");
        }

        public void LoadSceneAsync(string sceneName)
        {
            SceneManager.LoadSceneAsync(sceneName);
            Debug.Log("Scene" + sceneName + " loaded async");
        }

        public void UnloadSceneAsync(string sceneName)
        {
            SceneManager.UnloadSceneAsync(sceneName);
            Debug.Log("Scene" + sceneName + " unloaded async");
        }
        
        private IEnumerator TransitionToScene(string sceneName)
        {
            SceneTransition sceneTransition = transition.InitializeTransition(sceneCanvas);
            
            yield return sceneTransition.StartTransition();
            
            //Loading Screen
            SceneManager.LoadScene(loadingSceneName);
            yield return new WaitForSeconds(fakeLoadingTime);
            
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);

            while (!asyncOperation.isDone)
                yield return null;
            
            yield return sceneTransition.EndTransition();
        }
    }
}