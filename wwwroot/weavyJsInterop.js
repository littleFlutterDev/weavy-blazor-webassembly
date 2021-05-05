console.log("weavyJsInterop.js");

export function weavy(...options) {
    var jwtOptions = { jwt: () => DotNet.invokeMethodAsync('BlazorClientApp', 'GetJwtAsync') };
    return new window.Weavy(jwtOptions, ...options);
}