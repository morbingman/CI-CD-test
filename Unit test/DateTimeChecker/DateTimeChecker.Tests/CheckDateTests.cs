using NUnit.Framework;
using DateTimeChecker;

namespace DateTimeChecker.Tests
{
    /// <summary>
    /// End-to-end tests for CheckDate — the single method that mirrors
    /// exactly what the Check button does in the UI.
    /// These tests simulate real user input coming in as raw strings.
    /// </summary>
    [TestFixture]
    public class CheckDateTests
    {
        private DateValidator _validator;

        [SetUp]
        public void SetUp() => _validator = new DateValidator();

        // ── Valid dates ──────────────────────────────────────────────────

        [Test]
        public void CheckDate_ValidDate_ReturnsSuccess()
            => Assert.That(_validator.CheckDate("15", "6", "2024").IsValid, Is.True);

        [Test]
        public void CheckDate_Feb29_LeapYear_ReturnsSuccess()
            => Assert.That(_validator.CheckDate("29", "2", "2024").IsValid, Is.True);

        [Test]
        public void CheckDate_LastDayOfYear_Dec31_ReturnsSuccess()
            => Assert.That(_validator.CheckDate("31", "12", "2024").IsValid, Is.True);

        [Test]
        public void CheckDate_FirstDayOfYear_Jan1_ReturnsSuccess()
            => Assert.That(_validator.CheckDate("1", "1", "2024").IsValid, Is.True);

        [Test]
        public void CheckDate_BoundaryYear_1000_ReturnsSuccess()
            => Assert.That(_validator.CheckDate("1", "1", "1000").IsValid, Is.True);

        [Test]
        public void CheckDate_BoundaryYear_3000_ReturnsSuccess()
            => Assert.That(_validator.CheckDate("31", "12", "3000").IsValid, Is.True);

        // ── Invalid dates ────────────────────────────────────────────────

        [Test]
        public void CheckDate_Feb29_NonLeapYear_ReturnsFail()
        {
            var result = _validator.CheckDate("29", "2", "2023");
            Assert.That(result.IsValid, Is.False);
            Assert.That(result.ErrorMessage, Does.Contain("NOT a valid date"));
        }

        [Test]
        public void CheckDate_April31_ReturnsFail()
        {
            var result = _validator.CheckDate("31", "4", "2024");
            Assert.That(result.IsValid, Is.False);
            Assert.That(result.ErrorMessage, Does.Contain("NOT a valid date"));
        }

        [Test]
        public void CheckDate_Nov31_ReturnsFail()
            => Assert.That(_validator.CheckDate("31", "11", "2024").IsValid, Is.False);

        // ── Invalid field inputs ─────────────────────────────────────────

        [Test]
        public void CheckDate_NonNumericDay_ReturnsFail()
        {
            var result = _validator.CheckDate("xx", "6", "2024");
            Assert.That(result.IsValid, Is.False);
            Assert.That(result.ErrorMessage, Does.Contain("number"));
        }

        [Test]
        public void CheckDate_NonNumericMonth_ReturnsFail()
        {
            var result = _validator.CheckDate("15", "June", "2024");
            Assert.That(result.IsValid, Is.False);
            Assert.That(result.ErrorMessage, Does.Contain("number"));
        }

        [Test]
        public void CheckDate_NonNumericYear_ReturnsFail()
        {
            var result = _validator.CheckDate("15", "6", "TwoThousand");
            Assert.That(result.IsValid, Is.False);
            Assert.That(result.ErrorMessage, Does.Contain("number"));
        }

        [Test]
        public void CheckDate_AllEmpty_ReturnsFail()
            => Assert.That(_validator.CheckDate("", "", "").IsValid, Is.False);

        [Test]
        public void CheckDate_DayOutOfRange_ReturnsFail()
        {
            var result = _validator.CheckDate("32", "6", "2024");
            Assert.That(result.IsValid, Is.False);
            Assert.That(result.ErrorMessage, Does.Contain("between 1 and 31"));
        }

        [Test]
        public void CheckDate_MonthOutOfRange_ReturnsFail()
        {
            var result = _validator.CheckDate("15", "13", "2024");
            Assert.That(result.IsValid, Is.False);
            Assert.That(result.ErrorMessage, Does.Contain("between 1 and 12"));
        }

        [Test]
        public void CheckDate_YearBelowMinimum_ReturnsFail()
        {
            var result = _validator.CheckDate("15", "6", "999");
            Assert.That(result.IsValid, Is.False);
            Assert.That(result.ErrorMessage, Does.Contain("between 1000 and 3000"));
        }

        [Test]
        public void CheckDate_YearAboveMaximum_ReturnsFail()
        {
            var result = _validator.CheckDate("15", "6", "3001");
            Assert.That(result.IsValid, Is.False);
            Assert.That(result.ErrorMessage, Does.Contain("between 1000 and 3000"));
        }

        // ── Century year edge cases ──────────────────────────────────────

        [Test]
        public void CheckDate_Feb29_Year2000_CenturyLeap_ReturnsSuccess()
            => Assert.That(_validator.CheckDate("29", "2", "2000").IsValid, Is.True);

        [Test]
        public void CheckDate_Feb29_Year1900_CenturyNonLeap_ReturnsFail()
            => Assert.That(_validator.CheckDate("29", "2", "1900").IsValid, Is.False);
    }
}
