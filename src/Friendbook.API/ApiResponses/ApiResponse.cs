namespace Friendbook.API.ApiResponses
{
    public class ApiResponse<T>
    {
        public int StatusCode { get; set; }
        public string ErrorMessage { get; set; }
        public string PropertyName { get; set; }
        public T Entities { get; set; }

        public bool IsSuccessful => StatusCode >= 200 && StatusCode < 300 ? true : false;

    }
}
