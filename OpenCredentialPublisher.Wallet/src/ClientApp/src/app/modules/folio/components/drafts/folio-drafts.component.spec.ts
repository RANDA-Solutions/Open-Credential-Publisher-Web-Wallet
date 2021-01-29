import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FolioDraftsComponent } from './folio-drafts.component';

describe('FolioDraftsComponent', () => {
  let component: FolioDraftsComponent;
  let fixture: ComponentFixture<FolioDraftsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FolioDraftsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FolioDraftsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
