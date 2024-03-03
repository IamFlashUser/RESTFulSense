﻿// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using RESTFulSense.Tests.Acceptance.Models;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using Xunit;

namespace RESTFulSense.Tests.Acceptance.Tests
{
    public partial class RestfulApiClientTests
    {
        [Fact]
        private async Task ShouldPutContentReturnsExpectedWithSerializedAndDeserializedContentAsync()
        {
            // given
            TEntity randomTEntity = GetRandomTEntity();
            TEntity expectedTEntity = randomTEntity;
            string mediaType = "text/json";
            bool ignoreDefaultValues = false;

            this.wiremockServer
                .Given(Request.Create()
                    .WithPath(relativeUrl)
                    .UsingPut())
                        .RespondWith(Response.Create()
                            .WithStatusCode(200)
                            .WithBodyAsJson(randomTEntity));

            // when
            TEntity actualTEntity =
                await this.restfulApiClient.PutContentAsync<TEntity>(
                    relativeUrl,
                    content: randomTEntity,
                    mediaType: mediaType,
                    ignoreDefaultValues: ignoreDefaultValues,
                    SerializationContentFunction<TEntity>,
                    DeserializationContentFunction);

            // then
            actualTEntity.Should().BeEquivalentTo(expectedTEntity);
        }

        [Fact]
        private async Task ShouldPutContentWithTContentReturnsTResultAsync()
        {
            // given
            TEntity randomTEntity = GetRandomTEntity();
            TEntity expectedTEntity = randomTEntity;
            string mediaType = "text/json";
            bool ignoreDefaultValues = false;

            this.wiremockServer.Given(
                Request.Create()
                    .WithPath(relativeUrl)
                    .UsingPut())
                        .RespondWith(
                            Response.Create()
                                .WithStatusCode(200)
                                .WithHeader("Content-Type", mediaType)
                                .WithBody(
                                    JsonConvert.SerializeObject(randomTEntity)));

            // when
            TEntity actualTEntity =
                await this.restfulApiClient.PutContentAsync<TEntity, TEntity>(
                    relativeUrl,
                    content: randomTEntity,
                    mediaType: mediaType,
                    ignoreDefaultValues: ignoreDefaultValues,
                    serializationFunction: SerializationContentFunction<TEntity>,
                    deserializationFunction: DeserializationContentFunction);

            // then
            actualTEntity.Should().BeEquivalentTo(expectedTEntity);
        }

        [Fact]
        private async Task ShouldPutContentWithTContentReturnsTResultWithCancellationTokenAsync()
        {
            // given
            TEntity randomTEntity = GetRandomTEntity();
            TEntity expectedTEntity = randomTEntity;

            CancellationToken cancellationToken =
                new CancellationToken(canceled: false);

            string mediaType = "text/json";
            bool ignoreDefaultValues = false;

            this.wiremockServer.Given(
                Request.Create()
                    .WithPath(relativeUrl)
                    .UsingPut())
                        .RespondWith(
                            Response.Create()
                                .WithStatusCode(200)
                                .WithHeader("Content-Type", mediaType)
                                .WithBody(
                                    JsonConvert.SerializeObject(randomTEntity)));

            // when
            TEntity actualTEntity =
                await this.restfulApiClient.PutContentAsync<TEntity, TEntity>(
                    relativeUrl,
                    content: randomTEntity,
                    cancellationToken: cancellationToken,
                    mediaType: mediaType,
                    ignoreDefaultValues: ignoreDefaultValues,
                    serializationFunction: SerializationContentFunction,
                    deserializationFunction: DeserializationContentFunction);

            // then
            actualTEntity.Should().BeEquivalentTo(expectedTEntity);
        }

        [Fact]
        private async Task ShouldPutContentReturnsContentWithDeserializationAsync()
        {
            // given
            TEntity randomTEntity = GetRandomTEntity();
            TEntity expectedTEntity = randomTEntity;

            this.wiremockServer.Given(
                Request.Create()
                    .WithPath(relativeUrl)
                    .UsingPut())
                        .RespondWith(
                            Response.Create()
                                .WithStatusCode(200)
                                .WithBody(JsonConvert.SerializeObject(randomTEntity)));

            // when
            TEntity actualTEntity =
                await this.restfulApiClient.PutContentAsync<TEntity>(
                    relativeUrl,
                    deserializationFunction: DeserializationContentFunction);

            // then
            actualTEntity.Should().BeEquivalentTo(expectedTEntity);
        }

        [Fact]
        private async Task ShouldPutContentReturnsContentWithCancellationTokenDeserializationAsync()
        {
            // given
            TEntity randomTEntity = GetRandomTEntity();
            TEntity expectedTEntity = randomTEntity;
            var cancellationToken = new CancellationToken(canceled: false);

            this.wiremockServer.Given(
                Request.Create()
                    .WithPath(relativeUrl)
                    .UsingPut())
                        .RespondWith(
                            Response.Create()
                                .WithStatusCode(200)
                                .WithBody(JsonConvert.SerializeObject(randomTEntity)));

            // when
            TEntity actualTEntity =
                await this.restfulApiClient.PutContentAsync<TEntity>(
                    relativeUrl,
                    cancellationToken: cancellationToken,
                    deserializationFunction: DeserializationContentFunction);

            // then
            actualTEntity.Should().BeEquivalentTo(expectedTEntity);
        }
    }
}