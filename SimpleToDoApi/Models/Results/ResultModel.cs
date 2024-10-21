namespace SimpleToDoApi.Models.Results
{
    public sealed class ResultModel<T>
    {
        public bool IsSucces { get; set; }
        public T? Result { get; set; } = default;
        public List<string> Errors { get; set; } = [];

        private ResultModel(bool isSucces, T? result)
        {
            IsSucces = isSucces;
            Result = result;
        }

        private ResultModel(bool isSucces, T? result, List<string> errors) : this(isSucces, result)
            => Errors = errors;

        public static ResultModel<T> Success(T result) => new(true, result);

        public static ResultModel<T> Error(List<string> errors) => new(false, default, errors);

        public static ResultModel<T> Error(T result) => new(false, result);

        public static ResultModel<T> Error(T result, List<string> errors)
            => new(false, result, errors);
    }
}
