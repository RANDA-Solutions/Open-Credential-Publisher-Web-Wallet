import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LoginWith2faComponent } from './login-with2fa.component';

describe('LoginWith2faComponent', () => {
  let component: LoginWith2faComponent;
  let fixture: ComponentFixture<LoginWith2faComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ LoginWith2faComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(LoginWith2faComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
