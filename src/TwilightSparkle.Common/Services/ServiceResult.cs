using System;

namespace TwilightSparkle.Common.Services
{
    public class ServiceResult<TValue, TError>
        where TValue : class
        where TError : struct
    {
        private readonly TValue _value;
        private readonly TError? _errorType;

        public bool IsSuccess { get; }

        public TValue Value
        {
            get
            {
                if (!IsSuccess)
                {
                    throw new InvalidOperationException("Can't get value of an unsuccessful result");
                }

                return _value;
            }
        }

        public TError ErrorType
        {
            get
            {
                if (IsSuccess)
                {
                    throw new InvalidOperationException("Can't get error type of a successful result");
                }

                return _errorType.Value;
            }
        }


        private ServiceResult(bool isSuccess, TValue value, TError? errorType)
        {
            IsSuccess = isSuccess;
            _value = value;
            _errorType = errorType;
        }


        public static ServiceResult<TValue, TError> CreateSuccess(TValue value)
        {
            var result = new ServiceResult<TValue, TError>(true, value, null);

            return result;
        }

        public static ServiceResult<TValue, TError> CreateFailed(TError errorType)
        {
            var result = new ServiceResult<TValue, TError>(false, null, errorType);

            return result;
        }
    }

    public class ServiceResult<TError>
        where TError : struct
    {
        private readonly TError? _errorType;

        public bool IsSuccess { get; }

        public TError ErrorType
        {
            get
            {
                if (IsSuccess)
                {
                    throw new InvalidOperationException("Can't get error type of a successful result");
                }

                return _errorType.Value;
            }
        }


        private ServiceResult(bool isSuccess, TError? errorType)
        {
            IsSuccess = isSuccess;
            _errorType = errorType;
        }


        public static ServiceResult<TError> CreateSuccess()
        {
            var result = new ServiceResult<TError>(true, null);

            return result;
        }

        public static ServiceResult<TError> CreateFailed(TError errorType)
        {
            var result = new ServiceResult<TError>(false, errorType);

            return result;
        }
    }

    public static class ServiceResult
    {
        public static ServiceResult<TValue, TError> CreateSuccess<TValue, TError>(TValue value)
            where TValue : class
            where TError : struct
            => ServiceResult<TValue, TError>.CreateSuccess(value);

        public static ServiceResult<TValue, TError> CreateFailed<TValue, TError>(TError errorType)
            where TValue : class
            where TError : struct
            => ServiceResult<TValue, TError>.CreateFailed(errorType);

        public static ServiceResult<TError> CreateSuccess<TError>()
            where TError : struct
            => ServiceResult<TError>.CreateSuccess();

        public static ServiceResult<TError> CreateFailed<TError>(TError errorType)
            where TError : struct
            => ServiceResult<TError>.CreateFailed(errorType);
    }
}