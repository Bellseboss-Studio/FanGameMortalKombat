using System.Collections;
using System.Threading.Tasks;

public static class TaskExtension
{
    public static async void WrapErrors(this Task task)
    {
        await task;
    }
    public static IEnumerator AsCoroutine(this Task task)
    {
        while (!task.IsCompleted)
        {
            yield return null;
        }
 
        if (task.IsFaulted)
        {
            throw task.Exception;
        }
    }
}