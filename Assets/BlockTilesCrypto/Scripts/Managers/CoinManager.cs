using System;
using System.Collections.Generic;
using DG.Tweening;
using PlayFab;
using PlayFab.ClientModels;
using PlayFabPersonal.Economy;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    // [SerializeField] private TMP_Text gamesocCoinText;
    [SerializeField] private GameObject animatedCoinPrefab;
    [SerializeField] private Transform animatedCoinStartPosition;
    [SerializeField] private Transform target;
    [SerializeField] private Transform coinSpawnerContainer;
    [SerializeField] private int maxCoins;

    [SerializeField][Range(0.5f, 0.9f)] float minAnimDuration;
    [SerializeField][Range(0.9f, 2f)] float maxAnimDuration;
    [SerializeField] private Ease easeType;
    [SerializeField] private float spread;

    readonly Queue<GameObject> coinsQueue = new();
    [SerializeField] private Vector3 targetPosition;
    public int earnedCoinsInTheRound;

    private int previousScore = 0;

    public static CoinManager Instance { get; private set; }

    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        
        Canvas canvas = target.GetComponentInParent<Canvas>();
        if (canvas != null)
        {
            RectTransform targetRectTransform = target as RectTransform;
            Vector2 screenPosition = RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, targetRectTransform.position);
            // Debug.Log($"Target Screen Position: {screenPosition}");

            RectTransformUtility.ScreenPointToWorldPointInRectangle(
                targetRectTransform,
                screenPosition,
                canvas.worldCamera,
                out targetPosition
            );

            // Debug.Log($"Target World Position: {targetPosition}");
        }
        else
        {
            Debug.LogError("Target's parent does not have a Canvas component.");
            // Provide a default position or handle the error accordingly
            targetPosition = Vector3.zero;
        }

        PrepareCoins();
    }


    private void Start()
    {
        InGameScript.OnSuccessLineBreak += PlayCoinAnimation_OnSuccessLineBreak;
        earnedCoinsInTheRound = 0;
    }

    private void PlayCoinAnimation_OnSuccessLineBreak(object sender, InGameScript.SetCoinOnLineBreakEventArgs e)
    {
        int newScore = e.scoreValue;
        int scoreDifference = Mathf.Abs(previousScore - newScore);

        //AddCurrency(scoreDifference);
       // earnedCoinsInTheRound += scoreDifference;
        //Debug.Log("earned points "+earnedCoinsInTheRound);

        previousScore = newScore;
        // VirtualCurrency.Instance.AddGameSocCurrency(inGameScript.GetCurrentScore());
        AddCoins(UnityEngine.Random.Range(5, 10));
    }

    // public void AddCurrency(int scoreDifference)
    // {
    //     if (scoreDifference > 0)
    //     {
    //         VirtualCurrency.Instance.AddGameSocCurrency(scoreDifference);
    //     }
    // }

    // private void OnSuccessGetUserInventory(GetUserInventoryResult result)
    // {
    //     float gameSocCoins = result.VirtualCurrency["CN"];
    //     float coinToAdd = MathF.Abs(score - gameSocCoins);
    //     if (coinToAdd > 0)
    //         VirtualCurrency.Instance.AddGameSocCurrency((int)coinToAdd);
    // }

    // private void OnErrorGetUserInventory(PlayFabError error)
    // {
    // }

    private void PrepareCoins()
    {
        for (int i = 0; i < maxCoins; i++)
        {
            GameObject coin;
            coin = Instantiate(animatedCoinPrefab, coinSpawnerContainer);
            coin.SetActive(false);
            coinsQueue.Enqueue(coin);
        }
    }

    public void AddCoins(int amount)
    {
        Animate(amount);
    }

    private void Animate(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            if (coinsQueue.Count > 0)
            {
                GameObject coin = coinsQueue.Dequeue();
                coin.SetActive(true);
                coin.transform.position = animatedCoinStartPosition.position + new Vector3(UnityEngine.Random.Range(-spread, spread), 0f, 0f);

                float duration = UnityEngine.Random.Range(minAnimDuration, maxAnimDuration);
                coin.transform.DOMove(targetPosition, duration)
                .SetEase(easeType)
                .OnComplete(() =>
                {
                    coin.SetActive(false);
                    coinsQueue.Enqueue(coin);
                });
            }
        }
    }

    public int GetPreviousScore()
    {
        return previousScore;
    }

    public void SetPreviousScore(int previousScore)
    {
        this.previousScore = previousScore;
    }

    private void OnDestroy()
    {
       // Debug.Log("CoinManager OnDestroy");
        InGameScript.OnSuccessLineBreak -= PlayCoinAnimation_OnSuccessLineBreak;
    }

}
