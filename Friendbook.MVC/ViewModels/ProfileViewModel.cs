namespace Friendbook.MVC.ViewModels
{
    public record ProfileViewModel(string FullName, string? ProfileImageImageUrl, string Email);
    public record UserProfileViewModel(string FullName, string? ProfileImageImageUrl, string Email, string Id);
}
