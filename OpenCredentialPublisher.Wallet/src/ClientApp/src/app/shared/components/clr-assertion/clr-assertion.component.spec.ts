import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ClrAssertionComponent } from './clr-assertion.component';

describe('ClrAssertionComponent', () => {
  let component: ClrAssertionComponent;
  let fixture: ComponentFixture<ClrAssertionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ClrAssertionComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ClrAssertionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
