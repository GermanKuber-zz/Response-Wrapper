﻿using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

public class LastTestShould
{
    [Fact]
    public void Do_Something_Success_Case()
    {
        var sut = Response<string>.Ok("Test");
        var callMethod = false;
        sut.Success(message => callMethod = true);
        callMethod.Should().Be(true);
    }
    [Fact]
    public void Do_Something_Error_Case()
    {
        var sut = Response<string>.Error("Error");
        var callMethod = false;
        sut.Error(errors => callMethod = true);
        callMethod.Should().Be(true);
    }

    [Fact]
    public void Get_Errors_in_Error_Case()
    {
        var errors = new List<string> { "Error 1", "Error 2" };
        var sut = Response<string>.Error(errors);
        var countErrors = 0;
        sut.Error(errorsToShow => countErrors = errorsToShow.Count());
        countErrors.Should().Be(errors.Count());
    }


    public class Response<TResponse>
    {
        public bool SuccessResult { get; }
        public List<string> Errors { get; set; } = new List<string>();
        public TResponse Result { get; set; }


        public Response<TResponse> Success(Action<TResponse> callback)
        {
            if (SuccessResult)
                callback(Result);
            return this;
        }
        public TResult Success<TResult>(Func<TResponse, TResult> callback)
        {
            if (SuccessResult)
                return callback(Result);
            return default(TResult);
        }
        public Response<TResponse> Error(Action<List<string>> callback)
        {
            if (!SuccessResult)
                callback(Errors);
            return this;
        }

        public static Response<TResponse> Error(List<string> errors) =>
            new Response<TResponse>(errors);
        public static Response<TResponse> Error() =>
            new Response<TResponse>(new List<string>());

        public static Response<TResponse> Ok(TResponse response) =>
            new Response<TResponse>(response);
        public static Response<TResponse> Error(string error) =>
            new Response<TResponse>(new List<string> { error });

        protected Response(TResponse response)
        {
            SuccessResult = true;
            Result = response;
        }


        protected Response(List<string> errors)
        {
            SuccessResult = false;
            Errors = new List<string>();
            Errors.AddRange(errors);
        }
    }
}
