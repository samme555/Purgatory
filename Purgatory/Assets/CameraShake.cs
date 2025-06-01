using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    /// <summary>
    /// performs camera shake effect by randomly offsetting camera's local position
    /// can be triggered with customizable duration and magnitude
    /// used when player takes damage
    /// </summary>

    //coroutine that shakes camera for specified duration and strength
    public IEnumerator Shake(float duration, float magnitude)
    {
        //store cameras original pos before shaking in order to restore it
        Vector3 originalPos = transform.localPosition;

        float elapsed = 0f;
        while(elapsed < duration)
        {
            //generate small random offset values and scale by magnitude
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = originalPos + new Vector3(x, y, 0f); //apply offset to cameras local position

            elapsed += Time.deltaTime; //increase elapsed time by time since last frame
            yield return null;
        }

        transform.localPosition = originalPos; //restore camera position to orgininal position
    }

    public void TriggerShake(float duration = 0.15f, float magnitude = 0.15f) //public method to trigger the shake with optimal duration and magnitude.
    {
        StartCoroutine(Shake(duration, magnitude));
    }
}
