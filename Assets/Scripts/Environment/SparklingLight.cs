using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Environment
{
    /// <summary>
    /// Light that flickers on intervals, but mostly off
    /// </summary>
    public class SparklingLight : MonoBehaviour
    {
        [SerializeField] private Light2D light2D;
        [SerializeField] private float randomStartTime = 2f;
        [SerializeField] private float minOnTime = 1f;
        [SerializeField] private float maxOnTime = 3f;
        [SerializeField] private float sparkleIntervals = 0.1f;

        private void OnValidate()
        {
            if (light2D == null)
                light2D = GetComponent<Light2D>();
        }
        
        private void Start()
        {
            light2D.enabled = false;
            StartCoroutine(SparkleEffect());
        }

        private IEnumerator SparkleEffect()
        {
            while (true)
            {
                //wait a random time before light turning on, for the duration, the light flickers before going off until next time
                float onTime = UnityEngine.Random.Range(minOnTime, maxOnTime)+Time.time;

                while (Time.time < onTime)
                {
                    light2D.enabled = true;
                    yield return new WaitForSeconds(sparkleIntervals);
                    light2D.enabled = false;
                    yield return new WaitForSeconds(sparkleIntervals);
                }
                light2D.enabled = false;
                
                yield return new WaitForSeconds(UnityEngine.Random.Range(randomStartTime, randomStartTime * 2));
            }
        }
    }
}
