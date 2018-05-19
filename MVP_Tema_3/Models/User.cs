

namespace MVP_Tema_3
{
    public partial class User
    {
        public override string ToString()
        {
            return $"{FirstName} {LastName} {Patients?.Count??0}";
        }
    }
}
