import { RouterModule, Routes } from '@angular/router';
import { DeleteWalletComponent } from './pages/delete/delete.component';
import { EditWalletComponent } from './pages/edit/edit.component';
import { InvitationComponent } from './pages/invitation/invitation.component';
import { SendWalletComponent } from './pages/send/send.component';
import { WalletListComponent } from './pages/wallet-list.component';

export const walletRoutes: Routes = [
  {
      path: 'wallet-list',
      component: WalletListComponent,
  },
  {
    path: 'invitation/:id',
    component: InvitationComponent,
  },
  {
    path: 'edit/:id',
    component: EditWalletComponent,
  },
  {
    path: 'send/:id',
    component: SendWalletComponent,

  },
  {
    path: 'delete/:id',
    component: DeleteWalletComponent,

  },
  {
    path: '', redirectTo: 'wallet-list', pathMatch: 'full'
  }
];

export const walletRouter = RouterModule.forChild(walletRoutes);
