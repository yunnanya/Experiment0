using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] private string targetScene = "MiGong1"; // Ŀ�곡������
    [SerializeField] private string playerTag = "Player";     // ��ұ�ǩ

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            SceneManager.LoadScene(targetScene);
            // �����첽���ؿ��滻Ϊ��
            // StartCoroutine(LoadSceneAsync());
        }
    }

    // ��ѡ�첽���ط���
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