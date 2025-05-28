using UnityEngine;

public class CorrectPathTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // ȷ�������"Player"��ǩ
        {
            ScoreManager.Instance.AddScore(10);
            // ��ѡ�����ô�������ֹ�ظ�����
            GetComponent<BoxCollider>().enabled = false;
        }
    }
}