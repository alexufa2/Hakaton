import {Injectable} from '@angular/core';
import {Observable, of} from 'rxjs';
import {map, catchError, tap} from 'rxjs/operators';
import {BaseService} from '../shared/base.service';
import {ResponseInterface} from '../shared/classes';

@Injectable({
  providedIn: 'root'
})
export class AuthService extends BaseService
{
  isLoggedIn = null;
  userData = null;
  queryxrf = null;

  checkAuth () : Observable<any>
  {
    return of(false);

    //return this.http.get<ResponseInterface>('/status')
      //.pipe(map(response => {this.isLoggedIn = response.data.logged as boolean; return this.isLoggedIn}));
  }

  signIn (Email : string, Password : string) : Observable<any>
  {
    return this.http.post<ResponseInterface>('users/authenticate', JSON.stringify({Email, Password}));
  }

  signUp (Email : string, Password : string, Name = 'Dmitry', Surname = 'Berezhnov') : Observable<any>
  {
    return this.http.post<ResponseInterface>('users/register', JSON.stringify({Id : 151, Email, Password, Name, Surname}));
  }

  getQueryToken ()
  {
    return this.http.get<ResponseInterface>('users/queryxrf');
  }

  check () : Observable<any>
  {
    return this.http.get('users/confirmbykey?key=333')
  }

  logout ()
  {
    delete localStorage['userDate'];
    this.userData = null;
    this.isLoggedIn = false;
  }
}
