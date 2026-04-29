import { TestBed } from '@angular/core/testing';

import { Wish } from './wish';

describe('Wish', () => {
  let service: Wish;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(Wish);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
