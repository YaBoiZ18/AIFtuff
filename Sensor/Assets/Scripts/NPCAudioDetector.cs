using UnityEngine;
using System.Collections;

public class NPCAudioDetector : MonoBehaviour
{
    public float audibleThreshold = 0.5f; // Volume threshold
    public float sampleDelayMS = 1000f;   // Delay in milliseconds
    public bool canBeHeard = false;

    private float adjSampleDelay;
    private Coroutine samplingCoroutine;
    public Transform playerTransform; // Reference to the player

    void Start()
    {
        adjSampleDelay = sampleDelayMS / 1000f;
        samplingCoroutine = StartCoroutine(SampleAudio());
    }

    IEnumerator SampleAudio()
    {
        while (true)
        {
            yield return new WaitForSeconds(adjSampleDelay);
            canBeHeard = GetAvgVol() >= audibleThreshold;
        }
    }

    float GetAvgVol()
    {
        float[] data = new float[256];
        AudioListener.GetOutputData(data, 0);
        float a = 0;

        foreach (float s in data)
        {
            a += Mathf.Abs(s);
        }

        return a / 256;
    }
}
