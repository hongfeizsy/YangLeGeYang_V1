using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class Card : MonoBehaviour
{
    [SerializeField] CardDestination cardDestination;
    bool pressingEnabled = true;

    private void OnMouseDown()
    {
        if (!pressingEnabled) { return; }
        Vector2 destinationPos = cardDestination.transform.position;
        transform.DOMove(destinationPos, 1);
    }

    
}
