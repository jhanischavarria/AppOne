namespace UPB.LogicPatient.Models
{
    public class Patient
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string CI { get; set; }
        public string BloodGroup { get; set; }


            //Randomly assign blood group
            

        public Patient()
        {
            BloodGroup = GetRandomBloodGroup();
        }

        // Method to randomly assign blood group
        private string GetRandomBloodGroup()
        {
            var bloodGroups = new List<string> { "A", "B", "AB", "O" };
            Random rand = new Random();
            return bloodGroups[rand.Next(bloodGroups.Count)];
        }
    }
}
