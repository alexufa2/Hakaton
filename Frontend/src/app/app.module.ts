import {NgModule} from '@angular/core';
import {BrowserModule} from '@angular/platform-browser';
import {HttpClientModule, HTTP_INTERCEPTORS} from '@angular/common/http';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {AppRoutingModule} from './app-routing.module';

import {MAT_SNACK_BAR_DEFAULT_OPTIONS, MatAutocompleteModule} from '@angular/material';
import {MatButtonModule} from '@angular/material/button';
import {MatMenuModule} from '@angular/material/menu';
import {MatTableModule} from '@angular/material/table';
import {MatSidenavModule} from '@angular/material/sidenav';
import {MatToolbarModule} from '@angular/material/toolbar';
import {MatStepperModule} from '@angular/material/stepper';
import {MatProgressBarModule} from '@angular/material/progress-bar';
import {MatProgressSpinnerModule} from '@angular/material/progress-spinner';
import {MatDialogModule} from '@angular/material/dialog';
import {MatInputModule} from '@angular/material/input';
import {MatCheckboxModule} from '@angular/material/checkbox';
import {MatSnackBarModule} from '@angular/material/snack-bar';
import {MatTooltipModule} from '@angular/material/tooltip';
import {MatSelectModule} from '@angular/material/select';
import {MatCardModule} from '@angular/material/card';
import {MatDividerModule} from '@angular/material/divider';
import {MatIconModule} from '@angular/material/icon';

import {AppComponent} from './app.component';
import {LoaderComponent} from './shared/loader.component';
import {BaseService} from './shared/base.service';
import {BreadcrumpsComponent} from './shared/breadcrumps.component';
import { MainComponent } from './main/main.component';
import { AuthComponent } from './auth/auth.component';
import { SignInComponent } from './auth/sign-in/sign-in.component';
import { SignUpComponent } from './auth/sign-up/sign-up.component';
import {AuthService} from './auth/auth.service';
import { SidebarComponent } from './main/sidebar/sidebar.component';
import { HeaderComponent } from './main/header/header.component';
import { SearchComponent } from './main/stat-address/search/search.component';
import { StatBlocksComponent } from './main/stat-address/stat-blocks/stat-blocks.component';
import { StatBlocksDetailComponent } from './main/stat-address/stat-blocks-detail/stat-blocks-detail.component';
import { ProfileComponent } from './main/header/profile/profile.component';
import {ProgressRequisitesDialog, VerificationDrawerComponent} from './main/verification-drawer/verification-drawer.component';
import {StatAddressComponent} from './main/stat-address/stat-address.component';
import {MainService} from './main/main.service';
import { StatAddressMapComponent } from './main/stat-address-map/stat-address-map.component';

@NgModule({
  declarations: [AppComponent, ProgressRequisitesDialog, VerificationDrawerComponent, StatAddressComponent,
    LoaderComponent, BreadcrumpsComponent, MainComponent, AuthComponent, SignInComponent, SignUpComponent, SidebarComponent, HeaderComponent, SearchComponent, StatBlocksComponent, StatBlocksDetailComponent, ProfileComponent, StatAddressMapComponent],
  imports: [MatAutocompleteModule, BrowserModule, BrowserAnimationsModule, HttpClientModule, AppRoutingModule, FormsModule, ReactiveFormsModule, MatButtonModule, MatTableModule, MatIconModule,
    MatSidenavModule, MatToolbarModule, MatStepperModule, MatProgressSpinnerModule, MatProgressBarModule, MatDialogModule, MatInputModule, MatMenuModule, MatCheckboxModule, MatSnackBarModule,
    MatTooltipModule, MatSelectModule, MatDividerModule, MatCardModule, MatIconModule],
  providers: [{provide: MAT_SNACK_BAR_DEFAULT_OPTIONS, useValue: {duration: 3000}}, {provide: HTTP_INTERCEPTORS, useClass: BaseService, multi: true}, AuthService, MainService],
  bootstrap: [AppComponent],
  entryComponents : [ProgressRequisitesDialog]
})
export class AppModule
{
}
