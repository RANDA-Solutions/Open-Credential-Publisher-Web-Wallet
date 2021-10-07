import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AssertionPopupComponent } from './assertion-popup';

describe('OpenBadgesComponent', () => {
  let component: AssertionPopupComponent;
  let fixture: ComponentFixture<AssertionPopupComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AssertionPopupComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AssertionPopupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
