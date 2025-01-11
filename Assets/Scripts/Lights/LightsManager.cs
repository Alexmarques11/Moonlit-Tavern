using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightsManager : MonoBehaviour
{

    public GameObject[] lights;
    private List<Light2D> buildingLights = new List<Light2D>();

    public WaveSpawner waveSpawner;
    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject light in lights)
        {
            light.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (waveSpawner.waveComplete)
        {
            SetLights(false);
        }
        else
        {
            SetLights(true);
        }

    }

    void SetLights(bool isActive)
    {
        foreach (GameObject light in lights)
        {
            light.SetActive(isActive);
        }

        foreach (Light2D buildingLight in buildingLights)
        {
            buildingLight.enabled = isActive;
        }
    }


    public void RegisterBuildingLight(Light2D buildingLight)
    {
        if (!buildingLights.Contains(buildingLight))
        {
            buildingLights.Add(buildingLight);
        }
    }

    public void DeregisterBuildingLight(Light2D buildingLight)
    {
        if (buildingLights.Contains(buildingLight))
        {
            buildingLights.Remove(buildingLight);
        }
    }
}
