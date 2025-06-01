using UnityEngine;

// Forces a particle system to restart properly after instantiation
public class RestartParticleSystem : MonoBehaviour
{
    private ParticleSystem ps; // Reference to the attached particle system
    private bool initialized = false; // Ensures initialization happens only once

    // Get the ParticleSystem component
    private void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    // Wait until physics step to clear and restart particles
    private void FixedUpdate()
    {
        if (!initialized && ps != null)
        {
            ps.Clear(true); // Clear existing particles
            ps.Play(true);  // Start playing the particle system
            initialized = true; // Prevent re-triggering
        }
    }

}
