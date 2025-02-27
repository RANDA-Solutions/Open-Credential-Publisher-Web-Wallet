import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { SourcesService } from '@modules/sources/sources.service';
import { ApiBadRequestResponse } from '@shared/models/apiBadRequestResponse';
import { ApiOkResponse } from '@shared/models/apiOkResponse';
import { SourceVM } from '@shared/models/source';
import { SourceConnectInput } from '@shared/models/sourceConnectInput';
import { take } from 'rxjs/operators';

@Component({
  selector: 'app-register-link',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {
  modelErrors = new Array<string>();
  sources = new Array<SourceVM>();
  sourceId = -1;
  redirectUrl = '';
  userName = -1;
  password = '';
  modelIsValid = true;
  showSpinner = false;
  connectForm: FormGroup;
  private debug = false;

  constructor(private sourcesService: SourcesService, private fb: FormBuilder ) {
    this.createForm();
  }

  ngOnInit() {
    if (this.debug) console.log('register ngOnInit');
    this.showSpinner = true;
    this.getData();
  }

  ngOnDestroy(): void {
  }

  onSubmit():any {
    if (this.debug) console.log('register connect');
    const formModel = this.connectForm.value;
    const input = {
      selectedSource: this.connectForm.get('sourceId').value,
      sourceUrl: this.connectForm.get('sourceUrl').value,
      sourceTypeId: this.connectForm.get('sourceTypeId').value
    } as SourceConnectInput;
    this.sourcesService.connect(input)
    .pipe(take(1)).subscribe(data => {
      console.log(data);
      if (data.statusCode == 200) {
        this.redirectUrl = (<ApiOkResponse>data).result as string;
        window.location.href = this.redirectUrl;
      } else {
        this.modelErrors = (<ApiBadRequestResponse>data).errors;
      }
      this.showSpinner = false;
    });
  }

  createForm() {
    this.connectForm = this.fb.group({
      sourceTypeId: [1],
      sourceId: null as number,
      sourceUrl: '',
    });
  }
  getData():any {
    this.showSpinner = true;
    if (this.debug) console.log('register getData');
    this.sourcesService.getSourceList()
      .pipe(take(1)).subscribe(data => {
        console.log(data);
        if (data.statusCode == 200) {
          this.sources = (<ApiOkResponse>data).result as Array<SourceVM>;
        } else {
          this.sources = new Array<SourceVM>();
          this.modelErrors = (<ApiBadRequestResponse>data).errors;
        }
        this.showSpinner = false;
      });
  }
  selectSource(e) {
    this.connectForm.get('sourceId').setValue(e.target.value, {
      onlySelf: true
    })
  }
}
