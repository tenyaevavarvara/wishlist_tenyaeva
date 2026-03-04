namespace wishlist_tenyaeva.DTO
{
    public class BookedWishResponseDto
    {
        public int Id { get; set; }
        public DateTime BookedAt { get; set; }
        public int WishId { get; set; }
        public string WishTitle { get; set; }
        public decimal? WishPrice { get; set; }
        public int OwnerId { get; set; }
        public string OwnerName { get; set; }
        public string WishlistName { get; set; }
    }
}