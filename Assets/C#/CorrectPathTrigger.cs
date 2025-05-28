using UnityEngine;

public class CorrectPathTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // 确保玩家有"Player"标签
        {
            ScoreManager.Instance.AddScore(10);
            // 可选：禁用触发器防止重复触发
            GetComponent<BoxCollider>().enabled = false;
        }
    }
}