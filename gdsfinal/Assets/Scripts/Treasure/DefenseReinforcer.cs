using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Pickups/DefenseReinforcer")]
public class DefenseReinforcer : PickupEffect
{
    [SerializeField] private GameObject strengthenText;

    public override void OnPickup(GameObject player)
    {
        int i = Random.Range(1, 10);
        player.GetComponent<Status>().AddDefense(i);
        DamageUI damageUI = Instantiate(strengthenText, PlayerController.instance.transform.position + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0), Quaternion.identity).GetComponent<DamageUI>();
        damageUI.ShowStrengthen("Defense +" + i, Color.blue);
    }
}
