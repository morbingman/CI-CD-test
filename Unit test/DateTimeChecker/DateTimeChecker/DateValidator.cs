using System;

namespace DateTimeChecker
{
    /// <summary>
    /// Contains all date validation logic, extracted from MainForm
    /// so it can be tested independently without any UI dependency.
    /// </summary>
    public class DateValidator
    {
        // ── Validation Results ────────────────────────────────────────────

        public class ValidationResult
        {
            public bool IsValid { get; set; }
            public string ErrorMessage { get; set; }

            public static ValidationResult Success() =>
                new ValidationResult { IsValid = true };

            public static ValidationResult Failure(string message) =>
                new ValidationResult { IsValid = false, ErrorMessage = message };
        }

        // ── Field Validators ─────────────────────────────────────────────

        public ValidationResult ValidateDay(string input, out int day)
        {
            if (!int.TryParse(input, out day))
                return ValidationResult.Failure("Day must be a number.");

            if (day < 1 || day > 31)
                return ValidationResult.Failure("Day must be between 1 and 31.");

            return ValidationResult.Success();
        }

        public ValidationResult ValidateMonth(string input, out int month)
        {
            if (!int.TryParse(input, out month))
                return ValidationResult.Failure("Month must be a number.");

            if (month < 1 || month > 12)
                return ValidationResult.Failure("Month must be between 1 and 12.");

            return ValidationResult.Success();
        }

        public ValidationResult ValidateYear(string input, out int year)
        {
            if (!int.TryParse(input, out year))
                return ValidationResult.Failure("Year must be a number.");

            if (year < 1000 || year > 3000)
                return ValidationResult.Failure("Year must be between 1000 and 3000.");

            return ValidationResult.Success();
        }

        // ── Core Logic ───────────────────────────────────────────────────

        /// <summary>
        /// Returns true if the year is a leap year in the Gregorian calendar.
        /// Rule: divisible by 400, OR divisible by 4 but NOT by 100.
        /// </summary>
        public bool IsLeapYear(int year)
        {
            return (year % 400 == 0) || (year % 4 == 0 && year % 100 != 0);
        }

        /// <summary>
        /// Returns the maximum number of days in a given month/year.
        /// </summary>
        public int GetMaxDaysInMonth(int year, int month)
        {
            if (month == 2)
                return IsLeapYear(year) ? 29 : 28;

            if (month == 4 || month == 6 || month == 9 || month == 11)
                return 30;

            return 31;
        }

        /// <summary>
        /// Returns true if the day exists within the given month and year.
        /// </summary>
        public bool IsValidDate(int year, int month, int day)
        {
            return day <= GetMaxDaysInMonth(year, month);
        }

        /// <summary>
        /// Full end-to-end check: parse and validate all three string inputs,
        /// then confirm the resulting date exists in the Gregorian calendar.
        /// </summary>
        public ValidationResult CheckDate(string dayInput, string monthInput, string yearInput)
        {
            var dayResult = ValidateDay(dayInput, out int day);
            if (!dayResult.IsValid) return dayResult;

            var monthResult = ValidateMonth(monthInput, out int month);
            if (!monthResult.IsValid) return monthResult;

            var yearResult = ValidateYear(yearInput, out int year);
            if (!yearResult.IsValid) return yearResult;

            if (!IsValidDate(year, month, day))
                return ValidationResult.Failure(
                    $"{day:D2}/{month:D2}/{year} is NOT a valid date.");

            return ValidationResult.Success();
        }
    }
}
