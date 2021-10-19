import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ClrSectionAssertionsComponent } from './clr-section-assertions.component';

describe('ClrSectionAssertionsComponent', () => {
  let component: ClrSectionAssertionsComponent;
  let fixture: ComponentFixture<ClrSectionAssertionsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ClrSectionAssertionsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ClrSectionAssertionsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
