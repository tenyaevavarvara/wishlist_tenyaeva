import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WishlistForm } from './wishlist-form';

describe('WishlistForm', () => {
  let component: WishlistForm;
  let fixture: ComponentFixture<WishlistForm>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [WishlistForm],
    }).compileComponents();

    fixture = TestBed.createComponent(WishlistForm);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
