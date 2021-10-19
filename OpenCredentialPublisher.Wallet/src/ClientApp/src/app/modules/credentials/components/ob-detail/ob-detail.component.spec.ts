import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OBDetailComponent } from './ob-detail.component';

describe('OBDetailComponent', () => {
  let component: OBDetailComponent;
  let fixture: ComponentFixture<OBDetailComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ OBDetailComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(OBDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
