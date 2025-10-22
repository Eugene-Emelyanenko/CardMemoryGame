using System.Collections;
using UnityEngine;

public class GlobalCoroutineRunner : MonoBehaviour
{
    public Coroutine Run(IEnumerator routine) => StartCoroutine(routine);
    public void Stop(Coroutine routine)
    {
        if (routine != null) StopCoroutine(routine);
    }
}