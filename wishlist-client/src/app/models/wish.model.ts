export enum WishStatus {
  Available = 0,
  Booked = 1,
  Fulfilled = 2
}

export const WishStatusLabels: { [key in WishStatus]: string } = {
  [WishStatus.Available]: 'Доступно',
  [WishStatus.Booked]: 'Забронировано',
  [WishStatus.Fulfilled]: 'Исполнено'
};

export const WishStatusColors: { [key in WishStatus]: string } = {
  [WishStatus.Available]: 'primary',
  [WishStatus.Booked]: 'accent',
  [WishStatus.Fulfilled]: 'warn'
};

export interface Wish {
  id: number;
  title: string;
  description?: string;
  link?: string;
  price?: number;
  status: WishStatus;
  createdAt: Date;
  wishlistId: number;
  wishlistName: string;
  bookedByUserId?: number;
  bookedByUserName?: string;
}

export interface CreateWishDto {
  title: string;
  description?: string;
  link?: string;
  price?: number;
  wishlistId: number;
}