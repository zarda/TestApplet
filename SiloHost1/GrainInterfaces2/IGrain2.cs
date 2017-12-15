using System.Threading.Tasks;
using Orleans;

namespace GrainInterfaces2
{
    /// <summary>
    /// Grain interface IGrain2
    /// </summary>
    public interface IGrain2 : IGrainWithIntegerKey
    {
        Task<string> SayHello(string msg);
        Task<string> SayHello(string msg, int num);
    }
}