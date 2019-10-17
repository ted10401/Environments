
namespace JSLCore.Coroutine
{
    public class CoroutineManager : MonoSingleton<CoroutineManager>
    {
        public CoroutineChain Create()
        {
            return new CoroutineChain();
        }
    }
}