using NUnit.Framework;
using DateTimeChecker;

namespace DateTimeChecker.Tests
{
    /// <summary>
    /// Tests for the IsLeapYear method.
    /// Leap year rules (Gregorian calendar):
    ///   - Divisible by 400  → leap year        (e.g. 2000, 2400)
    ///   - Divisible by 100 but not 400 → NOT   (e.g. 1900, 2100)
    ///   - Divisible by 4 but not 100   → leap  (e.g. 2024, 2028)
    ///   - Not divisible by 4            → NOT   (e.g. 2023, 2019)
    /// </summary>
    [TestFixture]
    public class LeapYearTests
    {
        private DateValidator _validator;

        [SetUp]
        public void SetUp() => _validator = new DateValidator();

        // ── Divisible by 400 → always a leap year ───────────────────────

        [Test]
        public void IsLeapYear_DivisibleBy400_ReturnsTrue()
            => Assert.That(_validator.IsLeapYear(2000), Is.True);

        [Test]
        public void IsLeapYear_2400_DivisibleBy400_ReturnsTrue()
            => Assert.That(_validator.IsLeapYear(2400), Is.True);

        // ── Divisible by 100 but not 400 → NOT a leap year ──────────────

        [Test]
        public void IsLeapYear_DivisibleBy100NotBy400_ReturnsFalse()
            => Assert.That(_validator.IsLeapYear(1900), Is.False);

        [Test]
        public void IsLeapYear_2100_DivisibleBy100NotBy400_ReturnsFalse()
            => Assert.That(_validator.IsLeapYear(2100), Is.False);

        // ── Divisible by 4 but not 100 → leap year ──────────────────────

        [Test]
        public void IsLeapYear_DivisibleBy4NotBy100_ReturnsTrue()
            => Assert.That(_validator.IsLeapYear(2024), Is.True);

        [Test]
        public void IsLeapYear_2028_DivisibleBy4NotBy100_ReturnsTrue()
            => Assert.That(_validator.IsLeapYear(2028), Is.True);

        // ── Not divisible by 4 → NOT a leap year ────────────────────────

        [Test]
        public void IsLeapYear_NotDivisibleBy4_ReturnsFalse()
            => Assert.That(_validator.IsLeapYear(2023), Is.False);

        [Test]
        public void IsLeapYear_2019_NotDivisibleBy4_ReturnsFalse()
            => Assert.That(_validator.IsLeapYear(2019), Is.False);

        // ── TestCase: batch check ────────────────────────────────────────

        [TestCase(1600, true)]
        [TestCase(2000, true)]
        [TestCase(2400, true)]
        [TestCase(1700, false)]
        [TestCase(1800, false)]
        [TestCase(1900, false)]
        [TestCase(2100, false)]
        [TestCase(2024, true)]
        [TestCase(2028, true)]
        [TestCase(2023, false)]
        public void IsLeapYear_TestCase(int year, bool expected)
            => Assert.That(_validator.IsLeapYear(year), Is.EqualTo(expected));
    }
}
