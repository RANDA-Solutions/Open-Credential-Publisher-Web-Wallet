import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EnableAuthenticatorComponent } from './enable-authenticator.component';

describe('EnableAuthenticatorComponent', () => {
  let component: EnableAuthenticatorComponent;
  let fixture: ComponentFixture<EnableAuthenticatorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EnableAuthenticatorComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(EnableAuthenticatorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
