﻿using CommonTestUtilities.IdEncryption;
using CommonTestUtilities.Tokens;
using FluentAssertions;
using System.Net;

namespace WebAPI.Test.Recipe.GetById
{
    public class GetRecipeByIdInvalidTokenTest : MyRecipeBookClassFixture
    {
        private const string METHOD = "recipe";

        public GetRecipeByIdInvalidTokenTest(CustomWebApplicationFactory factory) : base(factory) { }

        [Fact]
        public async Task Error_Token_Invalid()
        {
            var id = IdEncripterBuilder.Build().Encode(1);

            var response = await DoGet($"{METHOD}/{id}", token: "tokenInvalid");

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Error_Token_With_User_NotFound()
        {
            var id = IdEncripterBuilder.Build().Encode(1);

            var token = JwtTokenGeneratorBuilder.Build().Generate(Guid.NewGuid());

            var response = await DoGet($"{METHOD}/{id}", token: token);

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
    }
}
