using System.Threading.Tasks;
using Orleans;

namespace GrainInterfaces1
{
    /// <summary>
    /// Grain interface IGrain1
    /// </summary>
    public interface IGrain1 : IGrainWithStringKey
    {
        Task<string> SayHello(string msg);
        Task<string> SayHello(string msg, int num);

        Task<string> SayHelloOld(string greeting);
    }
}