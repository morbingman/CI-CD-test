var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<DateValidator>();
builder.Services.AddCors(options =>
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

var app = builder.Build();
app.UseCors();
app.UseSwagger();
app.UseSwaggerUI();

app.MapPost("/validate-date", (DateRequest req, DateValidator validator) =>
{
    var result = validator.CheckDate(req.Day, req.Month, req.Year);
    if (result.IsValid)
    {
        int.TryParse(req.Day, out int d);
        int.TryParse(req.Month, out int m);
        int.TryParse(req.Year, out int y);
        return Results.Ok(new DateResponse(true, $"{d:D2}/{m:D2}/{y} is a valid date."));
    }
    return Results.BadRequest(new DateResponse(false, result.ErrorMessage));
})
.WithName("ValidateDate")
.WithSummary("Validate a date")
.WithDescription("Checks whether the given day/month/year forms a valid Gregorian calendar date.");

app.MapGet("/health", () => Results.Ok(new { status = "ok" }))
   .WithName("Health")
   .WithSummary("Health check");

app.Run();

// ── Models ────────────────────────────────────────────────────────────────────

record DateRequest(string Day, string Month, string Year);
record DateResponse(bool IsValid, string Message);

// ── DateValidator ─────────────────────────────────────────────────────────────

public class DateValidator
{
    public class ValidationResult
    {
        public bool IsValid { get; set; }
        public string ErrorMessage { get; set; }
        public static ValidationResult Success() => new ValidationResult { IsValid = true };
        public static ValidationResult Failure(string message) => new ValidationResult { IsValid = false, ErrorMessage = message };
    }

    public ValidationResult ValidateDay(string input, out int day)
    {
        if (!int.TryParse(input, out day)) return ValidationResult.Failure("Day must be a number.");
        if (day < 1 || day > 31) return ValidationResult.Failure("Day must be between 1 and 31.");
        return ValidationResult.Success();
    }

    public ValidationResult ValidateMonth(string input, out int month)
    {
        if (!int.TryParse(input, out month)) return ValidationResult.Failure("Month must be a number.");
        if (month < 1 || month > 12) return ValidationResult.Failure("Month must be between 1 and 12.");
        return ValidationResult.Success();
    }

    public ValidationResult ValidateYear(string input, out int year)
    {
        if (!int.TryParse(input, out year)) return ValidationResult.Failure("Year must be a number.");
        if (year < 1000 || year > 3000) return ValidationResult.Failure("Year must be between 1000 and 3000.");
        return ValidationResult.Success();
    }

    public bool IsLeapYear(int year) =>
        (year % 400 == 0) || (year % 4 == 0 && year % 100 != 0);

    public int GetMaxDaysInMonth(int year, int month)
    {
        if (month == 2) return IsLeapYear(year) ? 29 : 28;
        if (month == 4 || month == 6 || month == 9 || month == 11) return 30;
        return 31;
    }

    public bool IsValidDate(int year, int month, int day) =>
        day <= GetMaxDaysInMonth(year, month);

    public ValidationResult CheckDate(string dayInput, string monthInput, string yearInput)
    {
        var dayResult = ValidateDay(dayInput, out int day);
        if (!dayResult.IsValid) return dayResult;

        var monthResult = ValidateMonth(monthInput, out int month);
        if (!monthResult.IsValid) return monthResult;

        var yearResult = ValidateYear(yearInput, out int year);
        if (!yearResult.IsValid) return yearResult;

        if (!IsValidDate(year, month, day))
            return ValidationResult.Failure($"{day:D2}/{month:D2}/{year} is NOT a valid date.");

        return ValidationResult.Success();
    }
}
