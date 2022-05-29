namespace WebAPI.Extensions
{
    //1st rule:extesnion class should be static class
    //2nd rule:first parameter of this type must be of type which we want to by using this keyword 
    public static class StringExtensions
    {
        public static bool isEmpty(this string s)
        {
            return string.IsNullOrEmpty(s.TrimEnd());
        }
    }
}
