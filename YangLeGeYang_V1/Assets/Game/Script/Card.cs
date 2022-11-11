using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class Card : MonoBehaviour
{
    [SerializeField] CardDestination cardDestination;

    private void OnMouseDown()
    {
        Vector2 destinationPos = cardDestination.transform.position;
        transform.DOMove(destinationPos, 1);
    }
}
