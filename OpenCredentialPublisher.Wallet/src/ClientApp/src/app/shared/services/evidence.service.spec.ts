import { TestBed } from '@angular/core/testing';

import { EvidenceService } from './evidence.service';

describe('EvidenceService', () => {
  let service: EvidenceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(EvidenceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
