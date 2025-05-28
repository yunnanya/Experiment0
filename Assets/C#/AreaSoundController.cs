using UnityEngine;

public class AreaSoundController : MonoBehaviour
{
    [Header("音频配置")]
    public AudioSource targetAudioSource; // 外部音频组件

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && targetAudioSource != null)
        {
            targetAudioSource.Play(); // 播放外部音频
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && targetAudioSource != null)
        {
            targetAudioSource.Stop(); // 停止外部音频
        }
    }
}