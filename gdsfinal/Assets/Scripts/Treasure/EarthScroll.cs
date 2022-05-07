using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Pickups/EarthScroll")]
public class EarthScroll : PickupEffect
{
    [SerializeField] private GameObject strengthenText;
    [SerializeField] private int reduceTimes;

    public override void OnPickup(GameObject player)
    {
        player.GetComponent<Weapon>().ScrollE(reduceTimes);
        DamageUI damageUI = Instantiate(strengthenText, PlayerController.instance.transform.position + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0), Quaternion.identity).GetComponent<DamageUI>();
        damageUI.ShowStrengthen("Earth Scroll", Color.white);
    }
}
