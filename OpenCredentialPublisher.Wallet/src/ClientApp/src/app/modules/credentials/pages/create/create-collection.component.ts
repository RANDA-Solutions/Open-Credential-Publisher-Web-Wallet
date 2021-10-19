import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CredentialService } from '@core/services/credentials.service';
import { CredentialFilterService } from '@modules/credentials/services/credentialFilter.service';
import { ApiBadRequestResponse } from '@shared/models/apiBadRequestResponse';
import { ApiOkResponse } from '@shared/models/apiOkResponse';
import { ClrCollectionVM } from '@shared/models/clrCollectionVM';
import { ClrVM } from '@shared/models/clrVM';
import { take } from 'rxjs/operators';

@Component({
  selector: 'app-create-collection-component',
  templateUrl: './create-collection.component.html'
})
export class CreateCollectionComponent  implements OnInit{
  redirectUrl = '';
  clrCollection = new ClrCollectionVM();
  showSpinner = false;
  showCreatingSpinner = false;
  modelErrors = new Array<string>();

  constructor(private route: ActivatedRoute, private router: Router, private credentialService: CredentialService, private credentialFilterService: CredentialFilterService) {
  }
  ngOnInit() {
    this.showSpinner = true;
    this.getData();
  }
  create() {
    this.showCreatingSpinner = true;
    this.credentialService.create(this.clrCollection)
    .pipe(take(1)).subscribe(data => {
      console.log(data);
      if (data.statusCode == 200) {
        this.credentialFilterService.setFilter("collection");
        this.router.navigate(['/credentials']);
      } else {
        this.modelErrors = (<ApiBadRequestResponse>data).errors;
      }
      this.showCreatingSpinner = false;
    }, (error) => {
      this.showCreatingSpinner = false;
    });
  }
  selectionChange(clr: ClrVM){
    clr.isSelected = !clr.isSelected;
  }
  getData():any {
    this.showSpinner = true;
    this.credentialService.getAllClrs()
      .pipe(take(1)).subscribe(data => {
        console.log(data);
        if (data.statusCode == 200) {
          this.clrCollection = (<ApiOkResponse>data).result as ClrCollectionVM;
        } else {
          this.clrCollection = new ClrCollectionVM();
          this.modelErrors = (<ApiBadRequestResponse>data).errors;
        }
        this.showSpinner = false;
      });
  }
}
