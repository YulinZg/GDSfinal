using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    GameObject wallSprite;

    private void OnEnable()
    {
        wallSprite = transform.parent.GetChild(0).gameObject;
        wallSprite.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            wallSprite.SetActive(true);
            CameraController.instance.changeTarget(transform);
            MiniMapCamera.instance.setPos(transform.position);
        }
    }
}
