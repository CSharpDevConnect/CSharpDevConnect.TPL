using System.Collections.Generic;
using System.Threading.Tasks;

namespace CSharpDevConnect.TPL.Exercises.UseTask
{
    internal interface ITaskLoader<T>
    {
        Task[] Load(IEnumerable<T> objectsToLoad);
    }
}