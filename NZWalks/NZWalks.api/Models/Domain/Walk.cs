namespace NZWalks.api.Models.Domain
{
    public class Walk
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Length { get; set; } 
        public Guid RegionId { get; set; }
        public Guid WalkDifficultyID{ get; set; }

        //Navigation property

        public Region Regin { get; set; }
        public WalkDifficulty WalkDifficulty { get; set; }

        // this Walk has only one relationship with one region and one walk difficulty
    }
}
