import {Injectable} from '@angular/core';
import {Observable} from 'rxjs';
import {ResponseInterface} from '../../../shared/classes';
import {BaseService} from '../../../shared/base.service';
import {of} from 'rxjs/internal/observable/of';

@Injectable({
  providedIn: 'root'
})
export class SearchService extends BaseService
{
  search (searchText : string) : Observable<ResponseInterface>
  {
    searchText = searchText.toLowerCase().trim();
    if (searchText === '')
    {
      return of({} as ResponseInterface);
    }

    return this.http.post<ResponseInterface>(`AddressInfo/filter`, JSON.stringify({SearchString : "Проспект"}));
  }
}
