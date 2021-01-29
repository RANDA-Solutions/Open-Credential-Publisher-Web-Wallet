import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PortfolioVerificationModalComponent } from './portfolio-verification-modal.component';

describe('PortfolioVerificationModalComponent', () => {
  let component: PortfolioVerificationModalComponent;
  let fixture: ComponentFixture<PortfolioVerificationModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PortfolioVerificationModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PortfolioVerificationModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
