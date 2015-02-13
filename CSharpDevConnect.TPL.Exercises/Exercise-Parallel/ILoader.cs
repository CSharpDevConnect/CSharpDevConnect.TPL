using System.Collections.Generic;
using System.Threading.Tasks;

namespace CSharpDevConnect.TPL.Exercises
{
    internal interface ILoader<T>
    {
        ParallelLoopResult Load(IEnumerable<T> objectsToLoad);
    }
}