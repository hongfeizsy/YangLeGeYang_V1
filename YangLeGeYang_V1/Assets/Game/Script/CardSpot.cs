using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSpot : MonoBehaviour
{
    [SerializeField] CardSpot neighberingSpotOnRight;
    [SerializeField] int spotNumber;
    [SerializeField] CardType cardType;
    [SerializeField] Card cardInSpot;
    bool isOccupied = false;

    public GameObject MatchTargetSpot(int targetSpotNumber)
    {
        if (targetSpotNumber == spotNumber) return gameObject;
        else return null;
    }

    public int SpotNumber
    {
        get { return spotNumber; }
        //set { spotNumber = value; }
    }

    public bool SpotOccupied
    {
        get { return isOccupied; }
        set { isOccupied = value; }
    }

    public CardType CardTypeInSpot
    {
        get { return cardType; }
        set { cardType = value; }
    }

    public Card CardInSpot
    {
        get { return cardInSpot; }
        set { cardInSpot = value; }
    }

    public CardSpot GetNeigheringSpotOnRight()
    {
        return neighberingSpotOnRight;
    }
}
