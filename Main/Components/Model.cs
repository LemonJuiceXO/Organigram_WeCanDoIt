namespace Main.Components
{
    public class Model
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Telephone { get; set; }

        public Model()
        {
            FirstName = "John";
            LastName = "Doe";
            Address = "123 Main St";
            Telephone = "555-555-5555";
        }

    }
}