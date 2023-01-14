using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class CardBox : MonoBehaviour
{
    [SerializeField] GameObject loseMessage;
    CardSpot[] cardSpots;
    bool shouldMoveCardsToLeft;   // Should the remaining cards in the box move left?
    float timeElapsed = 0f;
    bool gameContinue = true;

    void Start()
    {
        cardSpots = FindObjectsOfType<CardSpot>();
        shouldMoveCardsToLeft = false;
    }

    void Update()
    {
        if (shouldMoveCardsToLeft) {
            StartCoroutine(MoveCardsToLeft());
        }

        if (CheckOccupancyStatus())
        {
            timeElapsed += Time.deltaTime;
        }
        else { timeElapsed = 0f; }

        if (timeElapsed > 0.5f) 
        { 
            gameContinue = false; 
            loseMessage.SetActive(true);
        }
    }

    public bool ShouldMoveCardsToLeft
    {
        get { return shouldMoveCardsToLeft; }
        set { shouldMoveCardsToLeft = value; }
    }

    private IEnumerator MoveCardsToLeft() 
    {
        shouldMoveCardsToLeft = false;
        yield return new WaitForSeconds(0.5f);    // Should be kept for particle effect?

        List<int> occupiedSpotNumbers = new List<int>();
        List<int> emptySpotNumbers = new List<int>();
        foreach (CardSpot spot in cardSpots)
        {
            if (spot.SpotOccupied)
            {
                occupiedSpotNumbers.Add(spot.SpotNumber);
            }
            else 
            {
                emptySpotNumbers.Add(spot.SpotNumber);
            }

        }
        
        occupiedSpotNumbers.Sort();
        emptySpotNumbers.Sort();
        if (occupiedSpotNumbers.Count > 0) 
        {
            int maxOccupiedSpotNumber = occupiedSpotNumbers.Max();
            if (occupiedSpotNumbers.Count < (maxOccupiedSpotNumber + 1))
            {
                List<int> refIntList = Enumerable.Range(0, maxOccupiedSpotNumber + 1).ToList();
                int startSpotNumber = refIntList.Where(x => !occupiedSpotNumbers.Contains(x)).ToList().Max() + 1;
                List<int> spotToMoveLeft = Enumerable.Range(startSpotNumber, (occupiedSpotNumbers.Max() - startSpotNumber + 1)).ToList();

                foreach (int spotNumber in spotToMoveLeft)
                {
                    foreach (CardSpot spot in cardSpots)
                    {
                        if (spot.SpotNumber == spotNumber)
                        {
                            spot.CardInSpot.MoveToSpot(spotNumber - 3);
                            spot.CardTypeInSpot = CardType.Null;
                            spot.CardInSpot = null;
                            spot.SpotOccupied = false;
                        }
                    }
                }
            }
        }
    }

    private bool CheckOccupancyStatus()
    {
        int occupancyCount = 0;
        foreach (CardSpot spot in cardSpots)
        {
            if (spot.SpotOccupied) { occupancyCount++; }
        }

        if (occupancyCount == 7) { return true; }
        return false;
    }

    public bool GameContinue 
    {
        get { return gameContinue; }
    }
}
