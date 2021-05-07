﻿using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace BlazorApp.Weavy {
    //
    // Summary:
    //     Wrapped IJSObjectReference to the Weavy instance in Javascript.
    //     Adds .Space() and .Destroy() methods.
    public class WeavyReference : ExtendableJSObjectReference {
        private bool Initialized = false;
        public WeavyJsInterop Wvy;
        public object Options;
        public ValueTask<IJSObjectReference> WhenWeavy;

        public WeavyReference(WeavyJsInterop wvy = null, object options = null, IJSObjectReference weavy = null) : base(weavy) {
            Options = options;
            Wvy = wvy;
        }

        public async Task Init() {
            if(!Initialized) {
                Initialized = true;
                WhenWeavy = Wvy.Weavy(Options);
                ObjectReference = await WhenWeavy;
            } else {
                await WhenWeavy;
            }
        }

        public async ValueTask<SpaceReference> Space(object spaceSelector = null) {
            await Init();
            return new(await ObjectReference.InvokeAsync<IJSObjectReference>("space", new object[] { spaceSelector }));
        }

        public async Task Destroy() {
            await ObjectReference.InvokeVoidAsync("destroy");
            await DisposeAsync();
        }
    }

    //
    // Summary:
    //     Wrapped IJSObjectReference to a Weavy Space in Javascript.
    //     Adds .App() and .Remove() methods.
    public class SpaceReference : ExtendableJSObjectReference {
        public SpaceReference(IJSObjectReference space) : base(space) { }

        public async ValueTask<AppReference> App(object appSelector = null) {
            return new(await ObjectReference.InvokeAsync<IJSObjectReference>("app", new object[] { appSelector }));
        }

        public async Task Remove() {
            await ObjectReference.InvokeVoidAsync("remove");
            await DisposeAsync();
        }
    }

    //
    // Summary:
    //     Wrapped IJSObjectReference to a Weavy App in Javascript.
    //     Adds .Open(), .Close(), .Toggle() and .Remove() methods()
    public class AppReference : ExtendableJSObjectReference {
        public AppReference(IJSObjectReference app) : base(app) { }

        public ValueTask Open() {
            return ObjectReference.InvokeVoidAsync("open");
        }

        public ValueTask Close() {
            return ObjectReference.InvokeVoidAsync("close");
        }

        public ValueTask Toggle() {
            return ObjectReference.InvokeVoidAsync("toggle");
        }

        public async Task Remove() {
            await ObjectReference.InvokeVoidAsync("remove");
            await DisposeAsync();
        }
    }
}
