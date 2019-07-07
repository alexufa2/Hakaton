import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {MainService} from '../../main.service';
import {SearchResult} from '../../../shared/classes';

declare var VK;

@Component({
  selector: 'app-stat-blocks',
  templateUrl: './stat-blocks.component.html',
  styleUrls: ['./stat-blocks.component.scss']
})
export class StatBlocksComponent implements OnInit
{

  constructor (private service : MainService)
  {
  }

  @Input() statBlocks = [];
  @Input() selectedResult : SearchResult;
  @Output() statBlockSelected = new EventEmitter<any>();
  @Output() back = new EventEmitter<any>();

  status = 'LOADING';

  ngOnInit ()
  {
    this.getStatBlocks();
  }

  getStatBlocks ()
  {
    this.service.getAddressInfo(this.selectedResult.id).subscribe(response =>
    {
      this.statBlocks = response.data.map(item => JSON.parse(item));
      this.statBlocks.push({real:false, ServiceName : 'Сведения о сдаче жилых домов за 2018г.', NameValueData : [{Name : 'Объект', Value : '10-этажное здание'}, {Name : 'Сдан', Value : '20 апреля 2018г'}]});
      console.log(this.statBlocks[0]);
      this.status = 'INITIAL';
    });
  }

}
