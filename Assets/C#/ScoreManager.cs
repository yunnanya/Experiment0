using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    [SerializeField] Text totalScoreText;
    [SerializeField] Text bonusText;
    [SerializeField] AudioClip bonusSound;

    private int totalScore = 0;
    private AudioSource audioSource;

    void Awake()
    {
        Instance = this;
        audioSource = GetComponent<AudioSource>();
        bonusText.gameObject.SetActive(false);
    }

    public void AddScore(int points)
    {
        totalScore += points;
        totalScoreText.text = $"×Ü·Ö: {totalScore}";

        bonusText.text = $"+{points}·Ö";
        StartCoroutine(ShowBonusText());

        audioSource.PlayOneShot(bonusSound);
    }

    IEnumerator ShowBonusText()
    {
        bonusText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        bonusText.gameObject.SetActive(false);
    }
}