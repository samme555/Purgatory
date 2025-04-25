using UnityEngine;

public class RestartParticleSystem : MonoBehaviour
{
    private ParticleSystem ps;
    private bool initialized = false;

    private void Start()
    {
       ps = GetComponent<ParticleSystem>();
    }

    private void FixedUpdate()
    {
        if (!initialized && ps != null)
        {
            ps.Clear(true);
            ps.Play(true);
            initialized = true;
        }
    }

}
