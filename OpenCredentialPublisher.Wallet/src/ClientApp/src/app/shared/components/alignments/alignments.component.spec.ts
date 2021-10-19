import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AlignmentsComponent } from './alignments.component';

describe('AlignmentsComponent', () => {
  let component: AlignmentsComponent;
  let fixture: ComponentFixture<AlignmentsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AlignmentsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AlignmentsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
