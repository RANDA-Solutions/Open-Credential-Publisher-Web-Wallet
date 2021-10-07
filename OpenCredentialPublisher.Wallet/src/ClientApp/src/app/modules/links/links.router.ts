import { RouterModule, Routes } from '@angular/router';
import { CreateRecipientComponent } from './pages/create-recipient/create-recipient.component';
import { CreateLinkComponent } from './pages/create/create-link.component';
import { DeleteRecipientComponent } from './pages/delete-recipient/delete-recipient.component';
import { LinkDeleteComponent } from './pages/delete/link-delete.component';
import { EditRecipientComponent } from './pages/edit-recipient/edit-recipient.component';
import { LinkListComponent } from './pages/link-list/link-list.component';
import { RecipientListComponent } from './pages/recipient-list/recipient-list.component';
import { ShareLinkComponent } from './pages/share-link/share-link.component';

export const linkRoutes: Routes = [
  {
      path: '',
      component: LinkListComponent,
  },
  {
    path: 'create',
    component: CreateLinkComponent,
  },
  {
    path: 'delete/:id',
    component: LinkDeleteComponent,
  },
  {
    path: 'share/:id',
    component: ShareLinkComponent,
  },
  {
    path: 'recipients',
    component: RecipientListComponent,
  },
  {
    path: 'recipients/create',
    component: CreateRecipientComponent,
  },
  {
    path: 'recipients/delete/:id',
    component: DeleteRecipientComponent,
  },
  {
    path: 'recipients/edit/:id',
    component: EditRecipientComponent,
  }
];

export const linkRouter = RouterModule.forChild(linkRoutes);
