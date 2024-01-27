using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class camerashakemanager : MonoBehaviour
{
    public static camerashakemanager instance;
    [SerializeField] private float Global_shake_force = 1f;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    public void cameraShake(CinemachineImpulseSource impulseSource)
    {
        impulseSource.GenerateImpulseWithForce(Global_shake_force);
    }
}
