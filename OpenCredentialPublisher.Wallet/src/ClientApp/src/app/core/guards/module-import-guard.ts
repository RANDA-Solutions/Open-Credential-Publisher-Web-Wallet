export function throwIfAlreadyLoaded(parentModule: any,
    moduleName: string) {
    if (parentModule) {
        throw new Error(`${moduleName} has already been loaded.
           Import this module in the AppModule only.`);
    }
}
