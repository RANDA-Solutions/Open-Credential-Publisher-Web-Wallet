import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AppService } from '@core/services/app.service';
import { environment } from '@environment/environment';
import { LinksService } from '@modules/links/links.service';
import { ApiBadRequestResponse } from '@shared/models/apiBadRequestResponse';
import { RecipientModel } from '@shared/models/recipientModel';
import { take } from 'rxjs/operators';

@Component({
  selector: 'app-create-recipient',
  templateUrl: './create-recipient.component.html',
  styleUrls: ['./create-recipient.component.scss']
})
export class CreateRecipientComponent implements OnInit {
  modelErrors = new Array<string>();
  vm = new RecipientModel();
   private debug = false;
  showSpinner = false;
  constructor(private appService: AppService, private linksService: LinksService
    , private route: ActivatedRoute, private router: Router) { }

  ngOnInit(): void {
    if (this.debug) console.log('CreateRecipientComponent ngOnInit');
  }
  save(){
    this.showSpinner = true;
    if (this.debug) console.log('CreateRecipientComponent save');
    this.linksService.createRecipient(this.vm)
      .pipe(take(1)).subscribe(data => {
        if (this.debug) console.log(data);
        if (data.statusCode == 200) {
          this.router.navigate(['/links'])
        } else {
          this.modelErrors = (<ApiBadRequestResponse>data).errors;
        }
        this.showSpinner = false;
      });
  }
}
