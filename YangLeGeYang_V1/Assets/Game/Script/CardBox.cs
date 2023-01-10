using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardBox : MonoBehaviour
{
    CardSpot[] cardSpots;
    bool shouldMoveCardsToLeft;   // Should the remaining cards in the box move left?

    // Start is called before the first frame update
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
        // foreach (CardSpot spot in cardSpots)
        // {
        //     if (spot.SpotNumber > 2 & spot.SpotOccupied) 
        //     {
        //         spot.CardInSpot.MoveToSpot(spot.SpotNumber - 3);
        //         spot.CardTypeInSpot = CardType.Null;
        //         spot.CardInSpot = null;
        //         spot.SpotOccupied = false;
        //     }
        // }

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
            int minOccupiedSpotNumber = occupiedSpotNumbers.Min();
            int minEmptySpotNumber = emptySpotNumbers.Min();
            int maxOccupiedSpotNumber = occupiedSpotNumbers.Max();
            // print("max empty: " + minEmptySpotNumber);
            // print("max occupied: " + minOccupiedSpotNumber);
            if (minOccupiedSpotNumber < minEmptySpotNumber)
            {
                print("Move.");
                List<int> refIntList = Enumerable.Range(0, maxOccupiedSpotNumber + 1).ToList();
                print("Start point to move left: " + (refIntList.Where(x => !occupiedSpotNumbers.Contains(x)).ToList().Max() + 1));
                // List<int> spotNumbersMovingLeft = Enumerable.Range(minEmptySpotNumber + 1, minOccupiedSpotNumber - minEmptySpotNumber).ToList();
                // print(spotNumbersMovingLeft.Count);
            }
            else { print("Stay."); }
        }
    }
}
