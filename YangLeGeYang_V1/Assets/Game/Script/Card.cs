using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using System.Linq;

public enum CardType
{
    Null, Carrot, Meat, Apple, Bee, Brush, Cotton, Dagger, Mooncake, Pumpkin, Rabbit
}

public class Card : MonoBehaviour
{
    [SerializeField] CardType cardType;
    int cardIndex;
    CardSpot[] cardSpots;
    bool isInBox = false;
    bool isTouchable = false;
    CardSpawner spawner;
    IEnumerator coroutine;
    Vector3 coordidate;
    CardBox cardBox;

    private void Start() 
    {
        cardSpots = FindObjectsOfType<CardSpot>();
        spawner = transform.parent.GetComponent<CardSpawner>();
        cardBox = FindObjectOfType<CardBox>();
    }

    private void OnMouseDown()
    {
        if (!cardBox.GameContinue) { return; }
        if (isInBox) { return; }     // Can't be isTouchable, as the renderer can't be blur.

        isInBox = true;
        int spotNumberToMove = FindSpotNumber();
        transform.parent = null;
        // spawner.EnableCardInQueue();
        CardMatrixProducer producer = FindObjectOfType<CardMatrixProducer>();
        producer.RemoveItemsFromLists(cardIndex, coordidate);
        producer.SetCardTouchability();

        if (IsThreeTiles(spotNumberToMove))
        {
            float waitTimeToKill = 0.3f;  // The time used for a card from producer to card spot.
            MoveToSpot(spotNumberToMove);
            StartCoroutine(WaitAndKillThreeTiles(waitTimeToKill, spotNumberToMove));
        }
        else { MoveToSpot(spotNumberToMove); }
        
    }

    private int FindSpotNumber() 
    {
        int destinationSpotNumber = 6;
        List<Dictionary<string, int>> allSpotsInfo = new List<Dictionary<string, int>>();
        int minEmptySpotNumber = FindMinEmptySpotNumber(allSpotsInfo);

        bool sameTypeExist = false;
        int spotNumberWithSameType = 0;
        foreach (Dictionary<string, int> spotInfo in allSpotsInfo)
        {
            if (spotInfo["occupied"] == 1 & (int)cardType == spotInfo["CardType"])
            {
                sameTypeExist = true;
                if (spotNumberWithSameType < spotInfo["SpotNumber"])
                {
                    spotNumberWithSameType = spotInfo["SpotNumber"];
                }
            }
        }

        if (!sameTypeExist)
        {
            destinationSpotNumber = minEmptySpotNumber;
        }
        else
        {
            destinationSpotNumber = spotNumberWithSameType + 1;
            MoveCardsToRight(destinationSpotNumber);
        }

        return destinationSpotNumber;
    }

    private int FindMinEmptySpotNumber(List<Dictionary<string, int>> allSpotsInfo) 
    {
        int minEmptySpotNumber = 6;
        
        foreach (CardSpot spot in cardSpots)
        {
            Dictionary<string, int> spotInfo = new Dictionary<string, int>();
            if (spot.SpotOccupied)
            {
                spotInfo.Add("occupied", 1);
                spotInfo.Add("CardType", (int)spot.CardTypeInSpot);
                spotInfo.Add("SpotNumber", spot.SpotNumber);
            }
            else
            {
                spotInfo.Add("occupied", 0);
                spotInfo.Add("CardType", 0);
                spotInfo.Add("SpotNumber", spot.SpotNumber);
                if (spot.SpotNumber < minEmptySpotNumber) { minEmptySpotNumber = spot.SpotNumber; }
            }
            allSpotsInfo.Add(spotInfo);
        }

        return minEmptySpotNumber;
    }

    private void MoveCardsToRight(int destinationSpotNumber) 
    {
        List<int> cardNrToMoveRight = new List<int>();
        foreach (CardSpot spot in cardSpots)
        {
            if (spot.SpotOccupied && (spot.SpotNumber >= destinationSpotNumber))
            {
                cardNrToMoveRight.Add(spot.SpotNumber);
            }
        }

        cardNrToMoveRight.Sort((x, y) => y.CompareTo(x));  // Or cardNrToMoveRight.Sort(); cardNrToMoveRight.Reverse();
        foreach (int spotNumber in cardNrToMoveRight)
        {
            foreach (CardSpot cardSpot in cardSpots)
            {
                if (spotNumber == cardSpot.SpotNumber)
                {
                    cardSpot.CardInSpot.MoveToSpot(spotNumber + 1);
                    break;
                }
            }
        }
    }

    public void MoveToSpot(int spotNumber)
    {
        foreach (CardSpot cardSpot in cardSpots)
        {
            if (spotNumber == cardSpot.SpotNumber)
            {
                transform.DOMove(cardSpot.transform.position, 0.3f);
                cardSpot.CardTypeInSpot = cardType;
                cardSpot.SpotOccupied = true;
                cardSpot.CardInSpot = this;
                break;
            }
        }
    }

    private bool IsThreeTiles(int spotNumber) 
    {
        int sameTypeCount = 0;
        foreach (CardSpot cardSpot in cardSpots) 
        {
            if ((spotNumber - 1) == cardSpot.SpotNumber || (spotNumber - 2) == cardSpot.SpotNumber) 
            {
                if(cardSpot.CardTypeInSpot == cardType) { sameTypeCount++; }
            }
        }
        
        if (sameTypeCount == 2) { return true; }
        return false;
    }

    public bool IsTouchable {
        get { return isTouchable; }
        set {
            isTouchable = value;
            GetComponent<BoxCollider2D>().enabled = value;
            float alpha = 0.7f;
            if (value) { alpha = 1; }
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, alpha);
            transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, alpha);
        }
    }

    private IEnumerator WaitAndKillThreeTiles(float waitTimeToKill, int spotNumber) 
    {
        yield return new WaitForSeconds(waitTimeToKill);
        KillThreeTiles(spotNumber);
        FindObjectOfType<CardBox>().ShouldMoveCardsToLeft = true;
    }

    private void KillThreeTiles(int spotNumber)
    {
        foreach (CardSpot cardSpot in cardSpots)
        {
            if ((cardSpot.SpotNumber > spotNumber - 3) && (cardSpot.SpotNumber <= spotNumber))
            {
                cardSpot.DestroyCardInSpot();
                cardSpot.CardTypeInSpot = CardType.Null;
                cardSpot.CardInSpot = null;
                cardSpot.SpotOccupied = false;
            }
        }
    }

    public Vector3 Coordidate {
        get { return coordidate; }
        set { coordidate = value; }
    }

    public int CardIndex {
        get { return cardIndex; }
        set { cardIndex = value; }
    }
}