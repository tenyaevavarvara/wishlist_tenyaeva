import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MyWishlists } from './my-wishlists';

describe('MyWishlists', () => {
  let component: MyWishlists;
  let fixture: ComponentFixture<MyWishlists>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MyWishlists],
    }).compileComponents();

    fixture = TestBed.createComponent(MyWishlists);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
