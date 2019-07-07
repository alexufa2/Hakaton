import {Component, EventEmitter, OnInit, Output} from '@angular/core';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.scss']
})
export class SidebarComponent implements OnInit
{

  constructor ()
  {
  }

  @Output() setMode = new EventEmitter<string>();

  ngOnInit ()
  {
  }

}
