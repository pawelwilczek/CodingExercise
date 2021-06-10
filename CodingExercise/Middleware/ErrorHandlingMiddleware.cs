using CodingExercise.CustomExeptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodingExercise.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {            
                httpContext.Response.ContentType = "application/json";
                if(ex is ItemAlreadyExistException)
                {
                    var response = ex.Message;
                    httpContext.Response.StatusCode = 409;

                    await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(response));
                }
                else if(ex is DbUpdateConcurrencyException)
                {
                    var response = ex.Data["customMessage"];
                    httpContext.Response.StatusCode = 409;

                    _logger.LogError($"DbUpdateConcurrencyException occured: {ex.Message}");
                    await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = "An error occured";
                    httpContext.Response.StatusCode = 500;

                    _logger.LogError($"The following error occured: {ex.Message}");
                    await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(response));
                }
            }
        } 
    }
}
