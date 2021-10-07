import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: '[app-addl-properties]',
  templateUrl: './addl-properties.component.html',
  styleUrls: ['./addl-properties.component.scss']
})
export class AddlPropertiesComponent implements OnInit {
  @Input() additionalProperties: { [index: string]: any };
  @Input() achievementAdditionalProperties: { [index: string]: any };
  @Input() safeAssertionId: string;
  constructor() { }

  ngOnInit(): void {
  }

}
