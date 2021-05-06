console.log("weavyJsInterop.js");

export function weavy(...options) {
    return new window.Weavy(...options);
}