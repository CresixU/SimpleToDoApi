using FluentAssertions;
using SimpleToDoApi.Models.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleToDoApi.Tests.Models
{
    public class ResultModelTests
    {
        [Fact]
        public void Success_WhenCalled_ShouldReturnSuccessResultModel()
        {
            // Arrange
            var expectedResult = "Test Result";

            // Act
            var resultModel = ResultModel<string>.Success(expectedResult);

            // Assert
            resultModel.Should().NotBeNull();
            resultModel.IsSucces.Should().BeTrue();
            resultModel.Result.Should().Be(expectedResult);
            resultModel.Errors.Should().BeEmpty();
        }

        [Fact]
        public void Error_WhenCalledWithErrorsList_ShouldReturnErrorResultModelWithErrors()
        {
            // Arrange
            var expectedErrors = new List<string> { "Error 1", "Error 2" };

            // Act
            var resultModel = ResultModel<string>.Error(expectedErrors);

            // Assert
            resultModel.Should().NotBeNull();
            resultModel.IsSucces.Should().BeFalse();
            resultModel.Result.Should().BeNull();
            resultModel.Errors.Should().BeEquivalentTo(expectedErrors);
        }

        [Fact]
        public void Error_WhenCalledWithResult_ShouldReturnErrorResultModelWithResult()
        {
            // Arrange
            var expectedResult = "Error Result";

            // Act
            var resultModel = ResultModel<string>.Error(expectedResult);

            // Assert
            resultModel.Should().NotBeNull();
            resultModel.IsSucces.Should().BeFalse();
            resultModel.Result.Should().Be(expectedResult);
            resultModel.Errors.Should().BeEmpty();
        }

        [Fact]
        public void Error_WhenCalledWithResultAndErrors_ShouldReturnErrorResultModelWithResultAndErrors()
        {
            // Arrange
            var expectedResult = "Error Result";
            var expectedErrors = new List<string> { "Error 1", "Error 2" };

            // Act
            var resultModel = ResultModel<string>.Error(expectedResult, expectedErrors);

            // Assert
            resultModel.Should().NotBeNull();
            resultModel.IsSucces.Should().BeFalse();
            resultModel.Result.Should().Be(expectedResult);
            resultModel.Errors.Should().BeEquivalentTo(expectedErrors);
        }
    }
}
