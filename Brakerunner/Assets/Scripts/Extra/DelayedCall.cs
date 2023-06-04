using System.Collections;
using UnityEngine;
using Action = System.Action;

public static class DelayedCall
{
    public static void Create(MonoBehaviour caller, Action action, float delay)
    {
        caller.StartCoroutine(Delay(action, delay));
    }

    private static IEnumerator Delay(Action action, float delay)
    {
        yield return new WaitForSeconds(delay);
        action.Invoke();
    }
}
