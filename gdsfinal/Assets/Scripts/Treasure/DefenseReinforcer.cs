using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Pickups/DefenseReinforcer")]
public class DefenseReinforcer : PickupEffect
{
    [SerializeField] private GameObject strengthenText;
    [SerializeField] private int min;
    [SerializeField] private int max;

    public override void OnPickup(GameObject player)
    {
        int i = Random.Range(min, max + 1);
        player.GetComponent<Status>().AddDefense(i);
        DamageUI damageUI = Instantiate(strengthenText, PlayerController.instance.transform.position + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0), Quaternion.identity).GetComponent<DamageUI>();
        damageUI.ShowStrengthen("Defense +" + i, Color.blue);
    }
}
