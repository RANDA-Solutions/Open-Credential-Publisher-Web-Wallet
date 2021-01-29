import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { FolioComponent, FolioDraftsComponent, FolioSendAllComponent, FolioSendComponent, FolioStartComponent } from './components';


const routes: Routes = [
    {
        path: '',
        component: FolioComponent
    },
    {
        path: 'start',
        component: FolioStartComponent
    },
    {
        path: 'send',
        component: FolioSendComponent
    },
    {
        path: 'sendAll',
        component: FolioSendAllComponent
    },
    {
        path: 'drafts',
        component: FolioDraftsComponent
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
  })
  export class FolioRoutingModule {}