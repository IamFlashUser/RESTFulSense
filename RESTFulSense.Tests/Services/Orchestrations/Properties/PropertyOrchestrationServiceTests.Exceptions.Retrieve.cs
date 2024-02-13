﻿// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using System;
using FluentAssertions;
using Moq;
using RESTFulSense.Models.Orchestrations.Properties;
using RESTFulSense.Models.Orchestrations.Properties.Exceptions;
using Xunit;

namespace RESTFulSense.Tests.Services.Orchestrations.Properties
{
    public partial class PropertyOrchestrationServiceTests
    {
        [Theory]
        [MemberData(nameof(DependencyValidationExceptions))]
        private void ShouldThrowPropertyOrchestrationDependencyValidationExceptionIfDependencyExceptionOccurs(
            Exception dependencyValidationException)
        {
            // given
            object someObject = new object();
            object inputObject = someObject;
            PropertyModel somePropertyModel = CreateSomePropertyModel(inputObject);

            var expectedPropertyOrchestrationDependencyValidationException =
                new PropertyOrchestrationDependencyValidationException(
                    message: "Property orchestration dependency validation error occurred, fix errors and try again.",
                    innerException: dependencyValidationException);

            typeServiceMock.Setup(service =>
                service.RetrieveType(It.IsAny<object>()))
                    .Throws(dependencyValidationException);

            // when
            Action retrievePropertiesAction =
                () => propertyOrchestrationService.RetrieveProperties(somePropertyModel);

            PropertyOrchestrationDependencyValidationException
                actualPropertyOrchestrationDependencyValidationException =
                Assert.Throws<PropertyOrchestrationDependencyValidationException>(retrievePropertiesAction);

            // then
            actualPropertyOrchestrationDependencyValidationException.Should()
                .BeEquivalentTo(expectedPropertyOrchestrationDependencyValidationException);

            typeServiceMock.Verify(service =>
                service.RetrieveType(It.IsAny<object>()),
                    Times.Once());

            propertyServiceMock.Verify(service =>
                service.RetrieveProperties(It.IsAny<Type>()),
                    Times.Never());

            typeServiceMock.VerifyNoOtherCalls();
            propertyServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        private void ShouldThrowPropertyOrchestrationDependencyExceptionIfDependencyExceptionOccurs(
            Exception dependencyException)
        {
            // given
            object someObject = new object();
            object inputObject = someObject;
            PropertyModel somePropertyModel = CreateSomePropertyModel(inputObject);

            var expectedPropertyOrchestrationDependencyException =
                new PropertyOrchestrationDependencyException(
                    message: "Property orchestration dependency error occurred, fix errors and try again.",
                    innerException: dependencyException);

            this.typeServiceMock.Setup(service =>
                service.RetrieveType(It.IsAny<object>()))
                    .Throws(dependencyException);

            // when
            Action retrievePropertiesAction =
                () => this.propertyOrchestrationService.RetrieveProperties(somePropertyModel);

            PropertyOrchestrationDependencyException actualPropertyOrchestrationDependencyException =
                Assert.Throws<PropertyOrchestrationDependencyException>(retrievePropertiesAction);

            // then
            actualPropertyOrchestrationDependencyException.Should()
                .BeEquivalentTo(expectedPropertyOrchestrationDependencyException);

            this.typeServiceMock.Verify(service =>
                service.RetrieveType(It.IsAny<object>()),
                    Times.Once());

            this.propertyServiceMock.Verify(service =>
                service.RetrieveProperties(It.IsAny<Type>()),
                    Times.Never());

            this.typeServiceMock.VerifyNoOtherCalls();
            this.propertyServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        private void ShouldThrowPropertyOrchestrationServiceExceptionIfExceptionOccurs()
        {
            // given
            object someObject = new object();
            object inputObject = someObject;
            PropertyModel somePropertyModel = CreateSomePropertyModel(inputObject);

            Exception someException = new Exception();

            var expectedFailedPropertyOrchestrationException =
                new FailedPropertyOrchestrationException(
                    message: "Failed property orchestration service exception occurred, please contact support.",
                    innerException: someException);

            var expectedPropertyOrchestrationException =
                new PropertyOrchestrationServiceException(
                    message: "Property orchestration service error occurred, contact support.",
                    innerException: expectedFailedPropertyOrchestrationException);

            this.typeServiceMock.Setup(service =>
                service.RetrieveType(It.IsAny<object>()))
                    .Throws(someException);

            // when
            void retrievePropertiesAction() =>
                this.propertyOrchestrationService.RetrieveProperties(somePropertyModel);

            PropertyOrchestrationServiceException actualPropertyOrchestrationException =
                Assert.Throws<PropertyOrchestrationServiceException>(retrievePropertiesAction);

            // then
            actualPropertyOrchestrationException.Should()
                .BeEquivalentTo(expectedPropertyOrchestrationException);

            this.typeServiceMock.Verify(service =>
                service.RetrieveType(It.IsAny<object>()),
                    Times.Once());

            this.propertyServiceMock.Verify(service =>
                service.RetrieveProperties(It.IsAny<Type>()),
                    Times.Never());

            this.typeServiceMock.VerifyNoOtherCalls();
            this.propertyServiceMock.VerifyNoOtherCalls();
        }
    }
}
