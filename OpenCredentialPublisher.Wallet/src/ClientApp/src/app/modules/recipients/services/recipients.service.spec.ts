import { TestBed } from '@angular/core/testing';

import { RecipientsService } from './recipients.service';

describe('RecipientsService', () => {
  let service: RecipientsService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(RecipientsService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
