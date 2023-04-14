using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class NetworkPlayer : NetworkBehaviour, IPlayerLeft
{
    public static NetworkPlayer local { get; set; }
    public override void Spawned()
    {
        if (Object.HasInputAuthority)
        {
            local = this;
            Debug.Log("spawned local player");

        }
        else Debug.Log("spawned remote player");
    }
    public void PlayerLeft(PlayerRef player)
    {
        if (player == Object.InputAuthority)
        {
            Runner.Despawn(Object);
        }
            
    }
    void Start()
    {

    }
}
