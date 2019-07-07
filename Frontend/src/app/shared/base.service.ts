import {Injectable} from '@angular/core';
import {throwError} from "rxjs";
import {HttpClient, HttpErrorResponse, HttpEvent, HttpResponse, HttpInterceptor, HttpHandler, HttpRequest} from '@angular/common/http';
import {ResponseInterface} from "./classes";
import {isUndefined, isNull} from "util";
import {Observable} from "rxjs";
import {environment} from '../../environments/environment';
import {retry, catchError, map} from 'rxjs/operators';

@Injectable({
	providedIn: 'root'
})
export class BaseService implements HttpInterceptor
{
	constructor (protected http : HttpClient) {}

  intercept (req : HttpRequest<any>, next : HttpHandler) : Observable<HttpEvent<any>>
  {
    if (!isNull(req.url.match("^http")))
    {
      return next.handle(req);
    }

    let authToken = '';
    let userData = localStorage.getItem('userData');
    if (userData !== null)
    {
      userData = JSON.parse(userData);
      authToken = (userData as any).token;
    }

    req = req.clone({url : `${environment.serviceApiUrl}/${req.url}`,
      headers : req.headers.set('Content-Type', 'application/json').set('Authorization', `Bearer ${authToken}`)});

    return next.handle(req).pipe(retry(1), map(this.checkError), catchError(this.handleError));
  }

  protected checkError (response : any)
	{
	  if (!(response instanceof HttpResponse))
    {
      return response;
    }

    let body = <ResponseInterface>response.body;
		if (isUndefined(body.success) || !body.success)
		{
			throw new Error(body.data.message);
		}

		return response;
	}

	protected handleError (error: HttpErrorResponse)
	{
		let message = 'Ошибка на сервере. Сообщите в техподдержку.';

		if (error.error instanceof ErrorEvent)
		{
			console.error(error.error.message);
		}
		else
		{
			if (isUndefined(error.status))
			{
				message = error.message;
			}
			else
			{
				console.error(`${error.status} ${JSON.stringify(error.error)}`);
			}
		}

		return throwError(message);
	}
}
