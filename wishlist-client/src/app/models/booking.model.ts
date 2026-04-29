export interface BookedWish {
  id: number;
  bookedAt: Date;
  wishId: number;
  wishTitle: string;
  wishPrice?: number;
  wishLink?: string;
  wishDescription?: string;
  ownerId: number;
  ownerName: string;
  wishlistId: number;
  wishlistName: string;
  status: number;
}