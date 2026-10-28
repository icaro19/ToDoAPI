using Domain.Entities;
using FluentAssertions;
using Infrastructure.Security.Jwt;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Infrastructure.Tests.Security.Jwt
{
    [TestClass]
    public class JwtTokenGeneratorTests
    {
        private JwtTokenGenerator _generator = null!;

        private JwtSettings _settings = null!;

        [TestInitialize]
        public void Setup()
        {
            _settings = new JwtSettings
            {
                SecretKey =
                    "MinhaChaveSuperSecretaComMaisDe32Caracteres123456",
                Issuer = "TodoApi",
                Audience = "TodoApiUsers",
                ExpirationInMinutes = 60
            };

            var options =
                Options.Create(_settings);

            _generator =
                new JwtTokenGenerator(options);
        }

        [TestMethod]
        public void Should_Generate_Token()
        {
            var user =
                new User(
                    "João Silva",
                    "joao@email.com");

            var result =
                _generator.Generate(user);

            result.Should().NotBeNull();
            result.Token.Should().NotBeNullOrWhiteSpace();
        }

        [TestMethod]
        public void Should_Return_Expiration_Date()
        {
            var user =
                new User(
                    "João Silva",
                    "joao@email.com");

            var before =
                DateTime.UtcNow;

            var result =
                _generator.Generate(user);

            var expected =
                before.AddMinutes(
                    _settings.ExpirationInMinutes);

            result.ExpiresAt.Should()
                .BeAfter(before);

            result.ExpiresAt.Should()
                .BeOnOrBefore(
                    expected.AddSeconds(5));
        }

        [TestMethod]
        public void Should_Contain_UserId_Claim()
        {
            var user =
                new User(
                    "João Silva",
                    "joao@email.com");

            var result =
                _generator.Generate(user);

            var handler =
                new JwtSecurityTokenHandler();

            var token =
                handler.ReadJwtToken(
                    result.Token);

            token.Claims
                .FirstOrDefault(x =>
                    x.Type == JwtRegisteredClaimNames.Sub)
                ?.Value
                .Should()
                .Be(user.Id.ToString());
        }

        [TestMethod]
        public void Should_Contain_Email_Claim()
        {
            var user =
                new User(
                    "João Silva",
                    "joao@email.com");

            var result =
                _generator.Generate(user);

            var handler =
                new JwtSecurityTokenHandler();

            var token =
                handler.ReadJwtToken(
                    result.Token);

            token.Claims
                .FirstOrDefault(x =>
                    x.Type == JwtRegisteredClaimNames.Email)
                ?.Value
                .Should()
                .Be(user.Email);
        }

        [TestMethod]
        public void Should_Contain_Name_Claim()
        {
            var user =
                new User(
                    "João Silva",
                    "joao@email.com");

            var result =
                _generator.Generate(user);

            var token =
                new JwtSecurityTokenHandler()
                    .ReadJwtToken(result.Token);

            token.Claims
                .FirstOrDefault(x =>
                    x.Type == JwtRegisteredClaimNames.Name)
                ?.Value
                .Should()
                .Be(user.Name);
        }

        [TestMethod]
        public void Should_Use_Configured_Issuer()
        {
            var user =
                new User(
                    "João Silva",
                    "joao@email.com");

            var result =
                _generator.Generate(user);

            var token =
                new JwtSecurityTokenHandler()
                    .ReadJwtToken(result.Token);

            token.Issuer.Should()
                .Be(_settings.Issuer);
        }

        [TestMethod]
        public void Should_Use_Configured_Audience()
        {
            var user =
                new User(
                    "João Silva",
                    "joao@email.com");

            var result =
                _generator.Generate(user);

            var token =
                new JwtSecurityTokenHandler()
                    .ReadJwtToken(result.Token);

            token.Audiences.Should()
                .Contain(_settings.Audience);
        }

        [TestMethod]
        public void Should_Generate_Different_Tokens()
        {
            var user =
                new User(
                    "João Silva",
                    "joao@email.com");

            var token1 =
                _generator.Generate(user);

            var token2 =
                _generator.Generate(user);

            token1.Token.Should()
                .NotBe(token2.Token);
        }

        [TestMethod]
        public void Should_Generate_Valid_Signed_Token()
        {
            var user =
                new User(
                    "João Silva",
                    "joao@email.com");

            var result =
                _generator.Generate(user);

            var key =
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(
                        _settings.SecretKey));

            var validationParameters =
                new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer =
                        _settings.Issuer,

                    ValidAudience =
                        _settings.Audience,

                    IssuerSigningKey = key
                };

            var handler =
                new JwtSecurityTokenHandler();

            handler.Invoking(x =>
                x.ValidateToken(
                    result.Token,
                    validationParameters,
                    out _))
                .Should()
                .NotThrow();
        }

        [TestMethod]
        public void Should_Invalidate_Token_With_Wrong_Key()
        {
            var user =
                new User(
                    "João Silva",
                    "joao@email.com");

            var result =
                _generator.Generate(user);

            var wrongKey =
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(
                        "OutraChaveTotalmenteDiferente123456"));

            var parameters =
                new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer =
                        _settings.Issuer,

                    ValidAudience =
                        _settings.Audience,

                    IssuerSigningKey = wrongKey
                };

            var handler =
                new JwtSecurityTokenHandler();

            Assert.Throws<SecurityTokenInvalidSignatureException>(
                () =>
                {
                    handler.ValidateToken(
                        result.Token,
                        parameters,
                        out _);
                });
        }
    }
}