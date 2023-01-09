using System.Collections;
using System.Collections.Generic;
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

    // Update is called once per frame
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
        yield return new WaitForSeconds(0.6f);
        foreach (CardSpot spot in cardSpots)
        {
            if (spot.SpotNumber > 2 & spot.SpotOccupied) {
                spot.CardInSpot.MoveToSpot(spot.SpotNumber - 3);
                spot.CardTypeInSpot = CardType.Null;
                spot.CardInSpot = null;
                spot.SpotOccupied = false;
            }
        }
    }
}
