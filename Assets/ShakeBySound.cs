﻿ using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ShakeBySound : MonoBehaviour
{
    public AudioSource audioSource;
    public float updateStep = 0.1f;
    public int sampleDataLength = 1024;

    private float currentUpdateTime = 0f;

    private float clipLoudness;
    private float[] clipSampleData;

    // Use this for initialization
    void Awake()
    {

        if (!audioSource)
        {
            Debug.LogError(GetType() + ".Awake: there was no audioSource set.");
        }
        clipSampleData = new float[sampleDataLength];

    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindWithTag("Player").GetComponent<Player>())
        {
            currentUpdateTime += Time.deltaTime;
            if (currentUpdateTime >= updateStep)
            {
                currentUpdateTime = 0f;
                audioSource.clip.GetData(clipSampleData, audioSource.timeSamples); //I read 1024 samples, which is about 80 ms on a 44khz stereo clip, beginning at the current sample position of the clip.
                clipLoudness = 0f;
                foreach (var sample in clipSampleData)
                {
                    clipLoudness += Mathf.Abs(sample);
                }
                clipLoudness /= sampleDataLength; //clipLoudness is what you are looking for

                float temp = clipLoudness * 10f;
                transform.position -= new Vector3(0, Mathf.Pow(temp, 4) / 5000, 0);
            }
        }
        else
            if (audioSource.volume > 0) audioSource.volume -= Time.deltaTime;
        else if (Input.GetKeyDown(KeyCode.R))
        {
           // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

    }
}
