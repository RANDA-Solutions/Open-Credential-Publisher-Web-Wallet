import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { environment } from '@environment/environment';
import { TwoFactorAuthenticationModelInput } from '@shared/models/TwoFactorAuthModelInput';
import { TwoFactorEnableAuthenticationVM } from '@shared/models/twoFactorEnableAuthenticationVM';

@Component({
  selector: 'app-generate-recovery-codes',
  templateUrl: './generate-recovery-codes.component.html',
  styleUrls: ['./generate-recovery-codes.component.scss']
})
export class GenerateRecoveryCodesComponent implements OnInit {
  vm = new TwoFactorEnableAuthenticationVM();
  input = new TwoFactorAuthenticationModelInput();
  userId: string;
  isError = false;
  message = 'loading two factor authentication';
  showSpinner = false;
  submitted = false;
  private debug = false;

  constructor(private route: ActivatedRoute, private router: Router) {
  }

  ngOnInit(): void {
  }

  generateCodes() {
    this.router.navigate(['/account/manage/show-recovery-codes']);
  }
}
