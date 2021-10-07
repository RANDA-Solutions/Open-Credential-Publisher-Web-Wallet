import { RouterModule, Routes } from '@angular/router';
import { ConnectCredentialComponent } from './pages/connect/connect-credential.component';
import { CreateCollectionComponent } from './pages/create/create-collection.component';
import { CredentialListComponent } from './pages/credential-list.component';
import { DeleteCredentialComponent } from './pages/delete/delete-credential.component';
import { DisplayCredentialComponent } from './pages/display/display-credential.component';
import { ConnectCredentialResolver } from './services/connect-credential-resolver.service';

export const credentialRoutes: Routes = [
    {
        path: '',
        component: CredentialListComponent
    },
    {
      path: 'create',
      component: CreateCollectionComponent
  },
    {
      path: 'display/:id',
      component: DisplayCredentialComponent
  },
  {
    path: 'delete/:id',
    component: DeleteCredentialComponent
},
  {
    path: 'connect',
    component: ConnectCredentialComponent,
    resolve: {
      model: ConnectCredentialResolver
    }
  }

];

export const credentialRouter = RouterModule.forChild(credentialRoutes);
