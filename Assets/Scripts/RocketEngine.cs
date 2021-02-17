﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(PhysicsEngine))]
public class RocketEngine : MonoBehaviour
{
    public Vector3 thrustUnitVector; // [none]
    public float fuelMass; // [kg]
    public float maxThrust; // kN[kg m s^-2]

    [Range (0, 1f)]
    public float thrustPercent; // [none]
    

    private PhysicsEngine physicsEngine;
    private float currentThrust; // N
    // Start is called before the first frame update
    void Start()
    {
        physicsEngine = GetComponent<PhysicsEngine>();
    }

    void FixedUpdate()
    {
        if (fuelMass > FuelThisUpdate()) {
            fuelMass -= FuelThisUpdate();
            physicsEngine.mass -= FuelThisUpdate();
            ExertForce();
        } else {
            Debug.LogWarning("Out of rocket fuel.");
        }

        physicsEngine.AddForce(thrustUnitVector);
    }

    void ExertForce () {
        currentThrust = thrustPercent * maxThrust * 1000f;
        Vector3 thrustVector = thrustUnitVector.normalized * currentThrust;
        physicsEngine.AddForce(thrustVector);
    }

    float FuelThisUpdate() {
        float exhaustMassFlow;              // []
        exhaustMassFlow = currentThrust / effectiveExhaustVelocity;

        return exhaustMassFlow * Time.deltaTime; // [kg]

    }
}
