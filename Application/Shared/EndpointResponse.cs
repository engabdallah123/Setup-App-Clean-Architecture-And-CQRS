using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Shared
{
    public record EndpointResponse<T>(
     T? Data,
     string Message = "",
     string MessageAr = "",
     bool IsSuccess = true,
     int StatusCode = 200,
     List<string>? Errors = null,
     DateTime? Timestamp = null
    )
    {
        public List<string> Errors { get; init; } = Errors ?? new();
        public DateTime? Timestamp { get; init; } = Timestamp ?? DateTime.UtcNow;

        public static EndpointResponse<T> SuccessResponse(
            T data,
            string message = "Operation completed successfully",
            string messageAr = "العملية تمت بنجاح",
            int statusCode = 200
        ) => new(data, message, messageAr, true, statusCode);

        public static EndpointResponse<T> ErrorResponse(
            string message = "Operation failed",
            string messageAr = "فشل العملية",
            int statusCode = 400,
            List<string>? errors = null
        ) => new(default!, message, messageAr, false, statusCode, errors ?? new() { message });

        public static EndpointResponse<T> ValidationErrorResponse(
            Dictionary<string, List<string>> validationErrors,
            string message = "Validation failed",
            string messageAr = "فشل التحقق من الصحة"
        )
        {
            var errors = new List<string>();
            foreach (var kvp in validationErrors)
                foreach (var err in kvp.Value)
                    errors.Add($"{kvp.Key}: {err}");

            return new(default!, message, messageAr, false, 400, errors);
        }

        public static EndpointResponse<T> NotFoundResponse(
            string message = "Resource not found",
            string messageAr = "المورد غير موجود"
        ) => new(default!, message, messageAr, false, 404, new() { message });

        public static EndpointResponse<T> UnauthorizedResponse(
            string message = "Unauthorized access",
            string messageAr = "الوصول غير مصرح به"
        ) => new(default!, message, messageAr, false, 401, new() { message });

        public static EndpointResponse<T> ForbiddenResponse(
            string message = "Access forbidden",
            string messageAr = "الوصول محظور"
        ) => new(default!, message, messageAr, false, 403, new() { message });

        public static EndpointResponse<T> ConflictResponse(
            string message = "Resource conflict",
            string messageAr = "تعارض المورد"
        ) => new(default!, message, messageAr, false, 409, new() { message });

        public static EndpointResponse<T> InternalServerErrorResponse(
            string message = "Internal server error occurred",
            string messageAr = "حدث خطأ داخلي في الخادم"
        ) => new(default!, message, messageAr, false, 500, new() { message });
    }
}
