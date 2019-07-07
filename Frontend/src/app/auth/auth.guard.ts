import {Injectable, isDevMode} from '@angular/core';
import {CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router} from '@angular/router';
import {Observable} from 'rxjs';
import {map, catchError} from 'rxjs/operators';

import {AuthService} from "./auth.service";

@Injectable({
	providedIn: 'root'
})
export class AuthGuard implements CanActivate
{
	constructor(private authService: AuthService, private router: Router) {}

	canActivate (next : ActivatedRouteSnapshot, state : RouterStateSnapshot) : Observable<boolean> | Promise<boolean> | boolean
	{
		if (this.authService.isLoggedIn === null)
		{
			return this.authService.checkAuth().pipe(map(response => this.analyzeStatus(response)));
		}
		else
		{
			return this.analyzeStatus(this.authService.isLoggedIn);
		}
	}

	private analyzeStatus (isLoggedIn : boolean)
	{
		if (isLoggedIn)
		{
			return true;
		}

		this.router.navigate(['/auth/signIn']);
		return false;
	}
}
