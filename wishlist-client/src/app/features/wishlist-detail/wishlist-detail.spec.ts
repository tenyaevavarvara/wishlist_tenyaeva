import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WishlistDetail } from './wishlist-detail';

describe('WishlistDetail', () => {
  let component: WishlistDetail;
  let fixture: ComponentFixture<WishlistDetail>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [WishlistDetail],
    }).compileComponents();

    fixture = TestBed.createComponent(WishlistDetail);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
