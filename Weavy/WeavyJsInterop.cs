using System;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace BlazorClientApp.Weavy {
    public class WeavyJsInterop : IDisposable {
        private bool initialized = false;
        private readonly IJSRuntime js;
        public IJSObjectReference bridge;
        private ValueTask<IJSObjectReference> WhenImport;

        public WeavyJsInterop(IJSRuntime js) {
            this.js = js;
        }

        public async Task Init() {
            if(!initialized) {
                WhenImport = js.InvokeAsync<IJSObjectReference>("import", "./weavyJsInterop.js");
                bridge = await WhenImport;
            } else {
                await WhenImport;
            }
        }

        public async ValueTask<IJSObjectReference> Weavy(object options = null) {
            await Init();
            var jwtOptions = new { jwt = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJzYW1hcmEiLCJuYW1lIjoiU2FtYXJhIEthdXIiLCJleHAiOjI1MTYyMzkwMjIsImlzcyI6InN0YXRpYy1mb3ItZGVtbyIsImNsaWVudF9pZCI6IldlYXZ5RGVtbyIsImRpciI6ImNoYXQtZGVtby1kaXIiLCJlbWFpbCI6InNhbWFyYS5rYXVyQGV4YW1wbGUuY29tIiwidXNlcm5hbWUiOiJzYW1hcmEifQ.UKLmVTsyN779VY9JLTLvpVDLc32Coem_0evAkzG47kM" };
            return await bridge.InvokeAsync<IJSObjectReference>("weavy", new object[] { jwtOptions, options });
        }

        public void Dispose() {
            bridge?.DisposeAsync();
        }
    }
}

