import { Component, Input, OnInit } from '@angular/core';
import { ClrDetailService } from '@core/services/clrdetail.service';
import { environment } from '@environment/environment';
import { AssociationTypeEnum } from '@shared/models/clrLibrary/enums/associationTypeEnum';
import { AssociationVM } from '@shared/models/clrSimplified/associationVM';

@Component({
  selector: 'app-association',
  templateUrl: './association.component.html',
  styleUrls: ['./association.component.scss']
})
export class AssociationComponent implements OnInit {
  @Input() clrId: number;
  @Input() association: AssociationVM;
  @Input() ancestors: string;
  @Input() ancestorKeys: string;
  showSpinner = false;
  public assocVM = new AssociationVM();
  private debug = false;
  constructor(private clrDetailService: ClrDetailService) { }

  ngOnChanges() {
    this.showSpinner = true;
    if (this.debug) console.log('AssociationComponent ngOnChanges');
    // this.getData();
  }

  ngOnInit(): void {
    if (this.debug) console.log('AssociationComponent ngOnInit');
  }

  public get AssociationTypeEnum() {
    return AssociationTypeEnum;
  }
  // getData():any {
  //   this.showSpinner = true;
  //   if (this.debug) console.log('AssociationComponent getData');
  //   this.clrDetailService.getAssociationVM(this.clrId, this.association.targetId)
  //     .pipe(take(1)).subscribe(data => {
  //       if (data.statusCode == 200) {
  //         this.assocVM = (<ApiOkResponse>data).result as AssociationVM;
  //       } else {
  //         this.assocVM = new AssociationVM();
  //       }
  //       this.showSpinner = false;
  //     });
  // }

  // public get associationTypeEnum() {
  //   return AssociationTypeEnum;
  // }
  // public associationTypeText(value: AssociationTypeEnum) {
  //   return AssociationTypeEnum[value];
  // }
}
