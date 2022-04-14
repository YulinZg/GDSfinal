using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Pickups/CritProbabilityReinforcer")]
public class CritProbabilityReinforcer : PickupEffect
{
    [SerializeField] private GameObject strengthenText;

    public override void OnPickup(GameObject player)
    {
        int i = Random.Range(1, 10);
        player.GetComponent<Status>().AddCritProbability(i);
        DamageUI damageUI = Instantiate(strengthenText, PlayerController.instance.transform.position + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0), Quaternion.identity).GetComponent<DamageUI>();
        damageUI.ShowStrengthen("Critical Probability +" + i, Color.magenta);
    }
}
