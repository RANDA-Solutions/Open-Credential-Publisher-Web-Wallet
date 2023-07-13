import { Component, NgZone, OnDestroy, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { CodeService } from "@modules/access/services/code.service";
import { LoginService } from "@root/app/auth/login.service";

@Component({
  selector: 'app-code-waiting',
  templateUrl: './code-waiting.component.html',
  styleUrls: ['./code-waiting.component.scss']
})
export class CodeWaitingComponent implements OnInit, OnDestroy {
  constructor(private codeService: CodeService, private loginService: LoginService, private zone: NgZone, private router: Router) {}

  private timeout: any;

  ngOnInit(): void {
    let self = this;
    this.timeout = setInterval(() => {
      self.zone.run(() => {
        this.codeService.verifyCode().subscribe(response => {
            if (response.invalid) {
                this.router.navigate(['/access/code/invalid']);
            }
            else if (response.expired) {
              this.router.navigate(['/access/code/expired']);
            }
            else if (response.claimed) {
              this.loginService.completeLogin().then(resp => {
                if (resp) {
                  this.router.navigate([response.returnUrl]);
                }
              });

            }
        }, error => { },
        () => {

        });
      }, self);
    }, 2500);
  }

  ngOnDestroy(): void {
    if (this.timeout) {
        clearInterval(this.timeout);
    }
  }
}
