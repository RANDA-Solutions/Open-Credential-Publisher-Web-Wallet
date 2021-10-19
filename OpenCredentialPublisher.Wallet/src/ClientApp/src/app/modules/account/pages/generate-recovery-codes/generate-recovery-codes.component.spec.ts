import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GenerateRecoveryCodesComponent } from './generate-recovery-codes.component';

describe('GenerateRecoveryCodesComponent', () => {
  let component: GenerateRecoveryCodesComponent;
  let fixture: ComponentFixture<GenerateRecoveryCodesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ GenerateRecoveryCodesComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(GenerateRecoveryCodesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
