import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditRecipientComponent } from './edit-recipient.component';

describe('EditRecipientComponent', () => {
  let component: EditRecipientComponent;
  let fixture: ComponentFixture<EditRecipientComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EditRecipientComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(EditRecipientComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
