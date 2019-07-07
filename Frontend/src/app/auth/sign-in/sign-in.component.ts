import {Component, OnInit} from '@angular/core';
import {AuthService} from '../auth.service';
import {Router} from '@angular/router';

@Component({
  selector: 'app-sign-in',
  templateUrl: './sign-in.component.html',
  styleUrls: ['./sign-in.component.scss']
})
export class SignInComponent implements OnInit
{

  ngOnInit ()
  {
  }

  constructor (public authService : AuthService, private router : Router)
  {
  }

  login = 'bereg.dmit@gmail.com';
  password = '123123';
  message = '';

  send ()
  {
    if (this.login === '' || this.password === '')
    {
      this.message = 'Введите логин и пароль';
      return;
    }

    this.authService.signIn(this.login, this.password).subscribe(response =>
    {
      if (response.success === false)
      {
        this.message = 'Неверный логин или пароль';
        return;
      }

      this.authService.userData = response.data;
      this.authService.isLoggedIn = true;
      localStorage['userData'] = JSON.stringify(this.authService.userData);

      this.router.navigate(['']);
    }, error => alert(error));
  }
}
