using NUnit.Framework;
using DateTimeChecker;

namespace DateTimeChecker.Tests
{
    /// <summary>
    /// Tests for ValidateDay, ValidateMonth, ValidateYear —
    /// the string-parsing layer that sits between raw UI input and core logic.
    /// </summary>
    [TestFixture]
    public class FieldValidatorTests
    {
        private DateValidator _validator;

        [SetUp]
        public void SetUp() => _validator = new DateValidator();

        // ── ValidateDay ──────────────────────────────────────────────────

        [Test]
        public void ValidateDay_ValidInput_ReturnsSuccess()
        {
            var result = _validator.ValidateDay("15", out int day);
            Assert.That(result.IsValid, Is.True);
            Assert.That(day, Is.EqualTo(15));
        }

        [Test]
        public void ValidateDay_BoundaryLow_Day1_ReturnsSuccess()
        {
            var result = _validator.ValidateDay("1", out int day);
            Assert.That(result.IsValid, Is.True);
            Assert.That(day, Is.EqualTo(1));
        }

        [Test]
        public void ValidateDay_BoundaryHigh_Day31_ReturnsSuccess()
        {
            var result = _validator.ValidateDay("31", out int day);
            Assert.That(result.IsValid, Is.True);
            Assert.That(day, Is.EqualTo(31));
        }

        [Test]
        public void ValidateDay_Zero_ReturnsFail()
        {
            var result = _validator.ValidateDay("0", out _);
            Assert.That(result.IsValid, Is.False);
            Assert.That(result.ErrorMessage, Does.Contain("between 1 and 31"));
        }

        [Test]
        public void ValidateDay_32_ReturnsFail()
        {
            var result = _validator.ValidateDay("32", out _);
            Assert.That(result.IsValid, Is.False);
            Assert.That(result.ErrorMessage, Does.Contain("between 1 and 31"));
        }

        [Test]
        public void ValidateDay_NonNumeric_ReturnsFail()
        {
            var result = _validator.ValidateDay("abc", out _);
            Assert.That(result.IsValid, Is.False);
            Assert.That(result.ErrorMessage, Does.Contain("number"));
        }

        [Test]
        public void ValidateDay_Empty_ReturnsFail()
        {
            var result = _validator.ValidateDay("", out _);
            Assert.That(result.IsValid, Is.False);
        }

        [Test]
        public void ValidateDay_Whitespace_ReturnsFail()
        {
            var result = _validator.ValidateDay("  ", out _);
            Assert.That(result.IsValid, Is.False);
        }

        [Test]
        public void ValidateDay_Negative_ReturnsFail()
        {
            var result = _validator.ValidateDay("-1", out _);
            Assert.That(result.IsValid, Is.False);
        }

        // ── ValidateMonth ────────────────────────────────────────────────

        [Test]
        public void ValidateMonth_ValidInput_ReturnsSuccess()
        {
            var result = _validator.ValidateMonth("6", out int month);
            Assert.That(result.IsValid, Is.True);
            Assert.That(month, Is.EqualTo(6));
        }

        [Test]
        public void ValidateMonth_BoundaryLow_Month1_ReturnsSuccess()
        {
            var result = _validator.ValidateMonth("1", out int month);
            Assert.That(result.IsValid, Is.True);
            Assert.That(month, Is.EqualTo(1));
        }

        [Test]
        public void ValidateMonth_BoundaryHigh_Month12_ReturnsSuccess()
        {
            var result = _validator.ValidateMonth("12", out int month);
            Assert.That(result.IsValid, Is.True);
            Assert.That(month, Is.EqualTo(12));
        }

        [Test]
        public void ValidateMonth_Zero_ReturnsFail()
        {
            var result = _validator.ValidateMonth("0", out _);
            Assert.That(result.IsValid, Is.False);
            Assert.That(result.ErrorMessage, Does.Contain("between 1 and 12"));
        }

        [Test]
        public void ValidateMonth_13_ReturnsFail()
        {
            var result = _validator.ValidateMonth("13", out _);
            Assert.That(result.IsValid, Is.False);
            Assert.That(result.ErrorMessage, Does.Contain("between 1 and 12"));
        }

        [Test]
        public void ValidateMonth_NonNumeric_ReturnsFail()
        {
            var result = _validator.ValidateMonth("Jan", out _);
            Assert.That(result.IsValid, Is.False);
            Assert.That(result.ErrorMessage, Does.Contain("number"));
        }

        // ── ValidateYear ─────────────────────────────────────────────────

        [Test]
        public void ValidateYear_ValidInput_ReturnsSuccess()
        {
            var result = _validator.ValidateYear("2024", out int year);
            Assert.That(result.IsValid, Is.True);
            Assert.That(year, Is.EqualTo(2024));
        }

        [Test]
        public void ValidateYear_BoundaryLow_1000_ReturnsSuccess()
        {
            var result = _validator.ValidateYear("1000", out int year);
            Assert.That(result.IsValid, Is.True);
            Assert.That(year, Is.EqualTo(1000));
        }

        [Test]
        public void ValidateYear_BoundaryHigh_3000_ReturnsSuccess()
        {
            var result = _validator.ValidateYear("3000", out int year);
            Assert.That(result.IsValid, Is.True);
            Assert.That(year, Is.EqualTo(3000));
        }

        [Test]
        public void ValidateYear_999_BelowMinimum_ReturnsFail()
        {
            var result = _validator.ValidateYear("999", out _);
            Assert.That(result.IsValid, Is.False);
            Assert.That(result.ErrorMessage, Does.Contain("between 1000 and 3000"));
        }

        [Test]
        public void ValidateYear_3001_AboveMaximum_ReturnsFail()
        {
            var result = _validator.ValidateYear("3001", out _);
            Assert.That(result.IsValid, Is.False);
            Assert.That(result.ErrorMessage, Does.Contain("between 1000 and 3000"));
        }

        [Test]
        public void ValidateYear_NonNumeric_ReturnsFail()
        {
            var result = _validator.ValidateYear("year", out _);
            Assert.That(result.IsValid, Is.False);
            Assert.That(result.ErrorMessage, Does.Contain("number"));
        }
    }
}
