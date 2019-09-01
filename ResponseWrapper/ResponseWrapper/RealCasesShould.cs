using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

public class RealCasesShould
{

    [Fact]
    public void Login_Success()
    {
        var methodWasCalled = false;

        new LoginService()
             .Login("TEST", "TEST1")
             .Success(user =>
             {
                 var usuario = user;
                 methodWasCalled = true;
             });
        methodWasCalled.Should().BeTrue();
    }

    [Fact]
    public void Login_Error()
    {
        var methodWasCalled = false;
        new LoginService()
             .Login("TEST", "TEST")
             .Success(user =>
             {
                 var usuario = user;
             })
             .Error(erros =>
             {
                 var errores = erros;
                 methodWasCalled = true;
             });
        methodWasCalled.Should().BeTrue();
    }

    [Fact]
    public void Login_Success_Return()
    {
        var methodWasCalled = false;

        // Declare a local function.
        Response<User> LoginUser(string userName, string password) =>
            new LoginService()
              .Login(userName, password)
              .Success(user =>
              {
                  var usuario = user;
              })
              .Error(erros =>
              {
                  var errores = erros;
              });

        LoginUser("TEST", "TEST1")
            .Success(user =>
            {
                methodWasCalled = true;
            });

        methodWasCalled.Should().BeTrue();
    }

    [Fact]
    public void Login_Error_Return()
    {
        var methodWasCalled = false;

        // Declare a local function.
        Response<User> LoginUser(string userName, string password) =>
            new LoginService()
              .Login(userName, password)
              .Success(user =>
              {
                  var usuario = user;
              })
              .Error(erros =>
              {
                  var errores = erros;
              });

        LoginUser("TEST", "TEST")
                        .Error(erros =>
                        {
                            methodWasCalled = true;
                        });

        methodWasCalled.Should().BeTrue();
    }


    public class LoginService
    {
        public Response<User> Login(string user, string password)
        {
            if (user == "TEST" && password == "TEST1")
                return Response<User>.Ok(new User());
            else if (user == password)
                return Response<User>.Error("Usuario y Password no pueden ser iguales");
            else
                return Response<User>.Error("Usuario incorrecto");
        }
    }
    public class User { }
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