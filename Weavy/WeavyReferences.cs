using Microsoft.JSInterop;
using System.Collections;
using System.Threading.Tasks;

namespace BlazorApp.Weavy {
    public class WeavyReference : ExtendableJSObjectReference {
        private bool initialized = false;
        public WeavyJsInterop wvy;
        public object options;
        public ValueTask<IJSObjectReference> WhenWeavy;

        public WeavyReference(WeavyJsInterop wvy = null, object options = null, IJSObjectReference weavy = null) : base(weavy) {
            this.options = options;
            this.wvy = wvy;
        }

        public async Task Init() {
            if(!initialized) {
                initialized = true;
                WhenWeavy = wvy.Weavy(options);
                objectReference = await WhenWeavy;
            } else {
                await WhenWeavy;
            }
        }

        public async ValueTask<SpaceReference> Space(object spaceSelector = null) {
            await Init();
            return new(await objectReference.InvokeAsync<IJSObjectReference>("space", new object[] { spaceSelector }));
        }

        public async Task Destroy() {
            await objectReference.InvokeVoidAsync("destroy");
            await DisposeAsync();
        }
    }

    public class SpaceReference : ExtendableJSObjectReference {
        public SpaceReference(IJSObjectReference space) : base(space) { }

        public async ValueTask<AppReference> App(object appSelector = null) {
            return new(await objectReference.InvokeAsync<IJSObjectReference>("app", new object[] { appSelector }));
        }

        public async Task Remove() {
            await objectReference.InvokeVoidAsync("remove");
            await DisposeAsync();
        }
    }

    public class AppReference : ExtendableJSObjectReference {
        public AppReference(IJSObjectReference app) : base(app) { }

        public ValueTask Open() {
            return objectReference.InvokeVoidAsync("open", new object[] { });
        }

        public ValueTask Close() {
            return objectReference.InvokeVoidAsync("close", new object[] { });
        }

        public ValueTask Toggle() {
            return objectReference.InvokeVoidAsync("toggle", new object[] { });
        }

        public async Task Remove() {
            await objectReference.InvokeVoidAsync("remove");
            await DisposeAsync();
        }
    }
}
