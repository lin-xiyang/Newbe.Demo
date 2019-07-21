using System;
using System.Threading.Tasks;
using Orleans;

namespace Mahua.Interfaces
{
    public interface IHelloGrain : IGrainWithStringKey
    {
        Task<string> GetId();
    }
}