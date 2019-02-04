using Socratic.ArgParser.Annotations;
using Socratic.ArgParser.Exceptions;
using System;
using Xunit;

// ReSharper disable ClassNeverInstantiated.Local
namespace Socratic.ArgParser.Tests
{
    public class ArgParserTests
    {
        [Fact]
        public void WhenTheArgumentsAreSerializedThenTheResultShouldBeOfTheSpecifiedType()
        {
            var testArgs = new[] { "0" };

            var result = ArgParser.Serialize<TestArgClass>(testArgs);

            Assert.IsType<TestArgClass>(result);
        }

        [Fact]
        public void WhenTheArgumentsAreSerializedThenTheResultShouldContainTheSerializedValues()
        {
            var testArgs = new[] { "10" };

            var result = ArgParser.Serialize<TestArgClass>(testArgs);

            Assert.Equal(10, result.FirstArg);
        }

        [Fact]
        public void WhenDuplicateIndexValueExistsThenShouldThrowDuplicateIndexException()
        {
            var testArgs = new[] { "10" };

            Assert.Throws<DuplicateIndexException>(() => ArgParser.Serialize<DuplicateArgClass>(testArgs));
        }

        [Fact]
        public void WhenAnIndexIsSerializedWhichHasNotBeenDefinedThenShouldThrowIndexNotFoundException()
        {
            var testArgs = new[] { "10", "100" };

            Assert.Throws<IndexNotFoundException>(() => ArgParser.Serialize<SkipIndexArgClass>(testArgs));
        }

        [Fact]
        public void WhenFewerArgumentsAreGivenThanTheNumberOfRequiredPropertiesInTheTargetClassThenShouldThrowArgumentCountException()
        {
            var testArgs = new[] { "10" };

            Assert.Throws<ArgumentCountException>(() => ArgParser.Serialize<MultipleArgClass>(testArgs));
        }

        [Fact]
        public void WhenNullOrEmptyArgumentArrayIsPassedThenShouldReturnNull()
        {
            var emptyArgs = new string[0];
            string[] nullArgs = null;

            var emptyArgResult = ArgParser.Serialize<TestArgClass>(emptyArgs);
            var nullArgResult = ArgParser.Serialize<TestArgClass>(nullArgs);

            Assert.Null(emptyArgResult);
            Assert.Null(nullArgResult);
        }

        [Fact]
        public void GivenValidInputValuesThenAllSimpleTypesShouldSuccessfullySerialize()
        {
            var testArgs = new[] { "12", "test", "2/3/2019 5:00 PM", "1.5" };

            var result = ArgParser.Serialize<ExhaustiveArgClass>(testArgs);

            Assert.Equal(12, result.IntArg);
            Assert.Equal("test", result.StringArg);
            Assert.Equal(new DateTime(2019, 2, 3, 17, 0, 0), result.DateArg);
            Assert.Equal(1.5, result.DoubleArg);
        }

        [Fact]
        public void WhenAnOptionalArgIndexIsLowerThanARequiredArgIndexThenShouldThrowOptionalArgumentBoundsException()
        {
            var testArgs = new[] { "0", "1" };

            Assert.Throws<OptionalArgumentBoundsException>(() =>
                ArgParser.Serialize<OptionalArgBeforeRequiredArgClass>(testArgs));
        }

        [Fact]
        public void WhenNoArgumentIsPassedCorrespondingToAnOptionalArgumentAndADefaultValueIsSpecifiedThenTheDefaultValueShouldBeUsed()
        {
            var testArgs = new[] { "10" };

            var result = ArgParser.Serialize<OptionalWithDefaultValueHappyArgClass>(testArgs);

            Assert.Equal(-1, result.OptionalIntArg);
            Assert.Equal(DateTime.Today, result.OptionalDateTimeArg);
        }

        [Fact]
        public void WhenNullIsPassedForAnOptionalAnArgumentValueThenTheTheDefaultValueShouldBeUsed()
        {
            var testArgs = new[] { "10", "NULL", "null" };

            var result = ArgParser.Serialize<OptionalWithDefaultValueHappyArgClass>(testArgs);

            Assert.Equal(-1, result.OptionalIntArg);
            Assert.Equal(DateTime.Today, result.OptionalDateTimeArg);
        }

        [Fact]
        public void WhenNullIsPassedForARequiredArgumentValueThenShouldThrowRequiredArgumentNullException()
        {
            var testArgs = new[] { "Null" };

            Assert.Throws<RequiredArgumentNullException>(() =>
                ArgParser.Serialize<OptionalWithDefaultValueHappyArgClass>(testArgs));
        }

        private class TestArgClass
        {
            [Arg(0)]
            public int FirstArg { get; set; }
        }

        private class DuplicateArgClass
        {
            [Arg(0)] public int FirstArg { get; set; }
            [Arg(0)] public int DuplicateIndexArg { get; set; }
        }

        private class SkipIndexArgClass
        {
            [Arg(0)] public int FirstArg { get; set; }
            [Arg(2)] public int SecondArg { get; set; }
        }

        private class MultipleArgClass
        {
            [Arg(0)] public int FirstArg { get; set; }
            [Arg(1)] public int SecondArg { get; set; } 
        }

        private class ExhaustiveArgClass
        {
            [Arg(0)] public int IntArg { get; set; }
            [Arg(1)] public string StringArg { get; set; }
            [Arg(2)] public DateTime DateArg { get; set; }
            [Arg(3)] public double DoubleArg { get; set; }
        }

        private class OptionalArgBeforeRequiredArgClass
        {
            [Arg(0, Optional = true)] public int OptionalArg { get; set; } = -1;
            [Arg(1)] public int RequiredArg { get; set; }
        }

        private class OptionalWithDefaultValueHappyArgClass
        {
            [Arg(0)] public int FirstArg { get; set; }
            [Arg(1, Optional = true)] public int OptionalIntArg { get; set; } = -1;
            [Arg(2, Optional = true)] public DateTime OptionalDateTimeArg { get; set; } = DateTime.Today;
        }
    }    
}
