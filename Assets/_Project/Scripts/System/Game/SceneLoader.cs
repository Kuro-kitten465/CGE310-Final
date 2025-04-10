using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace Kuro.GameSystem
{
    public class SceneLoader : MonoBehaviour
    {
        [SerializeField] private float _delayBeforeLoad = 0f;
        [SerializeField] private string _sceneName;

        public void LoadScene()
        {
            if (SceneManager.GetSceneByName(_sceneName) == null) return;
            
            StartCoroutine(LoadSceneAsync(_sceneName));
        }

        private IEnumerator LoadSceneAsync(string sceneName)
        {
            float elapsedTime = 0f;

            while(elapsedTime < _delayBeforeLoad)
            {
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            yield return SceneManager.LoadSceneAsync(sceneName);
        }
    }
}
