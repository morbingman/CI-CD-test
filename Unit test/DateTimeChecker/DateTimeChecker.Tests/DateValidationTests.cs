using NUnit.Framework;
using DateTimeChecker;

namespace DateTimeChecker.Tests
{
    /// <summary>
    /// Tests for IsValidDate and GetMaxDaysInMonth.
    /// Covers: 31-day months, 30-day months, February in leap/non-leap years,
    /// boundary days (first/last), and common invalid dates.
    /// </summary>
    [TestFixture]
    public class DateValidationTests
    {
        private DateValidator _validator;

        [SetUp]
        public void SetUp() => _validator = new DateValidator();

        // ── GetMaxDaysInMonth ────────────────────────────────────────────

        [TestCase(2024, 1,  31)]  // January
        [TestCase(2024, 3,  31)]  // March
        [TestCase(2024, 5,  31)]  // May
        [TestCase(2024, 7,  31)]  // July
        [TestCase(2024, 8,  31)]  // August
        [TestCase(2024, 10, 31)]  // October
        [TestCase(2024, 12, 31)]  // December
        public void GetMaxDaysInMonth_31DayMonths_Returns31(int year, int month, int expected)
            => Assert.That(_validator.GetMaxDaysInMonth(year, month), Is.EqualTo(expected));

        [TestCase(2024, 4,  30)]  // April
        [TestCase(2024, 6,  30)]  // June
        [TestCase(2024, 9,  30)]  // September
        [TestCase(2024, 11, 30)]  // November
        public void GetMaxDaysInMonth_30DayMonths_Returns30(int year, int month, int expected)
            => Assert.That(_validator.GetMaxDaysInMonth(year, month), Is.EqualTo(expected));

        [Test]
        public void GetMaxDaysInMonth_February_LeapYear_Returns29()
            => Assert.That(_validator.GetMaxDaysInMonth(2024, 2), Is.EqualTo(29));

        [Test]
        public void GetMaxDaysInMonth_February_NonLeapYear_Returns28()
            => Assert.That(_validator.GetMaxDaysInMonth(2023, 2), Is.EqualTo(28));

        // ── IsValidDate: Happy Paths ─────────────────────────────────────

        [Test]
        public void IsValidDate_FirstDayOfYear_ReturnsTrue()
            => Assert.That(_validator.IsValidDate(2024, 1, 1), Is.True);

        [Test]
        public void IsValidDate_LastDayOf31DayMonth_ReturnsTrue()
            => Assert.That(_validator.IsValidDate(2024, 1, 31), Is.True);

        [Test]
        public void IsValidDate_LastDayOf30DayMonth_ReturnsTrue()
            => Assert.That(_validator.IsValidDate(2024, 4, 30), Is.True);

        [Test]
        public void IsValidDate_Feb29_LeapYear_ReturnsTrue()
            => Assert.That(_validator.IsValidDate(2024, 2, 29), Is.True);

        [Test]
        public void IsValidDate_Feb28_NonLeapYear_ReturnsTrue()
            => Assert.That(_validator.IsValidDate(2023, 2, 28), Is.True);

        // ── IsValidDate: Invalid Dates ───────────────────────────────────

        [Test]
        public void IsValidDate_Feb29_NonLeapYear_ReturnsFalse()
            => Assert.That(_validator.IsValidDate(2023, 2, 29), Is.False);

        [Test]
        public void IsValidDate_Feb30_AnyYear_ReturnsFalse()
            => Assert.That(_validator.IsValidDate(2024, 2, 30), Is.False);

        [Test]
        public void IsValidDate_Day31_In30DayMonth_ReturnsFalse()
            => Assert.That(_validator.IsValidDate(2024, 4, 31), Is.False);

        [Test]
        public void IsValidDate_Day31_InNovember_ReturnsFalse()
            => Assert.That(_validator.IsValidDate(2024, 11, 31), Is.False);

        [Test]
        public void IsValidDate_Day31_InJune_ReturnsFalse()
            => Assert.That(_validator.IsValidDate(2024, 6, 31), Is.False);

        // ── IsValidDate: Century year edge cases ─────────────────────────

        [Test]
        public void IsValidDate_Feb29_Year2000_DivisibleBy400_ReturnsTrue()
            => Assert.That(_validator.IsValidDate(2000, 2, 29), Is.True);

        [Test]
        public void IsValidDate_Feb29_Year1900_DivisibleBy100NotBy400_ReturnsFalse()
            => Assert.That(_validator.IsValidDate(1900, 2, 29), Is.False);

        [Test]
        public void IsValidDate_Feb29_Year2100_DivisibleBy100NotBy400_ReturnsFalse()
            => Assert.That(_validator.IsValidDate(2100, 2, 29), Is.False);

        // ── IsValidDate: TestCase batch ──────────────────────────────────

        [TestCase(2024, 1,  1,   true)]
        [TestCase(2024, 12, 31,  true)]
        [TestCase(2024, 2,  29,  true)]   // leap year
        [TestCase(2023, 2,  29,  false)]  // not a leap year
        [TestCase(2024, 4,  31,  false)]  // April has 30 days
        [TestCase(2024, 6,  31,  false)]  // June has 30 days
        [TestCase(1900, 2,  29,  false)]  // century non-leap
        [TestCase(2000, 2,  29,  true)]   // century leap
        public void IsValidDate_TestCase(int year, int month, int day, bool expected)
            => Assert.That(_validator.IsValidDate(year, month, day), Is.EqualTo(expected));
    }
}
