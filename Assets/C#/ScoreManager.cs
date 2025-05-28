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

        //添加判空保护
        audioSource = GetComponent<AudioSource>();
        //添加判空保护
        bonusText.gameObject.SetActive(false);
    }

    public void AddScore(int points)
    {
        totalScore += points;
        totalScoreText.text = $"总分: {totalScore}";

        bonusText.text = $"+{points}分";
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