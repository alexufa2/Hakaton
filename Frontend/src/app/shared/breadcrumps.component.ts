import {Component, OnInit, Input} from '@angular/core';
import {ReclameCabinet} from './classes';
import {Router} from "@angular/router";

@Component({
  selector: 'app-breadcrumps',
  template: `
    <div>
      <a routerLink="['/accounts']">Все аккаунты</a>
      <i class="crump-separator material-icons">keyboard_arrow_right</i>
      <span>«{{item?.login}}»</span>
    </div>
  `,
  styles: []
})
export class BreadcrumpsComponent implements OnInit
{

  constructor (private router : Router) {}

  @Input() item : ReclameCabinet;

  ngOnInit ()
  {
  }

}
