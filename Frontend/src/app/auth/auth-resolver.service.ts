import {Injectable} from '@angular/core';
import {Resolve, RouterStateSnapshot, ActivatedRouteSnapshot} from '@angular/router';
import {Observable} from 'rxjs';

import {AuthService} from "./auth.service";

@Injectable()
export class AuthResolverService implements Resolve<boolean>
{
	constructor(private authService: AuthService) {}

	resolve (route : ActivatedRouteSnapshot, state : RouterStateSnapshot) : Observable<boolean> | boolean
	{
		if (this.authService.isLoggedIn === null)
		{
			return this.authService.checkAuth();
		}
		else
		{
			return this.authService.isLoggedIn;
		}
	}
}