using UnityEngine;
using System.Collections;

public class StairsActivation : MonoBehaviour
{
    [SerializeField] GameObject stairs;
    private Room room;

    private void Start()
    {
        room = GetComponentInParent<Room>();
        StartCoroutine(WatchForClear());
    }
    private IEnumerator WatchForClear()
    {
        // 1) Immediately hide them
        stairs.SetActive(false);

        // 2) Wait for the boss to actually spawn/activate
        yield return new WaitUntil(() => room.BossIsAlive());

        // 3) Then wait until that same flag flips false (boss dead)
        yield return new WaitWhile(() => room.BossIsAlive());

        // 4) Finally show the stairs
        stairs.SetActive(true);
    }
}
