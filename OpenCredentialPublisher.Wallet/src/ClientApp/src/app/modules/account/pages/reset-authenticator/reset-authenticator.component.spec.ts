import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ResetAuthenticatorComponent } from './reset-authenticator.component';

describe('ResetAuthenticatorComponent', () => {
  let component: ResetAuthenticatorComponent;
  let fixture: ComponentFixture<ResetAuthenticatorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ResetAuthenticatorComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ResetAuthenticatorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
