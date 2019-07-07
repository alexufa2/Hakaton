import {Component, OnInit} from '@angular/core';
import {SearchResult} from '../../shared/classes';
import {MainService} from '../main.service';

@Component({
  selector: 'app-stat-address',
  templateUrl: './stat-address.component.html'
})
export class StatAddressComponent implements OnInit
{
  selectedResult : SearchResult = null;
  lastSelectedResults : SearchResult[] = [];
  selectedStatBlock = null;

  constructor ()
  {
  }

  ngOnInit ()
  {
    let lastSelected = localStorage['lastSelectedResults'];
    return;
    if (lastSelected !== undefined)
    {
      this.lastSelectedResults = JSON.parse(lastSelected);
      this.resultSelected(this.lastSelectedResults[0]);
    }
  }

  resultSelected (result : SearchResult)
  {
    this.selectedResult = result;
  }

  statBlockSelected (result : any)
  {
    this.selectedStatBlock = result;
  }
}
