﻿namespace TurnTheGameOn.SimpleTrafficSystem
{
    using UnityEngine;

    [HelpURL("https://simpletrafficsystem.turnthegameon.com/documentation/api/aitrafficlightmanager")]
    public class AITrafficLightManager : MonoBehaviour
    {
        [Tooltip("Array of AITrafficLightCycles played as a looped sequence.")]
        public AITrafficLightCycle[] trafficLightCycles;
        private float timer;
        private enum CycleState { Green, Red, Yellow, Complete }
        private CycleState lightState;
        private int cycleIndex;

        private void Start()
        {
            if (trafficLightCycles.Length > 0)
            {
                // Set all lights to red
                for (int i = 0; i < trafficLightCycles.Length; i++)
                {
                    for (int j = 0; j < trafficLightCycles[i].trafficLights.Length; j++)
                    {
                        trafficLightCycles[i].trafficLights[j].EnableRedLight();
                    }
                }
                lightState = CycleState.Red;
                cycleIndex = -1;
                timer = 0.0f;
            }
            else
            {
                Debug.LogWarning("There are no lights assigned to this TrafficLightManger, it will be disabled.");
                enabled = false;
            }
        }

        private void FixedUpdate()
        {
            if (timer > 0.0f)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                if (lightState == CycleState.Complete)
                {
                    lightState = CycleState.Green;
                    timer = trafficLightCycles[cycleIndex].greenTimer;
                    for (int i = 0; i < trafficLightCycles[cycleIndex].trafficLights.Length; i++)
                    {
                        trafficLightCycles[cycleIndex].trafficLights[i].EnableGreenLight();
                    }
                }
                else if (lightState == CycleState.Green)
                {
                    lightState = CycleState.Yellow;
                    timer = trafficLightCycles[cycleIndex].yellowTimer;
                    for (int i = 0; i < trafficLightCycles[cycleIndex].trafficLights.Length; i++)
                    {
                        trafficLightCycles[cycleIndex].trafficLights[i].EnableYellowLight();
                    }
                }
                else if (lightState == CycleState.Yellow)
                {
                    lightState = CycleState.Red;
                    timer = trafficLightCycles[cycleIndex].redtimer;
                    for (int i = 0; i < trafficLightCycles[cycleIndex].trafficLights.Length; i++)
                    {
                        trafficLightCycles[cycleIndex].trafficLights[i].EnableRedLight();
                    }
                }
                else if (lightState == CycleState.Red)
                {
                    lightState = CycleState.Complete;
                    cycleIndex = cycleIndex != trafficLightCycles.Length - 1 ? cycleIndex + 1 : 0;
                }
            }
        }

    }
}