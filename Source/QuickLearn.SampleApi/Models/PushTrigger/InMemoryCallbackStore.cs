using QuickLearn.SampleApi.Models;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;

namespace QuickLearn.SampleApi.Controllers
{
    public class InMemoryCallbackStore<TConfiguration>
        : IClientCallbackStore<TConfiguration>, ICallbackStore<TConfiguration>
    {

        public InMemoryCallbackStore()
        {

        }

        Dictionary<string, Callback<TConfiguration>> storage
            = new Dictionary<string, Callback<TConfiguration>>();

        #region These would exist on the server side where the API lives
        public async Task DeleteCallbackAsync(string triggerId)
        {
            if (storage.ContainsKey(triggerId))
            {
                storage.Remove(triggerId);
            }

            return;
        }

        public async Task<IEnumerable<Callback<TConfiguration>>> ReadCallbacksAsync()
        {
            return storage.Values;
        }
        #endregion

        #region This would exist near the source of the event that triggers the notifications

        public async Task WriteCallbackAsync(string triggerId, Uri callbackUri, TConfiguration triggerConfig)
        {
            storage[triggerId] = new Callback<TConfiguration>(callbackUri, triggerConfig);

            return;
        }

        #endregion
    }
}