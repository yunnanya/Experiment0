using UnityEngine;

public class EndGameTrigger : MonoBehaviour
{
    [SerializeField] private string playerTag = "Player"; // ��ұ�ǩ

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            EndGame();
        }
    }

    private void EndGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // �༭��ģʽ��ֹͣ����
#else
            Application.Quit(); // �������˳���Ϸ
#endif
    }
}