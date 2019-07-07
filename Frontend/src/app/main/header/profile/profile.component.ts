import {Component, EventEmitter, OnInit, Output} from '@angular/core';
import {AuthService} from '../../../auth/auth.service';
import {Router} from '@angular/router';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit
{

  constructor (public authService : AuthService, private router : Router)
  {
  }

  @Output() openVerificationDrawer = new EventEmitter<any>();

  ngOnInit ()
  {
  }

  logout ()
  {
    this.authService.logout();
    this.router.navigate(['/auth/signIn']);
  }
}
