import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LoginWithRecoveryComponent } from './login-with-recovery.component';

describe('LoginWithRecoveryComponent', () => {
  let component: LoginWithRecoveryComponent;
  let fixture: ComponentFixture<LoginWithRecoveryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ LoginWithRecoveryComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(LoginWithRecoveryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
