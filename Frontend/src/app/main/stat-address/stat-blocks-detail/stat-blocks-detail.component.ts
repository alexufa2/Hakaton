import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {MainService} from '../../main.service';

@Component({
  selector: 'app-stat-blocks-detail',
  templateUrl: './stat-blocks-detail.component.html',
  styleUrls: ['./stat-blocks-detail.component.scss']
})
export class StatBlocksDetailComponent implements OnInit
{

  constructor (private service : MainService)
  {
  }

  @Input() selectedResult;
  @Input() selectedStatBlock;
  @Output() back = new EventEmitter<any>();

  info : any[] = [];

  ngOnInit ()
  {
    this.getData();
  }

  getData ()
  {
    /*this.service.getAddressInfo(this.selectedResult.id, this.selectedStatBlock.id).subscribe(response =>
    {
      this.info = response.data;
    })*/
  }
}
