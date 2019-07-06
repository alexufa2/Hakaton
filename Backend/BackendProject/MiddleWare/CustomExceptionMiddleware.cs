using TemplateApp.MiddleWare.ExceptionBase;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace TemplateApp.MiddleWare
{
	public class CustomExceptionMiddleware
	{
		private readonly RequestDelegate _next;

		public CustomExceptionMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task Invoke(HttpContext context)
		{
			try
			{
				await _next.Invoke(context);


				if (context.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
				{
					if (context.User.Identity.IsAuthenticated)
					{
						//the user is authenticated, yet we are returning a 401
						//let's return a 403 instead
						context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
					}
				}
			}
			catch (Exception ex)
			{
				await HandleExceptionAsync(context, ex);
			}
		}

		private async Task HandleExceptionAsync(HttpContext context, Exception exception)
		{
			var response = context.Response;
			var customException = exception as BaseCustomException;
			var statusCode = (int)HttpStatusCode.InternalServerError;
			var message = "Unexpected error";
			var description = "Unexpected error";

			if (null != customException)
			{
				message = customException.Message;
				description = customException.Description;
				statusCode = customException.Code;
			}
			else
			{
				message = exception.Message;
				statusCode = 500;
			}

			response.ContentType = "application/json";
			response.StatusCode = statusCode;
			await response.WriteAsync(JsonConvert.SerializeObject(new CustomErrorResponse
			{
				Message = message,
				Description = description
			}));
		}
	}
}
