import { TestBed } from '@angular/core/testing';

import { SalespersonService } from './salesperson.service';
import { HttpClientModule } from '@angular/common/http';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { environment } from '../../environments/environment';
import { IAddSalesPerson, ISalesPerson } from '../domain/models';

describe('SalespersonService', () => {
  let service: SalespersonService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [ HttpClientTestingModule ],
      providers: [ SalespersonService ]
    });
    service = TestBed.inject(SalespersonService);
    httpMock = TestBed.inject(HttpTestingController);

  });

  afterEach(() => {
    httpMock.verify();
  })

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  describe('getSalesPersons', () => {
    it('should return an Observable<IAvailableSalesPerson[]>', () => {
      const dummySalesPersons = [
        { salesPersonId: 1, fullName: 'Sales 1', districtId: 1},
        { salesPersonId: 2, fullName: 'Sales 2', districtId: 2},
      ];

      service.getSalesPersons().subscribe(salesPersons => {
        expect(salesPersons.length).toBe(2);
        expect(salesPersons).toEqual(dummySalesPersons);
      });

      const request = httpMock.expectOne(`${environment.districtUrl}/salesperson`);
      expect(request.request.method).toBe('GET');
      request.flush(dummySalesPersons);
    });
  });

  describe('addSalesPerson', () => {
    it('should return an Observable<IAvailableSalesPerson>', () => {
      const dummySalesPerson = { salesPersonId: 1, fullName: 'Sales 1', salesType: 'Primary'} as ISalesPerson;

      service.createSalesPerson(dummySalesPerson).subscribe(salesPerson => {
        expect(salesPerson).toEqual(dummySalesPerson);
      });

      const request = httpMock.expectOne(`${environment.districtUrl}/salesperson`);
      expect(request.request.method).toBe('POST');
      request.flush(dummySalesPerson);
    });
  });

  describe('getSalespersonsForDistrict', () => {
    it('should return an Observable<ISalesPerson[]>', () => {
      const dummySalesPersons : ISalesPerson[] = [
        { salesPersonId: 1, fullName: 'Sales 1', salesType: 'Primary'},
        { salesPersonId: 2, fullName: 'Sales 2', salesType: 'Secondary'},
      ];

      service.getSalespersonsForDistrict(1).subscribe(salesPersons => {
        expect(salesPersons.length).toBe(2);
        expect(salesPersons).toEqual(dummySalesPersons);
      });

      const request = httpMock.expectOne(`${environment.districtUrl}/salesperson/GetForDistrict/1`);
      expect(request.request.method).toBe('GET');
      request.flush(dummySalesPersons);
    });
  });

  describe('deleteSalesPersonDistrict', () => {
    it('should return an Observable<ISalesPerson>', () => {
      const dummySalesPerson = { salesPersonId: 1, fullName: 'Sales 1', salesType: 'Primary'} as ISalesPerson;

      service.deleteSalesPersonDistrict(dummySalesPerson).subscribe(salesPerson => {
        expect(salesPerson).toEqual(dummySalesPerson);
      });

      const request = httpMock.expectOne(`${environment.districtUrl}/salesperson/DeleteSalesPersonDistrict`);
      expect(request.request.method).toBe('DELETE');
      request.flush(dummySalesPerson);
    });
  });

  describe('addSalesPersonToDistrict', () => {
    it('should return an Observable<ISalesPerson>', () => {
      const dummySalesPerson = { salesPersonId: 1, districtId: 1, salesType: 'Primary'} as IAddSalesPerson;

      service.addSalesPersonToDistrict(dummySalesPerson).subscribe(salesPerson => {
        expect(salesPerson).toEqual(dummySalesPerson);
      });

      const request = httpMock.expectOne(`${environment.districtUrl}/salesperson/addSalesPersonToDistrict`);
      expect(request.request.method).toBe('POST');
      request.flush(dummySalesPerson);
    });
  });

});
