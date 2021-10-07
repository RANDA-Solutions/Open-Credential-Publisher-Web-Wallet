import { NgModule } from '@angular/core';
import { ServerModule } from '@angular/platform-server';
import { ModuleMapLoaderModule } from '@nguniversal/module-map-ngfactory-loader';
import { NGXLogger } from 'ngx-logger';
import { AppModule } from './app.module';

@NgModule({
    imports: [AppModule, ServerModule, ModuleMapLoaderModule],
    //bootstrap: [AppComponent]
})
export class AppServerModule { 
    constructor(private logger: NGXLogger) {
        this.logger.info("app server module");
    }
}
