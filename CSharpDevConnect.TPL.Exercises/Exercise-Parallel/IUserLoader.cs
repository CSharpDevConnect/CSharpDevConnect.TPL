namespace CSharpDevConnect.TPL.Exercises
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    internal interface IUserLoader
    {
        ParallelLoopResult Load(IEnumerable<User> users);
    }
}