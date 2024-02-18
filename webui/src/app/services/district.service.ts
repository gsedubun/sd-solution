import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { IAddDistrict, IDistrict, IEditDistrict } from '../domain/models';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class DistrictService {
  updateDistrict(value: IEditDistrict) {
    return this.http.put(this.districtUrl+'/district/'+value.districtId, value);
  }
  deleteDistrict(districtId: number) {
    return this.http.delete(this.districtUrl+'/district/'+districtId);
  }
  addDistrict(value: IAddDistrict) : Observable<IDistrict> {
    return this.http.post<IDistrict>(this.districtUrl+'/district', value);
  }
  private readonly districtUrl: string = environment.districtUrl;
  constructor(private http: HttpClient) { }

  getDistricts() : Observable<IDistrict[]> {
    return this.http.get<IDistrict[]>(this.districtUrl+'/district');
  }
}
