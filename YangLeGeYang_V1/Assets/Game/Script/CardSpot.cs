using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSpot : MonoBehaviour
{
    [SerializeField] int spotNumber;
    [SerializeField] CardType cardType;
    [SerializeField] Card cardInSpot;
    bool isOccupied = false;

    public static CardSpot MatchTargetSpot(int targetSpotNumber)
    {
        CardSpot[] allSpots = FindObjectsOfType<CardSpot>();
        foreach (CardSpot spot in allSpots) 
        {
            if (targetSpotNumber == spot.spotNumber) 
            {
                return spot;
            }
        }
        return null;
    }

    public int SpotNumber
    {
        get { return spotNumber; }
    }

    public bool SpotOccupied
    {
        get { return isOccupied; }
        set { isOccupied = value; }
    }

    public CardType CardTypeInSpot   // I think this is unnecessary, and should be deleted later.
    {
        get { return cardType; }
        set { cardType = value; }
    }

    public Card CardInSpot
    {
        get { return cardInSpot; }
        set { cardInSpot = value; }
    }

    public void DestroyCardInSpot() {
        Destroy(cardInSpot.gameObject);
        cardType = CardType.Null;
        cardInSpot = null;
    }
}
