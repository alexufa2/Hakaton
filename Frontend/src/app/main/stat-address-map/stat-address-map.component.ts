import {Component, ElementRef, OnInit, ViewChild} from '@angular/core';

declare var H: any;

@Component({
  selector: 'app-stat-address-map',
  templateUrl: './stat-address-map.component.html',
  styleUrls: ['./stat-address-map.component.scss']
})
export class StatAddressMapComponent implements OnInit
{
  @ViewChild("map") public mapElement: ElementRef;


  private  mapUI : any;
  private  map : any;
  private platform: any;

  constructor ()
  {
    this.platform = new H.service.Platform({
      "app_id": "apyv7dCZBK4naxpkGN2Q",
      "app_code": "cTDOFrxW5UBdjifkB-BJlQ"
    });
  }

  ngOnInit ()
  {
  }

  ngAfterViewInit ()
  {
    let defaultLayers = this.platform.createDefaultLayers();
    this.map = new H.Map(
      this.mapElement.nativeElement,
      defaultLayers.normal.map,
      {
        zoom: 13,
        center: { lat: 54.74306, lng: 55.96779 }
      }
    );

    //var romeMarker = new H.map.Marker({lat:54.74306, lng: 55.96779});
    //this.map.addObject(romeMarker);

    var behavior = new H.mapevents.Behavior(new H.mapevents.MapEvents(this.map));
    // Create the default UI components
    this.mapUI = H.ui.UI.createDefault(this.map, defaultLayers);

    this.addInfoBubble();
  }

  addInfoBubble ()
  {
    var group = new H.map.Group() as any;
    this.map.addObject(group);

    // add 'tap' event listener, that opens info bubble, to the group
    group.addEventListener('tap', (evt) =>
    {
      // event target is the marker itself, group is a parent event target
      // for all objects that it contains
      var bubble =  new H.ui.InfoBubble(evt.target.getPosition(), {
        // read custom data
        content: evt.target.getData()
      });

      // show info bubble
      this.mapUI.addBubble(bubble);
    }, false);

    this.addMarkerToGroup(group, {lat:54.74306, lng:55.96779}, 'Тётю Свету чуть облицовкой не пришибло!!! Плохой ремонт!!!');
    this.addMarkerToGroup(group, {lat:54.74311, lng:55.97781}, 'Ремонт подъезда выполнен плохо');
    this.addMarkerToGroup(group, {lat:54.74314, lng:55.98791}, 'Ремонт подъезда выполнен плохо');
    this.addMarkerToGroup(group, {lat:54.74311, lng:55.99782}, 'Ремонт подъезда выполнен плохо');
    this.addMarkerToGroup(group, {lat:54.74311, lng:55.99782}, 'Ремонт подъезда выполнен плохо');
    this.addMarkerToGroup(group, {lat:54.75311, lng:55.99782}, 'Ремонт подъезда выполнен плохо');
    this.addMarkerToGroup(group, {lat:54.76311, lng:55.99782}, 'Ремонт подъезда выполнен плохо');
    this.addMarkerToGroup(group, {lat:54.77311, lng:55.99782}, 'Ремонт подъезда выполнен плохо');
    this.addMarkerToGroup(group, {lat:54.7211, lng:53.91083}, 'Ремонт подъезда выполнен плохо');
    this.addMarkerToGroup(group, {lat:54.7211, lng:53.91083}, 'Ремонт подъезда выполнен плохо');
    this.addMarkerToGroup(group, {lat:54.7211, lng:53.91083}, 'Ремонт подъезда выполнен плохо');
  }

  addMarkerToGroup (group, coordinate, html)
  {
    var marker = new H.map.Marker(coordinate);
    marker.setData(html);
    group.addObject(marker);
  }
}
