import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ClrProfileComponent } from './clr-profile.component';

describe('ClrProfileComponent', () => {
  let component: ClrProfileComponent;
  let fixture: ComponentFixture<ClrProfileComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ClrProfileComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ClrProfileComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
