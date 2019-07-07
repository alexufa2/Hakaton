import {Component} from '@angular/core';
import {AuthService} from './auth/auth.service';

@Component({
  selector: 'app-root',
  template: '<router-outlet></router-outlet>'
})
export class AppComponent
{
  constructor (public authService : AuthService)
  {
    let userData = localStorage.getItem('userData');
    if (userData !== null)
    {
      userData = JSON.parse(userData);
      this.authService.userData = userData;
      this.authService.isLoggedIn = true;
      console.log(this.authService.userData);

      this.authService.getQueryToken().subscribe(response =>
      {
        console.log(response);
      });
    }
  }
}
