using System;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace BlazorApp.Weavy {
    public class WeavyJsInterop : IDisposable {
        private readonly IJSRuntime JS;
        private bool Initialized = false;
        private IJSObjectReference Bridge;
        private ValueTask<IJSObjectReference> WhenImport;

        // Constructor
        // This is a good place to inject any authentication service you may use to provide JWT tokens.
        public WeavyJsInterop(IJSRuntime js) {
            JS = js;
        }

        // Initialization of the JS Interop Module
        // The initialization is only done once even if you call it multiple times
        public async Task Init() {
            if (!Initialized) {
                Initialized = true;
                WhenImport = JS.InvokeAsync<IJSObjectReference>("import", "./weavyJsInterop.js");
                Bridge = await WhenImport;
            } else {
                await WhenImport;
            }
        }

        // Calling Javascript to create a new instance of Weavy via the JS Interop Module
        public async ValueTask<IJSObjectReference> Weavy(object options = null) {
            await Init();
            // Demo JWT only for showcase.weavycloud.com
            // Configure your own JWT here
            var jwt = new { jwt = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJzYW1hcmEiLCJuYW1lIjoiU2FtYXJhIEthdXIiLCJleHAiOjI1MTYyMzkwMjIsImlzcyI6InN0YXRpYy1mb3ItZGVtbyIsImNsaWVudF9pZCI6IldlYXZ5RGVtbyIsImRpciI6ImNoYXQtZGVtby1kaXIiLCJlbWFpbCI6InNhbWFyYS5rYXVyQGV4YW1wbGUuY29tIiwidXNlcm5hbWUiOiJzYW1hcmEifQ.UKLmVTsyN779VY9JLTLvpVDLc32Coem_0evAkzG47kM" };
            return await Bridge.InvokeAsync<IJSObjectReference>("weavy", new object[] { jwt, options });
        }

        public void Dispose() {
            Bridge?.DisposeAsync();
        }
    }
}

