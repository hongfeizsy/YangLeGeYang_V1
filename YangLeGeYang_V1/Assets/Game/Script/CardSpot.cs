using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSpot : MonoBehaviour
{
    [SerializeField] int spotNumber;
    bool isOccupied = false;


    public int GetSpotNumber()
    {
        return spotNumber;
    }

    public bool GetOccupationStatus()
    {
        return isOccupied;
    }

    public GameObject MatchTargetSpot(int targetSpotNumber)
    {
        if (targetSpotNumber == spotNumber) return gameObject;
        else return null;
    }
}
