import { Component, Input, OnInit } from '@angular/core';
import { SmartResumeService } from '@core/services/smart-resume.service';
import { UtilsService } from '@core/services/utils.service';
import { ApiOkResponse } from '@shared/models/apiOkResponse';
import { ClrVM } from '@shared/models/clrVM';
import { take } from 'rxjs/operators';

@Component({
  selector: 'app-smart-resume',
  templateUrl: './smart-resume.component.html',
  styleUrls: ['./smart-resume.component.scss']
})
export class SmartResumeComponent implements OnInit {
  @Input() clr: ClrVM;

  message = '';
  showSpinner = false;
  miniSpinner = false;
  private debug = false;

  constructor(private utilsService: UtilsService, private smartResumeService: SmartResumeService) { }

  ngOnChanges() {
    
    if (this.debug) console.log('SmartResumeComponent ngOnChanges');
  }

  ngOnInit(): void {
    if (this.debug) console.log('SmartResumeComponent ngOnInit');
  }

  submit(){
    this.miniSpinner = true;
    
    this.smartResumeService.submit({ packageId: this.clr.packageId, clrId: this.clr.id })
      .pipe(take(1)).subscribe(data => {
        if (this.debug)
          console.log(data);
        if (data.statusCode == 200) {
          this.clr.smartResumeUrl = (<ApiOkResponse>data).result;
          if (this.clr.smartResumeUrl)
            this.clr.hasSmartResume = true;
        } 
        this.miniSpinner = false;
      });
  }
}
