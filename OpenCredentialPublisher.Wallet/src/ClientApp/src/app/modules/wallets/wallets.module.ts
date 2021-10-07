import { ModuleWithProviders, NgModule } from '@angular/core';
import { LogService } from '@core/error-handling/logerror.service';
import { SharedModule } from '@shared/shared.module';
import { DeleteWalletComponent } from './pages/delete/delete.component';
import { EditWalletComponent } from './pages/edit/edit.component';
import { InvitationComponent } from './pages/invitation/invitation.component';
import { SendWalletComponent } from './pages/send/send.component';
import { WalletListComponent } from './pages/wallet-list.component';
import { walletRouter } from './wallets.router';
import { WalletService } from './wallets.service';
@NgModule({
    imports: [
      walletRouter,
      SharedModule
    ],
    declarations: [
      WalletListComponent,
      InvitationComponent,
      DeleteWalletComponent,
      EditWalletComponent,
      SendWalletComponent
      ],
      providers: [ WalletService, LogService ],
      entryComponents: [
      ]
  })
export class WalletsModule {
  static forRoot(): ModuleWithProviders<WalletsModule> {
      return {
        ngModule: WalletsModule
      };
    }
  }
