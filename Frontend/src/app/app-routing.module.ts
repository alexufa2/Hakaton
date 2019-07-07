import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {MainComponent} from './main/main.component';
import {SignInComponent} from './auth/sign-in/sign-in.component';
import {SignUpComponent} from './auth/sign-up/sign-up.component';
import {AuthResolverService} from './auth/auth-resolver.service';
import {AuthGuard} from './auth/auth.guard';

const routes : Routes = [
  {
    path: '',
    component: MainComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'auth',
    children: [
      {path : "signIn", component : SignInComponent},
      {path : "signUp", component : SignUpComponent}
    ]
  },
	{
		path: '**',
		redirectTo: '/'
	},
];

@NgModule({
	imports: [RouterModule.forRoot(routes)],
	exports: [RouterModule],
  providers: [AuthResolverService]
})

export class AppRoutingModule
{
}
