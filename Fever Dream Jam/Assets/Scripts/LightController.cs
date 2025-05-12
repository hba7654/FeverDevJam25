using UnityEngine;

public class LightController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private Light myLight;

    public float minIntensity = 0f;
    public float maxIntensity = 80.0f;

    void Start()
    {
        myLight = GetComponent<Light>();
       // myLight.GetComponent<Light>().intensity = 0;
    }

    // Update is called once per frame
    void Update()
    {
        float duration = Random.Range(0.1f, 5.0f);
        float t = 0f;
        
        float startIntensity = myLight.intensity;
         float targetIntensity = Random.Range(minIntensity, maxIntensity);

        while (t < duration)
        {
              myLight.intensity = Mathf.Lerp(startIntensity, targetIntensity, t / duration);
            myLight.intensity = Random.Range(minIntensity, maxIntensity);
            t += Time.deltaTime;
            
        }
       
    }
}
