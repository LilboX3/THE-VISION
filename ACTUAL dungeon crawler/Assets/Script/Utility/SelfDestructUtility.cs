using System.Collections;
using UnityEngine;

public static class SelfDestructUtility
{
    public static IEnumerator SelfDestructAfter(this MonoBehaviour obj, float time)
    {
        yield return new WaitForSeconds(time);

        GameObject.Destroy(obj);
    }
}


