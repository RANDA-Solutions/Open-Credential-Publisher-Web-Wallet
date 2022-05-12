import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ConfirmEmailChangeComponent } from './confirm-email-change.component';

describe('ConfirmEmailChangeComponent', () => {
  let component: ConfirmEmailChangeComponent;
  let fixture: ComponentFixture<ConfirmEmailChangeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ConfirmEmailChangeComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ConfirmEmailChangeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
