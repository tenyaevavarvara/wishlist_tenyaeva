export interface Wishlist {
  id: number;
  name: string;
  description?: string;
  createdAt: Date;
  userId: number;
  ownerName: string;
  wishesCount: number;
  availableWishes: number;
  bookedWishes: number;
  fulfilledWishes: number;
}

export interface CreateWishlistDto {
  name: string;
  description?: string;
}

export interface UpdateWishlistDto {
  id: number;
  name: string;
  description?: string;
}