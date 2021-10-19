import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LinkSummaryComponent } from './link-summarycomponent';

describe('LinkSummaryComponent', () => {
  let component: LinkSummaryComponent;
  let fixture: ComponentFixture<LinkSummaryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ LinkSummaryComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(LinkSummaryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
