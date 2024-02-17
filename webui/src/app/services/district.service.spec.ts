import { TestBed } from '@angular/core/testing';

import { DistrictService } from './district.service';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { HttpClientModule } from '@angular/common/http';
import { IDistrict } from '../domain/models';
import { environment } from '../../environments/environment';

describe('DistrictService', () => {
  let service: DistrictService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [ HttpClientTestingModule ],
      providers: [ DistrictService ]
    });
    service = TestBed.inject(DistrictService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  })


  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  describe('getDistricts', () => {
    it('should return an Observable<IDistrict[]>', () => {
      const dummyDistricts: IDistrict[] = [
        { districtId: 1, districtName: 'A', primarySalesFullName: 'A', primarySalesId: 1},
        { districtId: 2, districtName: 'B', primarySalesFullName: 'B', primarySalesId: 2},
      ];

      service.getDistricts().subscribe(districts => {
        expect(districts.length).toBe(2);
        expect(districts).toEqual(dummyDistricts);
      });

      const request = httpMock.expectOne(`${environment.districtUrl}/district`);
      expect(request.request.method).toBe('GET');
      request.flush(dummyDistricts);
    });
  });

  describe('addDistrict', () => {
    it('should return an Observable<IDistrict>', () => {
      const dummyDistrict: IDistrict = { districtId: 1, districtName: 'A', primarySalesFullName: 'A', primarySalesId: 1};

      service.addDistrict(dummyDistrict).subscribe(district => {
        expect(district).toEqual(dummyDistrict);
      });

      const request = httpMock.expectOne(`${environment.districtUrl}/district`);
      expect(request.request.method).toBe('POST');
      request.flush(dummyDistrict);
    });
  });

});
