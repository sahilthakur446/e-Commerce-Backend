using eCommerce.Utilities.Enums;

namespace eCommerce.Utilities
{
    public static class GenderConverter
        {
        public static GenderApplicability GetTargetGender(string targetGender)
            {
            if (Enum.TryParse<GenderApplicability>(targetGender, out var gender))
                {
                return gender;
                }
            throw new InvalidOperationException("Invalid gender value");
            }

        public static string GetTargetGenderString(GenderApplicability? targetGender)
            {
            return Enum.GetName(typeof(GenderApplicability), targetGender);
            }

        public static UserGender GetUserGender(string userGender)
            {
            if (Enum.TryParse<UserGender>(userGender, out var gender))
                {
                return gender;
                }
            throw new InvalidOperationException("Invalid gender value");
            }

        public static string GetUserGenderString(UserGender? userGender)
            {
            return Enum.GetName(typeof(UserGender), userGender);
            }
        }
    }

