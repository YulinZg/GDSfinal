using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Pickups/LightningScroll")]
public class LightningScroll : PickupEffect
{
    [SerializeField] private GameObject strengthenText;
    [SerializeField] private int addNumber;

    public override void OnPickup(GameObject player)
    {
        player.GetComponent<Weapon>().ScrollL(addNumber);
        DamageUI damageUI = Instantiate(strengthenText, PlayerController.instance.transform.position + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0), Quaternion.identity).GetComponent<DamageUI>();
        damageUI.ShowStrengthen("Lightning Scroll", Color.white);
    }
}
