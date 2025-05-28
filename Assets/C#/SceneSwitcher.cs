using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] private string targetScene = "MiGong1"; // 目标场景名称
    [SerializeField] private string playerTag = "Player";     // 玩家标签

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            SceneManager.LoadScene(targetScene);
            // 如需异步加载可替换为：
            // StartCoroutine(LoadSceneAsync());
        }
    }

    // 可选异步加载方法
    /*
    private IEnumerator LoadSceneAsync()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(targetScene);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
    */
}