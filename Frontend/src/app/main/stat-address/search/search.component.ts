import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {FormControl} from "@angular/forms";
import {SearchResult} from "../../../shared/classes";
import {Subject} from "rxjs/internal/Subject";
import {debounceTime, distinctUntilChanged, finalize, switchMap, tap} from "rxjs/operators";
import {SearchService} from "./search.service";

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.scss']
})
export class SearchComponent implements OnInit
{

  constructor (private service : SearchService)
  {
  }

  @Output() resultSelected = new EventEmitter<SearchResult>();
  @Output() lastSelectedResultsChanged = new EventEmitter<SearchResult[]>();

  status = 'INITIAL';
  searchControl = new FormControl('');

  searchResults : Subject<SearchResult[]> = new Subject<SearchResult[]>();
  @Input() lastSelectedResults : SearchResult[] = [];

  ngOnInit ()
  {
    this.searchControl.valueChanges.pipe(debounceTime(100), distinctUntilChanged(), tap(() => this.status = 'SEARCHING'),
      switchMap(val => this.service.search(val).pipe(finalize(() => this.status = 'INITIAL')))).subscribe(response =>
    {
      this.searchResults.next(response.data);
    }, err => console.log(err));

  }

  select (selectedResult : SearchResult)
  {
    this.searchControl.patchValue('');
    let lastSelected = localStorage['lastSelectedResults'];
    if (lastSelected == null)
    {
      lastSelected = [selectedResult];
      localStorage['lastSelectedResults'] = JSON.stringify(lastSelected);
    }
    else
    {
      lastSelected = JSON.parse(localStorage['lastSelectedResults']);
      lastSelected = lastSelected.filter((res : SearchResult) => res.id !== selectedResult.id);
      lastSelected.unshift(selectedResult);
      if (lastSelected.length > 2)
      {
        lastSelected.pop();
      }
      localStorage['lastSelectedResults'] = JSON.stringify(lastSelected);
    }

    this.lastSelectedResultsChanged.emit(lastSelected);
    this.resultSelected.emit(selectedResult);
  }

}
