using UnityEngine;

public class AreaSoundController : MonoBehaviour
{
    [Header("��Ƶ����")]
    public AudioSource targetAudioSource; // �ⲿ��Ƶ���

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && targetAudioSource != null)
        {
            targetAudioSource.Play(); // �����ⲿ��Ƶ
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && targetAudioSource != null)
        {
            targetAudioSource.Stop(); // ֹͣ�ⲿ��Ƶ
        }
    }
}