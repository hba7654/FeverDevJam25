using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sequence_77 : Sequence
{
    [SerializeField] private Light[] lights;
    [SerializeField] private Wire[] wires;
    [SerializeField] private float dimSpeed;
    [SerializeField] private float newIntenisty;
    [SerializeField] private Transform canvas1Transform;
    [SerializeField] private Transform canvas2Transform;

    private bool success;

    protected override void Start()
    {
        base.Start();

        StartCoroutine(TurnOffLights());
        success = false;

    }

    private void Update()
    {
        foreach (Wire wire in wires)
        {
            if (!wire.IsSuccess)
            {
                print(wire.name);
                return;
            }
        }

        if (!success)
        {
            print("all done");
            player.puzzleComplete = true;
            StartCoroutine(TurnOnLights());
            OnPuzzleCompleted?.Invoke();
        }
        success = true;
    }

    private IEnumerator TurnOffLights()
    {
        float curIntensity = 10;
        foreach (Light light in lights) { light.GetComponent<LightController>().enabled = false; }

        while (curIntensity >= -1)
        {
            foreach (Light light in lights)
            {
                light.intensity = curIntensity;
            }
            curIntensity -= Time.deltaTime * dimSpeed;
            yield return null;
        }
        foreach (Light light in lights) { light.enabled = false; }
    }
    private IEnumerator TurnOnLights()
    {
        float curIntensity = 0;
        //foreach (Light light in lights) { light.GetComponent<LightController>().enabled = true; }

        while (curIntensity <= newIntenisty)
        {
            foreach (Light light in lights)
            {
                light.intensity = curIntensity;
            }
            curIntensity += Time.deltaTime * dimSpeed;
            yield return null;
        }
        foreach (Light light in lights) 
        { 
            light.enabled = true;
            light.intensity = newIntenisty;
        }
    }
}
