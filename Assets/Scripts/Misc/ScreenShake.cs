using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    [SerializeField, Min(0f)] private float shakeTime = 0.4f;
    [SerializeField, Min(0f)] private float shakeAmplitude = 1f;
    [SerializeField] private CinemachineBasicMultiChannelPerlin noise;

    private float originalAmplitude;
    private Coroutine shakeCoroutine;
    
    private void Awake()
    {
        originalAmplitude = noise.AmplitudeGain;
    }
    public void ShakeScreen()
    {
        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
        }
        shakeCoroutine = StartCoroutine(ShakeRoutine());
    }
    private IEnumerator ShakeRoutine()
    {
        float elapsed = 0f;
        while (elapsed < shakeTime)
        {
            float remainingShake = 1f - elapsed / shakeTime;
            noise.AmplitudeGain = originalAmplitude + shakeAmplitude * remainingShake;
            elapsed += Time.deltaTime;
            yield return null;
        }
        noise.AmplitudeGain = originalAmplitude;
        shakeCoroutine = null;
    }
}
