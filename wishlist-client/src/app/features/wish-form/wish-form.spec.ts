import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WishForm } from './wish-form';

describe('WishForm', () => {
  let component: WishForm;
  let fixture: ComponentFixture<WishForm>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [WishForm],
    }).compileComponents();

    fixture = TestBed.createComponent(WishForm);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
