using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadingScreenController : MonoBehaviour
{
    [Tooltip("How long to show this screen before switching back")]
    public float fakeLoadDuration = 2f;

    void Start()
    {
        StartCoroutine(DoLoad());
    }

    IEnumerator DoLoad()
    {
        // (Optional) Real async load:
        var op = SceneManager.LoadSceneAsync("Level 2");
        op.allowSceneActivation = false;

        // Wait at least your fake loading time
        yield return new WaitForSeconds(fakeLoadDuration);

        // (Optional) let real load finish:
        op.allowSceneActivation = true;
        yield return new WaitUntil(() => op.isDone);
    }
}
