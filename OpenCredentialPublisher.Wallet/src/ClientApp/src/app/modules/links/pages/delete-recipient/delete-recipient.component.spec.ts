import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DeleteRecipientComponent } from './delete-recipient.component';

describe('DeleteRecipientComponent', () => {
  let component: DeleteRecipientComponent;
  let fixture: ComponentFixture<DeleteRecipientComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DeleteRecipientComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DeleteRecipientComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
