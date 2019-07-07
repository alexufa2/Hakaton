import {Injectable} from '@angular/core';
import {Observable} from 'rxjs';
import {ResponseInterface} from '../shared/classes';
import {BaseService} from '../shared/base.service';
import {of} from 'rxjs/internal/observable/of';

@Injectable({
  providedIn: 'root'
})
export class MainService extends BaseService
{
  getStatBlocks (resultId : string) : Observable<ResponseInterface>
  {
    return of({data: [{name : '1'}]} as ResponseInterface);
    return this.http.get<ResponseInterface>(`search/${resultId}`);
  }

  getAddressInfo (addressId : string) : Observable<ResponseInterface>
  {
    return this.http.get<ResponseInterface>(`AddressInfo/get/${addressId}`);
  }
}
