using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyPack : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<TargetPosition>().OnVisit.AddListener(RefillEnergy);
    }

    private void RefillEnergy()
    {
        PlayerCar.Instance.Energy = 100f;
    }
}
