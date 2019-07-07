import {Component, OnInit} from '@angular/core';

@Component({
	selector: 'app-loader',
	template: `
		<div class="mt-5 p-5">
            <mat-spinner class="mt-5" style="margin: auto" diameter="60" strokeWidth="2"></mat-spinner>
			<p class="text-center color-999 mt-4" style="font-size: 13px;">Данные загружаются...</p>
		</div>
	`,
	styles: []
})
export class LoaderComponent implements OnInit
{

	constructor ()
	{

	}

	ngOnInit ()
	{

	}

}
