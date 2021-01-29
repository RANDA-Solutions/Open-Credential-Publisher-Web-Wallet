import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FolioSendComponent } from './folio-send.component';

describe('FolioSendComponent', () => {
  let component: FolioSendComponent;
  let fixture: ComponentFixture<FolioSendComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FolioSendComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FolioSendComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
