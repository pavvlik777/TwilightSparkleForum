namespace TwilightSparkle.Forum.Foundation.UserProfile
{
    public class UpdateUserProfileDto
    {
        public string UserProfileImageExternalId { get; }


        public UpdateUserProfileDto(string userProfileImageExternalId)
        {
            UserProfileImageExternalId = userProfileImageExternalId;
        }
    }
}