using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Pickups/ViolentScroll")]
public class ViolentScroll : PickupEffect
{
    [SerializeField] private GameObject strengthenText;

    public override void OnPickup(GameObject player)
    {
        player.GetComponent<Status>().ViolentScroll();
        DamageUI damageUI = Instantiate(strengthenText, PlayerController.instance.transform.position + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0), Quaternion.identity).GetComponent<DamageUI>();
        damageUI.ShowStrengthen("Violent Scroll", Color.white);
    }
}
