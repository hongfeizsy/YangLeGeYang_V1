using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class Card : MonoBehaviour
{
    CardSpot cardSpot;
    bool pressingEnabled = true;

    private void OnMouseDown()
    {
        if (!pressingEnabled) { return; }
        pressingEnabled = false;
        CardSpot cardSpot = FindSpareSpot();

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
            if (!spot.GetOccupationStatus() & (minSpotNumber > spot.GetSpotNumber())) 
            {
                minSpotNumber = spot.GetSpotNumber();
                //spareSpotNumbers.Add(spot.GetSpotNumber()); 
            }
        }

        foreach (CardSpot spot in allSpots)
        {
            if (minSpotNumber == spot.GetSpotNumber()) return spot;
        }

        return null;

        //return Mathf.Min(spareSpotNumbers.ToArray());
        //return minSpotNumber;
    }
}
