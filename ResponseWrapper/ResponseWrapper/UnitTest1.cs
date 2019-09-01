using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ResponseWrapper
{
    public class FirstTestShould
    {
        [Fact]
        public void Contain_instance_of_result()
        {
            var sut = new Response<string>("Test");
            sut.Result.Should().Be("Test");
        }
        [Fact]
        public void Know_if_is_success()
        {
            var sut = new Response<string>("Test");
            sut.SuccessResult.Should().Be(true);
        }
        [Fact]
        public void Know_if_is_a_error()
        {
            var sut = new Response<string>(new List<string> { "Error" });
            sut.SuccessResult.Should().Be(false);
        }
        [Fact]
        public void Return_all_errors()
        {
            var sut = new Response<string>(new List<string> { "Error", "Error 2" });
            sut.Errors.Count().Should().Be(2);
        }
        public class Response<TResponse>
        {
            public bool SuccessResult { get; }
            public List<string> Errors { get; set; } = new List<string>();
            public TResponse Result { get; set; }
            public Response(TResponse response)
            {
                SuccessResult = true;
                Result = response;
            }
            public Response(List<string> errors)
            {
                SuccessResult = false;
                Errors = new List<string>();
                Errors.AddRange(errors);
            }
        }

    }
}
public class LastStep
{
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


