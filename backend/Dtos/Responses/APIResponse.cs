﻿namespace backend.Dtos.Responses
{
    public class APIResponse<T>
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; } = string.Empty;
        public T? Data { get; set; }

        public APIResponse(int statusCode, string? message="", T? data= default) 
        { 
            StatusCode = statusCode;
            Message = message;
            Data = data;
        }


    }
}
