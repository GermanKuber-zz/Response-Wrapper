using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
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
