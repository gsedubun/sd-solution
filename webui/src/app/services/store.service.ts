import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { IStore } from '../domain/models';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class StoreService {
  deleteStore(store: IStore){
    return this.httpClient.delete(this.storeBaseUrl + '/Store/'+store.storeId);
  }
  createStore(value: IStore): Observable<IStore> {
    return this.httpClient.post<IStore>(this.storeBaseUrl + '/Store', value);
  }
  private readonly storeBaseUrl: string = environment.storeUrl;
  constructor(private httpClient : HttpClient) { }

  getStoresForDistrict(districtId: number): Observable<IStore[]> {
    return this.httpClient.get<IStore[]>( this.storeBaseUrl+ '/Store/GetStoresForDistrict/' + districtId);
  }
}
