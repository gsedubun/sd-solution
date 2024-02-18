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
    return this.http.delete<ISalesPerson>(this.districtUrl + '/salesperson/DeleteSalesPersonDistrict',{ body: salesPerson });
  }
  getAvailableSalesPersons(districtId: number):Observable<IAvailableSalesPerson[]>  {
    return this.http.get<IAvailableSalesPerson[]>( this.districtUrl+ `/District/${districtId}/AvailableSalesPersons/`);
  }
  private readonly districtUrl: string = environment.districtUrl;
  constructor(private http: HttpClient) { }

  getSalespersonsForDistrict(districtId: number): Observable<ISalesPerson[]> {
    return this.http.get<ISalesPerson[]>( this.districtUrl+ '/salesperson/GetForDistrict/' + districtId);
  }

  getSalesPersons(): Observable<IAvailableSalesPerson[]> {
    return this.http.get<IAvailableSalesPerson[]>(this.districtUrl + '/salesperson');
  }

  createSalesPerson(salesPerson: ISalesPerson): Observable<ISalesPerson> {
    return this.http.post<ISalesPerson>(this.districtUrl + '/salesperson', salesPerson);
  }

  addSalesPersonToDistrict(sales: IAddSalesPerson): Observable<IAddSalesPerson> {
    return this.http.post<IAddSalesPerson>(this.districtUrl + `/salesperson/addSalesPersonToDistrict`, sales);
  }
}
