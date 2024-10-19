namespace Friendbook.MVC.ApiResponseMessages
{
    public class ApiResponseMessage<T>
    {
        public int StatusCode { get; set; }
        public string ErrorMessage { get; set; }
        public T Entities { get; set; }
        public string PropertyName { get; set; }
        public bool IsSuccessfull { get; set; }
        
    }
}
