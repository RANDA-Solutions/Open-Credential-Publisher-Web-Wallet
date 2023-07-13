import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AppService } from '@core/services/app.service';
import { LinksService } from '@modules/links/links.service';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { ApiBadRequestResponse } from '@shared/models/apiBadRequestResponse';
import { RecipientModel } from '@shared/models/recipientModel';
import { take } from 'rxjs/operators';

@UntilDestroy()
@Component({
  selector: 'app-create-recipient',
  templateUrl: './create-recipient.component.html',
  styleUrls: ['./create-recipient.component.scss']
})
export class CreateRecipientComponent implements OnInit {
  modelErrors = new Array<string>();
  vm = new RecipientModel();
  linkId: string = null;
  private sub: any;
  private debug = true;
  showSpinner = false;
  constructor(private appService: AppService, private linksService: LinksService
    , private route: ActivatedRoute, private router: Router) { }

  ngOnInit(): void {
    if (this.debug) console.log('CreateRecipientComponent ngOnInit');

    this.sub = this.route.queryParams.pipe(untilDestroyed(this)).subscribe(params => {
      this.linkId = params['linkId'];
   });
  }
  save(){
    this.showSpinner = true;
    if (this.debug) console.log('CreateRecipientComponent save');
    this.linksService.createRecipient(this.vm)
      .pipe(take(1)).subscribe(data => {
        if (this.debug) console.log(data);
        if (data.statusCode == 200) {
          if (this.linkId) {
            this.router.navigate(['/links/share', this.linkId], { queryParams: { infoMessage: "New recipient created." }});
          }
          else {
            this.router.navigate(['/recipients'], { queryParams: { infoMessage: "New recipient created." }});
          }
        } else {
          this.modelErrors = (<ApiBadRequestResponse>data).errors;
        }
        this.showSpinner = false;
      }, (error: HttpErrorResponse) => {
        console.log(error);
      });
  }
}
