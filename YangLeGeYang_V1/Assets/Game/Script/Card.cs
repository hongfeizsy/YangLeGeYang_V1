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

    private int FindSpotNumber() 
    {
        int destinationSpotNumber = 6;
        int minEmptySpotNumber = 6;
        CardSpot[] allSpots = FindObjectsOfType<CardSpot>();
        List<Dictionary<string, int>> allSpotsInfo = new List<Dictionary<string, int>>();
        foreach (CardSpot spot in allSpots)
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
            List<int> cardNrToMoveRight = new List<int>();
            foreach (CardSpot spot in allSpots) 
            {
                if (spot.SpotOccupied && (spot.SpotNumber >= destinationSpotNumber))
                {
                    cardNrToMoveRight.Add(spot.SpotNumber);
                }
            }

            cardNrToMoveRight.Reverse();
            foreach (int spotNumber in cardNrToMoveRight) 
            {
                foreach (CardSpot cardSpot in allSpots)
                {
                    if (spotNumber == cardSpot.SpotNumber) 
                    {
                        cardSpot.CardInSpot.MoveToSpot(spotNumber + 1);
                    }
                }
            }
        }

        return destinationSpotNumber;
    }
}