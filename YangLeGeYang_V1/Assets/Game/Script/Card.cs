using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public enum CardType
{
    Null, Carrot, Meat
}

public class Card : MonoBehaviour
{
    [SerializeField] CardType cardType;
    CardSpot[] cardSpots;
    bool pressingEnabled = true;

    private void Start() 
    {
        cardSpots = FindObjectsOfType<CardSpot>();
    }

    private void OnMouseDown()
    {
        if (!pressingEnabled) { return; }
        pressingEnabled = false;
        int spotNumberToMove = FindSpotNumber();
        MoveToSpot(spotNumberToMove);
    }

    private int FindSpotNumber() 
    {
        int destinationSpotNumber = 6;
        int minEmptySpotNumber = 6;
        List<Dictionary<string, int>> allSpotsInfo = new List<Dictionary<string, int>>();
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

    private void MoveToSpot(int spotNumber)
    {
        foreach (CardSpot cardSpot in cardSpots)
        {
            if (spotNumber == cardSpot.SpotNumber)
            {
                transform.DOMove(cardSpot.transform.position, 1);
                cardSpot.CardTypeInSpot = cardType;
                cardSpot.SpotOccupied = true;
                cardSpot.CardInSpot = this;
                break;
            }
        }
    }
}