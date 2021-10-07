import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ClrSectionComponent } from './clr-section.component';

describe('ClrSectionComponent', () => {
  let component: ClrSectionComponent;
  let fixture: ComponentFixture<ClrSectionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ClrSectionComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ClrSectionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
