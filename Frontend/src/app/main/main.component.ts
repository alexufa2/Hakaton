import {Component, OnInit} from '@angular/core';
import {AuthService} from '../auth/auth.service';

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.scss']
})
export class MainComponent implements OnInit
{

  constructor (public authService : AuthService)
  {
  }

  mode = 'STAT_ADDRESS';
  vertificationDrawerOpened = false;

  ngOnInit ()
  {
  }

}
