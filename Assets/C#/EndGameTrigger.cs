using UnityEngine;

public class EndGameTrigger : MonoBehaviour
{
    [SerializeField] private string playerTag = "Player"; // 玩家标签

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
        UnityEditor.EditorApplication.isPlaying = false; // 编辑器模式下停止运行
#else
            Application.Quit(); // 构建后退出游戏
#endif
    }
}