import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { IAddSalesPerson, IAvailableSalesPerson, ISalesPerson } from '../domain/models';

@Injectable({
  providedIn: 'root'
})
export class SalespersonService {
  deleteSalesPersonDistrict(salesPerson:ISalesPerson) : Observable<ISalesPerson>{
    return this.http.delete<ISalesPerson>(this.districtUrl + '/SalesPerson/DeleteSalesPersonDistrict',{ body: salesPerson });
  }
  getAvailableSalesPersons(districtId: number):Observable<IAvailableSalesPerson[]>  {
    return this.http.get<IAvailableSalesPerson[]>( this.districtUrl+ `/District/${districtId}/AvailableSalesPersons/`);
  }
  private readonly districtUrl: string = environment.districtUrl;
  constructor(private http: HttpClient) { }

  getSalespersonsForDistrict(districtId: number): Observable<ISalesPerson[]> {
    return this.http.get<ISalesPerson[]>( this.districtUrl+ '/SalesPerson/GetForDistrict/' + districtId);
  }

  getSalesPersons(): Observable<ISalesPerson[]> {
    return this.http.get<ISalesPerson[]>(this.districtUrl + '/SalesPerson');
  }

  createSalesPerson(salesPerson: ISalesPerson): Observable<ISalesPerson> {
    return this.http.post<ISalesPerson>(this.districtUrl + '/SalesPerson', salesPerson);
  }

  addSalesPersonToDistrict(sales: IAddSalesPerson): Observable<ISalesPerson> {
    return this.http.post<ISalesPerson>(this.districtUrl + `/SalesPerson/addSalesPersonToDistrict`, sales);
  }
}
