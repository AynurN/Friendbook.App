using Friendbook.Business.Enums;

namespace Friendbook.MVC.ViewModels
{
    public record ProfileViewModel(string FullName, string? ProfileImageImageUrl, string Email,string Id);
    public record UserProfileViewModel(string FullName, string? ProfileImageImageUrl, string Email, string Id);
    public record FriendProfileVM(ProfileViewModel vm, List<PostVM> posts, FriendshipStatus friendship);
}
