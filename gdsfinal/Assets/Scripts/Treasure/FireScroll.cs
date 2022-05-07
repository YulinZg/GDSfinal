using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Pickups/FireScroll")]
public class FireScroll : PickupEffect
{
    [SerializeField] private GameObject strengthenText;
    [SerializeField] private int addNumber;

    public override void OnPickup(GameObject player)
    {
        player.GetComponent<Weapon>().ScrollF(addNumber);
        DamageUI damageUI = Instantiate(strengthenText, PlayerController.instance.transform.position + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0), Quaternion.identity).GetComponent<DamageUI>();
        damageUI.ShowStrengthen("Fire Scroll", Color.white);
    }
}
