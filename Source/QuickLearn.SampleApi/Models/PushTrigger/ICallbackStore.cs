using System;
using System.Threading.Tasks;

namespace QuickLearn.SampleApi.Models
{
    public interface ICallbackStore<TConfiguration>
    {
        Task WriteCallbackAsync(string triggerId, Uri callbackUri, TConfiguration triggerConfig);

        Task DeleteCallbackAsync(string triggerId);
    }
}