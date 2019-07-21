using System;
using System.Threading.Tasks;
using Mahua.Interfaces;
using Orleans;

namespace Mahua.Implements
{
    public class HelloGrain : Grain, IHelloGrain
    {
        public Task<string> GetId()
        {
            return Task.FromResult(Guid.NewGuid().ToString());
        }
    }
}