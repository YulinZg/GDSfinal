using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Pickups/MetalScroll")]
public class MetalScroll : PickupEffect
{
    [SerializeField] private GameObject strengthenText;

    public override void OnPickup(GameObject player)
    {
        player.GetComponent<Weapon>().ScrollM();
        DamageUI damageUI = Instantiate(strengthenText, PlayerController.instance.transform.position + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0), Quaternion.identity).GetComponent<DamageUI>();
        damageUI.ShowStrengthen("Metal Scroll", Color.white);
    }
}
