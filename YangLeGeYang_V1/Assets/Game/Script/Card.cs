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
    CardSpot cardSpot;
    bool pressingEnabled = true;

    private void OnMouseDown()
    {
        if (!pressingEnabled) { return; }
        pressingEnabled = false;
        //cardSpot = FindSpareSpot();
        cardSpot = FindDestinationSpot();
        cardSpot.CardTypeInSpot = cardType;
        cardSpot.SpotOccupied = true;
        cardSpot.CardInSpot = this;
        Vector2 destinationPos = cardSpot.transform.position;
        transform.DOMove(destinationPos, 1);
    }

    private CardSpot FindSpareSpot()
    {
        CardSpot[] allSpots = FindObjectsOfType<CardSpot>();
        int minSpotNumber = 6;
        List<int> spareSpotNumbers = new List<int>();
        foreach (CardSpot spot in allSpots)
        {
            if (!spot.SpotOccupied & (minSpotNumber > spot.SpotNumber)) 
            {
                minSpotNumber = spot.SpotNumber;
                //spareSpotNumbers.Add(spot.GetSpotNumber()); 
            }
        }

        foreach (CardSpot spot in allSpots)
        {
            if (minSpotNumber == spot.SpotNumber) return spot;
        }

        return null;

        //return Mathf.Min(spareSpotNumbers.ToArray());
        //return minSpotNumber;
    }

    private CardSpot FindDestinationSpot()
    {
        int destinationSpotNumber = 6;
        int minEmptySpotNumber = 6;
        CardSpot[] allSpots = FindObjectsOfType<CardSpot>();
        List<Dictionary<string, int>> allSpotsInfo = new List<Dictionary<string, int>>();
        foreach (CardSpot spot in allSpots)
        {
            Dictionary<string, int> spotInfo = new Dictionary<string, int>();
            if(spot.SpotOccupied) 
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
            print(spotInfo["SpotNumber"].ToString() + ": " + spotInfo["CardType"].ToString());
            if (spotInfo["occupied"] == 1 & (int)cardType == spotInfo["CardType"])
            {
                sameTypeExist = true;
                if (spotNumberWithSameType < spotInfo["SpotNumber"])
                {
                    spotNumberWithSameType = spotInfo["SpotNumber"];
                }
            }
        }

        //if (sameTypeExist) { destinationSpotNumber = spotNumberWithSameType + 1; }
        //else { destinationSpotNumber = minEmptySpotNumber; }

        if (!sameTypeExist) 
        { 
            destinationSpotNumber = minEmptySpotNumber; 
        }
        else
        {
            //print("Same card exists.");
            destinationSpotNumber = spotNumberWithSameType + 1;
            foreach (CardSpot cardSpot in allSpots)
            {
                if (cardSpot.SpotNumber > destinationSpotNumber & cardSpot.SpotNumber < minEmptySpotNumber)
                {
                    print(cardSpot.SpotNumber);
                    cardSpot.CardInSpot.transform.DOMove(cardSpot.GetNeigheringSpotOnRight().transform.position, 0.5f);
                }
            }
        }

        //if (destinationSpotNumber <= minEmptySpotNumber - 1)
        //{
        //    foreach (CardSpot cardSpot in allSpots)
        //    {
        //        if (cardSpot.SpotNumber > destinationSpotNumber & cardSpot.SpotNumber < minEmptySpotNumber)
        //        {
        //            cardSpot.CardInSpot.transform.DOMove(cardSpot.GetNeigheringSpotOnRight().transform.position, 0.5f);
        //        }
        //    }

        //    //for (int i = destinationSpotNumber + 1; i < minEmptySpotNumber; i++)
        //    //{

        //    //}
        //    //MoveCardsToRight();
        //}

        foreach (CardSpot spot in allSpots)
        {
            if (destinationSpotNumber == spot.SpotNumber) return spot;
        }
        return null;
    }

    private void MoveCardsToRight()
    {
        throw new NotImplementedException();
    }

}
