import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { IDistrict } from '../domain/models';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class DistrictService {
  addDistrict(value: any) : Observable<IDistrict> {
    return this.http.post<IDistrict>(this.districtUrl+'/district', value);
  }
  private readonly districtUrl: string = environment.districtUrl;
  constructor(private http: HttpClient) { }

  getDistricts() : Observable<IDistrict[]> {
    return this.http.get<IDistrict[]>(this.districtUrl+'/district');
  }
}
