using UnityEngine;
using JSLCore.Coroutine;

public class UnitTesting_Coroutine : MonoBehaviour
{
    private CoroutineManager m_coroutineManager;

    private void Awake()
    {
        m_coroutineManager = CoroutineManager.Instance;
    }

    private void Update()
    {
        m_coroutineManager.Create()
            .Enqueue(OnCoroutineFinish)
            .StartCoroutine();
    }

    private void OnCoroutineFinish()
    {

    }
}
