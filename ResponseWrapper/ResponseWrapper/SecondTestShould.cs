using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using Xunit;

public class SecondTestShould
{

    [Fact]
    public void Contain_instance_of_result()
    {
        var sut = Response<string>.Ok("Test");
        sut.Result.Should().Be("Test");
    }
    [Fact]
    public void Know_if_is_success()
    {
        var sut = Response<string>.Ok("Test");
        sut.SuccessResult.Should().Be(true);
    }
    [Fact]
    public void Know_if_is_a_error()
    {
        var sut = Response<string>.Error("Error");
        sut.SuccessResult.Should().Be(false);
    }
    [Fact]
    public void Return_all_errors()
    {
        var sut = Response<string>.Error(new List<string> { "Error", "Error 2" });
        sut.Errors.Count().Should().Be(2);
    }
    public class Response<TResponse>
    {
        public bool SuccessResult { get; }
        public List<string> Errors { get; set; } = new List<string>();
        public TResponse Result { get; set; }

        public static Response<TResponse> Error(List<string> errors) =>
            new Response<TResponse>(errors);
        public static Response<TResponse> Error(string error) =>
            new Response<TResponse>(new List<string> { error });
        public static Response<TResponse> Error() =>
            new Response<TResponse>(new List<string>());

        public static Response<TResponse> Ok(TResponse response) =>
            new Response<TResponse>(response);

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