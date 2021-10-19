import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddlPropertiesComponent } from './addl-properties.component';

describe('AddlPropertiesComponent', () => {
  let component: AddlPropertiesComponent;
  let fixture: ComponentFixture<AddlPropertiesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AddlPropertiesComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AddlPropertiesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
