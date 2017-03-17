using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuickLearn.SampleApi.Models
{
    public interface IClientCallbackStore<TConfiguration>
    {
        Task<IEnumerable<Callback<TConfiguration>>> ReadCallbacksAsync();
    }
}