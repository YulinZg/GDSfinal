using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Pickups/WaterScroll")]
public class WaterScroll : PickupEffect
{
    [SerializeField] private GameObject strengthenText;
    [SerializeField] private float addTime;

    public override void OnPickup(GameObject player)
    {
        player.GetComponent<Weapon>().ScrollW(addTime);
        DamageUI damageUI = Instantiate(strengthenText, PlayerController.instance.transform.position + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0), Quaternion.identity).GetComponent<DamageUI>();
        damageUI.ShowStrengthen("Water Scroll", Color.white);
    }
}
