import { RouterModule, Routes } from '@angular/router';
import { RegisterCallbackComponent } from './pages/callback/callback.component';
import { SourceDeleteComponent } from './pages/delete/source-delete.component';
import { SourceDetailComponent } from './pages/details/source-details.component';
import { RegisterComponent } from './pages/register/register.component';
import { SourceListComponent } from './pages/source-list.component';

export const sourceRoutes: Routes = [
    {
        path: 'source-list',
        component: SourceListComponent,

    },
    {
        path: 'register',
        component: RegisterComponent,

    },
    {
        path: 'details/:id',
        component: SourceDetailComponent,

    },
    {
        path: 'callback',
        component: RegisterCallbackComponent,

    },
    {
        path: 'delete/:id',
        component: SourceDeleteComponent,

    },
    {
      path: '', redirectTo: 'source-list', pathMatch: 'full'
    }
];

export const sourceRouter = RouterModule.forChild(sourceRoutes);
