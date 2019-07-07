import {Component, OnInit} from '@angular/core';
import {AuthService} from '../auth.service';
import {Router} from '@angular/router';

@Component({
  selector: 'app-sign-up',
  templateUrl: './sign-up.component.html',
  styleUrls: ['./sign-up.component.scss']
})
export class SignUpComponent implements OnInit
{

  ngOnInit ()
  {
    this.authService.check().subscribe(response => console.log('response'));
  }

  constructor (public authService : AuthService, private router : Router)
  {
  }

  name = '';
  surname = '';
  thirdname = '';
  email = 'bereg.dmit@gmail.com';
  password = '123123';
  message = '';

  register ()
  {
    if (this.email === '' || this.password === '')
    {
      this.message = 'Введите email и пароль';
      return;
    }

    this.authService.signUp(this.email, this.password).subscribe(response =>
    {
      console.log(response);
      return;
      if (response.success === false)
      {
        this.message = 'Неверный логин или пароль';
        return;
      }
      this.authService.userData = response.data;
      this.authService.isLoggedIn = true;
      this.router.navigate(['']);
    });
  }
}
