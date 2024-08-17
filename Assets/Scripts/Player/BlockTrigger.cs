using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockTrigger : MonoBehaviour
{
    PlayerPower playerPower;

    private void Start()
    {
        playerPower = Player.GetPlayer().GetComponent<PlayerPower>();
    }

    private void OnTriggerEnter(Collider other)
    {
        playerPower.canSummonBlock = false;
    }

    private void OnTriggerExit(Collider other)
    {
        playerPower.canSummonBlock = true;
    }
}
