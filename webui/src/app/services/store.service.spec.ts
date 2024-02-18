import { TestBed } from '@angular/core/testing';

import { StoreService } from './store.service';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { IStore } from '../domain/models';
import { environment } from '../../environments/environment';

describe('StoreService', () => {
  let service: StoreService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [ HttpClientTestingModule ],
      providers: [ StoreService ]
    });
    service = TestBed.inject(StoreService);
    httpMock = TestBed.inject(HttpTestingController);

  });

  afterEach(() => {
    httpMock.verify();
  })

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  describe('getStores', () => {
    it('should return an Observable<IStore[]>', () => {
      const dummyStores : IStore[] = [
        { storeId: 1, storeName: 'Store 1', districtId: 1, districtName: 'District 1'},
        { storeId: 2, storeName: 'Store 2', districtId: 2, districtName: 'District 2'},
      ];

      service.getStoresForDistrict(1).subscribe(stores => {
        expect(stores.length).toBe(2);
        expect(stores).toEqual(dummyStores);
      });

      const request = httpMock.expectOne(`${environment.storeUrl}/Store/GetStoresForDistrict/1`);
      expect(request.request.method).toBe('GET');
      request.flush(dummyStores);
    });
  });

  describe('addStore', () => {
    it('should return an Observable<IStore>', () => {
      const dummyStore : IStore = { storeId: 1, storeName: 'Store 1', districtId: 1, districtName: 'District 1'};

      service.createStore(dummyStore).subscribe(store => {
        expect(store).toEqual(dummyStore);
      });

      const request = httpMock.expectOne(`${environment.storeUrl}/Store`);
      expect(request.request.method).toBe('POST');
      request.flush(dummyStore);
    });

  });

  describe('deleteStore', () => {
    it('should return an Observable<IStore>', () => {
      const dummyStore : IStore = { storeId: 1, storeName: 'Store 1', districtId: 1, districtName: 'District 1'};

      service.deleteStore(dummyStore).subscribe(store => {
        expect(store).toEqual(dummyStore);
      });

      const request = httpMock.expectOne(`${environment.storeUrl}/Store/1`);
      expect(request.request.method).toBe('DELETE');
      request.flush(dummyStore);
    });

  });
});
