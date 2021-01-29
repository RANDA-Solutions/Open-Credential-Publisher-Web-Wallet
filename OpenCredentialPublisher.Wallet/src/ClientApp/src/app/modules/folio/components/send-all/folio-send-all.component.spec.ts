import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FolioSendAllComponent } from './folio-send-all.component';

describe('FolioSendAllComponent', () => {
  let component: FolioSendAllComponent;
  let fixture: ComponentFixture<FolioSendAllComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FolioSendAllComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FolioSendAllComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
