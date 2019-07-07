import {Component, OnInit, Input, Output, EventEmitter, ViewChild, ElementRef, Inject} from '@angular/core';
import {AbstractControl, FormArray, FormBuilder, FormGroup, Validators} from '@angular/forms';
import {MAT_DIALOG_DATA, MatDialog, MatDialogRef, MatStepper} from '@angular/material';
import {fixDrawerHtmlScroll} from '../../shared/drawerHtmlScrollFix';
import {delay, finalize, map} from 'rxjs/operators';

@Component({
	selector: 'app-verification-drawer',
	templateUrl: './verification-drawer.component.html',
  styleUrls : ['./verification-drawer.component.scss']
})
export class VerificationDrawerComponent implements OnInit
{
	constructor (private formBuilder: FormBuilder, public progressDialog: MatDialog, private fb : FormBuilder)
  {
	  this.fixDrawerHtmlScroll = fixDrawerHtmlScroll;
    this.form = this.fb.group({
      phone : ['', Validators.required],
      passportNumber : ['', Validators.required],
      passportDate : ['', Validators.required],
      passportPlace : ['', Validators.required],
    });
  }

  fixDrawerHtmlScroll : Function;
	@Input() opened;

	@Output() openedChange = new EventEmitter<boolean>();

	status = "INITIAL";
  form : FormGroup;
  errorText = '';


	ngOnInit ()
	{
  }

	setDefaultValues ()
	{
    this.form.patchValue({phone : '', passportNumber : '', passportDate : '', passportPlace : ''});
	}

  submit ()
  {
    this.status = 'SUCCESS';
    //setTimeout(() => this.close());
  }

  confirmClose ()
  {
    if (this.form.get('phone').value === '')
    {
      this.close();
      return;
    }

    let dialogRef = this.progressDialog.open(ProgressRequisitesDialog, {width: '500px', disableClose : true});

    dialogRef.beforeClose().subscribe(result =>
    {
      if (result)
      {
        this.close();
      }
    });
  }

  close ()
  {
    this.openedChange.emit(false);
  }

  onClosedStart ()
  {
    this.close();
    fixDrawerHtmlScroll(false);
  }

  onOpenedStart ()
  {
    this.setDefaultValues();
    this.status = 'INITIAL';
    fixDrawerHtmlScroll();
  }
}


@Component
({
  selector: 'requisites-progress-dialog',
  template: `
        <h2 mat-dialog-title>Несохраненные изменения</h2>
        
        <mat-dialog-content>
          Вы действительно хотите прервать заполнение данных?<br>
          Все несохраненные изменения будут потеряны.
        </mat-dialog-content>
		
        <mat-dialog-actions class="mt-2">
          <button mat-raised-button (click)="dialogRef.close()" color="primary">Продолжить заполнение</button>
          <button mat-button (click)="dialogRef.close(true)">Отменить изменения</button>
        </mat-dialog-actions>		
	`,
})
export class ProgressRequisitesDialog
{
  constructor (public dialogRef : MatDialogRef<ProgressRequisitesDialog>, @Inject(MAT_DIALOG_DATA) public data : any) {}
}
