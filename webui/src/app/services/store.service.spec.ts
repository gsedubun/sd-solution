import { TestBed } from '@angular/core/testing';

import { StoreService } from './store.service';
import { HttpClientTestingModule } from '@angular/common/http/testing';

describe('StoreService', () => {
  let service: StoreService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [ HttpClientTestingModule ],
      providers: [ StoreService ]
    });
    service = TestBed.inject(StoreService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
