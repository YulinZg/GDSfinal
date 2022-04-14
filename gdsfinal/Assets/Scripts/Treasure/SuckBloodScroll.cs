using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Pickups/SuckBloodScroll")]
public class SuckBloodScroll : PickupEffect
{
    public override void OnPickup(GameObject player)
    {
        player.GetComponent<PlayerController>().canSuckBlood = true;
    }
}
